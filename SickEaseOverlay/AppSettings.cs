using System.Text.Json;

namespace SickEaseOverlay
{
    internal sealed class AppSettings
    {
        // Absent (0) in older files, which is how Migrate() spots them. Save() stamps the current version.
        public int SettingsVersion { get; set; }
        public ScreenMode ScreenMode { get; set; } = ScreenMode.Fullscreen;
        public string? MonitorDeviceName { get; set; }
        public string? AppProcessName { get; set; }
        public string? AppWindowTitle { get; set; }
        public int Opacity { get; set; } = 100;
        public int CrosshairColorArgb { get; set; } = unchecked((int)0xFFFF0000); // Red
        public int CrosshairSize { get; set; } = 3;
        public int CrosshairLength { get; set; } = 23; // Cross/X arm length, or hollow shape radius, px
        public int CrosshairGap { get; set; }          // empty center of Cross/X, px
        public bool CrosshairHollow { get; set; }      // outline instead of filled Circle/Square/Diamond
        public CrosshairType CrosshairType { get; set; } = CrosshairType.Cross;
        public bool UseSecondaryCrosshair { get; set; } = true; // kept for 1.0.x settings files; false means None
        public SecondaryCrosshairType SecondaryCrosshairType { get; set; } = SecondaryCrosshairType.Edges;
        public int SecondaryCrosshairSize { get; set; } = 6; // 5px steps
        public int SecondaryCrosshairColorArgb { get; set; } // 0 (absent) = same as the crosshair color
        public Keys Hotkey { get; set; } = DefaultHotkey;

        private const Keys DefaultHotkey = Keys.Control | Keys.Shift | Keys.F10;
        private const int CurrentSettingsVersion = 2;

        private static readonly string SettingsPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "SickEaseOverlay",
            "settings.json");

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            WriteIndented = true
        };

        public static AppSettings Load()
        {
            try
            {
                if (File.Exists(SettingsPath))
                {
                    string json = File.ReadAllText(SettingsPath);
                    AppSettings? loaded = JsonSerializer.Deserialize<AppSettings>(json);
                    if (loaded != null)
                        return loaded.Migrate().Clamp();
                }
            }
            catch
            {
                // Corrupted/unreadable settings — fall back to defaults.
            }
            return new AppSettings().Clamp();
        }

        public void Save()
        {
            try
            {
                string? dir = Path.GetDirectoryName(SettingsPath);
                if (!string.IsNullOrEmpty(dir))
                    Directory.CreateDirectory(dir);
                SettingsVersion = CurrentSettingsVersion;
                File.WriteAllText(SettingsPath, JsonSerializer.Serialize(this, JsonOptions));
            }
            catch
            {
                // Best-effort persistence; ignore write failures.
            }
        }

        // Converts older files so they draw exactly as before.
        private AppSettings Migrate()
        {
            if (SettingsVersion < 2)
            {
                CrosshairLength = 20 + CrosshairSize; // arm length used to be tied to thickness
                SecondaryCrosshairSize *= 2;          // steps went from 10px to 5px
            }
            return this;
        }

        private AppSettings Clamp()
        {
            Opacity = Math.Clamp(Opacity, 0, 100);
            CrosshairSize = Math.Clamp(CrosshairSize, 1, 10);
            CrosshairLength = Math.Clamp(CrosshairLength, 5, 200);
            CrosshairGap = Math.Clamp(CrosshairGap, 0, 100);
            if (SecondaryCrosshairColorArgb == 0)
                SecondaryCrosshairColorArgb = CrosshairColorArgb;
            SecondaryCrosshairSize = Math.Clamp(SecondaryCrosshairSize, 1, 20);
            if (!Enum.IsDefined(typeof(ScreenMode), ScreenMode))
                ScreenMode = ScreenMode.Fullscreen;
            if (!Enum.IsDefined(typeof(CrosshairType), CrosshairType))
                CrosshairType = CrosshairType.Cross;
            if (!Enum.IsDefined(typeof(SecondaryCrosshairType), SecondaryCrosshairType))
                SecondaryCrosshairType = SecondaryCrosshairType.Edges;
            if (!UseSecondaryCrosshair)
                SecondaryCrosshairType = SecondaryCrosshairType.None;
            if (Hotkey != Keys.None && !IsValidHotkey(Hotkey))
                Hotkey = DefaultHotkey;
            return this;
        }

        // Needs Ctrl or Alt (or an F-key) so a plain key like "A" can't hijack typing system-wide.
        internal static bool IsValidHotkey(Keys keys)
        {
            Keys key = keys & Keys.KeyCode;
            if (key is Keys.None or Keys.ShiftKey or Keys.ControlKey or Keys.Menu or Keys.LWin or Keys.RWin)
                return false;
            return (keys & (Keys.Control | Keys.Alt)) != 0 || key is >= Keys.F1 and <= Keys.F24;
        }
    }
}
