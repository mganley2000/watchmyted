namespace Energy.EnergyWatcher
{
    partial class TEDConfigurationForm
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
            this.butSave = new System.Windows.Forms.Button();
            this.butCancel = new System.Windows.Forms.Button();
            this.txtGatewayURL1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.chkEnable1 = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtGatewayURL2 = new System.Windows.Forms.TextBox();
            this.tabTEDGateway = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.txtGatewayURL3 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tabTEDGateway.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // butSave
            // 
            this.butSave.Location = new System.Drawing.Point(251, 372);
            this.butSave.Name = "butSave";
            this.butSave.Size = new System.Drawing.Size(75, 23);
            this.butSave.TabIndex = 0;
            this.butSave.Text = "Save";
            this.butSave.UseVisualStyleBackColor = true;
            this.butSave.Click += new System.EventHandler(this.butSave_Click);
            // 
            // butCancel
            // 
            this.butCancel.Location = new System.Drawing.Point(332, 372);
            this.butCancel.Name = "butCancel";
            this.butCancel.Size = new System.Drawing.Size(75, 23);
            this.butCancel.TabIndex = 1;
            this.butCancel.Text = "Cancel";
            this.butCancel.UseVisualStyleBackColor = true;
            this.butCancel.Click += new System.EventHandler(this.butCancel_Click);
            // 
            // txtGatewayURL1
            // 
            this.txtGatewayURL1.Location = new System.Drawing.Point(113, 57);
            this.txtGatewayURL1.Name = "txtGatewayURL1";
            this.txtGatewayURL1.Size = new System.Drawing.Size(456, 20);
            this.txtGatewayURL1.TabIndex = 2;
            this.txtGatewayURL1.Text = "http://ted5000/history/minutehistory.xml?MTU=0&INDEX=1&COUNT=10";
            // 
            // label2
            // 
            this.label2.AllowDrop = true;
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(33, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Minute URL";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(253, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(154, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Access TED Gateway API";
            // 
            // chkEnable1
            // 
            this.chkEnable1.AutoSize = true;
            this.chkEnable1.Checked = true;
            this.chkEnable1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkEnable1.Location = new System.Drawing.Point(66, 19);
            this.chkEnable1.Name = "chkEnable1";
            this.chkEnable1.Size = new System.Drawing.Size(15, 14);
            this.chkEnable1.TabIndex = 5;
            this.chkEnable1.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(16, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Enable";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(62, 90);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(403, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Example:   http://ted5000/history/minutehistory.xml?MTU=0&&INDEX=1&&COUNT=10";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(62, 169);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(407, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Example:   http://ted5000/history/secondhistory.xml?MTU=0&&INDEX=1&&COUNT=10";
            // 
            // label8
            // 
            this.label8.AllowDrop = true;
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(28, 139);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(79, 13);
            this.label8.TabIndex = 9;
            this.label8.Text = "Second URL";
            // 
            // txtGatewayURL2
            // 
            this.txtGatewayURL2.Location = new System.Drawing.Point(113, 136);
            this.txtGatewayURL2.Name = "txtGatewayURL2";
            this.txtGatewayURL2.Size = new System.Drawing.Size(456, 20);
            this.txtGatewayURL2.TabIndex = 8;
            this.txtGatewayURL2.Text = "http://ted5000/history/secondhistory.xml?MTU=0&INDEX=1&COUNT=10";
            // 
            // tabTEDGateway
            // 
            this.tabTEDGateway.Controls.Add(this.tabPage1);
            this.tabTEDGateway.Location = new System.Drawing.Point(12, 34);
            this.tabTEDGateway.Name = "tabTEDGateway";
            this.tabTEDGateway.SelectedIndex = 0;
            this.tabTEDGateway.Size = new System.Drawing.Size(630, 320);
            this.tabTEDGateway.TabIndex = 14;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label6);
            this.tabPage1.Controls.Add(this.txtGatewayURL3);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.txtGatewayURL1);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.chkEnable1);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.txtGatewayURL2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(622, 294);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "MTU 0";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(63, 245);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(400, 13);
            this.label6.TabIndex = 16;
            this.label6.Text = "Example:   http://ted5000/history/hourlyhistory.xml?MTU=0&&INDEX=1&&COUNT=10";
            // 
            // txtGatewayURL3
            // 
            this.txtGatewayURL3.Location = new System.Drawing.Point(113, 212);
            this.txtGatewayURL3.Name = "txtGatewayURL3";
            this.txtGatewayURL3.Size = new System.Drawing.Size(456, 20);
            this.txtGatewayURL3.TabIndex = 15;
            this.txtGatewayURL3.Text = "http://ted5000/history/hourlyhistory.xml?MTU=0&INDEX=1&COUNT=10";
            // 
            // label7
            // 
            this.label7.AllowDrop = true;
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(44, 215);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "Hour URL";
            // 
            // TEDConfigurationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(654, 407);
            this.Controls.Add(this.tabTEDGateway);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.butCancel);
            this.Controls.Add(this.butSave);
            this.Name = "TEDConfigurationForm";
            this.Text = "TED Configuration";
            this.tabTEDGateway.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button butSave;
        private System.Windows.Forms.Button butCancel;
        private System.Windows.Forms.TextBox txtGatewayURL1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkEnable1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtGatewayURL2;
        private System.Windows.Forms.TabControl tabTEDGateway;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TextBox txtGatewayURL3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
    }
}