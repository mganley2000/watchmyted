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
    public partial class TEDConfigurationForm : Form
    {
        private Configuration config = null;

        public TEDConfigurationForm()
        {
            InitializeComponent();

            config = DatabaseController.GetConfiguration();

            chkEnable1.Checked = config.TEDMTU0Enabled;
            txtGatewayURL1.Text = config.TEDUrl1;
            txtGatewayURL2.Text = config.TEDUrl2;
            txtGatewayURL3.Text = config.TEDUrl3;

        }

        private void butSave_Click(object sender, EventArgs e)
        {
            int rowsUpdated = 0;

            // MTU 0
            config.TEDMTU0Enabled = chkEnable1.Checked;
            config.TEDUrl1 = txtGatewayURL1.Text;
            config.TEDUrl2 = txtGatewayURL2.Text;
            config.TEDUrl3 = txtGatewayURL3.Text;

            rowsUpdated = DatabaseController.UpdateTEDConfiguration("default", config);

            this.Close();
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
