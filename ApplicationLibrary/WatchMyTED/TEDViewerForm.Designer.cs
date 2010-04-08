namespace Energy.EnergyWatcher
{
    partial class TEDViewerForm
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
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.webMain = new System.Windows.Forms.WebBrowser();
            this.butParseMinutes = new System.Windows.Forms.Button();
            this.butView = new System.Windows.Forms.Button();
            this.butParseSeconds = new System.Windows.Forms.Button();
            this.butView2 = new System.Windows.Forms.Button();
            this.tabTEDQuery = new System.Windows.Forms.TabControl();
            this.tabPage1.SuspendLayout();
            this.tabTEDQuery.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.butView2);
            this.tabPage1.Controls.Add(this.butParseSeconds);
            this.tabPage1.Controls.Add(this.butView);
            this.tabPage1.Controls.Add(this.butParseMinutes);
            this.tabPage1.Controls.Add(this.webMain);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(378, 414);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "MTU 0";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // webMain
            // 
            this.webMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.webMain.Location = new System.Drawing.Point(6, 68);
            this.webMain.MinimumSize = new System.Drawing.Size(20, 20);
            this.webMain.Name = "webMain";
            this.webMain.Size = new System.Drawing.Size(366, 340);
            this.webMain.TabIndex = 4;
            this.webMain.Url = new System.Uri("", System.UriKind.Relative);
            // 
            // butParseMinutes
            // 
            this.butParseMinutes.Location = new System.Drawing.Point(6, 39);
            this.butParseMinutes.Name = "butParseMinutes";
            this.butParseMinutes.Size = new System.Drawing.Size(92, 23);
            this.butParseMinutes.TabIndex = 5;
            this.butParseMinutes.Text = "Test Parse";
            this.butParseMinutes.UseVisualStyleBackColor = true;
            this.butParseMinutes.Click += new System.EventHandler(this.butParseMinutes_Click);
            // 
            // butView
            // 
            this.butView.Location = new System.Drawing.Point(6, 10);
            this.butView.Name = "butView";
            this.butView.Size = new System.Drawing.Size(128, 23);
            this.butView.TabIndex = 3;
            this.butView.Text = "Get TED Minutes";
            this.butView.UseVisualStyleBackColor = true;
            this.butView.Click += new System.EventHandler(this.butView_Click);
            // 
            // butParseSeconds
            // 
            this.butParseSeconds.Location = new System.Drawing.Point(158, 39);
            this.butParseSeconds.Name = "butParseSeconds";
            this.butParseSeconds.Size = new System.Drawing.Size(92, 23);
            this.butParseSeconds.TabIndex = 7;
            this.butParseSeconds.Text = "Test Parse";
            this.butParseSeconds.UseVisualStyleBackColor = true;
            this.butParseSeconds.Click += new System.EventHandler(this.butParseSeconds_Click);
            // 
            // butView2
            // 
            this.butView2.Location = new System.Drawing.Point(158, 10);
            this.butView2.Name = "butView2";
            this.butView2.Size = new System.Drawing.Size(128, 23);
            this.butView2.TabIndex = 6;
            this.butView2.Text = "Get TED Seconds";
            this.butView2.UseVisualStyleBackColor = true;
            this.butView2.Click += new System.EventHandler(this.butView2_Click);
            // 
            // tabTEDQuery
            // 
            this.tabTEDQuery.Controls.Add(this.tabPage1);
            this.tabTEDQuery.Location = new System.Drawing.Point(12, 12);
            this.tabTEDQuery.Name = "tabTEDQuery";
            this.tabTEDQuery.SelectedIndex = 0;
            this.tabTEDQuery.Size = new System.Drawing.Size(386, 440);
            this.tabTEDQuery.TabIndex = 8;
            // 
            // TEDViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 465);
            this.Controls.Add(this.tabTEDQuery);
            this.Name = "TEDViewerForm";
            this.Text = "TED Query";
            this.tabPage1.ResumeLayout(false);
            this.tabTEDQuery.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Button butView2;
        private System.Windows.Forms.Button butParseSeconds;
        private System.Windows.Forms.Button butView;
        private System.Windows.Forms.Button butParseMinutes;
        private System.Windows.Forms.WebBrowser webMain;
        private System.Windows.Forms.TabControl tabTEDQuery;

    }
}