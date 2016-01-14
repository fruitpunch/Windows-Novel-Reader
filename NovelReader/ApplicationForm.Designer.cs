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
            this.ttsController = new NovelReader.TTSController();
            this.tabControl.SuspendLayout();
            this.novelListTab.SuspendLayout();
            this.ttsListTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl.Controls.Add(this.novelListTab);
            this.tabControl.Controls.Add(this.ttsListTab);
            this.tabControl.Location = new System.Drawing.Point(25, 25);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1000, 700);
            this.tabControl.TabIndex = 0;
            // 
            // novelListTab
            // 
            this.novelListTab.Controls.Add(this.novelListController);
            this.novelListTab.Location = new System.Drawing.Point(4, 22);
            this.novelListTab.Name = "novelListTab";
            this.novelListTab.Padding = new System.Windows.Forms.Padding(3);
            this.novelListTab.Size = new System.Drawing.Size(992, 674);
            this.novelListTab.TabIndex = 0;
            this.novelListTab.Text = "Novel List";
            this.novelListTab.UseVisualStyleBackColor = true;
            // 
            // novelListController
            // 
            this.novelListController.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.novelListController.Location = new System.Drawing.Point(11, 12);
            this.novelListController.Name = "novelListController";
            this.novelListController.Size = new System.Drawing.Size(970, 650);
            this.novelListController.TabIndex = 0;
            // 
            // ttsListTab
            // 
            this.ttsListTab.Controls.Add(this.ttsController);
            this.ttsListTab.Location = new System.Drawing.Point(4, 22);
            this.ttsListTab.Name = "ttsListTab";
            this.ttsListTab.Padding = new System.Windows.Forms.Padding(3);
            this.ttsListTab.Size = new System.Drawing.Size(992, 674);
            this.ttsListTab.TabIndex = 1;
            this.ttsListTab.Text = "TTS List";
            this.ttsListTab.UseVisualStyleBackColor = true;
            // 
            // ttsController
            // 
            this.ttsController.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ttsController.Location = new System.Drawing.Point(11, 12);
            this.ttsController.Name = "ttsController";
            this.ttsController.Size = new System.Drawing.Size(970, 650);
            this.ttsController.TabIndex = 0;
            // 
            // ApplicationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1059, 750);
            this.Controls.Add(this.tabControl);
            this.Name = "ApplicationForm";
            this.Text = "Novel Reader";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ApplicationForm_FormClosing);
            this.Load += new System.EventHandler(this.ApplicationForm_Load);
            this.tabControl.ResumeLayout(false);
            this.novelListTab.ResumeLayout(false);
            this.ttsListTab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private NovelListController novelListController1;
        private TTSController ttsControl1;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage novelListTab;
        private NovelListController novelListController;
        private System.Windows.Forms.TabPage ttsListTab;
        private TTSController ttsController;

    }
}

