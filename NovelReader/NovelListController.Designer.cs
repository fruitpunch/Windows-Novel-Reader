﻿namespace NovelReader
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvNovelList = new System.Windows.Forms.DataGridView();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnRankUp = new System.Windows.Forms.Button();
            this.btnRankDown = new System.Windows.Forms.Button();
            this.btnAddNovel = new System.Windows.Forms.Button();
            this.labelLastUpdateTime = new System.Windows.Forms.Label();
            this.refreshUpdateLabelTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dgvNovelList)).BeginInit();
            this.SuspendLayout();
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
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvNovelList.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvNovelList.Location = new System.Drawing.Point(40, 40);
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
            this.btnTest.Location = new System.Drawing.Point(158, 610);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 23);
            this.btnTest.TabIndex = 1;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnRankUp
            // 
            this.btnRankUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRankUp.Location = new System.Drawing.Point(871, 222);
            this.btnRankUp.Name = "btnRankUp";
            this.btnRankUp.Size = new System.Drawing.Size(73, 23);
            this.btnRankUp.TabIndex = 2;
            this.btnRankUp.Text = "Rank Up";
            this.btnRankUp.UseVisualStyleBackColor = true;
            this.btnRankUp.Click += new System.EventHandler(this.btnRankUp_Click);
            // 
            // btnRankDown
            // 
            this.btnRankDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRankDown.Location = new System.Drawing.Point(871, 291);
            this.btnRankDown.Name = "btnRankDown";
            this.btnRankDown.Size = new System.Drawing.Size(73, 23);
            this.btnRankDown.TabIndex = 3;
            this.btnRankDown.Text = "Rank Down";
            this.btnRankDown.UseVisualStyleBackColor = true;
            this.btnRankDown.Click += new System.EventHandler(this.btnRankDown_Click);
            // 
            // btnAddNovel
            // 
            this.btnAddNovel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddNovel.Location = new System.Drawing.Point(40, 610);
            this.btnAddNovel.Name = "btnAddNovel";
            this.btnAddNovel.Size = new System.Drawing.Size(75, 23);
            this.btnAddNovel.TabIndex = 4;
            this.btnAddNovel.Text = "Add Novel";
            this.btnAddNovel.UseVisualStyleBackColor = true;
            this.btnAddNovel.Click += new System.EventHandler(this.btnAddNovel_Click);
            // 
            // labelLastUpdateTime
            // 
            this.labelLastUpdateTime.AutoSize = true;
            this.labelLastUpdateTime.Location = new System.Drawing.Point(37, 11);
            this.labelLastUpdateTime.Name = "labelLastUpdateTime";
            this.labelLastUpdateTime.Size = new System.Drawing.Size(35, 13);
            this.labelLastUpdateTime.TabIndex = 5;
            this.labelLastUpdateTime.Text = "label1";
            // 
            // refreshUpdateLabelTimer
            // 
            this.refreshUpdateLabelTimer.Interval = 1000;
            this.refreshUpdateLabelTimer.Tick += new System.EventHandler(this.refreshUpdateLabelTimer_Tick);
            // 
            // NovelListController
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelLastUpdateTime);
            this.Controls.Add(this.btnAddNovel);
            this.Controls.Add(this.btnRankDown);
            this.Controls.Add(this.btnRankUp);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.dgvNovelList);
            this.Name = "NovelListController";
            this.Size = new System.Drawing.Size(970, 650);
            this.Load += new System.EventHandler(this.NovelListController_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvNovelList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvNovelList;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnRankUp;
        private System.Windows.Forms.Button btnRankDown;
        private System.Windows.Forms.Button btnAddNovel;
        private System.Windows.Forms.Label labelLastUpdateTime;
        private System.Windows.Forms.Timer refreshUpdateLabelTimer;
    }
}
