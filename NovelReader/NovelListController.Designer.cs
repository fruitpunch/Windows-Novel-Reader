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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.refreshUpdateLabelTimer = new System.Windows.Forms.Timer(this.components);
            this.dgvNovelList = new System.Windows.Forms.DataGridView();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnRankUp = new System.Windows.Forms.Button();
            this.btnRankDown = new System.Windows.Forms.Button();
            this.btnAddNovel = new System.Windows.Forms.Button();
            this.labelLastUpdateTime = new System.Windows.Forms.Label();
            this.upUpdateFreq = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNovelList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.upUpdateFreq)).BeginInit();
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
            this.dgvNovelList.AllowUserToResizeRows = false;
            this.dgvNovelList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvNovelList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvNovelList.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvNovelList.Location = new System.Drawing.Point(40, 40);
            this.dgvNovelList.Margin = new System.Windows.Forms.Padding(2);
            this.dgvNovelList.MultiSelect = false;
            this.dgvNovelList.Name = "dgvNovelList";
            this.dgvNovelList.RowHeadersVisible = false;
            this.dgvNovelList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvNovelList.Size = new System.Drawing.Size(800, 550);
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
            this.btnTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnTest.Location = new System.Drawing.Point(145, 604);
            this.btnTest.Margin = new System.Windows.Forms.Padding(2);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(99, 37);
            this.btnTest.TabIndex = 1;
            this.btnTest.Text = "Update Novels";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnRankUp
            // 
            this.btnRankUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRankUp.Location = new System.Drawing.Point(865, 245);
            this.btnRankUp.Margin = new System.Windows.Forms.Padding(2);
            this.btnRankUp.Name = "btnRankUp";
            this.btnRankUp.Size = new System.Drawing.Size(75, 37);
            this.btnRankUp.TabIndex = 2;
            this.btnRankUp.Text = "Rank Up";
            this.btnRankUp.UseVisualStyleBackColor = true;
            this.btnRankUp.Click += new System.EventHandler(this.btnRankUp_Click);
            // 
            // btnRankDown
            // 
            this.btnRankDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRankDown.Location = new System.Drawing.Point(865, 314);
            this.btnRankDown.Margin = new System.Windows.Forms.Padding(2);
            this.btnRankDown.Name = "btnRankDown";
            this.btnRankDown.Size = new System.Drawing.Size(75, 37);
            this.btnRankDown.TabIndex = 3;
            this.btnRankDown.Text = "Rank Down";
            this.btnRankDown.UseVisualStyleBackColor = true;
            this.btnRankDown.Click += new System.EventHandler(this.btnRankDown_Click);
            // 
            // btnAddNovel
            // 
            this.btnAddNovel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddNovel.Location = new System.Drawing.Point(40, 604);
            this.btnAddNovel.Margin = new System.Windows.Forms.Padding(2);
            this.btnAddNovel.Name = "btnAddNovel";
            this.btnAddNovel.Size = new System.Drawing.Size(75, 37);
            this.btnAddNovel.TabIndex = 4;
            this.btnAddNovel.Text = "Add Novel";
            this.btnAddNovel.UseVisualStyleBackColor = true;
            this.btnAddNovel.Click += new System.EventHandler(this.btnAddNovel_Click);
            // 
            // labelLastUpdateTime
            // 
            this.labelLastUpdateTime.AutoSize = true;
            this.labelLastUpdateTime.Location = new System.Drawing.Point(278, 13);
            this.labelLastUpdateTime.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelLastUpdateTime.Name = "labelLastUpdateTime";
            this.labelLastUpdateTime.Size = new System.Drawing.Size(106, 13);
            this.labelLastUpdateTime.TabIndex = 5;
            this.labelLastUpdateTime.Text = "Last Updated: X Ago";
            // 
            // upUpdateFreq
            // 
            this.upUpdateFreq.CausesValidation = false;
            this.upUpdateFreq.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.upUpdateFreq.Location = new System.Drawing.Point(172, 11);
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
            this.upUpdateFreq.Size = new System.Drawing.Size(47, 20);
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
            this.label1.Location = new System.Drawing.Point(28, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Update Frequency(Minutes)";
            // 
            // NovelListController
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.upUpdateFreq);
            this.Controls.Add(this.labelLastUpdateTime);
            this.Controls.Add(this.btnAddNovel);
            this.Controls.Add(this.btnRankDown);
            this.Controls.Add(this.btnRankUp);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.dgvNovelList);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "NovelListController";
            this.Size = new System.Drawing.Size(970, 650);
            this.Load += new System.EventHandler(this.NovelListController_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvNovelList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.upUpdateFreq)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
    }
}
