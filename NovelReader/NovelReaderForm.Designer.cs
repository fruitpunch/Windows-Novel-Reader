namespace NovelReader
{
    partial class NovelReaderForm
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
            this.dgvChapterList = new System.Windows.Forms.DataGridView();
            this.rtbChapterTextBox = new System.Windows.Forms.RichTextBox();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnPlay = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.cbAutoPlay = new System.Windows.Forms.CheckBox();
            this.labelTitle = new System.Windows.Forms.Label();
            this.btnPrevious = new System.Windows.Forms.Button();
            this.btnRedownload = new System.Windows.Forms.Button();
            this.btnDeleteChapter = new System.Windows.Forms.Button();
            this.btnAddChapter = new System.Windows.Forms.Button();
            this.btnFinishReading = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChapterList)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvChapterList
            // 
            this.dgvChapterList.AllowUserToAddRows = false;
            this.dgvChapterList.AllowUserToDeleteRows = false;
            this.dgvChapterList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dgvChapterList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvChapterList.Location = new System.Drawing.Point(15, 15);
            this.dgvChapterList.Name = "dgvChapterList";
            this.dgvChapterList.RowHeadersVisible = false;
            this.dgvChapterList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvChapterList.Size = new System.Drawing.Size(350, 670);
            this.dgvChapterList.TabIndex = 0;
            this.dgvChapterList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvChapterList_CellDoubleClick);
            this.dgvChapterList.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvChapterList_CellFormatting);
            this.dgvChapterList.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvChapterList_CellValueChanged);
            this.dgvChapterList.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvChapterList_CurrentCellDirtyStateChanged);
            // 
            // rtbChapterTextBox
            // 
            this.rtbChapterTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.rtbChapterTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.rtbChapterTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rtbChapterTextBox.Location = new System.Drawing.Point(380, 61);
            this.rtbChapterTextBox.Name = "rtbChapterTextBox";
            this.rtbChapterTextBox.ReadOnly = true;
            this.rtbChapterTextBox.Size = new System.Drawing.Size(790, 624);
            this.rtbChapterTextBox.TabIndex = 1;
            this.rtbChapterTextBox.Text = "";
            this.rtbChapterTextBox.TextChanged += new System.EventHandler(this.rtbChapterTextBox_TextChanged);
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnNext.Location = new System.Drawing.Point(588, 691);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(80, 35);
            this.btnNext.TabIndex = 3;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnEdit.Location = new System.Drawing.Point(756, 19);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(125, 35);
            this.btnEdit.TabIndex = 4;
            this.btnEdit.Text = "Edit";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnPlay
            // 
            this.btnPlay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPlay.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPlay.Location = new System.Drawing.Point(815, 691);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(75, 35);
            this.btnPlay.TabIndex = 5;
            this.btnPlay.Text = "Play";
            this.btnPlay.UseVisualStyleBackColor = true;
            // 
            // btnPause
            // 
            this.btnPause.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPause.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPause.Location = new System.Drawing.Point(896, 691);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(75, 35);
            this.btnPause.TabIndex = 6;
            this.btnPause.Text = "Pause";
            this.btnPause.UseVisualStyleBackColor = true;
            // 
            // btnStop
            // 
            this.btnStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnStop.Location = new System.Drawing.Point(977, 691);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 35);
            this.btnStop.TabIndex = 7;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            // 
            // cbAutoPlay
            // 
            this.cbAutoPlay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbAutoPlay.AutoSize = true;
            this.cbAutoPlay.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.cbAutoPlay.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbAutoPlay.Location = new System.Drawing.Point(1057, 699);
            this.cbAutoPlay.Name = "cbAutoPlay";
            this.cbAutoPlay.Size = new System.Drawing.Size(87, 21);
            this.cbAutoPlay.TabIndex = 8;
            this.cbAutoPlay.Text = "Auto Play";
            this.cbAutoPlay.UseVisualStyleBackColor = false;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.Location = new System.Drawing.Point(379, 15);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(71, 31);
            this.labelTitle.TabIndex = 9;
            this.labelTitle.Text = "Title";
            // 
            // btnPrevious
            // 
            this.btnPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPrevious.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPrevious.Location = new System.Drawing.Point(380, 691);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(80, 35);
            this.btnPrevious.TabIndex = 2;
            this.btnPrevious.Text = "Prev";
            this.btnPrevious.UseVisualStyleBackColor = true;
            this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
            // 
            // btnRedownload
            // 
            this.btnRedownload.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRedownload.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnRedownload.Location = new System.Drawing.Point(887, 19);
            this.btnRedownload.Name = "btnRedownload";
            this.btnRedownload.Size = new System.Drawing.Size(135, 35);
            this.btnRedownload.TabIndex = 10;
            this.btnRedownload.Text = "Download Chapter";
            this.btnRedownload.UseVisualStyleBackColor = true;
            this.btnRedownload.Click += new System.EventHandler(this.btnRedownload_Click);
            // 
            // btnDeleteChapter
            // 
            this.btnDeleteChapter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteChapter.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDeleteChapter.Location = new System.Drawing.Point(1028, 20);
            this.btnDeleteChapter.Name = "btnDeleteChapter";
            this.btnDeleteChapter.Size = new System.Drawing.Size(135, 35);
            this.btnDeleteChapter.TabIndex = 11;
            this.btnDeleteChapter.Text = "Delete Chapter";
            this.btnDeleteChapter.UseVisualStyleBackColor = true;
            this.btnDeleteChapter.Click += new System.EventHandler(this.btnDeleteChapter_Click);
            // 
            // btnAddChapter
            // 
            this.btnAddChapter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddChapter.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAddChapter.Location = new System.Drawing.Point(15, 691);
            this.btnAddChapter.Name = "btnAddChapter";
            this.btnAddChapter.Size = new System.Drawing.Size(135, 35);
            this.btnAddChapter.TabIndex = 12;
            this.btnAddChapter.Text = "Add Chapter";
            this.btnAddChapter.UseVisualStyleBackColor = true;
            this.btnAddChapter.Click += new System.EventHandler(this.btnAddChapter_Click);
            // 
            // btnFinishReading
            // 
            this.btnFinishReading.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnFinishReading.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnFinishReading.Location = new System.Drawing.Point(466, 691);
            this.btnFinishReading.Name = "btnFinishReading";
            this.btnFinishReading.Size = new System.Drawing.Size(116, 35);
            this.btnFinishReading.TabIndex = 13;
            this.btnFinishReading.Text = "Finish Reading";
            this.btnFinishReading.UseVisualStyleBackColor = true;
            this.btnFinishReading.Click += new System.EventHandler(this.btnFinishReading_Click);
            // 
            // NovelReaderForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ClientSize = new System.Drawing.Size(1184, 730);
            this.Controls.Add(this.btnFinishReading);
            this.Controls.Add(this.btnAddChapter);
            this.Controls.Add(this.btnDeleteChapter);
            this.Controls.Add(this.btnRedownload);
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.cbAutoPlay);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnPause);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnPrevious);
            this.Controls.Add(this.rtbChapterTextBox);
            this.Controls.Add(this.dgvChapterList);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "NovelReaderForm";
            this.Text = "NovelReaderForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.NovelReaderForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dgvChapterList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvChapterList;
        private System.Windows.Forms.RichTextBox rtbChapterTextBox;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.CheckBox cbAutoPlay;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Button btnPrevious;
        private System.Windows.Forms.Button btnRedownload;
        private System.Windows.Forms.Button btnDeleteChapter;
        private System.Windows.Forms.Button btnAddChapter;
        private System.Windows.Forms.Button btnFinishReading;
    }
}