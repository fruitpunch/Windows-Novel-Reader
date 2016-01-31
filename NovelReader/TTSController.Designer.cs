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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvTTS = new System.Windows.Forms.DataGridView();
            this.udThreadCount = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.btnResetList = new System.Windows.Forms.Button();
            this.udTTSSpeed = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.topPanel = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTTS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udThreadCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.udTTSSpeed)).BeginInit();
            this.bottomPanel.SuspendLayout();
            this.topPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvTTS
            // 
            this.dgvTTS.AllowUserToAddRows = false;
            this.dgvTTS.AllowUserToDeleteRows = false;
            this.dgvTTS.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTTS.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dgvTTS.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvTTS.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightSalmon;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(5);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTTS.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvTTS.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTTS.EnableHeadersVisualStyles = false;
            this.dgvTTS.Location = new System.Drawing.Point(0, 50);
            this.dgvTTS.Margin = new System.Windows.Forms.Padding(2);
            this.dgvTTS.MultiSelect = false;
            this.dgvTTS.Name = "dgvTTS";
            this.dgvTTS.RowHeadersVisible = false;
            this.dgvTTS.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTTS.Size = new System.Drawing.Size(910, 550);
            this.dgvTTS.TabIndex = 0;
            // 
            // udThreadCount
            // 
            this.udThreadCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.udThreadCount.BackColor = System.Drawing.Color.SteelBlue;
            this.udThreadCount.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.udThreadCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.udThreadCount.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.udThreadCount.Location = new System.Drawing.Point(177, 20);
            this.udThreadCount.Margin = new System.Windows.Forms.Padding(2);
            this.udThreadCount.Maximum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.udThreadCount.Name = "udThreadCount";
            this.udThreadCount.Size = new System.Drawing.Size(40, 18);
            this.udThreadCount.TabIndex = 1;
            this.udThreadCount.ValueChanged += new System.EventHandler(this.udThreadCount_ValueChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.SteelBlue;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Location = new System.Drawing.Point(12, 20);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(164, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Thread Count ( 0 to 4 ):";
            // 
            // btnResetList
            // 
            this.btnResetList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnResetList.BackColor = System.Drawing.Color.SteelBlue;
            this.btnResetList.FlatAppearance.BorderSize = 0;
            this.btnResetList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResetList.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnResetList.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnResetList.Location = new System.Drawing.Point(790, 0);
            this.btnResetList.Margin = new System.Windows.Forms.Padding(2);
            this.btnResetList.Name = "btnResetList";
            this.btnResetList.Size = new System.Drawing.Size(120, 50);
            this.btnResetList.TabIndex = 14;
            this.btnResetList.Text = "Reset List";
            this.btnResetList.UseVisualStyleBackColor = false;
            this.btnResetList.Click += new System.EventHandler(this.btnResetList_Click);
            // 
            // udTTSSpeed
            // 
            this.udTTSSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.udTTSSpeed.BackColor = System.Drawing.Color.SteelBlue;
            this.udTTSSpeed.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.udTTSSpeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.udTTSSpeed.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.udTTSSpeed.Location = new System.Drawing.Point(480, 20);
            this.udTTSSpeed.Margin = new System.Windows.Forms.Padding(2);
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
            this.udTTSSpeed.Size = new System.Drawing.Size(40, 18);
            this.udTTSSpeed.TabIndex = 15;
            this.udTTSSpeed.ValueChanged += new System.EventHandler(this.udTTSSpeed_ValueChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.SteelBlue;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label2.Location = new System.Drawing.Point(320, 20);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(156, 16);
            this.label2.TabIndex = 16;
            this.label2.Text = "TTS Speed ( -8 to 8 ):";
            // 
            // bottomPanel
            // 
            this.bottomPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bottomPanel.BackColor = System.Drawing.Color.SteelBlue;
            this.bottomPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.bottomPanel.Controls.Add(this.btnResetList);
            this.bottomPanel.Controls.Add(this.udThreadCount);
            this.bottomPanel.Controls.Add(this.label1);
            this.bottomPanel.Controls.Add(this.udTTSSpeed);
            this.bottomPanel.Controls.Add(this.label2);
            this.bottomPanel.Location = new System.Drawing.Point(0, 600);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(910, 50);
            this.bottomPanel.TabIndex = 17;
            // 
            // topPanel
            // 
            this.topPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.topPanel.BackColor = System.Drawing.Color.SteelBlue;
            this.topPanel.Controls.Add(this.label3);
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(910, 50);
            this.topPanel.TabIndex = 18;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(343, 39);
            this.label3.TabIndex = 19;
            this.label3.Text = "Text To Speech List";
            // 
            // TTSController
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.Controls.Add(this.topPanel);
            this.Controls.Add(this.bottomPanel);
            this.Controls.Add(this.dgvTTS);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "TTSController";
            this.Size = new System.Drawing.Size(910, 650);
            this.Load += new System.EventHandler(this.TTSController_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTTS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udThreadCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.udTTSSpeed)).EndInit();
            this.bottomPanel.ResumeLayout(false);
            this.bottomPanel.PerformLayout();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvTTS;
        private System.Windows.Forms.NumericUpDown udThreadCount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnResetList;
        private System.Windows.Forms.NumericUpDown udTTSSpeed;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel bottomPanel;
        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.Label label3;
    }
}
