using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32;

namespace SickEaseOverlay
{
    enum ScreenMode
    {
        Fullscreen,
        Application
    }

    enum CrosshairType
    {
        None,
        Cross,
        Circle,
        Square
    }

    public partial class MainForm : Form
    {
        private OverlayForm? overlay;

        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        // Monitor Settings
        private Screen[] cachedScreens = Screen.AllScreens;

        // Windows Settings
        private readonly List<(IntPtr hWnd, string title, string processName)> windows = new List<(IntPtr hWnd, string title, string processName)>();

        public MainForm()
        {
            InitializeComponent();
            SystemEvents.DisplaySettingsChanged += (s, e) => UpdateScreenList();
        }

        private void UpdateScreenList()
        {
            cachedScreens = Screen.AllScreens;
            comboMonitor.Items.Clear();
            for (int i = 0; i < cachedScreens.Length; i++)
            {
                comboMonitor.Items.Add(cachedScreens[i].DeviceName);
                if (Screen.PrimaryScreen != null && Screen.AllScreens[i] == Screen.PrimaryScreen)
                    comboMonitor.SelectedIndex = i;
            }
            if (Screen.PrimaryScreen == null)
                comboMonitor.SelectedIndex = 0;
        }

        private void UpdateWindowsList()
        {
            windows.Clear();

            EnumWindows((hWnd, lParam) =>
            {
                if (IsWindowVisible(hWnd))
                {
                    StringBuilder sb = new StringBuilder(256);
                    GetWindowText(hWnd, sb, sb.Capacity);
                    string title = sb.ToString();
                    if (!string.IsNullOrWhiteSpace(title))
                    {
                        GetWindowThreadProcessId(hWnd, out uint processId);
                        try
                        {
                            string processName = Process.GetProcessById((int)processId).ProcessName;
                            windows.Add((hWnd, title, processName));
                        }
                        catch { }
                    }
                }
                return true;
            }, IntPtr.Zero);
            comboApp.Items.Clear();
            foreach (var (hWnd, title, processName) in windows)
            {
                comboApp.Items.Add($"[{processName}.exe]{title}");
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            overlay = new OverlayForm(colorDialog.Color, barSize.Value, (CrosshairType)comboCrosshair.SelectedIndex, checkboxUseSecondary.Checked, barSecondarySize.Value);

            // Initialize Mode Combo box
            foreach (string mode in Enum.GetNames<ScreenMode>())
                comboScreenMode.Items.Add(mode.ToString());
            comboScreenMode.SelectedIndex = 0;

            // Initialize Monitor Combo box with available screens
            UpdateScreenList();

            // Initialize Windows List and fill Combo box with available Windows
            UpdateWindowsList();

            // Initialize Crosshair Combo box
            foreach (string type in Enum.GetNames<CrosshairType>())
                comboCrosshair.Items.Add(type.ToString());
            comboCrosshair.SelectedIndex = 1;

            // Display Overlay
            overlay.Show();
        }

        private void comboScreenMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Handle screen mode selection change
            switch ((ScreenMode)comboScreenMode.SelectedIndex)
            {
                case ScreenMode.Fullscreen:
                    labelMonitor.Enabled = true;
                    comboMonitor.Enabled = true;
                    labelMonitor.Visible = true;
                    comboMonitor.Visible = true;
                    labelApp.Enabled = false;
                    comboApp.Enabled = false;
                    labelApp.Visible = false;
                    comboApp.Visible = false;
                    UpdateScreenList();
                    break;
                case ScreenMode.Application:
                    labelMonitor.Enabled = false;
                    comboMonitor.Enabled = false;
                    labelMonitor.Visible = false;
                    comboMonitor.Visible = false;
                    labelApp.Enabled = true;
                    comboApp.Enabled = true;
                    labelApp.Visible = true;
                    comboApp.Visible = true;
                    UpdateWindowsList();
                    break;
            }
        }

        private void comboMonitor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboScreenMode.SelectedIndex == (int)ScreenMode.Fullscreen && comboMonitor.SelectedIndex != -1)
                overlay?.SetTargetMonitor(cachedScreens[comboMonitor.SelectedIndex]);
        }

        private void comboApp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboScreenMode.SelectedIndex == (int)ScreenMode.Application)
                overlay?.SetTargetWindow(windows[comboApp.SelectedIndex].hWnd);
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
