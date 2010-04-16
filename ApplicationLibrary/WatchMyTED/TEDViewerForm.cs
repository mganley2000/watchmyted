using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Energy.Library;

namespace Energy.WatchMyTED
{
    /// <summary>
    /// View TED data via query of TED Gateway
    /// </summary>
    public partial class TEDViewerForm : Form
    {
        private Configuration config = null;

        public TEDViewerForm()
        {
            InitializeComponent();

            config = DatabaseController.GetConfiguration();

            // make invisible unless using for testing in development
            butParseMinutes.Visible = false;
            butParseSeconds.Visible = false;
        }

        private void butView_Click(object sender, EventArgs e)
        {
            webMain.Navigate(config.TEDUrl1);
        }

        private void butParseMinutes_Click(object sender, EventArgs e)
        {
            TEDMinuteList test = null;

            test = TEDController.GetMinutes(config);
        }

        private void butView2_Click(object sender, EventArgs e)
        {
            webMain.Navigate(config.TEDUrl2);
        }

        private void butParseSeconds_Click(object sender, EventArgs e)
        {
            TEDSecondList test = null;

            test = TEDController.GetSeconds(config);
        }
    }
}
