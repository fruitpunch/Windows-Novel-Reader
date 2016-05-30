namespace NovelReader
{
    partial class SettingController
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
            this.dgvLanguageSelector = new System.Windows.Forms.DataGridView();
            this.BottomPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.topPanel = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.upUpdateFreq = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.audioExportLocationLabel = new System.Windows.Forms.Label();
            this.audioExportBrowseButton = new System.Windows.Forms.Button();
            this.exportFolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.textExportBrowseButton = new System.Windows.Forms.Button();
            this.textExportLocationLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLanguageSelector)).BeginInit();
            this.topPanel.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.upUpdateFreq)).BeginInit();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvLanguageSelector
            // 
            this.dgvLanguageSelector.AllowUserToAddRows = false;
            this.dgvLanguageSelector.AllowUserToDeleteRows = false;
            this.dgvLanguageSelector.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvLanguageSelector.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dgvLanguageSelector.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvLanguageSelector.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightSalmon;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(5);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvLanguageSelector.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvLanguageSelector.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLanguageSelector.EnableHeadersVisualStyles = false;
            this.dgvLanguageSelector.Location = new System.Drawing.Point(450, 100);
            this.dgvLanguageSelector.MultiSelect = false;
            this.dgvLanguageSelector.Name = "dgvLanguageSelector";
            this.dgvLanguageSelector.RowHeadersVisible = false;
            this.dgvLanguageSelector.Size = new System.Drawing.Size(460, 500);
            this.dgvLanguageSelector.TabIndex = 0;
            this.dgvLanguageSelector.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvLanguageSelector_CellClick);
            this.dgvLanguageSelector.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvLanguageSelector_CellValueChanged);
            this.dgvLanguageSelector.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvLanguageSelector_CurrentCellDirtyStateChanged);
            // 
            // BottomPanel
            // 
            this.BottomPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BottomPanel.BackColor = System.Drawing.Color.SteelBlue;
            this.BottomPanel.Location = new System.Drawing.Point(0, 600);
            this.BottomPanel.Name = "BottomPanel";
            this.BottomPanel.Size = new System.Drawing.Size(910, 50);
            this.BottomPanel.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 39);
            this.label1.TabIndex = 4;
            this.label1.Text = "Setting";
            // 
            // topPanel
            // 
            this.topPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.topPanel.BackColor = System.Drawing.Color.SteelBlue;
            this.topPanel.Controls.Add(this.label1);
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(910, 50);
            this.topPanel.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panel3.BackColor = System.Drawing.Color.LightBlue;
            this.panel3.Controls.Add(this.textExportLocationLabel);
            this.panel3.Controls.Add(this.textExportBrowseButton);
            this.panel3.Controls.Add(this.audioExportBrowseButton);
            this.panel3.Controls.Add(this.audioExportLocationLabel);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.upUpdateFreq);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Location = new System.Drawing.Point(0, 50);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(450, 550);
            this.panel3.TabIndex = 3;
            // 
            // upUpdateFreq
            // 
            this.upUpdateFreq.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.upUpdateFreq.BackColor = System.Drawing.Color.DeepSkyBlue;
            this.upUpdateFreq.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.upUpdateFreq.CausesValidation = false;
            this.upUpdateFreq.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.upUpdateFreq.ForeColor = System.Drawing.SystemColors.ControlText;
            this.upUpdateFreq.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.upUpdateFreq.Location = new System.Drawing.Point(234, 20);
            this.upUpdateFreq.Margin = new System.Windows.Forms.Padding(2);
            this.upUpdateFreq.Maximum = new decimal(new int[] {
            1440,
            0,
            0,
            0});
            this.upUpdateFreq.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.upUpdateFreq.Name = "upUpdateFreq";
            this.upUpdateFreq.Size = new System.Drawing.Size(40, 18);
            this.upUpdateFreq.TabIndex = 8;
            this.upUpdateFreq.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.upUpdateFreq.ValueChanged += new System.EventHandler(this.upUpdateFreq_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.LightBlue;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label3.Location = new System.Drawing.Point(10, 20);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(215, 16);
            this.label3.TabIndex = 9;
            this.label3.Text = "Update Frequency ( Minutes ):";
            // 
            // panel4
            // 
            this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel4.BackColor = System.Drawing.Color.SkyBlue;
            this.panel4.Controls.Add(this.label2);
            this.panel4.Location = new System.Drawing.Point(450, 50);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(460, 50);
            this.panel4.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label2.Location = new System.Drawing.Point(0, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(437, 31);
            this.label2.TabIndex = 5;
            this.label2.Text = "Text To Speech Language/Voice";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.LightBlue;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label4.Location = new System.Drawing.Point(10, 70);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(167, 16);
            this.label4.TabIndex = 10;
            this.label4.Text = "Export Audio Location :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.LightBlue;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label5.Location = new System.Drawing.Point(10, 123);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(157, 16);
            this.label5.TabIndex = 11;
            this.label5.Text = "Export Text Location :";
            // 
            // audioExportLocationLabel
            // 
            this.audioExportLocationLabel.AutoSize = true;
            this.audioExportLocationLabel.Location = new System.Drawing.Point(214, 73);
            this.audioExportLocationLabel.Name = "audioExportLocationLabel";
            this.audioExportLocationLabel.Size = new System.Drawing.Size(0, 13);
            this.audioExportLocationLabel.TabIndex = 12;
            // 
            // audioExportBrowseButton
            // 
            this.audioExportBrowseButton.Location = new System.Drawing.Point(182, 67);
            this.audioExportBrowseButton.Name = "audioExportBrowseButton";
            this.audioExportBrowseButton.Size = new System.Drawing.Size(26, 23);
            this.audioExportBrowseButton.TabIndex = 13;
            this.audioExportBrowseButton.Text = "...";
            this.audioExportBrowseButton.UseVisualStyleBackColor = true;
            this.audioExportBrowseButton.Click += new System.EventHandler(this.audioExportBrowseButton_Click);
            // 
            // textExportBrowseButton
            // 
            this.textExportBrowseButton.Location = new System.Drawing.Point(182, 120);
            this.textExportBrowseButton.Name = "textExportBrowseButton";
            this.textExportBrowseButton.Size = new System.Drawing.Size(26, 23);
            this.textExportBrowseButton.TabIndex = 14;
            this.textExportBrowseButton.Text = "...";
            this.textExportBrowseButton.UseVisualStyleBackColor = true;
            this.textExportBrowseButton.Click += new System.EventHandler(this.textExportBrowseButton_Click);
            // 
            // textExportLocationLabel
            // 
            this.textExportLocationLabel.AutoSize = true;
            this.textExportLocationLabel.Location = new System.Drawing.Point(214, 125);
            this.textExportLocationLabel.Name = "textExportLocationLabel";
            this.textExportLocationLabel.Size = new System.Drawing.Size(0, 13);
            this.textExportLocationLabel.TabIndex = 15;
            // 
            // SettingController
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.BottomPanel);
            this.Controls.Add(this.topPanel);
            this.Controls.Add(this.dgvLanguageSelector);
            this.Name = "SettingController";
            this.Size = new System.Drawing.Size(910, 650);
            this.Load += new System.EventHandler(this.SettingController_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLanguageSelector)).EndInit();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.upUpdateFreq)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvLanguageSelector;
        private System.Windows.Forms.Panel BottomPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown upUpdateFreq;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label textExportLocationLabel;
        private System.Windows.Forms.Button textExportBrowseButton;
        private System.Windows.Forms.Button audioExportBrowseButton;
        private System.Windows.Forms.Label audioExportLocationLabel;
        private System.Windows.Forms.FolderBrowserDialog exportFolderBrowser;
    }
}
