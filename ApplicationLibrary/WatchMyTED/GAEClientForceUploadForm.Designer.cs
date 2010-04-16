namespace Energy.WatchMyTED
{
    partial class GAEClientForceUploadForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.butUpload = new System.Windows.Forms.Button();
            this.labStatus = new System.Windows.Forms.Label();
            this.butDone = new System.Windows.Forms.Button();
            this.txtStartDatetime = new System.Windows.Forms.TextBox();
            this.labStartDatetime = new System.Windows.Forms.Label();
            this.txtEndDatetime = new System.Windows.Forms.TextBox();
            this.labEndDatetime = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(93, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(184, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "GAE Force Upload of Readings";
            // 
            // butUpload
            // 
            this.butUpload.Location = new System.Drawing.Point(35, 133);
            this.butUpload.Name = "butUpload";
            this.butUpload.Size = new System.Drawing.Size(75, 23);
            this.butUpload.TabIndex = 7;
            this.butUpload.Text = "Upload";
            this.butUpload.UseVisualStyleBackColor = true;
            this.butUpload.Click += new System.EventHandler(this.butUpload_Click);
            // 
            // labStatus
            // 
            this.labStatus.AutoSize = true;
            this.labStatus.Location = new System.Drawing.Point(116, 138);
            this.labStatus.Name = "labStatus";
            this.labStatus.Size = new System.Drawing.Size(41, 13);
            this.labStatus.TabIndex = 8;
            this.labStatus.Text = "-status-";
            // 
            // butDone
            // 
            this.butDone.Location = new System.Drawing.Point(144, 174);
            this.butDone.Name = "butDone";
            this.butDone.Size = new System.Drawing.Size(75, 23);
            this.butDone.TabIndex = 9;
            this.butDone.Text = "Done";
            this.butDone.UseVisualStyleBackColor = true;
            this.butDone.Click += new System.EventHandler(this.butDone_Click);
            // 
            // txtStartDatetime
            // 
            this.txtStartDatetime.Location = new System.Drawing.Point(116, 69);
            this.txtStartDatetime.Name = "txtStartDatetime";
            this.txtStartDatetime.Size = new System.Drawing.Size(195, 20);
            this.txtStartDatetime.TabIndex = 10;
            // 
            // labStartDatetime
            // 
            this.labStartDatetime.AutoSize = true;
            this.labStartDatetime.Location = new System.Drawing.Point(36, 72);
            this.labStartDatetime.Name = "labStartDatetime";
            this.labStartDatetime.Size = new System.Drawing.Size(74, 13);
            this.labStartDatetime.TabIndex = 11;
            this.labStartDatetime.Text = "Start Datetime";
            // 
            // txtEndDatetime
            // 
            this.txtEndDatetime.Location = new System.Drawing.Point(116, 96);
            this.txtEndDatetime.Name = "txtEndDatetime";
            this.txtEndDatetime.Size = new System.Drawing.Size(195, 20);
            this.txtEndDatetime.TabIndex = 12;
            // 
            // labEndDatetime
            // 
            this.labEndDatetime.AutoSize = true;
            this.labEndDatetime.Location = new System.Drawing.Point(39, 99);
            this.labEndDatetime.Name = "labEndDatetime";
            this.labEndDatetime.Size = new System.Drawing.Size(71, 13);
            this.labEndDatetime.TabIndex = 13;
            this.labEndDatetime.Text = "End Datetime";
            // 
            // GAEClientForceUploadForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(364, 209);
            this.Controls.Add(this.labEndDatetime);
            this.Controls.Add(this.txtEndDatetime);
            this.Controls.Add(this.labStartDatetime);
            this.Controls.Add(this.txtStartDatetime);
            this.Controls.Add(this.butDone);
            this.Controls.Add(this.labStatus);
            this.Controls.Add(this.butUpload);
            this.Controls.Add(this.label1);
            this.Name = "GAEClientForceUploadForm";
            this.Text = "Google App Engine Client Force Upload";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button butUpload;
        private System.Windows.Forms.Label labStatus;
        private System.Windows.Forms.Button butDone;
        private System.Windows.Forms.TextBox txtStartDatetime;
        private System.Windows.Forms.Label labStartDatetime;
        private System.Windows.Forms.TextBox txtEndDatetime;
        private System.Windows.Forms.Label labEndDatetime;
    }
}