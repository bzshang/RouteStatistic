namespace RouteStatistic
{
    partial class RouteStatEditor
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblSummaryType = new System.Windows.Forms.Label();
            this.cbSummaryType = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(15, 71);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(148, 71);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblSummaryType
            // 
            this.lblSummaryType.AutoSize = true;
            this.lblSummaryType.Location = new System.Drawing.Point(13, 25);
            this.lblSummaryType.Name = "lblSummaryType";
            this.lblSummaryType.Size = new System.Drawing.Size(83, 13);
            this.lblSummaryType.TabIndex = 2;
            this.lblSummaryType.Text = "Summary Type :";
            // 
            // cbSummaryType
            // 
            this.cbSummaryType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSummaryType.FormattingEnabled = true;
            this.cbSummaryType.Location = new System.Drawing.Point(102, 22);
            this.cbSummaryType.Name = "cbSummaryType";
            this.cbSummaryType.Size = new System.Drawing.Size(121, 21);
            this.cbSummaryType.TabIndex = 3;
            // 
            // RouteStatEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(249, 123);
            this.Controls.Add(this.cbSummaryType);
            this.Controls.Add(this.lblSummaryType);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Name = "RouteStatEditor";
            this.Text = "RouteStatEditor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblSummaryType;
        private System.Windows.Forms.ComboBox cbSummaryType;
    }
}