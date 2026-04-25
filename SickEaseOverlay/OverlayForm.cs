using System.Runtime.InteropServices;

namespace SickEaseOverlay
{
    public partial class OverlayForm : Form
    {
        [DllImport("user32.dll")]
        private static extern int GetWindowLong(IntPtr hwnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hwnd, int nIndex, int dwNewLong);

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr hwnd, out RECT lpRect);

        [DllImport("user32.dll")]
        private static extern bool IsWindow(IntPtr hWnd);

        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        // Rendering
        private readonly Pen pen;
        private CrosshairType crosshairType;
        private bool useSecondaryCrosshair;
        private int secondaryCrosshairSize;
        private const int crossHairSize = 20;
        private const int secondaryCrosshairWidth = 3;
        private const int secondaryCrosshairHeight = 1;

        // Settings
        private IntPtr currentWindowHandle = IntPtr.Zero;

        internal OverlayForm(Color crosshairColor, int crosshairSize, CrosshairType crosshairType, bool useSecondaryCrosshair, int secondaryCrosshairSize)
        {
            pen = new Pen(crosshairColor, crosshairSize);
            this.crosshairType = crosshairType;
            this.useSecondaryCrosshair = useSecondaryCrosshair;
            this.secondaryCrosshairSize = secondaryCrosshairSize;
            InitializeComponent();
        }

        private void OverlayForm_Load(object sender, EventArgs e)
        {
            SetWindowLong(Handle, -20, GetWindowLong(Handle, -20) | 0x80000 | 0x20);
        }

        private void OverlayForm_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            int centerX = ClientSize.Width / 2;
            int centerY = ClientSize.Height / 2;

            switch (crosshairType)
            {
                case CrosshairType.Cross:
                    {
                        int size = crossHairSize + (int)pen.Width;
                        g.DrawLine(pen, centerX - size, centerY, centerX + size, centerY);
                        g.DrawLine(pen, centerX, centerY - size, centerX, centerY + size);
                    }
                    break;
                case CrosshairType.Square:
                    {
                        int size = 5 + (int)pen.Width * 2;
                        g.FillRectangle(pen.Brush, centerX - size, centerY - size, size * 2, size * 2);
                    }
                    break;
                case CrosshairType.Circle:
                    {
                        int size = 5 + (int)pen.Width * 2;
                        g.FillEllipse(pen.Brush, centerX - size / 2, centerY - size / 2, size, size);
                    }
                    break;
            }

            if (useSecondaryCrosshair)
            {
                int armW = secondaryCrosshairHeight * secondaryCrosshairSize * 10;
                int armL = secondaryCrosshairWidth * secondaryCrosshairSize * 10;

                // Top
                g.FillRectangle(pen.Brush, centerX - armW / 2, 0, armW, armL);
                // Bottom
                g.FillRectangle(pen.Brush, centerX - armW / 2, ClientSize.Height - armL, armW, armL);
                // Left
                g.FillRectangle(pen.Brush, 0, centerY - armW / 2, armL, armW);
                // Right
                g.FillRectangle(pen.Brush, ClientSize.Width - armL, centerY - armW / 2, armL, armW);
            }
        }

        private void applicationTimer_Tick(object sender, EventArgs e)
        {
            if (currentWindowHandle == IntPtr.Zero || !IsWindow(currentWindowHandle))
                return;

            if (GetWindowRect(currentWindowHandle, out RECT rect) &&
                (rect.Left != Location.X || rect.Top != Location.Y || Size.Width != rect.Right - rect.Left || Size.Height != rect.Bottom - rect.Top))
            {
                Location = new Point(rect.Left, rect.Top);
                ClientSize = Size = new Size(rect.Right - rect.Left, rect.Bottom - rect.Top);
                Invalidate();
            }
        }

        internal void SetTargetMonitor(Screen screen)
        {
            applicationTimer.Stop();
            currentWindowHandle = IntPtr.Zero;

            Top = screen.Bounds.Top;
            Left = screen.Bounds.Left;
            ClientSize = Size = new Size(screen.Bounds.Width, screen.Bounds.Height);
            Invalidate();
        }

        internal void SetTargetWindow(IntPtr hWnd)
        {
            currentWindowHandle = hWnd;
            applicationTimer.Start();
        }

        internal void SetCrosshairColor(Color color)
        {
            pen.Color = color;
            Invalidate();
        }

        internal void SetCrosshairSize(int size)
        {
            pen.Width = size;
            Invalidate();
        }

        internal void SetCrosshairType(CrosshairType type)
        {
            crosshairType = type;
            Invalidate();
        }

        internal void SetSecondaryCrosshair(bool useSecondaryCrosshair)
        {
            this.useSecondaryCrosshair = useSecondaryCrosshair;
            Invalidate();
        }

        internal void SetSecondaryCrosshairSize(int size)
        {
            secondaryCrosshairSize = size;
            Invalidate();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                pen?.Dispose();
                components?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
