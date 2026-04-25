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

    public enum CrosshairType
    {
        None,
        Cross,
        Circle,
        Square
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

        // Monitor Settings
        private Screen[] cachedScreens = Screen.AllScreens;

        // Windows Settings
        private readonly List<(IntPtr hWnd, string title, string processName)> windows = new();

        public MainForm()
        {
            InitializeComponent();
            SystemEvents.DisplaySettingsChanged += OnDisplaySettingsChanged;
            FormClosing += OnFormClosing;
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
                int desiredIndex = -1;
                for (int i = 0; i < windows.Count; i++)
                {
                    var (_, title, processName) = windows[i];
                    comboApp.Items.Add($"[{processName}.exe]{title}");
                    if (desiredIndex == -1 &&
                        settings.AppProcessName == processName &&
                        settings.AppWindowTitle == title)
                    {
                        desiredIndex = i;
                    }
                }
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

            // Apply persisted settings to UI controls (events are no-ops while overlay is null).
            comboScreenMode.SelectedIndex = (int)settings.ScreenMode;
            comboCrosshair.SelectedIndex = (int)settings.CrosshairType;

            barOpacity.Value = settings.Opacity;
            updownOpacity.Value = settings.Opacity;

            barSize.Value = settings.CrosshairSize;
            updownSize.Value = settings.CrosshairSize;

            barSecondarySize.Value = settings.SecondaryCrosshairSize;
            updownSecondarySize.Value = settings.SecondaryCrosshairSize;

            checkboxUseSecondary.Checked = settings.UseSecondaryCrosshair;

            Color crosshairColor = Color.FromArgb(settings.CrosshairColorArgb);
            colorDialog.Color = crosshairColor;
            colorPickerBox.BackColor = crosshairColor;

            // comboScreenMode_SelectedIndexChanged already fired UpdateScreenList/UpdateWindowsList
            // depending on mode. If the mode matched the default index, force a refresh.
            if (settings.ScreenMode == ScreenMode.Fullscreen && comboMonitor.Items.Count == 0)
                UpdateScreenList();
            else if (settings.ScreenMode == ScreenMode.Application && comboApp.Items.Count == 0)
                UpdateWindowsList();

            // Now build the overlay with finalized values.
            overlay = new OverlayForm(
                crosshairColor,
                settings.CrosshairSize,
                settings.CrosshairType,
                settings.UseSecondaryCrosshair,
                settings.SecondaryCrosshairSize);
            overlay.Opacity = settings.Opacity / 100.0;

            // Apply target.
            if (settings.ScreenMode == ScreenMode.Fullscreen)
            {
                if (comboMonitor.SelectedIndex >= 0)
                    overlay.SetTargetMonitor(cachedScreens[comboMonitor.SelectedIndex]);
            }
            else
            {
                if (comboApp.SelectedIndex >= 0)
                    overlay.SetTargetWindow(windows[comboApp.SelectedIndex].hWnd);
            }

            overlay.Show();
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
            settings.SecondaryCrosshairSize = barSecondarySize.Value;
            settings.UseSecondaryCrosshair = checkboxUseSecondary.Checked;
            settings.CrosshairColorArgb = colorDialog.Color.ToArgb();

            settings.MonitorDeviceName = (comboMonitor.SelectedIndex >= 0 && comboMonitor.SelectedIndex < cachedScreens.Length)
                ? cachedScreens[comboMonitor.SelectedIndex].DeviceName
                : null;

            if (comboApp.SelectedIndex >= 0 && comboApp.SelectedIndex < windows.Count)
            {
                var (_, title, processName) = windows[comboApp.SelectedIndex];
                settings.AppProcessName = processName;
                settings.AppWindowTitle = title;
            }
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
                    UpdateWindowsList();
                    break;
            }
        }

        private void comboMonitor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboScreenMode.SelectedIndex == (int)ScreenMode.Fullscreen &&
                comboMonitor.SelectedIndex >= 0 &&
                comboMonitor.SelectedIndex < cachedScreens.Length)
            {
                overlay?.SetTargetMonitor(cachedScreens[comboMonitor.SelectedIndex]);
            }
        }

        private void comboApp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboScreenMode.SelectedIndex == (int)ScreenMode.Application &&
                comboApp.SelectedIndex >= 0 &&
                comboApp.SelectedIndex < windows.Count)
            {
                overlay?.SetTargetWindow(windows[comboApp.SelectedIndex].hWnd);
            }
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
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                colorPickerBox.BackColor = colorDialog.Color;
                overlay?.SetCrosshairColor(colorDialog.Color);
            }
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
            if (comboCrosshair.SelectedIndex >= 0)
                overlay?.SetCrosshairType((CrosshairType)comboCrosshair.SelectedIndex);
        }

        private void checkboxUseSecondary_CheckedChanged(object sender, EventArgs e)
        {
            overlay?.SetSecondaryCrosshair(checkboxUseSecondary.Checked);
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
