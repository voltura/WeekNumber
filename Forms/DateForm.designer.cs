using System.ComponentModel;
using System.Windows.Forms;

namespace WeekNumber.Forms
{
    partial class DateForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Do not pass literals as localized parameters", MessageId = "System.Windows.Forms.Control.set_Text(System.String)")]
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DateForm));
            this.lblDateFormTitle = new System.Windows.Forms.Label();
            this.minimizePanelFrame = new System.Windows.Forms.Panel();
            this.minimizePanel = new System.Windows.Forms.Panel();
            this.titleIcon = new System.Windows.Forms.PictureBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.settingsPanel = new System.Windows.Forms.Panel();
            this.lblDay = new System.Windows.Forms.Label();
            this.lblMonth = new System.Windows.Forms.Label();
            this.lblYear = new System.Windows.Forms.Label();
            this.pictureBoxWeek = new System.Windows.Forms.PictureBox();
            this.lblInformation = new System.Windows.Forms.Label();
            this.ccbDay = new WeekNumber.Controls.CustomComboBox();
            this.ccbMonth = new WeekNumber.Controls.CustomComboBox();
            this.ccbYear = new WeekNumber.Controls.CustomComboBox();
            this.minimizePanelFrame.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.titleIcon)).BeginInit();
            this.settingsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWeek)).BeginInit();
            this.SuspendLayout();
            // 
            // lblDateFormTitle
            // 
            this.lblDateFormTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDateFormTitle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblDateFormTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDateFormTitle.Location = new System.Drawing.Point(0, 0);
            this.lblDateFormTitle.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblDateFormTitle.Name = "lblDateFormTitle";
            this.lblDateFormTitle.Padding = new System.Windows.Forms.Padding(56, 0, 0, 0);
            this.lblDateFormTitle.Size = new System.Drawing.Size(959, 58);
            this.lblDateFormTitle.TabIndex = 1;
            this.lblDateFormTitle.Text = "Select date to display its week number";
            this.lblDateFormTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblDateFormTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SettingsTitle_MouseDown);
            this.lblDateFormTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SettingsTitle_MouseMove);
            // 
            // minimizePanelFrame
            // 
            this.minimizePanelFrame.Controls.Add(this.minimizePanel);
            this.minimizePanelFrame.Location = new System.Drawing.Point(892, 0);
            this.minimizePanelFrame.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.minimizePanelFrame.Name = "minimizePanelFrame";
            this.minimizePanelFrame.Size = new System.Drawing.Size(63, 58);
            this.minimizePanelFrame.TabIndex = 3;
            this.minimizePanelFrame.Click += new System.EventHandler(this.MinimizePanelFrame_Click);
            this.minimizePanelFrame.MouseEnter += new System.EventHandler(this.MinimizePanel_MouseEnter);
            this.minimizePanelFrame.MouseLeave += new System.EventHandler(this.MinimizePanel_MouseLeave);
            // 
            // minimizePanel
            // 
            this.minimizePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.minimizePanel.BackColor = System.Drawing.Color.White;
            this.minimizePanel.Location = new System.Drawing.Point(12, 13);
            this.minimizePanel.Margin = new System.Windows.Forms.Padding(0);
            this.minimizePanel.Name = "minimizePanel";
            this.minimizePanel.Size = new System.Drawing.Size(44, 14);
            this.minimizePanel.TabIndex = 3;
            this.minimizePanel.Click += new System.EventHandler(this.MinimizePanel_Click);
            this.minimizePanel.MouseEnter += new System.EventHandler(this.MinimizePanel_MouseEnter);
            // 
            // titleIcon
            // 
            this.titleIcon.BackColor = System.Drawing.Color.Transparent;
            this.titleIcon.Image = ((System.Drawing.Image)(resources.GetObject("titleIcon.Image")));
            this.titleIcon.Location = new System.Drawing.Point(7, 7);
            this.titleIcon.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.titleIcon.Name = "titleIcon";
            this.titleIcon.Size = new System.Drawing.Size(46, 47);
            this.titleIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.titleIcon.TabIndex = 4;
            this.titleIcon.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.DeepSkyBlue;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DeepSkyBlue;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightBlue;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(682, 337);
            this.btnClose.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(256, 72);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.Close_Click);
            // 
            // settingsPanel
            // 
            this.settingsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.settingsPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(73)))), ((int)(((byte)(76)))), ((int)(((byte)(76)))));
            this.settingsPanel.Controls.Add(this.lblDay);
            this.settingsPanel.Controls.Add(this.lblMonth);
            this.settingsPanel.Controls.Add(this.lblYear);
            this.settingsPanel.Controls.Add(this.pictureBoxWeek);
            this.settingsPanel.Controls.Add(this.lblInformation);
            this.settingsPanel.Controls.Add(this.ccbDay);
            this.settingsPanel.Controls.Add(this.ccbMonth);
            this.settingsPanel.Controls.Add(this.ccbYear);
            this.settingsPanel.Controls.Add(this.btnClose);
            this.settingsPanel.Location = new System.Drawing.Point(0, 58);
            this.settingsPanel.Margin = new System.Windows.Forms.Padding(0);
            this.settingsPanel.Name = "settingsPanel";
            this.settingsPanel.Size = new System.Drawing.Size(959, 430);
            this.settingsPanel.TabIndex = 2;
            // 
            // lblDay
            // 
            this.lblDay.AutoSize = true;
            this.lblDay.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblDay.Location = new System.Drawing.Point(436, 33);
            this.lblDay.Name = "lblDay";
            this.lblDay.Size = new System.Drawing.Size(72, 41);
            this.lblDay.TabIndex = 20;
            this.lblDay.Text = "Day";
            // 
            // lblMonth
            // 
            this.lblMonth.AutoSize = true;
            this.lblMonth.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblMonth.Location = new System.Drawing.Point(236, 33);
            this.lblMonth.Name = "lblMonth";
            this.lblMonth.Size = new System.Drawing.Size(113, 41);
            this.lblMonth.TabIndex = 19;
            this.lblMonth.Text = "Month";
            // 
            // lblYear
            // 
            this.lblYear.AutoSize = true;
            this.lblYear.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblYear.Location = new System.Drawing.Point(36, 33);
            this.lblYear.Name = "lblYear";
            this.lblYear.Size = new System.Drawing.Size(77, 41);
            this.lblYear.TabIndex = 18;
            this.lblYear.Text = "Year";
            // 
            // pictureBoxWeek
            // 
            this.pictureBoxWeek.Location = new System.Drawing.Point(682, 43);
            this.pictureBoxWeek.Name = "pictureBoxWeek";
            this.pictureBoxWeek.Size = new System.Drawing.Size(256, 256);
            this.pictureBoxWeek.TabIndex = 17;
            this.pictureBoxWeek.TabStop = false;
            // 
            // lblInformation
            // 
            this.lblInformation.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblInformation.Location = new System.Drawing.Point(36, 162);
            this.lblInformation.Name = "lblInformation";
            this.lblInformation.Size = new System.Drawing.Size(581, 107);
            this.lblInformation.TabIndex = 16;
            this.lblInformation.Text = "Sunday, 5th of June 2021\r\nWeek 22";
            // 
            // ccbDay
            // 
            this.ccbDay.DropDownWidth = 174;
            this.ccbDay.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.ccbDay.FormattingEnabled = true;
            this.ccbDay.HighlightColor = System.Drawing.Color.DeepSkyBlue;
            this.ccbDay.Location = new System.Drawing.Point(443, 87);
            this.ccbDay.Name = "ccbDay";
            this.ccbDay.Size = new System.Drawing.Size(174, 48);
            this.ccbDay.TabIndex = 15;
            this.ccbDay.SelectedIndexChanged += new System.EventHandler(this.SelectedDayChanged);
            // 
            // ccbMonth
            // 
            this.ccbMonth.DropDownWidth = 174;
            this.ccbMonth.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.ccbMonth.FormattingEnabled = true;
            this.ccbMonth.HighlightColor = System.Drawing.Color.DeepSkyBlue;
            this.ccbMonth.Location = new System.Drawing.Point(243, 87);
            this.ccbMonth.Name = "ccbMonth";
            this.ccbMonth.Size = new System.Drawing.Size(174, 48);
            this.ccbMonth.TabIndex = 14;
            this.ccbMonth.SelectedIndexChanged += new System.EventHandler(this.SelectedMonthChanged);
            // 
            // ccbYear
            // 
            this.ccbYear.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.ccbYear.FormattingEnabled = true;
            this.ccbYear.HighlightColor = System.Drawing.Color.DeepSkyBlue;
            this.ccbYear.Location = new System.Drawing.Point(43, 87);
            this.ccbYear.Name = "ccbYear";
            this.ccbYear.Size = new System.Drawing.Size(174, 48);
            this.ccbYear.TabIndex = 13;
            this.ccbYear.SelectedIndexChanged += new System.EventHandler(this.SelectedYearChanged);
            // 
            // DateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.ClientSize = new System.Drawing.Size(959, 488);
            this.ControlBox = false;
            this.Controls.Add(this.titleIcon);
            this.Controls.Add(this.minimizePanelFrame);
            this.Controls.Add(this.settingsPanel);
            this.Controls.Add(this.lblDateFormTitle);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DateForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.TopMost = true;
            this.Shown += new System.EventHandler(this.DateForm_Shown);
            this.minimizePanelFrame.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.titleIcon)).EndInit();
            this.settingsPanel.ResumeLayout(false);
            this.settingsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWeek)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Label lblDateFormTitle;
        private Panel minimizePanelFrame;
        private Panel minimizePanel;
        private PictureBox titleIcon;
        private Button btnClose;
        private Panel settingsPanel;
        private PictureBox pictureBoxWeek;
        private Label lblInformation;
        private Controls.CustomComboBox ccbDay;
        private Controls.CustomComboBox ccbMonth;
        private Controls.CustomComboBox ccbYear;
        private Label lblMonth;
        private Label lblYear;
        private Label lblDay;
    }
}