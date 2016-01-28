namespace NovelReader
{
    partial class ApplicationForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
        private void InitializeComponent()
        {
            this.tabControl = new System.Windows.Forms.TabControl();
            this.novelListTab = new System.Windows.Forms.TabPage();
            this.novelListController = new NovelReader.NovelListController();
            this.ttsListTab = new System.Windows.Forms.TabPage();
            this.ttsController1 = new NovelReader.TTSController();
            this.settingTab = new System.Windows.Forms.TabPage();
            this.settingControl1 = new NovelReader.SettingControl();
            this.tabControl.SuspendLayout();
            this.novelListTab.SuspendLayout();
            this.ttsListTab.SuspendLayout();
            this.settingTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.novelListTab);
            this.tabControl.Controls.Add(this.ttsListTab);
            this.tabControl.Controls.Add(this.settingTab);
            this.tabControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl.HotTrack = true;
            this.tabControl.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.tabControl.Location = new System.Drawing.Point(25, 25);
            this.tabControl.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl.Name = "tabControl";
            this.tabControl.Padding = new System.Drawing.Point(0, 0);
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1155, 783);
            this.tabControl.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tabControl.TabIndex = 0;
            // 
            // novelListTab
            // 
            this.novelListTab.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.novelListTab.Controls.Add(this.novelListController);
            this.novelListTab.Location = new System.Drawing.Point(4, 25);
            this.novelListTab.Name = "novelListTab";
            this.novelListTab.Padding = new System.Windows.Forms.Padding(3);
            this.novelListTab.Size = new System.Drawing.Size(1147, 754);
            this.novelListTab.TabIndex = 0;
            this.novelListTab.Text = "Novel List";
            // 
            // novelListController
            // 
            this.novelListController.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.novelListController.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.novelListController.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.novelListController.Location = new System.Drawing.Point(0, 0);
            this.novelListController.Margin = new System.Windows.Forms.Padding(2);
            this.novelListController.Name = "novelListController";
            this.novelListController.Size = new System.Drawing.Size(1147, 754);
            this.novelListController.TabIndex = 0;
            // 
            // ttsListTab
            // 
            this.ttsListTab.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.ttsListTab.Controls.Add(this.ttsController1);
            this.ttsListTab.Location = new System.Drawing.Point(4, 25);
            this.ttsListTab.Name = "ttsListTab";
            this.ttsListTab.Padding = new System.Windows.Forms.Padding(3);
            this.ttsListTab.Size = new System.Drawing.Size(1147, 754);
            this.ttsListTab.TabIndex = 1;
            this.ttsListTab.Text = "TTS List";
            // 
            // ttsController1
            // 
            this.ttsController1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ttsController1.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.ttsController1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ttsController1.Location = new System.Drawing.Point(0, 0);
            this.ttsController1.Margin = new System.Windows.Forms.Padding(2);
            this.ttsController1.Name = "ttsController1";
            this.ttsController1.Size = new System.Drawing.Size(1147, 754);
            this.ttsController1.TabIndex = 0;
            // 
            // settingTab
            // 
            this.settingTab.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.settingTab.Controls.Add(this.settingControl1);
            this.settingTab.Location = new System.Drawing.Point(4, 25);
            this.settingTab.Name = "settingTab";
            this.settingTab.Padding = new System.Windows.Forms.Padding(3);
            this.settingTab.Size = new System.Drawing.Size(1147, 754);
            this.settingTab.TabIndex = 2;
            this.settingTab.Text = "Setting";
            // 
            // settingControl1
            // 
            this.settingControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.settingControl1.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.settingControl1.Location = new System.Drawing.Point(0, 0);
            this.settingControl1.Name = "settingControl1";
            this.settingControl1.Size = new System.Drawing.Size(1147, 754);
            this.settingControl1.TabIndex = 0;
            // 
            // ApplicationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.ClientSize = new System.Drawing.Size(1214, 833);
            this.Controls.Add(this.tabControl);
            this.Name = "ApplicationForm";
            this.Text = "Novel Reader (Pre Alpha)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ApplicationForm_FormClosing);
            this.Load += new System.EventHandler(this.ApplicationForm_Load);
            this.tabControl.ResumeLayout(false);
            this.novelListTab.ResumeLayout(false);
            this.ttsListTab.ResumeLayout(false);
            this.settingTab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private NovelListController novelListController1;
        private TTSController ttsControl1;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage novelListTab;
        private NovelListController novelListController;
        private System.Windows.Forms.TabPage ttsListTab;
        private TTSController ttsController1;
        private System.Windows.Forms.TabPage settingTab;
        private SettingControl settingControl1;

    }
}

