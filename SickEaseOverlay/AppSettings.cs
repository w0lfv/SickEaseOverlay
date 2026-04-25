using System.Text.Json;

namespace SickEaseOverlay
{
    internal sealed class AppSettings
    {
        public ScreenMode ScreenMode { get; set; } = ScreenMode.Fullscreen;
        public string? MonitorDeviceName { get; set; }
        public string? AppProcessName { get; set; }
        public string? AppWindowTitle { get; set; }
        public int Opacity { get; set; } = 100;
        public int CrosshairColorArgb { get; set; } = unchecked((int)0xFFFF0000); // Red
        public int CrosshairSize { get; set; } = 3;
        public CrosshairType CrosshairType { get; set; } = CrosshairType.Cross;
        public bool UseSecondaryCrosshair { get; set; } = true;
        public int SecondaryCrosshairSize { get; set; } = 3;

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
                        return loaded.Clamp();
                }
            }
            catch
            {
                // Corrupted/unreadable settings — fall back to defaults.
            }
            return new AppSettings();
        }

        public void Save()
        {
            try
            {
                string? dir = Path.GetDirectoryName(SettingsPath);
                if (!string.IsNullOrEmpty(dir))
                    Directory.CreateDirectory(dir);
                File.WriteAllText(SettingsPath, JsonSerializer.Serialize(this, JsonOptions));
            }
            catch
            {
                // Best-effort persistence; ignore write failures.
            }
        }

        private AppSettings Clamp()
        {
            Opacity = Math.Clamp(Opacity, 0, 100);
            CrosshairSize = Math.Clamp(CrosshairSize, 1, 10);
            SecondaryCrosshairSize = Math.Clamp(SecondaryCrosshairSize, 1, 10);
            if (!Enum.IsDefined(typeof(ScreenMode), ScreenMode))
                ScreenMode = ScreenMode.Fullscreen;
            if (!Enum.IsDefined(typeof(CrosshairType), CrosshairType))
                CrosshairType = CrosshairType.Cross;
            return this;
        }
    }
}
