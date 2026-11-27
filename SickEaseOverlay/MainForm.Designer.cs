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
            components = new System.ComponentModel.Container();
            toolTip = new ToolTip(components);
            barOpacity = new TrackBar();
            labelSecondary = new Label();
            comboSecondary = new ComboBox();
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
            checkboxShowOverlay = new CheckBox();
            labelHotkey = new Label();
            textHotkey = new TextBox();
            labelLength = new Label();
            updownLength = new NumericUpDown();
            barLength = new TrackBar();
            checkboxHollow = new CheckBox();
            labelGap = new Label();
            updownGap = new NumericUpDown();
            barGap = new TrackBar();
            secondaryColorBox = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)barOpacity).BeginInit();
            ((System.ComponentModel.ISupportInitialize)updownOpacity).BeginInit();
            ((System.ComponentModel.ISupportInitialize)colorPickerBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)updownSize).BeginInit();
            ((System.ComponentModel.ISupportInitialize)barSize).BeginInit();
            ((System.ComponentModel.ISupportInitialize)updownSecondarySize).BeginInit();
            ((System.ComponentModel.ISupportInitialize)barSecondarySize).BeginInit();
            ((System.ComponentModel.ISupportInitialize)updownLength).BeginInit();
            ((System.ComponentModel.ISupportInitialize)barLength).BeginInit();
            ((System.ComponentModel.ISupportInitialize)updownGap).BeginInit();
            ((System.ComponentModel.ISupportInitialize)barGap).BeginInit();
            ((System.ComponentModel.ISupportInitialize)secondaryColorBox).BeginInit();
            SuspendLayout();
            // 
            // barOpacity
            // 
            barOpacity.Location = new Point(53, 53);
            barOpacity.Margin = new Padding(0);
            barOpacity.Maximum = 100;
            barOpacity.Name = "barOpacity";
            barOpacity.Size = new Size(106, 45);
            barOpacity.TabIndex = 1;
            barOpacity.Value = 100;
            barOpacity.ValueChanged += barOpacity_ValueChanged;
            // 
            // labelSecondary
            // 
            labelSecondary.AutoSize = true;
            labelSecondary.Location = new Point(-1, 210);
            labelSecondary.Name = "labelSecondary";
            labelSecondary.Size = new Size(62, 15);
            labelSecondary.TabIndex = 23;
            labelSecondary.Text = "Secondary";
            // 
            // comboSecondary
            // 
            comboSecondary.BackColor = SystemColors.Control;
            comboSecondary.DropDownStyle = ComboBoxStyle.DropDownList;
            comboSecondary.FormattingEnabled = true;
            comboSecondary.Location = new Point(61, 206);
            comboSecondary.Name = "comboSecondary";
            comboSecondary.Size = new Size(110, 23);
            comboSecondary.TabIndex = 6;
            comboSecondary.SelectedIndexChanged += comboSecondary_SelectedIndexChanged;
            // 
            // comboMonitor
            // 
            comboMonitor.BackColor = SystemColors.Control;
            comboMonitor.DropDownStyle = ComboBoxStyle.DropDownList;
            comboMonitor.FormattingEnabled = true;
            comboMonitor.Location = new Point(60, 28);
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
            updownOpacity.Location = new Point(156, 56);
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
            colorPickerBox.Location = new Point(60, 81);
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
            labelSize.Location = new Point(12, 133);
            labelSize.Name = "labelSize";
            labelSize.Size = new Size(29, 15);
            labelSize.TabIndex = 10;
            labelSize.Text = "Size";
            // 
            // updownSize
            // 
            updownSize.Location = new Point(156, 130);
            updownSize.Maximum = new decimal(new int[] { 10, 0, 0, 0 });
            updownSize.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            updownSize.Name = "updownSize";
            updownSize.Size = new Size(46, 23);
            updownSize.TabIndex = 5;
            updownSize.Value = new decimal(new int[] { 3, 0, 0, 0 });
            updownSize.ValueChanged += updownSize_ValueChanged;
            // 
            // barSize
            // 
            barSize.Location = new Point(53, 127);
            barSize.Margin = new Padding(0);
            barSize.Minimum = 1;
            barSize.Name = "barSize";
            barSize.Size = new Size(106, 45);
            barSize.TabIndex = 4;
            barSize.Value = 3;
            barSize.ValueChanged += barSize_ValueChanged;
            // 
            // labelCrosshair
            // 
            labelCrosshair.AutoSize = true;
            labelCrosshair.Location = new Point(-1, 109);
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
            comboCrosshair.Location = new Point(61, 105);
            comboCrosshair.Name = "comboCrosshair";
            comboCrosshair.Size = new Size(76, 23);
            comboCrosshair.TabIndex = 3;
            comboCrosshair.SelectedIndexChanged += comboCrosshair_SelectedIndexChanged;
            // 
            // labelSecondarySize
            // 
            labelSecondarySize.AutoSize = true;
            labelSecondarySize.Location = new Point(11, 234);
            labelSecondarySize.Name = "labelSecondarySize";
            labelSecondarySize.Size = new Size(29, 15);
            labelSecondarySize.TabIndex = 15;
            labelSecondarySize.Text = "Size";
            // 
            // updownSecondarySize
            // 
            updownSecondarySize.Location = new Point(155, 231);
            updownSecondarySize.Maximum = new decimal(new int[] { 20, 0, 0, 0 });
            updownSecondarySize.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            updownSecondarySize.Name = "updownSecondarySize";
            updownSecondarySize.Size = new Size(46, 23);
            updownSecondarySize.TabIndex = 14;
            updownSecondarySize.Value = new decimal(new int[] { 6, 0, 0, 0 });
            updownSecondarySize.ValueChanged += updownSecondarySize_ValueChanged;
            // 
            // barSecondarySize
            // 
            barSecondarySize.Location = new Point(52, 228);
            barSecondarySize.Margin = new Padding(0);
            barSecondarySize.Maximum = 20;
            barSecondarySize.Minimum = 1;
            barSecondarySize.Name = "barSecondarySize";
            barSecondarySize.Size = new Size(106, 45);
            barSecondarySize.TabIndex = 13;
            barSecondarySize.Value = 6;
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
            comboScreenMode.Location = new Point(60, 2);
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
            comboApp.Location = new Point(60, 28);
            comboApp.Name = "comboApp";
            comboApp.Size = new Size(142, 23);
            comboApp.TabIndex = 18;
            comboApp.DropDown += comboApp_DropDown;
            comboApp.SelectedIndexChanged += comboApp_SelectedIndexChanged;
            // 
            // checkboxShowOverlay
            // 
            checkboxShowOverlay.AutoSize = true;
            checkboxShowOverlay.Checked = true;
            checkboxShowOverlay.CheckState = CheckState.Checked;
            checkboxShowOverlay.Location = new Point(2, 261);
            checkboxShowOverlay.Name = "checkboxShowOverlay";
            checkboxShowOverlay.Size = new Size(96, 19);
            checkboxShowOverlay.TabIndex = 20;
            checkboxShowOverlay.Text = "Show Overlay";
            checkboxShowOverlay.UseVisualStyleBackColor = true;
            checkboxShowOverlay.CheckedChanged += checkboxShowOverlay_CheckedChanged;
            // 
            // labelHotkey
            // 
            labelHotkey.AutoSize = true;
            labelHotkey.Location = new Point(4, 288);
            labelHotkey.Name = "labelHotkey";
            labelHotkey.Size = new Size(45, 15);
            labelHotkey.TabIndex = 22;
            labelHotkey.Text = "Hotkey";
            // 
            // textHotkey
            // 
            textHotkey.BackColor = SystemColors.Window;
            textHotkey.Location = new Point(60, 284);
            textHotkey.Name = "textHotkey";
            textHotkey.ReadOnly = true;
            textHotkey.Size = new Size(142, 23);
            textHotkey.TabIndex = 21;
            toolTip.SetToolTip(textHotkey, "Click, then press a key combination (with Ctrl or Alt, or an F-key).\r\nBackspace/Delete: no hotkey    Esc: cancel");
            textHotkey.Enter += textHotkey_Enter;
            textHotkey.Leave += textHotkey_Leave;
            // 
            // labelLength
            // 
            labelLength.AutoSize = true;
            labelLength.Location = new Point(5, 158);
            labelLength.Name = "labelLength";
            labelLength.Size = new Size(44, 15);
            labelLength.TabIndex = 26;
            labelLength.Text = "Length";
            // 
            // updownLength
            // 
            updownLength.Location = new Point(156, 155);
            updownLength.Maximum = new decimal(new int[] { 200, 0, 0, 0 });
            updownLength.Minimum = new decimal(new int[] { 5, 0, 0, 0 });
            updownLength.Name = "updownLength";
            updownLength.Size = new Size(46, 23);
            updownLength.TabIndex = 25;
            updownLength.Value = new decimal(new int[] { 23, 0, 0, 0 });
            updownLength.ValueChanged += updownLength_ValueChanged;
            // 
            // barLength
            // 
            barLength.LargeChange = 10;
            barLength.Location = new Point(53, 152);
            barLength.Margin = new Padding(0);
            barLength.Maximum = 200;
            barLength.Minimum = 5;
            barLength.Name = "barLength";
            barLength.Size = new Size(106, 45);
            barLength.TabIndex = 24;
            barLength.TickFrequency = 25;
            barLength.Value = 23;
            barLength.ValueChanged += barLength_ValueChanged;
            // 
            // checkboxHollow
            // 
            checkboxHollow.AutoSize = true;
            checkboxHollow.Location = new Point(141, 107);
            checkboxHollow.Name = "checkboxHollow";
            checkboxHollow.Size = new Size(62, 19);
            checkboxHollow.TabIndex = 27;
            checkboxHollow.Text = "Hollow";
            checkboxHollow.UseVisualStyleBackColor = true;
            checkboxHollow.CheckedChanged += checkboxHollow_CheckedChanged;
            toolTip.SetToolTip(checkboxHollow, "Outline instead of filled (Circle, Square, Diamond).\r\nLength sets the radius, Size the line thickness.");
            // 
            // labelGap
            // 
            labelGap.AutoSize = true;
            labelGap.Location = new Point(14, 183);
            labelGap.Name = "labelGap";
            labelGap.Size = new Size(28, 15);
            labelGap.TabIndex = 30;
            labelGap.Text = "Gap";
            // 
            // updownGap
            // 
            updownGap.Location = new Point(156, 180);
            updownGap.Maximum = new decimal(new int[] { 100, 0, 0, 0 });
            updownGap.Name = "updownGap";
            updownGap.Size = new Size(46, 23);
            updownGap.TabIndex = 29;
            updownGap.ValueChanged += updownGap_ValueChanged;
            // 
            // barGap
            // 
            barGap.Location = new Point(53, 177);
            barGap.Maximum = 100;
            barGap.Margin = new Padding(0);
            barGap.Name = "barGap";
            barGap.Size = new Size(106, 45);
            barGap.TabIndex = 28;
            barGap.TickFrequency = 10;
            barGap.ValueChanged += barGap_ValueChanged;
            toolTip.SetToolTip(barGap, "Empty space at the center of Cross/X, so the game's own crosshair stays visible.");
            // 
            // secondaryColorBox
            // 
            secondaryColorBox.BackColor = Color.Red;
            secondaryColorBox.BorderStyle = BorderStyle.FixedSingle;
            secondaryColorBox.Location = new Point(175, 208);
            secondaryColorBox.Name = "secondaryColorBox";
            secondaryColorBox.Size = new Size(28, 19);
            secondaryColorBox.TabIndex = 31;
            secondaryColorBox.TabStop = false;
            secondaryColorBox.Click += secondaryColorBox_Click;
            toolTip.SetToolTip(secondaryColorBox, "Secondary color (a dim color keeps Dots subtle)");
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(206, 311);
            Controls.Add(labelSecondary);
            Controls.Add(comboSecondary);
            Controls.Add(secondaryColorBox);
            Controls.Add(checkboxHollow);
            Controls.Add(labelGap);
            Controls.Add(updownGap);
            Controls.Add(barGap);
            Controls.Add(labelLength);
            Controls.Add(updownLength);
            Controls.Add(barLength);
            Controls.Add(labelHotkey);
            Controls.Add(textHotkey);
            Controls.Add(checkboxShowOverlay);
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
            ((System.ComponentModel.ISupportInitialize)updownLength).EndInit();
            ((System.ComponentModel.ISupportInitialize)barLength).EndInit();
            ((System.ComponentModel.ISupportInitialize)updownGap).EndInit();
            ((System.ComponentModel.ISupportInitialize)barGap).EndInit();
            ((System.ComponentModel.ISupportInitialize)secondaryColorBox).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TrackBar barOpacity;
        private Label labelSecondary;
        private ComboBox comboSecondary;
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
        private CheckBox checkboxShowOverlay;
        private Label labelHotkey;
        private TextBox textHotkey;
        private ToolTip toolTip;
        private Label labelLength;
        private NumericUpDown updownLength;
        private TrackBar barLength;
        private CheckBox checkboxHollow;
        private Label labelGap;
        private NumericUpDown updownGap;
        private TrackBar barGap;
        private PictureBox secondaryColorBox;
    }
}
