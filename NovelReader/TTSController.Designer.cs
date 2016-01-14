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
            ((System.ComponentModel.ISupportInitialize)(this.dgvTTS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udThreadCount)).BeginInit();
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
            this.dgvTTS.Location = new System.Drawing.Point(40, 40);
            this.dgvTTS.MultiSelect = false;
            this.dgvTTS.Name = "dgvTTS";
            this.dgvTTS.RowHeadersVisible = false;
            this.dgvTTS.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTTS.Size = new System.Drawing.Size(800, 550);
            this.dgvTTS.TabIndex = 0;
            // 
            // udThreadCount
            // 
            this.udThreadCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.udThreadCount.Location = new System.Drawing.Point(40, 608);
            this.udThreadCount.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.udThreadCount.Name = "udThreadCount";
            this.udThreadCount.Size = new System.Drawing.Size(47, 20);
            this.udThreadCount.TabIndex = 1;
            this.udThreadCount.ValueChanged += new System.EventHandler(this.udThreadCount_ValueChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(93, 610);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Thread Count(0-4)";
            // 
            // TTSController
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.udThreadCount);
            this.Controls.Add(this.dgvTTS);
            this.Name = "TTSController";
            this.Size = new System.Drawing.Size(970, 650);
            this.Load += new System.EventHandler(this.TTSController_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTTS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udThreadCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvTTS;
        private System.Windows.Forms.NumericUpDown udThreadCount;
        private System.Windows.Forms.Label label1;
    }
}
