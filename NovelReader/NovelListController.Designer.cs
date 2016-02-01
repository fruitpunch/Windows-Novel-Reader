namespace NovelReader
{
    partial class NovelListController
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.refreshUpdateLabelTimer = new System.Windows.Forms.Timer(this.components);
            this.dgvNovelList = new System.Windows.Forms.DataGridView();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnRankUp = new System.Windows.Forms.Button();
            this.btnRankDown = new System.Windows.Forms.Button();
            this.btnAddNovel = new System.Windows.Forms.Button();
            this.labelLastUpdateTime = new System.Windows.Forms.Label();
            this.upUpdateFreq = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.btnDeleteNovel = new System.Windows.Forms.Button();
            this.topPanel = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNovelList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.upUpdateFreq)).BeginInit();
            this.bottomPanel.SuspendLayout();
            this.topPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // refreshUpdateLabelTimer
            // 
            this.refreshUpdateLabelTimer.Interval = 1000;
            this.refreshUpdateLabelTimer.Tick += new System.EventHandler(this.refreshUpdateLabelTimer_Tick);
            // 
            // dgvNovelList
            // 
            this.dgvNovelList.AllowUserToAddRows = false;
            this.dgvNovelList.AllowUserToDeleteRows = false;
            this.dgvNovelList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvNovelList.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dgvNovelList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvNovelList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.LightSalmon;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(5);
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvNovelList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvNovelList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvNovelList.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvNovelList.EnableHeadersVisualStyles = false;
            this.dgvNovelList.Location = new System.Drawing.Point(0, 50);
            this.dgvNovelList.Margin = new System.Windows.Forms.Padding(2);
            this.dgvNovelList.MultiSelect = false;
            this.dgvNovelList.Name = "dgvNovelList";
            this.dgvNovelList.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            this.dgvNovelList.RowHeadersVisible = false;
            this.dgvNovelList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvNovelList.Size = new System.Drawing.Size(910, 550);
            this.dgvNovelList.TabIndex = 0;
            this.dgvNovelList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvNovelList_CellClick);
            this.dgvNovelList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvNovelList_CellDoubleClick);
            this.dgvNovelList.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvNovelList_CellFormatting);
            this.dgvNovelList.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvNovelList_CellValueChanged);
            this.dgvNovelList.CurrentCellDirtyStateChanged += new System.EventHandler(this.dgvNovelList_CurrentCellDirtyStateChanged);
            this.dgvNovelList.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvNovelList_RowPostPaint);
            this.dgvNovelList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgvNovelList_MouseClick);
            // 
            // btnTest
            // 
            this.btnTest.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTest.BackColor = System.Drawing.Color.SteelBlue;
            this.btnTest.FlatAppearance.BorderSize = 0;
            this.btnTest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTest.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTest.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnTest.Location = new System.Drawing.Point(780, 0);
            this.btnTest.Margin = new System.Windows.Forms.Padding(2);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(130, 50);
            this.btnTest.TabIndex = 1;
            this.btnTest.Text = "Update Novels";
            this.btnTest.UseVisualStyleBackColor = false;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnRankUp
            // 
            this.btnRankUp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRankUp.BackColor = System.Drawing.Color.SteelBlue;
            this.btnRankUp.FlatAppearance.BorderSize = 0;
            this.btnRankUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRankUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRankUp.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnRankUp.Location = new System.Drawing.Point(0, 0);
            this.btnRankUp.Margin = new System.Windows.Forms.Padding(2);
            this.btnRankUp.Name = "btnRankUp";
            this.btnRankUp.Size = new System.Drawing.Size(100, 50);
            this.btnRankUp.TabIndex = 2;
            this.btnRankUp.Text = "Rank Up";
            this.btnRankUp.UseVisualStyleBackColor = false;
            this.btnRankUp.Click += new System.EventHandler(this.btnRankUp_Click);
            // 
            // btnRankDown
            // 
            this.btnRankDown.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRankDown.BackColor = System.Drawing.Color.SteelBlue;
            this.btnRankDown.FlatAppearance.BorderSize = 0;
            this.btnRankDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRankDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRankDown.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnRankDown.Location = new System.Drawing.Point(100, 0);
            this.btnRankDown.Margin = new System.Windows.Forms.Padding(2);
            this.btnRankDown.Name = "btnRankDown";
            this.btnRankDown.Size = new System.Drawing.Size(100, 50);
            this.btnRankDown.TabIndex = 3;
            this.btnRankDown.Text = "Rank Down";
            this.btnRankDown.UseVisualStyleBackColor = false;
            this.btnRankDown.Click += new System.EventHandler(this.btnRankDown_Click);
            // 
            // btnAddNovel
            // 
            this.btnAddNovel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddNovel.BackColor = System.Drawing.Color.SteelBlue;
            this.btnAddNovel.FlatAppearance.BorderSize = 0;
            this.btnAddNovel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddNovel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddNovel.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnAddNovel.Location = new System.Drawing.Point(550, 0);
            this.btnAddNovel.Margin = new System.Windows.Forms.Padding(2);
            this.btnAddNovel.Name = "btnAddNovel";
            this.btnAddNovel.Size = new System.Drawing.Size(100, 50);
            this.btnAddNovel.TabIndex = 4;
            this.btnAddNovel.Text = "Add Novel";
            this.btnAddNovel.UseVisualStyleBackColor = false;
            this.btnAddNovel.Click += new System.EventHandler(this.btnAddNovel_Click);
            // 
            // labelLastUpdateTime
            // 
            this.labelLastUpdateTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelLastUpdateTime.AutoSize = true;
            this.labelLastUpdateTime.BackColor = System.Drawing.Color.SteelBlue;
            this.labelLastUpdateTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLastUpdateTime.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.labelLastUpdateTime.Location = new System.Drawing.Point(666, 17);
            this.labelLastUpdateTime.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelLastUpdateTime.Name = "labelLastUpdateTime";
            this.labelLastUpdateTime.Size = new System.Drawing.Size(150, 16);
            this.labelLastUpdateTime.TabIndex = 5;
            this.labelLastUpdateTime.Text = "Last Updated: X Ago";
            // 
            // upUpdateFreq
            // 
            this.upUpdateFreq.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.upUpdateFreq.BackColor = System.Drawing.Color.SteelBlue;
            this.upUpdateFreq.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.upUpdateFreq.CausesValidation = false;
            this.upUpdateFreq.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.upUpdateFreq.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.upUpdateFreq.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.upUpdateFreq.Location = new System.Drawing.Point(430, 18);
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
            this.upUpdateFreq.TabIndex = 6;
            this.upUpdateFreq.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.upUpdateFreq.ValueChanged += new System.EventHandler(this.upUpdateFreq_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.SteelBlue;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Location = new System.Drawing.Point(210, 17);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(215, 16);
            this.label1.TabIndex = 7;
            this.label1.Text = "Update Frequency ( Minutes ):";
            // 
            // bottomPanel
            // 
            this.bottomPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bottomPanel.BackColor = System.Drawing.Color.SteelBlue;
            this.bottomPanel.Controls.Add(this.btnDeleteNovel);
            this.bottomPanel.Controls.Add(this.btnTest);
            this.bottomPanel.Controls.Add(this.upUpdateFreq);
            this.bottomPanel.Controls.Add(this.btnAddNovel);
            this.bottomPanel.Controls.Add(this.label1);
            this.bottomPanel.Controls.Add(this.btnRankUp);
            this.bottomPanel.Controls.Add(this.btnRankDown);
            this.bottomPanel.Location = new System.Drawing.Point(0, 600);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(910, 50);
            this.bottomPanel.TabIndex = 8;
            // 
            // btnDeleteNovel
            // 
            this.btnDeleteNovel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteNovel.FlatAppearance.BorderSize = 0;
            this.btnDeleteNovel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteNovel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeleteNovel.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnDeleteNovel.Location = new System.Drawing.Point(650, 0);
            this.btnDeleteNovel.Name = "btnDeleteNovel";
            this.btnDeleteNovel.Size = new System.Drawing.Size(130, 50);
            this.btnDeleteNovel.TabIndex = 8;
            this.btnDeleteNovel.Text = "Delete Novel";
            this.btnDeleteNovel.UseVisualStyleBackColor = true;
            this.btnDeleteNovel.Click += new System.EventHandler(this.btnDeleteNovel_Click);
            // 
            // topPanel
            // 
            this.topPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.topPanel.BackColor = System.Drawing.Color.SteelBlue;
            this.topPanel.Controls.Add(this.label2);
            this.topPanel.Controls.Add(this.labelLastUpdateTime);
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(910, 50);
            this.topPanel.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(180, 39);
            this.label2.TabIndex = 6;
            this.label2.Text = "Novel List";
            // 
            // NovelListController
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.Controls.Add(this.topPanel);
            this.Controls.Add(this.bottomPanel);
            this.Controls.Add(this.dgvNovelList);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "NovelListController";
            this.Size = new System.Drawing.Size(910, 650);
            this.Load += new System.EventHandler(this.NovelListController_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvNovelList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.upUpdateFreq)).EndInit();
            this.bottomPanel.ResumeLayout(false);
            this.bottomPanel.PerformLayout();
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer refreshUpdateLabelTimer;
        private System.Windows.Forms.DataGridView dgvNovelList;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnRankUp;
        private System.Windows.Forms.Button btnRankDown;
        private System.Windows.Forms.Button btnAddNovel;
        private System.Windows.Forms.Label labelLastUpdateTime;
        private System.Windows.Forms.NumericUpDown upUpdateFreq;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel bottomPanel;
        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnDeleteNovel;
    }
}
