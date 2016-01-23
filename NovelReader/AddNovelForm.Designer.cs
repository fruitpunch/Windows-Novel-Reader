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
            this.btnSourceLink = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // sourceSelector
            // 
            this.sourceSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sourceSelector.FormattingEnabled = true;
            this.sourceSelector.Location = new System.Drawing.Point(121, 16);
            this.sourceSelector.Margin = new System.Windows.Forms.Padding(4);
            this.sourceSelector.Name = "sourceSelector";
            this.sourceSelector.Size = new System.Drawing.Size(161, 24);
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
            this.btnCancel.Location = new System.Drawing.Point(160, 200);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(150, 50);
            this.btnCancel.TabIndex = 15;
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
            this.btnConfirm.Location = new System.Drawing.Point(10, 200);
            this.btnConfirm.Margin = new System.Windows.Forms.Padding(4);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(150, 50);
            this.btnConfirm.TabIndex = 14;
            this.btnConfirm.Text = "Confirm";
            this.btnConfirm.UseVisualStyleBackColor = false;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // labelSourceID
            // 
            this.labelSourceID.AutoSize = true;
            this.labelSourceID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSourceID.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.labelSourceID.Location = new System.Drawing.Point(17, 88);
            this.labelSourceID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelSourceID.Name = "labelSourceID";
            this.labelSourceID.Size = new System.Drawing.Size(76, 16);
            this.labelSourceID.TabIndex = 13;
            this.labelSourceID.Text = "Source ID";
            // 
            // inputSourceID
            // 
            this.inputSourceID.Location = new System.Drawing.Point(121, 85);
            this.inputSourceID.Margin = new System.Windows.Forms.Padding(4);
            this.inputSourceID.Name = "inputSourceID";
            this.inputSourceID.Size = new System.Drawing.Size(161, 22);
            this.inputSourceID.TabIndex = 12;
            this.inputSourceID.TextChanged += new System.EventHandler(this.inputSourceID_TextChanged);
            // 
            // labelNovelSource
            // 
            this.labelNovelSource.AutoSize = true;
            this.labelNovelSource.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNovelSource.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.labelNovelSource.Location = new System.Drawing.Point(17, 19);
            this.labelNovelSource.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelNovelSource.Name = "labelNovelSource";
            this.labelNovelSource.Size = new System.Drawing.Size(102, 16);
            this.labelNovelSource.TabIndex = 11;
            this.labelNovelSource.Text = "Novel Source";
            // 
            // labelNovelTitle
            // 
            this.labelNovelTitle.AutoSize = true;
            this.labelNovelTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNovelTitle.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.labelNovelTitle.Location = new System.Drawing.Point(19, 154);
            this.labelNovelTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelNovelTitle.Name = "labelNovelTitle";
            this.labelNovelTitle.Size = new System.Drawing.Size(84, 16);
            this.labelNovelTitle.TabIndex = 10;
            this.labelNovelTitle.Text = "Novel Title";
            // 
            // inputNovelTitle
            // 
            this.inputNovelTitle.Location = new System.Drawing.Point(121, 150);
            this.inputNovelTitle.Margin = new System.Windows.Forms.Padding(4);
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
            this.labelStatus.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.labelStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStatus.Location = new System.Drawing.Point(123, 118);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(0, 16);
            this.labelStatus.TabIndex = 17;
            // 
            // btnSourceLink
            // 
            this.btnSourceLink.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.btnSourceLink.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSourceLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSourceLink.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnSourceLink.Location = new System.Drawing.Point(121, 47);
            this.btnSourceLink.Name = "btnSourceLink";
            this.btnSourceLink.Size = new System.Drawing.Size(161, 23);
            this.btnSourceLink.TabIndex = 19;
            this.btnSourceLink.Text = "Source Link";
            this.btnSourceLink.UseVisualStyleBackColor = false;
            this.btnSourceLink.Click += new System.EventHandler(this.btnSourceLink_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.panel1.Controls.Add(this.labelNovelSource);
            this.panel1.Controls.Add(this.btnSourceLink);
            this.panel1.Controls.Add(this.inputNovelTitle);
            this.panel1.Controls.Add(this.labelStatus);
            this.panel1.Controls.Add(this.labelNovelTitle);
            this.panel1.Controls.Add(this.sourceSelector);
            this.panel1.Controls.Add(this.inputSourceID);
            this.panel1.Controls.Add(this.labelSourceID);
            this.panel1.Location = new System.Drawing.Point(10, 10);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(300, 190);
            this.panel1.TabIndex = 20;
            // 
            // AddNovelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.ClientSize = new System.Drawing.Size(322, 261);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConfirm);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "AddNovelForm";
            this.Text = "AddNovelForm";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

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
        private System.Windows.Forms.Button btnSourceLink;
        private System.Windows.Forms.Panel panel1;
    }
}