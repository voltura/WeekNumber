using System.ComponentModel;
using System.Windows.Forms;

namespace WeekNumber.Forms
{
    partial class MessageForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MessageForm));
            this.lblMessageFormTitle = new System.Windows.Forms.Label();
            this.minimizePanelFrame = new System.Windows.Forms.Panel();
            this.minimizePanel = new System.Windows.Forms.Panel();
            this.titleIcon = new System.Windows.Forms.PictureBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.settingsPanel = new System.Windows.Forms.Panel();
            this.messageBox = new System.Windows.Forms.TextBox();
            this.Link = new System.Windows.Forms.LinkLabel();
            this.minimizePanelFrame.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.titleIcon)).BeginInit();
            this.settingsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblMessageFormTitle
            // 
            this.lblMessageFormTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblMessageFormTitle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblMessageFormTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessageFormTitle.Location = new System.Drawing.Point(0, 0);
            this.lblMessageFormTitle.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblMessageFormTitle.Name = "lblMessageFormTitle";
            this.lblMessageFormTitle.Padding = new System.Windows.Forms.Padding(56, 0, 0, 0);
            this.lblMessageFormTitle.Size = new System.Drawing.Size(959, 58);
            this.lblMessageFormTitle.TabIndex = 1;
            this.lblMessageFormTitle.Text = "Information";
            this.lblMessageFormTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblMessageFormTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SettingsTitle_MouseDown);
            this.lblMessageFormTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.SettingsTitle_MouseMove);
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
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.btnOK.FlatAppearance.BorderColor = System.Drawing.Color.DeepSkyBlue;
            this.btnOK.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DeepSkyBlue;
            this.btnOK.FlatAppearance.MouseOverBackColor = System.Drawing.Color.LightBlue;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Location = new System.Drawing.Point(682, 332);
            this.btnOK.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(256, 72);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.OK_Click);
            // 
            // settingsPanel
            // 
            this.settingsPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.settingsPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(73)))), ((int)(((byte)(76)))), ((int)(((byte)(76)))));
            this.settingsPanel.Controls.Add(this.messageBox);
            this.settingsPanel.Controls.Add(this.Link);
            this.settingsPanel.Controls.Add(this.btnOK);
            this.settingsPanel.Location = new System.Drawing.Point(0, 58);
            this.settingsPanel.Margin = new System.Windows.Forms.Padding(0);
            this.settingsPanel.Name = "settingsPanel";
            this.settingsPanel.Size = new System.Drawing.Size(959, 430);
            this.settingsPanel.TabIndex = 2;
            // 
            // messageBox
            // 
            this.messageBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(73)))), ((int)(((byte)(76)))), ((int)(((byte)(76)))));
            this.messageBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.messageBox.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold);
            this.messageBox.ForeColor = System.Drawing.Color.White;
            this.messageBox.Location = new System.Drawing.Point(12, 18);
            this.messageBox.Multiline = true;
            this.messageBox.Name = "messageBox";
            this.messageBox.ReadOnly = true;
            this.messageBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.messageBox.Size = new System.Drawing.Size(936, 234);
            this.messageBox.TabIndex = 12;
            this.messageBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // Link
            // 
            this.Link.ActiveLinkColor = System.Drawing.Color.LightSkyBlue;
            this.Link.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Link.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold);
            this.Link.LinkColor = System.Drawing.Color.DeepSkyBlue;
            this.Link.Location = new System.Drawing.Point(21, 265);
            this.Link.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.Link.Name = "Link";
            this.Link.Size = new System.Drawing.Size(917, 42);
            this.Link.TabIndex = 11;
            this.Link.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Link.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Link_LinkClicked);
            // 
            // MessageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.ClientSize = new System.Drawing.Size(959, 488);
            this.ControlBox = false;
            this.Controls.Add(this.titleIcon);
            this.Controls.Add(this.minimizePanelFrame);
            this.Controls.Add(this.settingsPanel);
            this.Controls.Add(this.lblMessageFormTitle);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MessageForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.TopMost = true;
            this.minimizePanelFrame.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.titleIcon)).EndInit();
            this.settingsPanel.ResumeLayout(false);
            this.settingsPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private Label lblMessageFormTitle;
        private Panel minimizePanelFrame;
        private Panel minimizePanel;
        private PictureBox titleIcon;
        private Button btnOK;
        private Panel settingsPanel;
        private LinkLabel Link;
        private TextBox messageBox;
    }
}