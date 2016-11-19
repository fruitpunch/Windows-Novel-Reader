namespace NovelReader
{
    partial class NovelSourceController
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.refreshUpdateLabelTimer = new System.Windows.Forms.Timer(this.components);
            this.btnFinishEdit = new System.Windows.Forms.Button();
            this.bottomPanel = new System.Windows.Forms.Panel();
            this.topPanel = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.mainBackPanel = new System.Windows.Forms.Panel();
            this.BlackListBackPanel = new System.Windows.Forms.Panel();
            this.dgvBlackList = new System.Windows.Forms.DataGridView();
            this.BlackListControlPanel = new System.Windows.Forms.Panel();
            this.btnRemoveBlackListItem = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.sourcesBackPanel = new System.Windows.Forms.Panel();
            this.dgvNovelSources = new System.Windows.Forms.DataGridView();
            this.sourceControllerPanel = new System.Windows.Forms.Panel();
            this.btnRankDownSource = new System.Windows.Forms.Button();
            this.btnRankUpSource = new System.Windows.Forms.Button();
            this.btnRemoveSource = new System.Windows.Forms.Button();
            this.btnAddSource = new System.Windows.Forms.Button();
            this.panel8 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.bottomPanel.SuspendLayout();
            this.topPanel.SuspendLayout();
            this.mainBackPanel.SuspendLayout();
            this.BlackListBackPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBlackList)).BeginInit();
            this.BlackListControlPanel.SuspendLayout();
            this.panel4.SuspendLayout();
            this.sourcesBackPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNovelSources)).BeginInit();
            this.sourceControllerPanel.SuspendLayout();
            this.panel8.SuspendLayout();
            this.SuspendLayout();
            // 
            // refreshUpdateLabelTimer
            // 
            this.refreshUpdateLabelTimer.Interval = 1000;
            // 
            // btnFinishEdit
            // 
            this.btnFinishEdit.BackColor = System.Drawing.Color.SteelBlue;
            this.btnFinishEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFinishEdit.FlatAppearance.BorderSize = 0;
            this.btnFinishEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFinishEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFinishEdit.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnFinishEdit.Location = new System.Drawing.Point(0, 0);
            this.btnFinishEdit.Margin = new System.Windows.Forms.Padding(2);
            this.btnFinishEdit.Name = "btnFinishEdit";
            this.btnFinishEdit.Size = new System.Drawing.Size(910, 50);
            this.btnFinishEdit.TabIndex = 4;
            this.btnFinishEdit.Text = "Finish";
            this.btnFinishEdit.UseVisualStyleBackColor = false;
            this.btnFinishEdit.Click += new System.EventHandler(this.btnFinishEdit_Click);
            // 
            // bottomPanel
            // 
            this.bottomPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.bottomPanel.BackColor = System.Drawing.Color.SteelBlue;
            this.bottomPanel.Controls.Add(this.btnFinishEdit);
            this.bottomPanel.Location = new System.Drawing.Point(0, 600);
            this.bottomPanel.Name = "bottomPanel";
            this.bottomPanel.Size = new System.Drawing.Size(910, 50);
            this.bottomPanel.TabIndex = 11;
            // 
            // topPanel
            // 
            this.topPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.topPanel.BackColor = System.Drawing.Color.SteelBlue;
            this.topPanel.Controls.Add(this.panel7);
            this.topPanel.Controls.Add(this.label2);
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(910, 50);
            this.topPanel.TabIndex = 12;
            // 
            // panel7
            // 
            this.panel7.Location = new System.Drawing.Point(0, 50);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(910, 545);
            this.panel7.TabIndex = 18;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(286, 39);
            this.label2.TabIndex = 6;
            this.label2.Text = "Source Manager";
            // 
            // mainBackPanel
            // 
            this.mainBackPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainBackPanel.Controls.Add(this.BlackListBackPanel);
            this.mainBackPanel.Controls.Add(this.sourcesBackPanel);
            this.mainBackPanel.Location = new System.Drawing.Point(0, 50);
            this.mainBackPanel.Name = "mainBackPanel";
            this.mainBackPanel.Size = new System.Drawing.Size(910, 550);
            this.mainBackPanel.TabIndex = 13;
            this.mainBackPanel.SizeChanged += new System.EventHandler(this.mainBackPanel_SizeChanged);
            // 
            // BlackListBackPanel
            // 
            this.BlackListBackPanel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BlackListBackPanel.AutoSize = true;
            this.BlackListBackPanel.BackColor = System.Drawing.Color.PowderBlue;
            this.BlackListBackPanel.Controls.Add(this.dgvBlackList);
            this.BlackListBackPanel.Controls.Add(this.BlackListControlPanel);
            this.BlackListBackPanel.Controls.Add(this.panel4);
            this.BlackListBackPanel.Location = new System.Drawing.Point(460, 5);
            this.BlackListBackPanel.Name = "BlackListBackPanel";
            this.BlackListBackPanel.Size = new System.Drawing.Size(445, 540);
            this.BlackListBackPanel.TabIndex = 16;
            // 
            // dgvBlackList
            // 
            this.dgvBlackList.AllowUserToAddRows = false;
            this.dgvBlackList.AllowUserToDeleteRows = false;
            this.dgvBlackList.AllowUserToResizeRows = false;
            this.dgvBlackList.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dgvBlackList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvBlackList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightSalmon;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(5);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvBlackList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvBlackList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvBlackList.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvBlackList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBlackList.EnableHeadersVisualStyles = false;
            this.dgvBlackList.Location = new System.Drawing.Point(0, 45);
            this.dgvBlackList.Margin = new System.Windows.Forms.Padding(2);
            this.dgvBlackList.MultiSelect = false;
            this.dgvBlackList.Name = "dgvBlackList";
            this.dgvBlackList.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            this.dgvBlackList.RowHeadersVisible = false;
            this.dgvBlackList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBlackList.Size = new System.Drawing.Size(445, 450);
            this.dgvBlackList.TabIndex = 1;
            this.dgvBlackList.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvBlackList_CellFormatting);
            // 
            // BlackListControlPanel
            // 
            this.BlackListControlPanel.BackColor = System.Drawing.Color.SkyBlue;
            this.BlackListControlPanel.Controls.Add(this.btnRemoveBlackListItem);
            this.BlackListControlPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.BlackListControlPanel.Location = new System.Drawing.Point(0, 495);
            this.BlackListControlPanel.Name = "BlackListControlPanel";
            this.BlackListControlPanel.Size = new System.Drawing.Size(445, 45);
            this.BlackListControlPanel.TabIndex = 17;
            // 
            // btnRemoveBlackListItem
            // 
            this.btnRemoveBlackListItem.BackColor = System.Drawing.Color.SteelBlue;
            this.btnRemoveBlackListItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnRemoveBlackListItem.FlatAppearance.BorderSize = 0;
            this.btnRemoveBlackListItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveBlackListItem.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemoveBlackListItem.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnRemoveBlackListItem.Location = new System.Drawing.Point(0, 0);
            this.btnRemoveBlackListItem.Margin = new System.Windows.Forms.Padding(2);
            this.btnRemoveBlackListItem.Name = "btnRemoveBlackListItem";
            this.btnRemoveBlackListItem.Size = new System.Drawing.Size(445, 45);
            this.btnRemoveBlackListItem.TabIndex = 5;
            this.btnRemoveBlackListItem.Text = "Remove Black List Item";
            this.btnRemoveBlackListItem.UseVisualStyleBackColor = false;
            this.btnRemoveBlackListItem.Click += new System.EventHandler(this.btnRemoveBlackListItem_Click);
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.SkyBlue;
            this.panel4.Controls.Add(this.label3);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(445, 45);
            this.panel4.TabIndex = 16;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Left;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(237, 33);
            this.label3.TabIndex = 8;
            this.label3.Text = "Blacklisted URL";
            // 
            // sourcesBackPanel
            // 
            this.sourcesBackPanel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.sourcesBackPanel.AutoSize = true;
            this.sourcesBackPanel.BackColor = System.Drawing.Color.PowderBlue;
            this.sourcesBackPanel.Controls.Add(this.dgvNovelSources);
            this.sourcesBackPanel.Controls.Add(this.sourceControllerPanel);
            this.sourcesBackPanel.Controls.Add(this.panel8);
            this.sourcesBackPanel.Location = new System.Drawing.Point(5, 5);
            this.sourcesBackPanel.Name = "sourcesBackPanel";
            this.sourcesBackPanel.Size = new System.Drawing.Size(445, 540);
            this.sourcesBackPanel.TabIndex = 15;
            this.sourcesBackPanel.SizeChanged += new System.EventHandler(this.sourcesBackPanel_SizeChanged);
            // 
            // dgvNovelSources
            // 
            this.dgvNovelSources.AllowUserToAddRows = false;
            this.dgvNovelSources.AllowUserToDeleteRows = false;
            this.dgvNovelSources.AllowUserToResizeRows = false;
            this.dgvNovelSources.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dgvNovelSources.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvNovelSources.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.LightSalmon;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.Padding = new System.Windows.Forms.Padding(5);
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvNovelSources.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvNovelSources.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvNovelSources.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvNovelSources.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvNovelSources.EnableHeadersVisualStyles = false;
            this.dgvNovelSources.Location = new System.Drawing.Point(0, 45);
            this.dgvNovelSources.Margin = new System.Windows.Forms.Padding(2);
            this.dgvNovelSources.MultiSelect = false;
            this.dgvNovelSources.Name = "dgvNovelSources";
            this.dgvNovelSources.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Sunken;
            this.dgvNovelSources.RowHeadersVisible = false;
            this.dgvNovelSources.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvNovelSources.Size = new System.Drawing.Size(445, 450);
            this.dgvNovelSources.TabIndex = 17;
            this.dgvNovelSources.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvNovelSources_CellFormatting);
            // 
            // sourceControllerPanel
            // 
            this.sourceControllerPanel.BackColor = System.Drawing.Color.SkyBlue;
            this.sourceControllerPanel.Controls.Add(this.btnRankDownSource);
            this.sourceControllerPanel.Controls.Add(this.btnRankUpSource);
            this.sourceControllerPanel.Controls.Add(this.btnRemoveSource);
            this.sourceControllerPanel.Controls.Add(this.btnAddSource);
            this.sourceControllerPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.sourceControllerPanel.Location = new System.Drawing.Point(0, 495);
            this.sourceControllerPanel.Name = "sourceControllerPanel";
            this.sourceControllerPanel.Size = new System.Drawing.Size(445, 45);
            this.sourceControllerPanel.TabIndex = 16;
            // 
            // btnRankDownSource
            // 
            this.btnRankDownSource.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btnRankDownSource.BackColor = System.Drawing.Color.SteelBlue;
            this.btnRankDownSource.FlatAppearance.BorderSize = 0;
            this.btnRankDownSource.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRankDownSource.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRankDownSource.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnRankDownSource.Location = new System.Drawing.Point(333, 0);
            this.btnRankDownSource.Margin = new System.Windows.Forms.Padding(2);
            this.btnRankDownSource.Name = "btnRankDownSource";
            this.btnRankDownSource.Size = new System.Drawing.Size(111, 45);
            this.btnRankDownSource.TabIndex = 9;
            this.btnRankDownSource.Text = "Rank Down";
            this.btnRankDownSource.UseVisualStyleBackColor = false;
            this.btnRankDownSource.Click += new System.EventHandler(this.btnRankDownSource_Click);
            // 
            // btnRankUpSource
            // 
            this.btnRankUpSource.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btnRankUpSource.BackColor = System.Drawing.Color.SteelBlue;
            this.btnRankUpSource.FlatAppearance.BorderSize = 0;
            this.btnRankUpSource.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRankUpSource.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRankUpSource.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnRankUpSource.Location = new System.Drawing.Point(222, 0);
            this.btnRankUpSource.Margin = new System.Windows.Forms.Padding(2);
            this.btnRankUpSource.Name = "btnRankUpSource";
            this.btnRankUpSource.Size = new System.Drawing.Size(111, 45);
            this.btnRankUpSource.TabIndex = 8;
            this.btnRankUpSource.Text = "Rank Up";
            this.btnRankUpSource.UseVisualStyleBackColor = false;
            this.btnRankUpSource.Click += new System.EventHandler(this.btnRankUpSource_Click);
            // 
            // btnRemoveSource
            // 
            this.btnRemoveSource.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btnRemoveSource.BackColor = System.Drawing.Color.SteelBlue;
            this.btnRemoveSource.FlatAppearance.BorderSize = 0;
            this.btnRemoveSource.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveSource.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemoveSource.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnRemoveSource.Location = new System.Drawing.Point(111, 0);
            this.btnRemoveSource.Margin = new System.Windows.Forms.Padding(2);
            this.btnRemoveSource.Name = "btnRemoveSource";
            this.btnRemoveSource.Size = new System.Drawing.Size(111, 45);
            this.btnRemoveSource.TabIndex = 7;
            this.btnRemoveSource.Text = "Remove";
            this.btnRemoveSource.UseVisualStyleBackColor = false;
            this.btnRemoveSource.Click += new System.EventHandler(this.btnRemoveSource_Click);
            // 
            // btnAddSource
            // 
            this.btnAddSource.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.btnAddSource.BackColor = System.Drawing.Color.SteelBlue;
            this.btnAddSource.FlatAppearance.BorderSize = 0;
            this.btnAddSource.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddSource.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddSource.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnAddSource.Location = new System.Drawing.Point(0, 0);
            this.btnAddSource.Margin = new System.Windows.Forms.Padding(2);
            this.btnAddSource.Name = "btnAddSource";
            this.btnAddSource.Size = new System.Drawing.Size(111, 45);
            this.btnAddSource.TabIndex = 6;
            this.btnAddSource.Text = "Add";
            this.btnAddSource.UseVisualStyleBackColor = false;
            this.btnAddSource.Click += new System.EventHandler(this.btnAddSource_Click);
            // 
            // panel8
            // 
            this.panel8.BackColor = System.Drawing.Color.SkyBlue;
            this.panel8.Controls.Add(this.label1);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel8.Location = new System.Drawing.Point(0, 0);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(445, 45);
            this.panel8.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 33);
            this.label1.TabIndex = 7;
            this.label1.Text = "Sources";
            // 
            // NovelSourceController
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.LightGray;
            this.Controls.Add(this.mainBackPanel);
            this.Controls.Add(this.bottomPanel);
            this.Controls.Add(this.topPanel);
            this.Name = "NovelSourceController";
            this.Size = new System.Drawing.Size(910, 650);
            this.bottomPanel.ResumeLayout(false);
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.mainBackPanel.ResumeLayout(false);
            this.mainBackPanel.PerformLayout();
            this.BlackListBackPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBlackList)).EndInit();
            this.BlackListControlPanel.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.sourcesBackPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvNovelSources)).EndInit();
            this.sourceControllerPanel.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer refreshUpdateLabelTimer;
        private System.Windows.Forms.Button btnFinishEdit;
        private System.Windows.Forms.Panel bottomPanel;
        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel mainBackPanel;
        private System.Windows.Forms.Panel BlackListBackPanel;
        private System.Windows.Forms.DataGridView dgvBlackList;
        private System.Windows.Forms.Panel BlackListControlPanel;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel sourcesBackPanel;
        private System.Windows.Forms.Panel sourceControllerPanel;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnRemoveBlackListItem;
        private System.Windows.Forms.DataGridView dgvNovelSources;
        private System.Windows.Forms.Button btnRankDownSource;
        private System.Windows.Forms.Button btnRankUpSource;
        private System.Windows.Forms.Button btnRemoveSource;
        private System.Windows.Forms.Button btnAddSource;
    }
}
