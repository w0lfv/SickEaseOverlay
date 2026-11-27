using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32;

namespace SickEaseOverlay
{
    public enum ScreenMode
    {
        Fullscreen,
        Application
    }

    // Values are saved as numbers: only ever append.
    public enum CrosshairType
    {
        None,
        Cross,
        Circle,
        Square,
        X,
        Diamond
    }

    public enum SecondaryCrosshairType
    {
        None,
        Edges,   // bars at the middle of each edge
        Corners, // L-brackets; keeps the middle of each edge (compass, hotbar, subtitles) clear
        Border,  // thin frame hugging the screen edge, under most HUD margins
        Dots     // evenly spaced dots over the whole screen as a fixed visual reference
    }

    public partial class MainForm : Form
    {
        private OverlayForm? overlay;
        private AppSettings settings = new();

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private const int WM_HOTKEY = 0x0312;
        private const uint MOD_ALT = 0x1, MOD_CONTROL = 0x2, MOD_SHIFT = 0x4, MOD_NOREPEAT = 0x4000;

        // Monitor Settings
        private Screen[] cachedScreens = Screen.AllScreens;

        // Windows Settings
        private readonly List<(IntPtr hWnd, string title, string processName)> windows = new();

        // Re-attaches to the saved app when it starts (or restarts) after this app.
        // ponytail: 2s poll; SetWinEventHook(EVENT_OBJECT_SHOW) if latency or CPU ever matters.
        private readonly System.Windows.Forms.Timer reconnectTimer = new() { Interval = 2000 };

        // Rows that can be hidden, plus every row below them, top to bottom. Hidden rows collapse and
        // the rest move up. ponytail: manual reflow over the designer's absolute layout; move to a
        // TableLayoutPanel if the form keeps growing.
        private readonly Control[] sizeRow, lengthRow, gapRow, secondarySizeRow;
        private readonly Control[][] rows;
        private readonly HashSet<Control[]> hiddenRows = new();
        private readonly Dictionary<Control, int> designTop;
        private readonly int[] rowTop;
        private readonly int designClientHeight;
        private readonly float designFontHeight;

        public MainForm()
        {
            InitializeComponent();
            SystemEvents.DisplaySettingsChanged += OnDisplaySettingsChanged;
            FormClosing += OnFormClosing;
            reconnectTimer.Tick += reconnectTimer_Tick;

            sizeRow = new Control[] { labelSize, barSize, updownSize };
            lengthRow = new Control[] { labelLength, barLength, updownLength };
            gapRow = new Control[] { labelGap, barGap, updownGap };
            secondarySizeRow = new Control[] { labelSecondarySize, barSecondarySize, updownSecondarySize };
            rows = new[]
            {
                sizeRow, lengthRow, gapRow,
                new Control[] { labelSecondary, comboSecondary, secondaryColorBox },
                secondarySizeRow,
                new Control[] { checkboxShowOverlay },
                new Control[] { labelHotkey, textHotkey },
            };
            designTop = rows.SelectMany(r => r).ToDictionary(c => c, c => c.Top);
            rowTop = rows.Select(r => r.Min(c => c.Top)).ToArray();
            designClientHeight = ClientSize.Height;
            designFontHeight = CurrentAutoScaleDimensions.Height;
        }

        private void ShowRow(Control[] row, bool show)
        {
            foreach (Control c in row)
                c.Visible = show;
            if (show) hiddenRows.Remove(row); else hiddenRows.Add(row);
        }

