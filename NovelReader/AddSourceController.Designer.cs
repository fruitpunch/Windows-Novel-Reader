namespace NovelReader
{
    partial class AddSourceController
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
            this.sourceSelector = new System.Windows.Forms.ComboBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.labelNovelSource = new System.Windows.Forms.Label();
            this.inputSourceID = new System.Windows.Forms.TextBox();
            this.labelSourceID = new System.Windows.Forms.Label();
            this.sourceChecker = new System.ComponentModel.BackgroundWorker();
            this.networkTimeoutTimer = new System.Windows.Forms.Timer(this.components);
            this.textCheckerTimer = new System.Windows.Forms.Timer(this.components);
            this.btnSourceLink = new System.Windows.Forms.Button();
            this.labelStatus = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // sourceSelector
            // 
            this.sourceSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sourceSelector.FormattingEnabled = true;
            this.sourceSelector.Location = new System.Drawing.Point(97, 8);
            this.sourceSelector.Margin = new System.Windows.Forms.Padding(4);
            this.sourceSelector.Name = "sourceSelector";
            this.sourceSelector.Size = new System.Drawing.Size(344, 21);
            this.sourceSelector.TabIndex = 9;
            this.sourceSelector.SelectionChangeCommitted += new System.EventHandler(this.sourceSelector_SelectionChangeCommitted);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnCancel.Location = new System.Drawing.Point(222, 80);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(223, 50);
            this.btnCancel.TabIndex = 22;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.btnConfirm.FlatAppearance.BorderSize = 0;
            this.btnConfirm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConfirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfirm.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnConfirm.Location = new System.Drawing.Point(0, 80);
            this.btnConfirm.Margin = new System.Windows.Forms.Padding(4);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(223, 50);
            this.btnConfirm.TabIndex = 21;
            this.btnConfirm.Text = "Confirm";
            this.btnConfirm.UseVisualStyleBackColor = false;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // labelNovelSource
            // 
            this.labelNovelSource.AutoSize = true;
            this.labelNovelSource.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNovelSource.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.labelNovelSource.Location = new System.Drawing.Point(13, 9);
            this.labelNovelSource.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelNovelSource.Name = "labelNovelSource";
            this.labelNovelSource.Size = new System.Drawing.Size(57, 16);
            this.labelNovelSource.TabIndex = 11;
            this.labelNovelSource.Text = "Source";
            // 
            // inputSourceID
            // 
            this.inputSourceID.Location = new System.Drawing.Point(97, 59);
            this.inputSourceID.Margin = new System.Windows.Forms.Padding(4);
            this.inputSourceID.Name = "inputSourceID";
            this.inputSourceID.Size = new System.Drawing.Size(344, 20);
            this.inputSourceID.TabIndex = 12;
            this.inputSourceID.TextChanged += new System.EventHandler(this.inputSourceID_TextChanged);
            // 
            // labelSourceID
            // 
            this.labelSourceID.AutoSize = true;
            this.labelSourceID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSourceID.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.labelSourceID.Location = new System.Drawing.Point(13, 60);
            this.labelSourceID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSourceID.Name = "labelSourceID";
            this.labelSourceID.Size = new System.Drawing.Size(76, 16);
            this.labelSourceID.TabIndex = 13;
            this.labelSourceID.Text = "Source ID";
            // 
            // sourceChecker
            // 
            this.sourceChecker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.sourceChecker_DoWork);
            this.sourceChecker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.sourceChecker_RunWorkerCompleted);
            // 
            // networkTimeoutTimer
            // 
            this.networkTimeoutTimer.Interval = 10000;
            this.networkTimeoutTimer.Tick += new System.EventHandler(this.networkTimer_Tick);
            // 
            // textCheckerTimer
            // 
            this.textCheckerTimer.Interval = 1500;
            this.textCheckerTimer.Tick += new System.EventHandler(this.textCheckerTimer_Tick);
            // 
            // btnSourceLink
            // 
            this.btnSourceLink.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.btnSourceLink.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSourceLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSourceLink.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnSourceLink.Location = new System.Drawing.Point(97, 29);
            this.btnSourceLink.Name = "btnSourceLink";
            this.btnSourceLink.Size = new System.Drawing.Size(344, 23);
            this.btnSourceLink.TabIndex = 19;
            this.btnSourceLink.Text = "Source Link";
            this.btnSourceLink.UseVisualStyleBackColor = false;
            this.btnSourceLink.Click += new System.EventHandler(this.btnSourceLink_Click);
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.labelStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStatus.Location = new System.Drawing.Point(220, 35);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(0, 16);
            this.labelStatus.TabIndex = 17;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.panel1.Controls.Add(this.labelNovelSource);
            this.panel1.Controls.Add(this.btnSourceLink);
            this.panel1.Controls.Add(this.labelStatus);
            this.panel1.Controls.Add(this.sourceSelector);
            this.panel1.Controls.Add(this.inputSourceID);
            this.panel1.Controls.Add(this.labelSourceID);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(445, 81);
            this.panel1.TabIndex = 23;
            // 
            // AddSourceController
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.panel1);
            this.MaximumSize = new System.Drawing.Size(445, 130);
            this.MinimumSize = new System.Drawing.Size(445, 130);
            this.Name = "AddSourceController";
            this.Size = new System.Drawing.Size(445, 130);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox sourceSelector;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Label labelNovelSource;
        private System.Windows.Forms.TextBox inputSourceID;
        private System.Windows.Forms.Label labelSourceID;
        private System.ComponentModel.BackgroundWorker sourceChecker;
        private System.Windows.Forms.Timer networkTimeoutTimer;
        private System.Windows.Forms.Timer textCheckerTimer;
        private System.Windows.Forms.Button btnSourceLink;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Panel panel1;
    }
}
