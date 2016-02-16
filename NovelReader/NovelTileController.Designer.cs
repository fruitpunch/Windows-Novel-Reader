namespace NovelReader
{
    partial class NovelTileController
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.novelPictureBox = new System.Windows.Forms.PictureBox();
            this.novelTitleLabel = new System.Windows.Forms.Label();
            this.novelStateCB = new System.Windows.Forms.ComboBox();
            this.newChapterLabel = new System.Windows.Forms.Label();
            this.btnMakeAudio = new System.Windows.Forms.Button();
            this.newestChapterLabel = new System.Windows.Forms.Label();
            this.newestChapterInfoLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.novelPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // novelPictureBox
            // 
            this.novelPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.novelPictureBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.novelPictureBox.Location = new System.Drawing.Point(10, 10);
            this.novelPictureBox.Name = "novelPictureBox";
            this.novelPictureBox.Size = new System.Drawing.Size(160, 160);
            this.novelPictureBox.TabIndex = 0;
            this.novelPictureBox.TabStop = false;
            // 
            // novelTitleLabel
            // 
            this.novelTitleLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.novelTitleLabel.AutoSize = true;
            this.novelTitleLabel.BackColor = System.Drawing.Color.Transparent;
            this.novelTitleLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.novelTitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.novelTitleLabel.Location = new System.Drawing.Point(10, 180);
            this.novelTitleLabel.Name = "novelTitleLabel";
            this.novelTitleLabel.Size = new System.Drawing.Size(43, 20);
            this.novelTitleLabel.TabIndex = 1;
            this.novelTitleLabel.Text = "Title";
            // 
            // novelStateCB
            // 
            this.novelStateCB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.novelStateCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.novelStateCB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.novelStateCB.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.novelStateCB.FormattingEnabled = true;
            this.novelStateCB.Location = new System.Drawing.Point(10, 253);
            this.novelStateCB.Name = "novelStateCB";
            this.novelStateCB.Size = new System.Drawing.Size(82, 24);
            this.novelStateCB.TabIndex = 2;
            this.novelStateCB.SelectedIndexChanged += new System.EventHandler(this.novelStateCB_SelectedIndexChanged);
            // 
            // newChapterLabel
            // 
            this.newChapterLabel.AutoSize = true;
            this.newChapterLabel.BackColor = System.Drawing.Color.Coral;
            this.newChapterLabel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.newChapterLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.newChapterLabel.Location = new System.Drawing.Point(0, 0);
            this.newChapterLabel.Name = "newChapterLabel";
            this.newChapterLabel.Size = new System.Drawing.Size(142, 13);
            this.newChapterLabel.TabIndex = 3;
            this.newChapterLabel.Text = "New Chapters Available";
            // 
            // btnMakeAudio
            // 
            this.btnMakeAudio.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnMakeAudio.BackColor = System.Drawing.Color.Transparent;
            this.btnMakeAudio.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMakeAudio.Location = new System.Drawing.Point(98, 252);
            this.btnMakeAudio.Name = "btnMakeAudio";
            this.btnMakeAudio.Size = new System.Drawing.Size(72, 25);
            this.btnMakeAudio.TabIndex = 4;
            this.btnMakeAudio.Text = "Audio";
            this.btnMakeAudio.UseVisualStyleBackColor = false;
            // 
            // newestChapterLabel
            // 
            this.newestChapterLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.newestChapterLabel.AutoSize = true;
            this.newestChapterLabel.BackColor = System.Drawing.Color.Transparent;
            this.newestChapterLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.newestChapterLabel.Location = new System.Drawing.Point(10, 205);
            this.newestChapterLabel.Name = "newestChapterLabel";
            this.newestChapterLabel.Size = new System.Drawing.Size(121, 16);
            this.newestChapterLabel.TabIndex = 5;
            this.newestChapterLabel.Text = "Newest Chapter:";
            // 
            // newestChapterInfoLabel
            // 
            this.newestChapterInfoLabel.AutoSize = true;
            this.newestChapterInfoLabel.Location = new System.Drawing.Point(11, 230);
            this.newestChapterInfoLabel.Name = "newestChapterInfoLabel";
            this.newestChapterInfoLabel.Size = new System.Drawing.Size(35, 13);
            this.newestChapterInfoLabel.TabIndex = 6;
            this.newestChapterInfoLabel.Text = "label1";
            // 
            // NovelTileController
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.newestChapterInfoLabel);
            this.Controls.Add(this.newestChapterLabel);
            this.Controls.Add(this.btnMakeAudio);
            this.Controls.Add(this.newChapterLabel);
            this.Controls.Add(this.novelStateCB);
            this.Controls.Add(this.novelTitleLabel);
            this.Controls.Add(this.novelPictureBox);
            this.Name = "NovelTileController";
            this.Size = new System.Drawing.Size(180, 280);
            this.DoubleClick += new System.EventHandler(this.NovelTileController_DoubleClick);
            ((System.ComponentModel.ISupportInitialize)(this.novelPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox novelPictureBox;
        private System.Windows.Forms.Label novelTitleLabel;
        private System.Windows.Forms.ComboBox novelStateCB;
        private System.Windows.Forms.Label newChapterLabel;
        private System.Windows.Forms.Button btnMakeAudio;
        private System.Windows.Forms.Label newestChapterLabel;
        private System.Windows.Forms.Label newestChapterInfoLabel;
    }
}