        // Stack the shown rows from the designer positions. Positions are rescaled by the font ratio so
        // this stays right after WinForms rescales the form for a different monitor DPI.
        private void LayoutRows()
        {
            float scale = CurrentAutoScaleDimensions.Height / designFontHeight;
            int hidden = 0;
            for (int i = 0; i < rows.Length; i++)
            {
                if (hiddenRows.Contains(rows[i]))
                {
                    hidden += (i + 1 < rows.Length ? rowTop[i + 1] : designClientHeight) - rowTop[i];
                    continue;
                }
                foreach (Control c in rows[i])
                    c.Top = (int)Math.Round((designTop[c] - hidden) * scale);
            }
            ClientSize = new Size(ClientSize.Width, (int)Math.Round((designClientHeight - hidden) * scale));
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_HOTKEY)
                checkboxShowOverlay.Checked = !checkboxShowOverlay.Checked;
            base.WndProc(ref m);
        }

        // While the Hotkey box has focus, every key press (incl. Tab, Alt+X, Ctrl+A) is captured here
        // before the TextBox or dialog-key handling can consume it.
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (!textHotkey.Focused)
                return base.ProcessCmdKey(ref msg, keyData);

            if (keyData is Keys.Back or Keys.Delete)
                settings.Hotkey = Keys.None;
            else if (AppSettings.IsValidHotkey(keyData))
                settings.Hotkey = keyData;
            else if (keyData != Keys.Escape)
                return true; // lone modifier or plain key: keep listening

