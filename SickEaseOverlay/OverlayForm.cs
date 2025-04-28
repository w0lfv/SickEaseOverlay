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
            // Set the graphics object to the form's graphics context
            Graphics g = e.Graphics;

            // Get the center of the screen to draw the sight
            int centerX = ClientSize.Width / 2;
            int centerY = ClientSize.Height / 2;

            switch (crosshairType)
            {
                case CrosshairType.Cross:
                    {
                        int size = crossHairSize + (int)pen.Width;
                        Point p1 = new Point(centerX - size, centerY); // Left
                        Point p2 = new Point(centerX + size, centerY); // Right
                        Point p3 = new Point(centerX, centerY + size); // Down
                        Point p4 = new Point(centerX, centerY - size); // Up

                        g.DrawLine(pen, p1, p2); // The horizontal line of sight
                        g.DrawLine(pen, p3, p4); // The vertical line of sight
                    }
                    break;
                case CrosshairType.Square:
                    {
                        int size = 5 + (int)pen.Width * 2;
                        g.FillRectangle(pen.Brush, new Rectangle(centerX - size, centerY - size, size * 2, size * 2)); // The square of sight
                    }
                    break;
                case CrosshairType.Circle:
                    {
                        int size = 5 + (int)pen.Width * 2;
                        g.FillEllipse(pen.Brush, new Rectangle(centerX - size / 2, centerY - size / 2, size, size));
                    }
                    break;
            }

            if (useSecondaryCrosshair)
            {
                // Top
                g.FillRectangle(pen.Brush, 
                    new Rectangle(
                        centerX - secondaryCrosshairHeight * secondaryCrosshairSize * 5,
                        0,
                        secondaryCrosshairHeight * secondaryCrosshairSize * 10,
                        secondaryCrosshairWidth * secondaryCrosshairSize * 10
                    )
                );

                // Bottom
                g.FillRectangle(pen.Brush,
                    new Rectangle(
                        centerX - secondaryCrosshairHeight * secondaryCrosshairSize * 5,
                        ClientSize.Height - secondaryCrosshairWidth * secondaryCrosshairSize * 10,
                        secondaryCrosshairHeight * secondaryCrosshairSize * 10,
                        secondaryCrosshairWidth * secondaryCrosshairSize * 10
                    )
                );

                // Left
                g.FillRectangle(pen.Brush,
                    new Rectangle(
                        0,
                        centerY - secondaryCrosshairHeight * secondaryCrosshairSize * 5,
                        secondaryCrosshairWidth * secondaryCrosshairSize * 10,
                        secondaryCrosshairHeight * secondaryCrosshairSize * 10
                    )
                );

                // Right
                g.FillRectangle(pen.Brush,
                    new Rectangle(
                        ClientSize.Width - secondaryCrosshairWidth * secondaryCrosshairSize * 10,
                        centerY - secondaryCrosshairHeight * secondaryCrosshairSize * 5,
                        secondaryCrosshairWidth * secondaryCrosshairSize * 10,
                        secondaryCrosshairHeight * secondaryCrosshairSize * 10
                    )
                );
            }
        }

        private void applicationTimer_Tick(object sender, EventArgs e)
        {
            // Change Location and Size if the application window is moved or resized
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
            //Kill following the application window Thread and Set the form to cover the entire screen
            applicationTimer.Stop();

            Top = screen.Bounds.Top;
            Left = screen.Bounds.Left;
            ClientSize = Size = new Size(screen.Bounds.Width, screen.Bounds.Height);
            Invalidate();
        }

        internal void SetTargetWindow(IntPtr hWnd)
        {
            // Start Following the application window Thread and Set the form to cover the entire screen
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
    }
}
