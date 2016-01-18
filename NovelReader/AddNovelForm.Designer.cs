namespace NovelReader
{
    partial class AddNovelForm
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
            this.components = new System.ComponentModel.Container();
            this.sourceSelector = new System.Windows.Forms.ComboBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.labelSourceID = new System.Windows.Forms.Label();
            this.inputSourceID = new System.Windows.Forms.TextBox();
            this.labelNovelSource = new System.Windows.Forms.Label();
            this.labelNovelTitle = new System.Windows.Forms.Label();
            this.inputNovelTitle = new System.Windows.Forms.TextBox();
            this.sourceChecker = new System.ComponentModel.BackgroundWorker();
            this.networkTimeoutTimer = new System.Windows.Forms.Timer(this.components);
            this.textCheckerTimer = new System.Windows.Forms.Timer(this.components);
            this.labelStatus = new System.Windows.Forms.Label();
            this.sourceLinkLabel = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // sourceSelector
            // 
            this.sourceSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sourceSelector.FormattingEnabled = true;
            this.sourceSelector.Location = new System.Drawing.Point(147, 41);
            this.sourceSelector.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.sourceSelector.Name = "sourceSelector";
            this.sourceSelector.Size = new System.Drawing.Size(161, 24);
            this.sourceSelector.TabIndex = 9;
            this.sourceSelector.SelectionChangeCommitted += new System.EventHandler(this.sourceSelector_SelectionChangeCommitted);
            // 
            // btnCancel
            // 
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(208, 218);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 45);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfirm.Location = new System.Drawing.Point(44, 218);
            this.btnConfirm.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(100, 45);
            this.btnConfirm.TabIndex = 14;
            this.btnConfirm.Text = "Confirm";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // labelSourceID
            // 
            this.labelSourceID.AutoSize = true;
            this.labelSourceID.Location = new System.Drawing.Point(43, 97);
            this.labelSourceID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSourceID.Name = "labelSourceID";
            this.labelSourceID.Size = new System.Drawing.Size(67, 16);
            this.labelSourceID.TabIndex = 13;
            this.labelSourceID.Text = "Source ID";
            // 
            // inputSourceID
            // 
            this.inputSourceID.Location = new System.Drawing.Point(147, 94);
            this.inputSourceID.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.inputSourceID.Name = "inputSourceID";
            this.inputSourceID.Size = new System.Drawing.Size(161, 22);
            this.inputSourceID.TabIndex = 12;
            this.inputSourceID.TextChanged += new System.EventHandler(this.inputSourceID_TextChanged);
            // 
            // labelNovelSource
            // 
            this.labelNovelSource.AutoSize = true;
            this.labelNovelSource.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNovelSource.Location = new System.Drawing.Point(43, 44);
            this.labelNovelSource.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelNovelSource.Name = "labelNovelSource";
            this.labelNovelSource.Size = new System.Drawing.Size(90, 16);
            this.labelNovelSource.TabIndex = 11;
            this.labelNovelSource.Text = "Novel Source";
            // 
            // labelNovelTitle
            // 
            this.labelNovelTitle.AutoSize = true;
            this.labelNovelTitle.Location = new System.Drawing.Point(45, 163);
            this.labelNovelTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelNovelTitle.Name = "labelNovelTitle";
            this.labelNovelTitle.Size = new System.Drawing.Size(73, 16);
            this.labelNovelTitle.TabIndex = 10;
            this.labelNovelTitle.Text = "Novel Title";
            // 
            // inputNovelTitle
            // 
            this.inputNovelTitle.Location = new System.Drawing.Point(147, 159);
            this.inputNovelTitle.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.inputNovelTitle.Name = "inputNovelTitle";
            this.inputNovelTitle.Size = new System.Drawing.Size(161, 22);
            this.inputNovelTitle.TabIndex = 16;
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
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStatus.Location = new System.Drawing.Point(149, 127);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(0, 16);
            this.labelStatus.TabIndex = 17;
            // 
            // sourceLinkLabel
            // 
            this.sourceLinkLabel.AutoSize = true;
            this.sourceLinkLabel.Location = new System.Drawing.Point(147, 74);
            this.sourceLinkLabel.Name = "sourceLinkLabel";
            this.sourceLinkLabel.Size = new System.Drawing.Size(78, 16);
            this.sourceLinkLabel.TabIndex = 18;
            this.sourceLinkLabel.TabStop = true;
            this.sourceLinkLabel.Text = "Source Link";
            this.sourceLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.sourceLinkLabel_LinkClicked);
            // 
            // AddNovelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 322);
            this.Controls.Add(this.sourceLinkLabel);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.sourceSelector);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.labelSourceID);
            this.Controls.Add(this.inputSourceID);
            this.Controls.Add(this.labelNovelSource);
            this.Controls.Add(this.labelNovelTitle);
            this.Controls.Add(this.inputNovelTitle);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "AddNovelForm";
            this.Text = "AddNovelForm";
            this.Load += new System.EventHandler(this.AddNovelForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox sourceSelector;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.Label labelSourceID;
        private System.Windows.Forms.TextBox inputSourceID;
        private System.Windows.Forms.Label labelNovelSource;
        private System.Windows.Forms.Label labelNovelTitle;
        private System.Windows.Forms.TextBox inputNovelTitle;
        private System.ComponentModel.BackgroundWorker sourceChecker;
        private System.Windows.Forms.Timer networkTimeoutTimer;
        private System.Windows.Forms.Timer textCheckerTimer;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.LinkLabel sourceLinkLabel;
    }
}