            ActiveControl = null; // ends recording; Leave re-registers
            return true;
        }

        private void textHotkey_Enter(object sender, EventArgs e)
        {
            UnregisterHotKey(Handle, 1); // so pressing the current combo records it instead of toggling
            textHotkey.Text = "Press keys...";
        }

        private void textHotkey_Leave(object sender, EventArgs e)
        {
            ApplyHotkey();
        }

        private void ApplyHotkey()
        {
            Keys hotkey = settings.Hotkey;
            UnregisterHotKey(Handle, 1);
            uint mods = ((hotkey & Keys.Alt) != 0 ? MOD_ALT : 0) |
                        ((hotkey & Keys.Control) != 0 ? MOD_CONTROL : 0) |
                        ((hotkey & Keys.Shift) != 0 ? MOD_SHIFT : 0);
            bool ok = hotkey == Keys.None ||
                      RegisterHotKey(Handle, 1, mods | MOD_NOREPEAT, (uint)(hotkey & Keys.KeyCode));
            textHotkey.Text = hotkey == Keys.None ? "None" : new KeysConverter().ConvertToString(hotkey);
            checkboxShowOverlay.Text = ok ? "Show Overlay" : "Show Overlay (hotkey in use)";
        }

        private void reconnectTimer_Tick(object? sender, EventArgs e)
        {
            if (comboScreenMode.SelectedIndex == (int)ScreenMode.Application &&
                settings.AppProcessName != null &&
                overlay?.HasLiveTarget == false &&
                !comboApp.DroppedDown)
            {
                UpdateWindowsList();
            }
        }

        private void OnDisplaySettingsChanged(object? sender, EventArgs e)
        {
            if (comboScreenMode.SelectedIndex == (int)ScreenMode.Fullscreen)
                UpdateScreenList();
        }

        private void UpdateScreenList()
        {
            cachedScreens = Screen.AllScreens;
            comboMonitor.BeginUpdate();
            try
            {
                comboMonitor.Items.Clear();
                int desiredIndex = -1;
                for (int i = 0; i < cachedScreens.Length; i++)
                {
                    comboMonitor.Items.Add(cachedScreens[i].DeviceName);
                    if (settings.MonitorDeviceName != null && cachedScreens[i].DeviceName == settings.MonitorDeviceName)
                        desiredIndex = i;
                    else if (desiredIndex == -1 && Screen.PrimaryScreen != null && cachedScreens[i] == Screen.PrimaryScreen)
                        desiredIndex = i;
                }
                if (desiredIndex == -1 && cachedScreens.Length > 0)
                    desiredIndex = 0;
                if (desiredIndex >= 0)
                    comboMonitor.SelectedIndex = desiredIndex;
            }
            finally
            {
                comboMonitor.EndUpdate();
            }
        }

        private void UpdateWindowsList()
        {
            windows.Clear();

            // Build pid -> process name map once instead of per-window GetProcessById.
            Dictionary<int, string> pidToName = new();
            foreach (Process p in Process.GetProcesses())
            {
                try { pidToName[p.Id] = p.ProcessName; }
                catch { /* process may have exited */ }
                finally { p.Dispose(); }
            }

            StringBuilder sb = new(256);
            EnumWindows((hWnd, lParam) =>
            {
                if (!IsWindowVisible(hWnd))
                    return true;
                int len = GetWindowTextLength(hWnd);
                if (len <= 0)
                    return true;
                sb.Clear();
                if (sb.Capacity < len + 1)
                    sb.Capacity = len + 1;
                GetWindowText(hWnd, sb, sb.Capacity);
                string title = sb.ToString();
                if (string.IsNullOrWhiteSpace(title))
                    return true;
                GetWindowThreadProcessId(hWnd, out uint processId);
                if (!pidToName.TryGetValue((int)processId, out string? processName))
                    return true;
                windows.Add((hWnd, title, processName));
                return true;
            }, IntPtr.Zero);

            comboApp.BeginUpdate();
            try
            {
                comboApp.Items.Clear();
                // Prefer exact process + title; fall back to process only, since many games put FPS/map names in the title.
                int desiredIndex = -1, processMatchIndex = -1;
                for (int i = 0; i < windows.Count; i++)
                {
                    var (_, title, processName) = windows[i];
                    comboApp.Items.Add($"[{processName}.exe]{title}");
                    if (settings.AppProcessName != processName)
                        continue;
                    if (processMatchIndex == -1)
                        processMatchIndex = i;
                    if (desiredIndex == -1 && settings.AppWindowTitle == title)
                        desiredIndex = i;
                }
                if (desiredIndex == -1)
                    desiredIndex = processMatchIndex;
                if (desiredIndex >= 0)
                    comboApp.SelectedIndex = desiredIndex;
            }
            finally
            {
                comboApp.EndUpdate();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            settings = AppSettings.Load();

            // Populate enum combos.
            foreach (string mode in Enum.GetNames<ScreenMode>())
                comboScreenMode.Items.Add(mode);
            foreach (string type in Enum.GetNames<CrosshairType>())
                comboCrosshair.Items.Add(type);
            foreach (string type in Enum.GetNames<SecondaryCrosshairType>())
                comboSecondary.Items.Add(type);

            // Apply persisted settings to UI controls (events are no-ops while overlay is null).
            comboScreenMode.SelectedIndex = (int)settings.ScreenMode;
            checkboxHollow.Checked = settings.CrosshairHollow; // before the type, which reads it to enable controls
            comboCrosshair.SelectedIndex = (int)settings.CrosshairType;

            barOpacity.Value = settings.Opacity;
            updownOpacity.Value = settings.Opacity;

            barSize.Value = settings.CrosshairSize;
            updownSize.Value = settings.CrosshairSize;

            barLength.Value = settings.CrosshairLength;
            updownLength.Value = settings.CrosshairLength;

            barGap.Value = settings.CrosshairGap;
            updownGap.Value = settings.CrosshairGap;

            barSecondarySize.Value = settings.SecondaryCrosshairSize;
            updownSecondarySize.Value = settings.SecondaryCrosshairSize;

            comboSecondary.SelectedIndex = (int)settings.SecondaryCrosshairType;

            // The swatches hold the chosen colors; the one ColorDialog is shared by both.
            colorPickerBox.BackColor = Color.FromArgb(settings.CrosshairColorArgb);
            secondaryColorBox.BackColor = Color.FromArgb(settings.SecondaryCrosshairColorArgb);

            // comboScreenMode_SelectedIndexChanged already fired UpdateScreenList/UpdateWindowsList
            // depending on mode. If the mode matched the default index, force a refresh.
            if (settings.ScreenMode == ScreenMode.Fullscreen && comboMonitor.Items.Count == 0)
                UpdateScreenList();
            else if (settings.ScreenMode == ScreenMode.Application && comboApp.Items.Count == 0)
                UpdateWindowsList();

            // Now build the overlay with finalized values.
            overlay = new OverlayForm(settings);

            // Apply target.
            if (settings.ScreenMode == ScreenMode.Fullscreen)
            {
                if (comboMonitor.SelectedIndex >= 0)
                    overlay.SetTargetMonitor(cachedScreens[comboMonitor.SelectedIndex]);
            }
            else
            {
                overlay.SetTargetWindow(comboApp.SelectedIndex >= 0 ? windows[comboApp.SelectedIndex].hWnd : IntPtr.Zero);
            }

            overlay.Show();

            ApplyHotkey();
            reconnectTimer.Start();
        }

        private void OnFormClosing(object? sender, FormClosingEventArgs e)
        {
            CaptureSettings();
            settings.Save();
        }

        private void CaptureSettings()
        {
            settings.ScreenMode = (ScreenMode)Math.Max(0, comboScreenMode.SelectedIndex);
            settings.CrosshairType = (CrosshairType)Math.Max(0, comboCrosshair.SelectedIndex);
            settings.Opacity = barOpacity.Value;
            settings.CrosshairSize = barSize.Value;
            settings.CrosshairLength = barLength.Value;
            settings.CrosshairGap = barGap.Value;
            settings.CrosshairHollow = checkboxHollow.Checked;
            settings.SecondaryCrosshairSize = barSecondarySize.Value;
            settings.SecondaryCrosshairType = (SecondaryCrosshairType)Math.Max(0, comboSecondary.SelectedIndex);
            settings.UseSecondaryCrosshair = settings.SecondaryCrosshairType != SecondaryCrosshairType.None;
            settings.CrosshairColorArgb = colorPickerBox.BackColor.ToArgb();
            settings.SecondaryCrosshairColorArgb = secondaryColorBox.BackColor.ToArgb();
            // Monitor/app targets are kept current by their SelectedIndexChanged handlers.
        }

        private void comboScreenMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch ((ScreenMode)comboScreenMode.SelectedIndex)
            {
                case ScreenMode.Fullscreen:
                    labelMonitor.Visible = comboMonitor.Visible = true;
                    labelMonitor.Enabled = comboMonitor.Enabled = true;
                    labelApp.Visible = comboApp.Visible = false;
                    labelApp.Enabled = comboApp.Enabled = false;
                    UpdateScreenList();
                    break;
                case ScreenMode.Application:
                    labelMonitor.Visible = comboMonitor.Visible = false;
                    labelMonitor.Enabled = comboMonitor.Enabled = false;
                    labelApp.Visible = comboApp.Visible = true;
                    labelApp.Enabled = comboApp.Enabled = true;
                    overlay?.SetTargetWindow(IntPtr.Zero); // hide until a window is matched
                    UpdateWindowsList();
                    break;
            }
        }

        // Selection handlers write straight to settings so list refreshes re-select the current choice,
        // not the one loaded at startup.
        private void comboMonitor_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = comboMonitor.SelectedIndex;
            if (comboScreenMode.SelectedIndex == (int)ScreenMode.Fullscreen && i >= 0 && i < cachedScreens.Length)
            {
                settings.MonitorDeviceName = cachedScreens[i].DeviceName;
                overlay?.SetTargetMonitor(cachedScreens[i]);
            }
        }

        private void comboApp_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = comboApp.SelectedIndex;
            if (comboScreenMode.SelectedIndex == (int)ScreenMode.Application && i >= 0 && i < windows.Count)
            {
                var (hWnd, title, processName) = windows[i];
                settings.AppProcessName = processName;
                settings.AppWindowTitle = title;
                overlay?.SetTargetWindow(hWnd);
            }
        }

        private void comboApp_DropDown(object sender, EventArgs e)
        {
            UpdateWindowsList();
        }

        private void checkboxShowOverlay_CheckedChanged(object sender, EventArgs e)
        {
            overlay?.SetShown(checkboxShowOverlay.Checked);
        }

        private void barOpacity_ValueChanged(object sender, EventArgs e)
        {
            updownOpacity.Value = barOpacity.Value;
            if (overlay != null)
                overlay.Opacity = barOpacity.Value / 100.0;
        }

        private void updownOpacity_ValueChanged(object sender, EventArgs e)
        {
            barOpacity.Value = (int)updownOpacity.Value;
        }

        private void colorPickerBox_Click(object sender, EventArgs e)
        {
            if (PickColor(colorPickerBox))
                overlay?.SetCrosshairColor(colorPickerBox.BackColor);
        }

        private void secondaryColorBox_Click(object sender, EventArgs e)
        {
            if (PickColor(secondaryColorBox))
                overlay?.SetSecondaryColor(secondaryColorBox.BackColor);
        }

        private bool PickColor(PictureBox swatch)
        {
            colorDialog.Color = swatch.BackColor;
            if (colorDialog.ShowDialog() != DialogResult.OK)
                return false;
            swatch.BackColor = colorDialog.Color;
            return true;
        }

        private void barSize_ValueChanged(object sender, EventArgs e)
        {
            updownSize.Value = barSize.Value;
            overlay?.SetCrosshairSize(barSize.Value);
        }

        private void updownSize_ValueChanged(object sender, EventArgs e)
        {
            barSize.Value = (int)updownSize.Value;
        }

        private void comboCrosshair_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateVisibleControls();
            if (comboCrosshair.SelectedIndex >= 0)
                overlay?.SetCrosshairType((CrosshairType)comboCrosshair.SelectedIndex);
        }

        private void checkboxHollow_CheckedChanged(object sender, EventArgs e)
        {
            UpdateVisibleControls();
            overlay?.SetCrosshairHollow(checkboxHollow.Checked);
        }

        // Show only the controls the current shapes use.
        private void UpdateVisibleControls()
        {
            var type = (CrosshairType)comboCrosshair.SelectedIndex;
            bool lines = type is CrosshairType.Cross or CrosshairType.X;
            bool shape = type is CrosshairType.Circle or CrosshairType.Square or CrosshairType.Diamond;
            bool secondary = comboSecondary.SelectedIndex > (int)SecondaryCrosshairType.None;
            checkboxHollow.Visible = shape;
            secondaryColorBox.Visible = secondary;
            ShowRow(sizeRow, lines || shape);
            ShowRow(lengthRow, lines || (shape && checkboxHollow.Checked));
            ShowRow(gapRow, lines);
            ShowRow(secondarySizeRow, secondary);
            LayoutRows();
        }

        private void barGap_ValueChanged(object sender, EventArgs e)
        {
            updownGap.Value = barGap.Value;
            overlay?.SetCrosshairGap(barGap.Value);
        }

        private void updownGap_ValueChanged(object sender, EventArgs e)
        {
            barGap.Value = (int)updownGap.Value;
        }

        private void barLength_ValueChanged(object sender, EventArgs e)
        {
            updownLength.Value = barLength.Value;
            overlay?.SetCrosshairLength(barLength.Value);
        }

        private void updownLength_ValueChanged(object sender, EventArgs e)
        {
            barLength.Value = (int)updownLength.Value;
        }

        private void comboSecondary_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateVisibleControls();
            if (comboSecondary.SelectedIndex >= 0)
                overlay?.SetSecondaryCrosshairType((SecondaryCrosshairType)comboSecondary.SelectedIndex);
        }

        private void barSecondarySize_ValueChanged(object sender, EventArgs e)
        {
            updownSecondarySize.Value = barSecondarySize.Value;
            overlay?.SetSecondaryCrosshairSize(barSecondarySize.Value);
        }

        private void updownSecondarySize_ValueChanged(object sender, EventArgs e)
        {
            barSecondarySize.Value = (int)updownSecondarySize.Value;
        }
    }
}
