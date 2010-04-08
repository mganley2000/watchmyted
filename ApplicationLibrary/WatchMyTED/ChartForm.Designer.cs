namespace Energy.EnergyWatcher
{
    partial class ChartForm
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
            this.panChart = new System.Windows.Forms.Panel();
            this.butClose = new System.Windows.Forms.Button();
            this.labChart = new System.Windows.Forms.Label();
            this.ddChart = new System.Windows.Forms.ComboBox();
            this.labDateSelect = new System.Windows.Forms.Label();
            this.ddDateSelect = new System.Windows.Forms.ComboBox();
            this.labMeter = new System.Windows.Forms.Label();
            this.ddMeter = new System.Windows.Forms.ComboBox();
            this.zedChart = new ZedGraph.ZedGraphControl();
            this.panChart.SuspendLayout();
            this.SuspendLayout();
            // 
            // panChart
            // 
            this.panChart.BackColor = System.Drawing.SystemColors.Control;
            this.panChart.Controls.Add(this.butClose);
            this.panChart.Controls.Add(this.labChart);
            this.panChart.Controls.Add(this.ddChart);
            this.panChart.Controls.Add(this.labDateSelect);
            this.panChart.Controls.Add(this.ddDateSelect);
            this.panChart.Controls.Add(this.labMeter);
            this.panChart.Controls.Add(this.ddMeter);
            this.panChart.Controls.Add(this.zedChart);
            this.panChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panChart.Location = new System.Drawing.Point(0, 0);
            this.panChart.Name = "panChart";
            this.panChart.Size = new System.Drawing.Size(642, 495);
            this.panChart.TabIndex = 0;
            // 
            // butClose
            // 
            this.butClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.butClose.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.butClose.Location = new System.Drawing.Point(555, 460);
            this.butClose.Name = "butClose";
            this.butClose.Size = new System.Drawing.Size(75, 23);
            this.butClose.TabIndex = 7;
            this.butClose.Text = "Close";
            this.butClose.UseVisualStyleBackColor = false;
            this.butClose.Click += new System.EventHandler(this.butClose_Click);
            // 
            // labChart
            // 
            this.labChart.AutoSize = true;
            this.labChart.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labChart.Location = new System.Drawing.Point(11, 49);
            this.labChart.Name = "labChart";
            this.labChart.Size = new System.Drawing.Size(37, 13);
            this.labChart.TabIndex = 6;
            this.labChart.Text = "Chart";
            // 
            // ddChart
            // 
            this.ddChart.FormattingEnabled = true;
            this.ddChart.Location = new System.Drawing.Point(54, 46);
            this.ddChart.Name = "ddChart";
            this.ddChart.Size = new System.Drawing.Size(121, 21);
            this.ddChart.TabIndex = 5;
            this.ddChart.SelectedIndexChanged += new System.EventHandler(this.ddChart_SelectedIndexChanged);
            // 
            // labDateSelect
            // 
            this.labDateSelect.AutoSize = true;
            this.labDateSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labDateSelect.Location = new System.Drawing.Point(214, 49);
            this.labDateSelect.Name = "labDateSelect";
            this.labDateSelect.Size = new System.Drawing.Size(71, 13);
            this.labDateSelect.TabIndex = 4;
            this.labDateSelect.Text = "View Dates";
            // 
            // ddDateSelect
            // 
            this.ddDateSelect.FormattingEnabled = true;
            this.ddDateSelect.Location = new System.Drawing.Point(291, 46);
            this.ddDateSelect.Name = "ddDateSelect";
            this.ddDateSelect.Size = new System.Drawing.Size(121, 21);
            this.ddDateSelect.TabIndex = 3;
            this.ddDateSelect.SelectedIndexChanged += new System.EventHandler(this.ddDateSelect_SelectedIndexChanged);
            // 
            // labMeter
            // 
            this.labMeter.AutoSize = true;
            this.labMeter.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labMeter.Location = new System.Drawing.Point(11, 15);
            this.labMeter.Name = "labMeter";
            this.labMeter.Size = new System.Drawing.Size(39, 13);
            this.labMeter.TabIndex = 2;
            this.labMeter.Text = "Meter";
            // 
            // ddMeter
            // 
            this.ddMeter.FormattingEnabled = true;
            this.ddMeter.Location = new System.Drawing.Point(54, 12);
            this.ddMeter.Name = "ddMeter";
            this.ddMeter.Size = new System.Drawing.Size(121, 21);
            this.ddMeter.TabIndex = 1;
            this.ddMeter.SelectedIndexChanged += new System.EventHandler(this.ddMeter_SelectedIndexChanged);
            // 
            // zedChart
            // 
            this.zedChart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.zedChart.AutoSize = true;
            this.zedChart.BackColor = System.Drawing.SystemColors.ControlLight;
            this.zedChart.Location = new System.Drawing.Point(12, 86);
            this.zedChart.Name = "zedChart";
            this.zedChart.ScrollGrace = 0;
            this.zedChart.ScrollMaxX = 0;
            this.zedChart.ScrollMaxY = 0;
            this.zedChart.ScrollMaxY2 = 0;
            this.zedChart.ScrollMinX = 0;
            this.zedChart.ScrollMinY = 0;
            this.zedChart.ScrollMinY2 = 0;
            this.zedChart.Size = new System.Drawing.Size(618, 358);
            this.zedChart.TabIndex = 0;
            // 
            // ChartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(642, 495);
            this.Controls.Add(this.panChart);
            this.Name = "ChartForm";
            this.Text = "Historical Charts";
            this.panChart.ResumeLayout(false);
            this.panChart.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panChart;
        private ZedGraph.ZedGraphControl zedChart;
        private System.Windows.Forms.Label labMeter;
        private System.Windows.Forms.ComboBox ddMeter;
        private System.Windows.Forms.Label labDateSelect;
        private System.Windows.Forms.ComboBox ddDateSelect;
        private System.Windows.Forms.Label labChart;
        private System.Windows.Forms.ComboBox ddChart;
        private System.Windows.Forms.Button butClose;
    }
}