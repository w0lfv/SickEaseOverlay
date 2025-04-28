namespace SickEaseOverlay
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            barOpacity = new TrackBar();
            checkboxUseSecondary = new CheckBox();
            comboMonitor = new ComboBox();
            labelMonitor = new Label();
            updownOpacity = new NumericUpDown();
            labelOpacity = new Label();
            colorPickerBox = new PictureBox();
            labelColor = new Label();
            colorDialog = new ColorDialog();
            labelSize = new Label();
            updownSize = new NumericUpDown();
            barSize = new TrackBar();
            labelCrosshair = new Label();
            comboCrosshair = new ComboBox();
            labelSecondarySize = new Label();
            updownSecondarySize = new NumericUpDown();
            barSecondarySize = new TrackBar();
            labelScreenMode = new Label();
            comboScreenMode = new ComboBox();
            labelApp = new Label();
            comboApp = new ComboBox();
            ((System.ComponentModel.ISupportInitialize)barOpacity).BeginInit();
            ((System.ComponentModel.ISupportInitialize)updownOpacity).BeginInit();
            ((System.ComponentModel.ISupportInitialize)colorPickerBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)updownSize).BeginInit();
            ((System.ComponentModel.ISupportInitialize)barSize).BeginInit();
            ((System.ComponentModel.ISupportInitialize)updownSecondarySize).BeginInit();
            ((System.ComponentModel.ISupportInitialize)barSecondarySize).BeginInit();
            SuspendLayout();
            // 
            // barOpacity
            // 
            barOpacity.Location = new Point(47, 53);
            barOpacity.Margin = new Padding(0);
            barOpacity.Maximum = 100;
            barOpacity.Name = "barOpacity";
            barOpacity.Size = new Size(106, 45);
            barOpacity.TabIndex = 1;
            barOpacity.Value = 100;
            barOpacity.ValueChanged += barOpacity_ValueChanged;
            // 
            // checkboxUseSecondary
            // 
            checkboxUseSecondary.AutoSize = true;
            checkboxUseSecondary.Checked = true;
            checkboxUseSecondary.CheckState = CheckState.Checked;
            checkboxUseSecondary.Location = new Point(2, 157);
            checkboxUseSecondary.Name = "checkboxUseSecondary";
            checkboxUseSecondary.Size = new Size(158, 19);
            checkboxUseSecondary.TabIndex = 6;
            checkboxUseSecondary.Text = "Use Secondary Crosshair";
            checkboxUseSecondary.UseVisualStyleBackColor = true;
            checkboxUseSecondary.CheckedChanged += checkboxUseSecondary_CheckedChanged;
            // 
            // comboMonitor
            // 
            comboMonitor.BackColor = SystemColors.Control;
            comboMonitor.DropDownStyle = ComboBoxStyle.DropDownList;
            comboMonitor.FormattingEnabled = true;
            comboMonitor.Location = new Point(54, 28);
            comboMonitor.Name = "comboMonitor";
            comboMonitor.Size = new Size(142, 23);
            comboMonitor.TabIndex = 0;
            comboMonitor.SelectedIndexChanged += comboMonitor_SelectedIndexChanged;
            // 
            // labelMonitor
            // 
            labelMonitor.AutoSize = true;
            labelMonitor.Location = new Point(2, 32);
            labelMonitor.Name = "labelMonitor";
            labelMonitor.Size = new Size(50, 15);
            labelMonitor.TabIndex = 3;
            labelMonitor.Text = "Monitor";
            // 
            // updownOpacity
            // 
            updownOpacity.Location = new Point(150, 56);
            updownOpacity.Name = "updownOpacity";
            updownOpacity.Size = new Size(46, 23);
            updownOpacity.TabIndex = 2;
            updownOpacity.Value = new decimal(new int[] { 100, 0, 0, 0 });
            updownOpacity.ValueChanged += updownOpacity_ValueChanged;
            // 
            // labelOpacity
            // 
            labelOpacity.AutoSize = true;
            labelOpacity.Location = new Point(2, 58);
            labelOpacity.Name = "labelOpacity";
            labelOpacity.Size = new Size(48, 15);
            labelOpacity.TabIndex = 5;
            labelOpacity.Text = "Opacity";
            // 
            // colorPickerBox
            // 
            colorPickerBox.BackColor = Color.Red;
            colorPickerBox.BorderStyle = BorderStyle.FixedSingle;
            colorPickerBox.Location = new Point(54, 81);
            colorPickerBox.Name = "colorPickerBox";
            colorPickerBox.Size = new Size(142, 19);
            colorPickerBox.TabIndex = 6;
            colorPickerBox.TabStop = false;
            colorPickerBox.Click += colorPickerBox_Click;
            // 
            // labelColor
            // 
            labelColor.AutoSize = true;
            labelColor.Location = new Point(9, 83);
            labelColor.Name = "labelColor";
            labelColor.Size = new Size(36, 15);
            labelColor.TabIndex = 7;
            labelColor.Text = "Color";
            // 
            // colorDialog
            // 
            colorDialog.Color = Color.Red;
            // 
            // labelSize
            // 
            labelSize.AutoSize = true;
            labelSize.Location = new Point(12, 109);
            labelSize.Name = "labelSize";
            labelSize.Size = new Size(29, 15);
            labelSize.TabIndex = 10;
            labelSize.Text = "Size";
            // 
            // updownSize
            // 
            updownSize.Location = new Point(150, 106);
            updownSize.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            updownSize.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            updownSize.Name = "updownSize";
            updownSize.Size = new Size(46, 23);
            updownSize.TabIndex = 4;
            updownSize.Value = new decimal(new int[] { 3, 0, 0, 0 });
            updownSize.ValueChanged += updownSize_ValueChanged;
            // 
            // barSize
            // 
            barSize.Location = new Point(47, 103);
            barSize.Margin = new Padding(0);
            barSize.Minimum = 1;
            barSize.Name = "barSize";
            barSize.Size = new Size(106, 45);
            barSize.TabIndex = 3;
            barSize.Value = 3;
            barSize.ValueChanged += barSize_ValueChanged;
            // 
            // labelCrosshair
            // 
            labelCrosshair.AutoSize = true;
            labelCrosshair.Location = new Point(-1, 135);
            labelCrosshair.Name = "labelCrosshair";
            labelCrosshair.Size = new Size(56, 15);
            labelCrosshair.TabIndex = 12;
            labelCrosshair.Text = "Crosshair";
            // 
            // comboCrosshair
            // 
            comboCrosshair.BackColor = SystemColors.Control;
            comboCrosshair.DropDownStyle = ComboBoxStyle.DropDownList;
            comboCrosshair.FormattingEnabled = true;
            comboCrosshair.Location = new Point(55, 131);
            comboCrosshair.Name = "comboCrosshair";
            comboCrosshair.Size = new Size(142, 23);
            comboCrosshair.TabIndex = 5;
            comboCrosshair.SelectedIndexChanged += comboCrosshair_SelectedIndexChanged;
            // 
            // labelSecondarySize
            // 
            labelSecondarySize.AutoSize = true;
            labelSecondarySize.Location = new Point(11, 180);
            labelSecondarySize.Name = "labelSecondarySize";
            labelSecondarySize.Size = new Size(29, 15);
            labelSecondarySize.TabIndex = 15;
            labelSecondarySize.Text = "Size";
            // 
            // updownSecondarySize
            // 
            updownSecondarySize.Location = new Point(149, 177);
            updownSecondarySize.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            updownSecondarySize.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            updownSecondarySize.Name = "updownSecondarySize";
            updownSecondarySize.Size = new Size(46, 23);
            updownSecondarySize.TabIndex = 14;
            updownSecondarySize.Value = new decimal(new int[] { 3, 0, 0, 0 });
            updownSecondarySize.ValueChanged += updownSecondarySize_ValueChanged;
            // 
            // barSecondarySize
            // 
            barSecondarySize.Location = new Point(46, 174);
            barSecondarySize.Margin = new Padding(0);
            barSecondarySize.Minimum = 1;
            barSecondarySize.Name = "barSecondarySize";
            barSecondarySize.Size = new Size(106, 45);
            barSecondarySize.TabIndex = 13;
            barSecondarySize.Value = 3;
            barSecondarySize.ValueChanged += barSecondarySize_ValueChanged;
            // 
            // labelScreenMode
            // 
            labelScreenMode.AutoSize = true;
            labelScreenMode.Location = new Point(7, 6);
            labelScreenMode.Name = "labelScreenMode";
            labelScreenMode.Size = new Size(38, 15);
            labelScreenMode.TabIndex = 17;
            labelScreenMode.Text = "Mode";
            // 
            // comboScreenMode
            // 
            comboScreenMode.BackColor = SystemColors.Control;
            comboScreenMode.DropDownStyle = ComboBoxStyle.DropDownList;
            comboScreenMode.FormattingEnabled = true;
            comboScreenMode.Location = new Point(54, 2);
            comboScreenMode.Name = "comboScreenMode";
            comboScreenMode.Size = new Size(142, 23);
            comboScreenMode.TabIndex = 16;
            comboScreenMode.SelectedIndexChanged += comboScreenMode_SelectedIndexChanged;
            // 
            // labelApp
            // 
            labelApp.AutoSize = true;
            labelApp.Location = new Point(11, 32);
            labelApp.Name = "labelApp";
            labelApp.Size = new Size(29, 15);
            labelApp.TabIndex = 19;
            labelApp.Text = "App";
            // 
            // comboApp
            // 
            comboApp.BackColor = SystemColors.Control;
            comboApp.DropDownStyle = ComboBoxStyle.DropDownList;
            comboApp.DropDownWidth = 200;
            comboApp.FormattingEnabled = true;
            comboApp.Location = new Point(54, 28);
            comboApp.Name = "comboApp";
            comboApp.Size = new Size(142, 23);
            comboApp.TabIndex = 18;
            comboApp.SelectedIndexChanged += comboApp_SelectedIndexChanged;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(200, 203);
            Controls.Add(labelApp);
            Controls.Add(comboApp);
            Controls.Add(labelScreenMode);
            Controls.Add(comboScreenMode);
            Controls.Add(labelSecondarySize);
            Controls.Add(updownSecondarySize);
            Controls.Add(barSecondarySize);
            Controls.Add(labelCrosshair);
            Controls.Add(comboCrosshair);
            Controls.Add(labelSize);
            Controls.Add(updownSize);
            Controls.Add(barSize);
            Controls.Add(labelColor);
            Controls.Add(colorPickerBox);
            Controls.Add(labelOpacity);
            Controls.Add(updownOpacity);
            Controls.Add(labelMonitor);
            Controls.Add(comboMonitor);
            Controls.Add(checkboxUseSecondary);
            Controls.Add(barOpacity);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            MaximizeBox = false;
            Name = "MainForm";
            Opacity = 0.9D;
            Text = "SickEaseOverlay";
            Load += MainForm_Load;
            ((System.ComponentModel.ISupportInitialize)barOpacity).EndInit();
            ((System.ComponentModel.ISupportInitialize)updownOpacity).EndInit();
            ((System.ComponentModel.ISupportInitialize)colorPickerBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)updownSize).EndInit();
            ((System.ComponentModel.ISupportInitialize)barSize).EndInit();
            ((System.ComponentModel.ISupportInitialize)updownSecondarySize).EndInit();
            ((System.ComponentModel.ISupportInitialize)barSecondarySize).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TrackBar barOpacity;
        private CheckBox checkboxUseSecondary;
        private ComboBox comboMonitor;
        private Label labelMonitor;
        private NumericUpDown updownOpacity;
        private Label labelOpacity;
        private PictureBox colorPickerBox;
        private Label labelColor;
        private ColorDialog colorDialog;
        private Label labelSize;
        private NumericUpDown updownSize;
        private TrackBar barSize;
        private Label labelCrosshair;
        private ComboBox comboCrosshair;
        private Label labelSecondarySize;
        private NumericUpDown updownSecondarySize;
        private TrackBar barSecondarySize;
        private Label labelScreenMode;
        private ComboBox comboScreenMode;
        private Label labelApp;
        private ComboBox comboApp;
    }
}
