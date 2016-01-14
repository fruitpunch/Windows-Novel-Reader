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
            this.sourceSelector = new System.Windows.Forms.ComboBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.labelSourceID = new System.Windows.Forms.Label();
            this.inputSourceID = new System.Windows.Forms.TextBox();
            this.labelNovelSource = new System.Windows.Forms.Label();
            this.labelNovelTitle = new System.Windows.Forms.Label();
            this.inputNovelTitle = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // sourceSelector
            // 
            this.sourceSelector.FormattingEnabled = true;
            this.sourceSelector.Location = new System.Drawing.Point(116, 77);
            this.sourceSelector.Name = "sourceSelector";
            this.sourceSelector.Size = new System.Drawing.Size(121, 21);
            this.sourceSelector.TabIndex = 16;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(160, 198);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnConfirm
            // 
            this.btnConfirm.Location = new System.Drawing.Point(37, 198);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(75, 23);
            this.btnConfirm.TabIndex = 14;
            this.btnConfirm.Text = "Confirm";
            this.btnConfirm.UseVisualStyleBackColor = true;
            this.btnConfirm.Click += new System.EventHandler(this.btnConfirm_Click);
            // 
            // labelSourceID
            // 
            this.labelSourceID.AutoSize = true;
            this.labelSourceID.Location = new System.Drawing.Point(34, 129);
            this.labelSourceID.Name = "labelSourceID";
            this.labelSourceID.Size = new System.Drawing.Size(55, 13);
            this.labelSourceID.TabIndex = 13;
            this.labelSourceID.Text = "Source ID";
            // 
            // inputSourceID
            // 
            this.inputSourceID.Location = new System.Drawing.Point(116, 126);
            this.inputSourceID.Name = "inputSourceID";
            this.inputSourceID.Size = new System.Drawing.Size(121, 20);
            this.inputSourceID.TabIndex = 12;
            // 
            // labelNovelSource
            // 
            this.labelNovelSource.AutoSize = true;
            this.labelNovelSource.Location = new System.Drawing.Point(34, 86);
            this.labelNovelSource.Name = "labelNovelSource";
            this.labelNovelSource.Size = new System.Drawing.Size(72, 13);
            this.labelNovelSource.TabIndex = 11;
            this.labelNovelSource.Text = "Novel Source";
            // 
            // labelNovelTitle
            // 
            this.labelNovelTitle.AutoSize = true;
            this.labelNovelTitle.Location = new System.Drawing.Point(34, 40);
            this.labelNovelTitle.Name = "labelNovelTitle";
            this.labelNovelTitle.Size = new System.Drawing.Size(58, 13);
            this.labelNovelTitle.TabIndex = 10;
            this.labelNovelTitle.Text = "Novel Title";
            // 
            // inputNovelTitle
            // 
            this.inputNovelTitle.Location = new System.Drawing.Point(113, 33);
            this.inputNovelTitle.Name = "inputNovelTitle";
            this.inputNovelTitle.Size = new System.Drawing.Size(124, 20);
            this.inputNovelTitle.TabIndex = 9;
            // 
            // AddNovelForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.sourceSelector);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.labelSourceID);
            this.Controls.Add(this.inputSourceID);
            this.Controls.Add(this.labelNovelSource);
            this.Controls.Add(this.labelNovelTitle);
            this.Controls.Add(this.inputNovelTitle);
            this.Name = "AddNovelForm";
            this.Text = "AddNovelForm";
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
    }
}