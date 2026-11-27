namespace SickEaseOverlay
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            // A second instance would stack another overlay and race on settings.json.
            using Mutex? instanceLock = TryAcquireInstanceLock();
            if (instanceLock == null)
            {
                MessageBox.Show("SickEaseOverlay is already running.", "SickEaseOverlay",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Application.Run(new MainForm());
        }

        private static Mutex? TryAcquireInstanceLock()
        {
            try
            {
                Mutex mutex = new(true, "SickEaseOverlay.SingleInstance", out bool createdNew);
                if (createdNew)
                    return mutex;
                mutex.Dispose();
            }
            catch (UnauthorizedAccessException)
            {
                // Held by an instance running as administrator.
            }
            return null;
        }
    }
}
