namespace SickEaseOverlay
{
    partial class OverlayForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            applicationTimer = new System.Windows.Forms.Timer(components);
            SuspendLayout();
            // 
            // applicationTimer
            // 
            applicationTimer.Interval = 16;
            applicationTimer.Tick += applicationTimer_Tick;
            // 
            // OverlayForm
            // 
            BackColor = Color.Wheat;
            ClientSize = new Size(10, 10);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.None;
            Name = "OverlayForm";
            ShowInTaskbar = false;
            TopMost = true;
            TransparencyKey = Color.Wheat;
            Load += OverlayForm_Load;
            Paint += OverlayForm_Paint;
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Timer applicationTimer;
    }
}