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
        private static extern bool GetClientRect(IntPtr hwnd, out RECT lpRect);

        [DllImport("user32.dll")]
        private static extern bool ClientToScreen(IntPtr hwnd, ref Point lpPoint);

        [DllImport("user32.dll")]
        private static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("user32.dll")]
        private static extern uint GetDpiForWindow(IntPtr hwnd);

        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        // Rendering
        private readonly Pen pen;
        private readonly SolidBrush secondaryBrush;
        private int crosshairSize;
        private int crosshairLength;
        private int crosshairGap;
        private bool crosshairHollow;
        private CrosshairType crosshairType;
        private SecondaryCrosshairType secondaryCrosshairType;
        private int secondaryCrosshairSize;
        private const int secondaryStep = 5; // px per secondary size step: bars are 1 step thick, 3 steps long
        private const int secondaryCrosshairWidth = 3;
        private const int secondaryCrosshairHeight = 1;

        // Visibility
        private bool shown = true;        // user toggle (checkbox / hotkey)
        private bool targetActive = true; // false while the target app is closed, minimized or in the background

        // Settings
        private IntPtr currentWindowHandle = IntPtr.Zero;

        internal OverlayForm(AppSettings settings)
        {
            InitializeComponent();
            pen = new Pen(AvoidTransparencyKey(Color.FromArgb(settings.CrosshairColorArgb)));
            secondaryBrush = new SolidBrush(AvoidTransparencyKey(Color.FromArgb(settings.SecondaryCrosshairColorArgb)));
            crosshairSize = settings.CrosshairSize;
            crosshairLength = settings.CrosshairLength;
            crosshairGap = settings.CrosshairGap;
            crosshairHollow = settings.CrosshairHollow;
            crosshairType = settings.CrosshairType;
            secondaryCrosshairType = settings.SecondaryCrosshairType;
            secondaryCrosshairSize = settings.SecondaryCrosshairSize;
            Opacity = settings.Opacity / 100.0;
        }

        internal bool HasLiveTarget => IsWindow(currentWindowHandle);

        private void OverlayForm_Load(object sender, EventArgs e)
        {
            SetWindowLong(Handle, -20, GetWindowLong(Handle, -20) | 0x80000 | 0x20);
        }

        // Bounds are always set explicitly in physical pixels; don't let WinForms rescale the
        // overlay when it lands on a monitor with a different DPI.
        protected override void OnDpiChanged(DpiChangedEventArgs e)
        {
            e.Cancel = true;
            base.OnDpiChanged(e);
            Invalidate(); // redraw at the new scale
        }

        private void OverlayForm_Paint(object sender, PaintEventArgs e)
        {
            if (!shown || !targetActive)
                return;

            // Sizes are in 96-DPI units and scaled to this monitor's DPI, so the crosshair keeps the
            // same apparent size at 100%, 150% or 200% display scaling.
            float scale = GetDpiForWindow(Handle) / 96f;
            int Scaled(float value) => (int)Math.Round(value * scale);

            Graphics g = e.Graphics;
            int centerX = ClientSize.Width / 2;
            int centerY = ClientSize.Height / 2;
            pen.Width = Scaled(crosshairSize);

            switch (crosshairType)
            {
                case CrosshairType.Cross:
                case CrosshairType.X:
                    {
                        // Four arms from the gap out to the length; X arms run diagonally.
                        int len = Scaled(crosshairLength), gap = Scaled(crosshairGap);
                        if (gap >= len)
                            break;
                        float d = crosshairType == CrosshairType.X ? 0.7071f : 1f;
                        (float dx, float dy)[] arms = crosshairType == CrosshairType.X
                            ? new[] { (d, d), (d, -d), (-d, d), (-d, -d) }
                            : new[] { (d, 0f), (-d, 0f), (0f, d), (0f, -d) };
                        foreach (var (dx, dy) in arms)
                            g.DrawLine(pen, centerX + dx * gap, centerY + dy * gap, centerX + dx * len, centerY + dy * len);
                    }
                    break;
                case CrosshairType.Circle:
                case CrosshairType.Square:
                case CrosshairType.Diamond:
                    {
                        // Filled shapes grow with Size; hollow ones are a Size-thick outline at Length radius.
                        int r = crosshairHollow ? Scaled(crosshairLength) : Scaled(5 + crosshairSize * 2);
                        Rectangle box = new(centerX - r, centerY - r, r * 2, r * 2);
                        Point[] diamond = { new(centerX, centerY - r), new(centerX + r, centerY), new(centerX, centerY + r), new(centerX - r, centerY) };
                        if (crosshairType == CrosshairType.Circle)
                        {
                            if (crosshairHollow) g.DrawEllipse(pen, box); else g.FillEllipse(pen.Brush, box);
                        }
                        else if (crosshairType == CrosshairType.Square)
                        {
                            if (crosshairHollow) g.DrawRectangle(pen, box); else g.FillRectangle(pen.Brush, box);
                        }
                        else
                        {
                            if (crosshairHollow) g.DrawPolygon(pen, diamond); else g.FillPolygon(pen.Brush, diamond);
                        }
                    }
                    break;
            }

            int w = ClientSize.Width, h = ClientSize.Height;
            int armW = Scaled(secondaryCrosshairHeight * secondaryCrosshairSize * secondaryStep);
            int armL = Scaled(secondaryCrosshairWidth * secondaryCrosshairSize * secondaryStep);
            switch (secondaryCrosshairType)
            {
                case SecondaryCrosshairType.Edges:
                    g.FillRectangles(secondaryBrush, new Rectangle[]
                    {
                        new(centerX - armW / 2, 0, armW, armL),        // top
                        new(centerX - armW / 2, h - armL, armW, armL), // bottom
                        new(0, centerY - armW / 2, armL, armW),        // left
                        new(w - armL, centerY - armW / 2, armL, armW), // right
                    });
                    break;
                case SecondaryCrosshairType.Corners:
                    g.FillRectangles(secondaryBrush, new Rectangle[]
                    {
                        new(0, 0, armL, armW), new(0, 0, armW, armL),                               // top-left
                        new(w - armL, 0, armL, armW), new(w - armW, 0, armW, armL),                 // top-right
                        new(0, h - armW, armL, armW), new(0, h - armL, armW, armL),                 // bottom-left
                        new(w - armL, h - armW, armL, armW), new(w - armW, h - armL, armW, armL),   // bottom-right
                    });
                    break;
                case SecondaryCrosshairType.Border:
                    {
                        int t = Scaled(secondaryCrosshairSize * 1.5f); // thinner than bars: it runs the full edge
                        g.FillRectangles(secondaryBrush, new Rectangle[]
                        {
                            new(0, 0, w, t), new(0, h - t, w, t), new(0, 0, t, h), new(w - t, 0, t, h),
                        });
                    }
                    break;
                case SecondaryCrosshairType.Dots:
                    {
                        // Dots Size x 20px apart, offset half a step from the center so none sits on the crosshair.
                        int spacing = Scaled(secondaryCrosshairSize * 20);
                        int d = Math.Max(2, Scaled(2 + secondaryCrosshairSize / 3f));
                        int[] ys = Offsets(centerY, h, spacing).ToArray();
                        foreach (int x in Offsets(centerX, w, spacing))
                            foreach (int y in ys)
                                g.FillEllipse(secondaryBrush, x - d / 2, y - d / 2, d, d);
                    }
                    break;
            }
        }

        // Positions center ± (k + 0.5) * spacing that fall within [0, extent).
        private static IEnumerable<int> Offsets(int center, int extent, int spacing)
        {
            for (int p = center - spacing / 2; p >= 0; p -= spacing) yield return p;
            for (int p = center + spacing / 2; p < extent; p += spacing) yield return p;
        }

        private void applicationTimer_Tick(object sender, EventArgs e)
        {
            RECT rect = default;
            bool active = IsWindow(currentWindowHandle) &&
                          IsTargetForeground() &&
                          GetClientRect(currentWindowHandle, out rect) &&
                          rect.Right > 0 && rect.Bottom > 0; // empty while minimized

            if (active)
            {
                // Follow the client area, not the window rect, so the title bar and borders don't shift the center.
                Point origin = Point.Empty;
                ClientToScreen(currentWindowHandle, ref origin);
                Rectangle bounds = new(origin.X, origin.Y, rect.Right, rect.Bottom);
                if (Bounds != bounds)
                {
                    Bounds = bounds;
                    Invalidate();
                }
            }

            if (active != targetActive)
            {
                targetActive = active;
                Invalidate();
            }
        }

        // Show only while the target app, or this app (so settings can be tuned live), has focus.
        private bool IsTargetForeground()
        {
            IntPtr fg = GetForegroundWindow();
            if (fg == IntPtr.Zero)
                return targetActive; // focus is mid-switch; keep the current state to avoid flicker
            GetWindowThreadProcessId(fg, out uint fgPid);
            GetWindowThreadProcessId(currentWindowHandle, out uint targetPid);
            return fgPid == targetPid || fgPid == Environment.ProcessId;
        }

        // A crosshair painted in the exact TransparencyKey color would be invisible; nudge it one step.
        private Color AvoidTransparencyKey(Color c) =>
            c.R == TransparencyKey.R && c.G == TransparencyKey.G && c.B == TransparencyKey.B
                ? Color.FromArgb(c.A, c.R, c.G, c.B ^ 1)
                : c;

        internal void SetTargetMonitor(Screen screen)
        {
            applicationTimer.Stop();
            currentWindowHandle = IntPtr.Zero;
            targetActive = true;
            Bounds = screen.Bounds;
            Invalidate();
        }

        // IntPtr.Zero hides the overlay until a window is chosen.
        internal void SetTargetWindow(IntPtr hWnd)
        {
            currentWindowHandle = hWnd;
            targetActive = false; // hidden until the first tick positions it
            Invalidate();
            applicationTimer.Start();
        }

        internal void SetShown(bool value)
        {
            shown = value;
            Invalidate();
        }

        internal void SetCrosshairColor(Color color)
        {
            pen.Color = AvoidTransparencyKey(color);
            Invalidate();
        }

        internal void SetCrosshairSize(int size)
        {
            crosshairSize = size;
            Invalidate();
        }

        internal void SetCrosshairLength(int length)
        {
            crosshairLength = length;
            Invalidate();
        }

        internal void SetCrosshairGap(int gap)
        {
            crosshairGap = gap;
            Invalidate();
        }

        internal void SetCrosshairHollow(bool hollow)
        {
            crosshairHollow = hollow;
            Invalidate();
        }

        internal void SetSecondaryColor(Color color)
        {
            secondaryBrush.Color = AvoidTransparencyKey(color);
            Invalidate();
        }

        internal void SetCrosshairType(CrosshairType type)
        {
            crosshairType = type;
            Invalidate();
        }

        internal void SetSecondaryCrosshairType(SecondaryCrosshairType type)
        {
            secondaryCrosshairType = type;
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
                secondaryBrush?.Dispose();
                components?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
