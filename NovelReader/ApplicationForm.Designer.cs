﻿namespace NovelReader
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
            this.buttonContainerPanel = new System.Windows.Forms.Panel();
            this.btnNovelList = new System.Windows.Forms.Button();
            this.btnTTSList = new System.Windows.Forms.Button();
            this.btnSetting = new System.Windows.Forms.Button();
            this.controlContainerPanel = new System.Windows.Forms.Panel();
            this.buttonContainerPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonContainerPanel
            // 
            this.buttonContainerPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonContainerPanel.BackColor = System.Drawing.Color.DarkSalmon;
            this.buttonContainerPanel.Controls.Add(this.btnSetting);
            this.buttonContainerPanel.Controls.Add(this.btnTTSList);
            this.buttonContainerPanel.Controls.Add(this.btnNovelList);
            this.buttonContainerPanel.Location = new System.Drawing.Point(0, 650);
            this.buttonContainerPanel.Name = "buttonContainerPanel";
            this.buttonContainerPanel.Size = new System.Drawing.Size(1015, 62);
            this.buttonContainerPanel.TabIndex = 0;
            // 
            // btnNovelList
            // 
            this.btnNovelList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btnNovelList.FlatAppearance.BorderSize = 0;
            this.btnNovelList.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.WindowFrame;
            this.btnNovelList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNovelList.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNovelList.ForeColor = System.Drawing.Color.Black;
            this.btnNovelList.Location = new System.Drawing.Point(203, 0);
            this.btnNovelList.Name = "btnNovelList";
            this.btnNovelList.Size = new System.Drawing.Size(200, 62);
            this.btnNovelList.TabIndex = 1;
            this.btnNovelList.Text = "Novel List";
            this.btnNovelList.UseVisualStyleBackColor = true;
            this.btnNovelList.Click += new System.EventHandler(this.btnNovelList_Click);
            // 
            // btnTTSList
            // 
            this.btnTTSList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btnTTSList.FlatAppearance.BorderSize = 0;
            this.btnTTSList.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.WindowFrame;
            this.btnTTSList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTTSList.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTTSList.ForeColor = System.Drawing.Color.Black;
            this.btnTTSList.Location = new System.Drawing.Point(403, 0);
            this.btnTTSList.Name = "btnTTSList";
            this.btnTTSList.Size = new System.Drawing.Size(200, 62);
            this.btnTTSList.TabIndex = 2;
            this.btnTTSList.Text = "TTS List";
            this.btnTTSList.UseVisualStyleBackColor = true;
            this.btnTTSList.Click += new System.EventHandler(this.btnTTSList_Click);
            // 
            // btnSetting
            // 
            this.btnSetting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btnSetting.FlatAppearance.BorderSize = 0;
            this.btnSetting.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.WindowFrame;
            this.btnSetting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSetting.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSetting.ForeColor = System.Drawing.Color.Black;
            this.btnSetting.Location = new System.Drawing.Point(603, 0);
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.Size = new System.Drawing.Size(200, 62);
            this.btnSetting.TabIndex = 3;
            this.btnSetting.Text = "Settings";
            this.btnSetting.UseVisualStyleBackColor = true;
            this.btnSetting.Click += new System.EventHandler(this.btnSetting_Click);
            // 
            // controlContainerPanel
            // 
            this.controlContainerPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.controlContainerPanel.Location = new System.Drawing.Point(0, 0);
            this.controlContainerPanel.Name = "controlContainerPanel";
            this.controlContainerPanel.Size = new System.Drawing.Size(1015, 650);
            this.controlContainerPanel.TabIndex = 1;
            // 
            // ApplicationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1009, 712);
            this.Controls.Add(this.controlContainerPanel);
            this.Controls.Add(this.buttonContainerPanel);
            this.Name = "ApplicationForm";
            this.Text = "Novel Reader (Pre Alpha)";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ApplicationForm_FormClosing);
            this.Load += new System.EventHandler(this.ApplicationForm_Load);
            this.buttonContainerPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private NovelListController novelListController1;
        private TTSController ttsControl1;
        private System.Windows.Forms.Panel buttonContainerPanel;
        private System.Windows.Forms.Button btnNovelList;
        private System.Windows.Forms.Button btnTTSList;
        private System.Windows.Forms.Button btnSetting;
        private System.Windows.Forms.Panel controlContainerPanel;
    }
}

