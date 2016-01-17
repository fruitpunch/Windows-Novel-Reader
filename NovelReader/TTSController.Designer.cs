namespace NovelReader
{
    partial class TTSController
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
            this.dgvTTS = new System.Windows.Forms.DataGridView();
            this.udThreadCount = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.btnResetList = new System.Windows.Forms.Button();
            this.udTTSSpeed = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTTS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udThreadCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udTTSSpeed)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvTTS
            // 
            this.dgvTTS.AllowUserToAddRows = false;
            this.dgvTTS.AllowUserToDeleteRows = false;
            this.dgvTTS.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTTS.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTTS.Location = new System.Drawing.Point(54, 49);
            this.dgvTTS.MultiSelect = false;
            this.dgvTTS.Name = "dgvTTS";
            this.dgvTTS.RowHeadersVisible = false;
            this.dgvTTS.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTTS.Size = new System.Drawing.Size(1066, 677);
            this.dgvTTS.TabIndex = 0;
            // 
            // udThreadCount
            // 
            this.udThreadCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.udThreadCount.Location = new System.Drawing.Point(57, 757);
            this.udThreadCount.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.udThreadCount.Name = "udThreadCount";
            this.udThreadCount.Size = new System.Drawing.Size(63, 22);
            this.udThreadCount.TabIndex = 1;
            this.udThreadCount.ValueChanged += new System.EventHandler(this.udThreadCount_ValueChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(54, 738);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Thread Count(0 to 4)";
            // 
            // btnResetList
            // 
            this.btnResetList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnResetList.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnResetList.Location = new System.Drawing.Point(499, 738);
            this.btnResetList.Name = "btnResetList";
            this.btnResetList.Size = new System.Drawing.Size(155, 45);
            this.btnResetList.TabIndex = 14;
            this.btnResetList.Text = "Reset List";
            this.btnResetList.UseVisualStyleBackColor = true;
            this.btnResetList.Click += new System.EventHandler(this.btnResetList_Click);
            // 
            // udTTSSpeed
            // 
            this.udTTSSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.udTTSSpeed.Location = new System.Drawing.Point(246, 757);
            this.udTTSSpeed.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.udTTSSpeed.Minimum = new decimal(new int[] {
            8,
            0,
            0,
            -2147483648});
            this.udTTSSpeed.Name = "udTTSSpeed";
            this.udTTSSpeed.Size = new System.Drawing.Size(63, 22);
            this.udTTSSpeed.TabIndex = 15;
            this.udTTSSpeed.ValueChanged += new System.EventHandler(this.udTTSSpeed_ValueChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(242, 738);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 16);
            this.label2.TabIndex = 16;
            this.label2.Text = "TTS Speed(-8 to 8)";
            // 
            // TTSController
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.udTTSSpeed);
            this.Controls.Add(this.btnResetList);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.udThreadCount);
            this.Controls.Add(this.dgvTTS);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "TTSController";
            this.Size = new System.Drawing.Size(1294, 800);
            this.Load += new System.EventHandler(this.TTSController_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTTS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udThreadCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udTTSSpeed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvTTS;
        private System.Windows.Forms.NumericUpDown udThreadCount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnResetList;
        private System.Windows.Forms.NumericUpDown udTTSSpeed;
        private System.Windows.Forms.Label label2;
    }
}
