namespace Energy.WatchMyTED
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loggingPowerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chart900SecondToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chart3600SecondToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chart720MinuteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chart1440MinuteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chartConsumptionhourlyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gAETestToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clientConfigurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clientTesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.forceUploadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tEDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.checkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.aboutWatchMyTEDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.zg1 = new ZedGraph.ZedGraphControl();
            this.ewNotify = new System.Windows.Forms.NotifyIcon(this.components);
            this.workBackgroundMain = new System.ComponentModel.BackgroundWorker();
            this.menuStrip1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.viewToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.toolStripMenuItem3});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(734, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem,
            this.loggingPowerToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(42, 20);
            this.toolStripMenuItem1.Text = "Data";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // loggingPowerToolStripMenuItem
            // 
            this.loggingPowerToolStripMenuItem.Checked = true;
            this.loggingPowerToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.loggingPowerToolStripMenuItem.Name = "loggingPowerToolStripMenuItem";
            this.loggingPowerToolStripMenuItem.Size = new System.Drawing.Size(191, 22);
            this.loggingPowerToolStripMenuItem.Text = "Log to Local Database";
            this.loggingPowerToolStripMenuItem.Click += new System.EventHandler(this.loggingPowerToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.chart900SecondToolStripMenuItem,
            this.chart3600SecondToolStripMenuItem,
            this.chart720MinuteToolStripMenuItem,
            this.chart1440MinuteToolStripMenuItem,
            this.chartConsumptionhourlyToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // chart900SecondToolStripMenuItem
            // 
            this.chart900SecondToolStripMenuItem.CheckOnClick = true;
            this.chart900SecondToolStripMenuItem.Name = "chart900SecondToolStripMenuItem";
            this.chart900SecondToolStripMenuItem.Size = new System.Drawing.Size(273, 22);
            this.chart900SecondToolStripMenuItem.Text = "Power per second Chart (last 15-min)";
            this.chart900SecondToolStripMenuItem.Click += new System.EventHandler(this.chart900SecondToolStripMenuItem_Click);
            // 
            // chart3600SecondToolStripMenuItem
            // 
            this.chart3600SecondToolStripMenuItem.Name = "chart3600SecondToolStripMenuItem";
            this.chart3600SecondToolStripMenuItem.Size = new System.Drawing.Size(273, 22);
            this.chart3600SecondToolStripMenuItem.Text = "Power per second Chart (last 60-min)";
            this.chart3600SecondToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chart3600SecondToolStripMenuItem.Click += new System.EventHandler(this.chart3600SecondToolStripMenuItem_Click);
            // 
            // chart720MinuteToolStripMenuItem
            // 
            this.chart720MinuteToolStripMenuItem.Name = "chart720MinuteToolStripMenuItem";
            this.chart720MinuteToolStripMenuItem.Size = new System.Drawing.Size(273, 22);
            this.chart720MinuteToolStripMenuItem.Text = "Power per minute Chart (last 12-hours)";
            this.chart720MinuteToolStripMenuItem.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.chart720MinuteToolStripMenuItem.Click += new System.EventHandler(this.chart720MinuteToolStripMenuItem_Click);
            // 
            // chart1440MinuteToolStripMenuItem
            // 
            this.chart1440MinuteToolStripMenuItem.Name = "chart1440MinuteToolStripMenuItem";
            this.chart1440MinuteToolStripMenuItem.Size = new System.Drawing.Size(273, 22);
            this.chart1440MinuteToolStripMenuItem.Text = "Power per minute Chart (last 24-hours)";
            this.chart1440MinuteToolStripMenuItem.Click += new System.EventHandler(this.chart1440MinuteToolStripMenuItem_Click);
            // 
            // chartConsumptionhourlyToolStripMenuItem
            // 
            this.chartConsumptionhourlyToolStripMenuItem.Name = "chartConsumptionhourlyToolStripMenuItem";
            this.chartConsumptionhourlyToolStripMenuItem.Size = new System.Drawing.Size(273, 22);
            this.chartConsumptionhourlyToolStripMenuItem.Text = "Live Hourly Energy Consumption Chart";
            this.chartConsumptionhourlyToolStripMenuItem.Click += new System.EventHandler(this.chartConsumptionhourlyToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gAETestToolStripMenuItem,
            this.tEDToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // gAETestToolStripMenuItem
            // 
            this.gAETestToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clientConfigurationToolStripMenuItem,
            this.clientTesToolStripMenuItem,
            this.forceUploadToolStripMenuItem});
            this.gAETestToolStripMenuItem.Name = "gAETestToolStripMenuItem";
            this.gAETestToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.gAETestToolStripMenuItem.Text = "Google App Engine";
            // 
            // clientConfigurationToolStripMenuItem
            // 
            this.clientConfigurationToolStripMenuItem.Name = "clientConfigurationToolStripMenuItem";
            this.clientConfigurationToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.clientConfigurationToolStripMenuItem.Text = "Client Configuration";
            this.clientConfigurationToolStripMenuItem.Click += new System.EventHandler(this.clientConfigurationToolStripMenuItem_Click);
            // 
            // clientTesToolStripMenuItem
            // 
            this.clientTesToolStripMenuItem.Name = "clientTesToolStripMenuItem";
            this.clientTesToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.clientTesToolStripMenuItem.Text = "Test Authentication";
            this.clientTesToolStripMenuItem.Click += new System.EventHandler(this.clientTesToolStripMenuItem_Click);
            // 
            // forceUploadToolStripMenuItem
            // 
            this.forceUploadToolStripMenuItem.Name = "forceUploadToolStripMenuItem";
            this.forceUploadToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.forceUploadToolStripMenuItem.Text = "Force Upload";
            this.forceUploadToolStripMenuItem.Click += new System.EventHandler(this.forceUploadToolStripMenuItem_Click);
            // 
            // tEDToolStripMenuItem
            // 
            this.tEDToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configurationToolStripMenuItem,
            this.viewToolStripMenuItem1});
            this.tEDToolStripMenuItem.Name = "tEDToolStripMenuItem";
            this.tEDToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.tEDToolStripMenuItem.Text = "The Energy Detective (TED)";
            // 
            // configurationToolStripMenuItem
            // 
            this.configurationToolStripMenuItem.Name = "configurationToolStripMenuItem";
            this.configurationToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.configurationToolStripMenuItem.Text = "Configuration";
            this.configurationToolStripMenuItem.Click += new System.EventHandler(this.configurationToolStripMenuItem_Click);
            // 
            // viewToolStripMenuItem1
            // 
            this.viewToolStripMenuItem1.Name = "viewToolStripMenuItem1";
            this.viewToolStripMenuItem1.Size = new System.Drawing.Size(150, 22);
            this.viewToolStripMenuItem1.Text = "Test Query";
            this.viewToolStripMenuItem1.Click += new System.EventHandler(this.viewToolStripMenuItem1_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.checkToolStripMenuItem,
            this.toolStripSeparator2,
            this.aboutWatchMyTEDToolStripMenuItem});
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.H)));
            this.toolStripMenuItem3.Size = new System.Drawing.Size(40, 20);
            this.toolStripMenuItem3.Text = "Help";
            // 
            // checkToolStripMenuItem
            // 
            this.checkToolStripMenuItem.Name = "checkToolStripMenuItem";
            this.checkToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.checkToolStripMenuItem.Text = "Contact";
            this.checkToolStripMenuItem.Click += new System.EventHandler(this.checkToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(184, 6);
            // 
            // aboutWatchMyTEDToolStripMenuItem
            // 
            this.aboutWatchMyTEDToolStripMenuItem.Name = "aboutWatchMyTEDToolStripMenuItem";
            this.aboutWatchMyTEDToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.aboutWatchMyTEDToolStripMenuItem.Text = "About Watch My TED";
            this.aboutWatchMyTEDToolStripMenuItem.Click += new System.EventHandler(this.aboutWatchMyTEDToolStripMenuItem_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 20);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.BackColor = System.Drawing.SystemColors.Control;
            this.splitContainer1.Panel1Collapsed = true;
            this.splitContainer1.Panel1MinSize = 0;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.BackColor = System.Drawing.SystemColors.Window;
            this.splitContainer1.Panel2.Controls.Add(this.statusStrip1);
            this.splitContainer1.Panel2.Controls.Add(this.zg1);
            this.splitContainer1.Size = new System.Drawing.Size(734, 493);
            this.splitContainer1.SplitterDistance = 151;
            this.splitContainer1.TabIndex = 3;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.statusLabel,
            this.statusProgress});
            this.statusStrip1.Location = new System.Drawing.Point(0, 471);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(734, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(0, 17);
            // 
            // statusLabel
            // 
            this.statusLabel.BackColor = System.Drawing.SystemColors.Menu;
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(56, 17);
            this.statusLabel.Text = "Loading...";
            // 
            // statusProgress
            // 
            this.statusProgress.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.statusProgress.MarqueeAnimationSpeed = 75;
            this.statusProgress.Name = "statusProgress";
            this.statusProgress.Overflow = System.Windows.Forms.ToolStripItemOverflow.Always;
            this.statusProgress.Size = new System.Drawing.Size(200, 16);
            // 
            // zg1
            // 
            this.zg1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.zg1.AutoScroll = true;
            this.zg1.AutoSize = true;
            this.zg1.Location = new System.Drawing.Point(17, 13);
            this.zg1.Name = "zg1";
            this.zg1.ScrollGrace = 0;
            this.zg1.ScrollMaxX = 0;
            this.zg1.ScrollMaxY = 0;
            this.zg1.ScrollMaxY2 = 0;
            this.zg1.ScrollMinX = 0;
            this.zg1.ScrollMinY = 0;
            this.zg1.ScrollMinY2 = 0;
            this.zg1.Size = new System.Drawing.Size(691, 413);
            this.zg1.TabIndex = 2;
            this.zg1.Visible = false;
            // 
            // ewNotify
            // 
            this.ewNotify.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.ewNotify.BalloonTipText = "Power and Energy Consumption";
            this.ewNotify.BalloonTipTitle = "Watch My TED";
            this.ewNotify.Icon = ((System.Drawing.Icon)(resources.GetObject("ewNotify.Icon")));
            this.ewNotify.Text = "Watch My TED";
            this.ewNotify.Visible = true;
            this.ewNotify.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ewNotify_MouseDoubleClick);
            // 
            // workBackgroundMain
            // 
            this.workBackgroundMain.WorkerReportsProgress = true;
            this.workBackgroundMain.WorkerSupportsCancellation = true;
            this.workBackgroundMain.DoWork += new System.ComponentModel.DoWorkEventHandler(this.workBackgroundMain_DoWork);
            this.workBackgroundMain.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.workBackgroundMain_RunWorkerCompleted);
            this.workBackgroundMain.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.workBackgroundMain_ProgressChanged);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(734, 517);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "Watch My TED";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem aboutWatchMyTEDToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loggingPowerToolStripMenuItem;
        private ZedGraph.ZedGraphControl zg1;
        private System.Windows.Forms.ToolStripMenuItem chart900SecondToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem chart3600SecondToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem chart720MinuteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem chartConsumptionhourlyToolStripMenuItem;
        private System.Windows.Forms.NotifyIcon ewNotify;
        private System.Windows.Forms.ToolStripMenuItem gAETestToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clientTesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clientConfigurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem forceUploadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tEDToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem chart1440MinuteToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker workBackgroundMain;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripProgressBar1;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.ToolStripProgressBar statusProgress;

    }
}

