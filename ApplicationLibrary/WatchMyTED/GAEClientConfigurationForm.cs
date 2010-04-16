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
    public partial class GAEClientConfigurationForm : Form
    {
        private Configuration config = null;

        public GAEClientConfigurationForm()
        {
            InitializeComponent();

            config = DatabaseController.GetConfiguration();

            txtServiceURL.Text = config.GAEReadingsServiceUrl;
            txtGmail.Text = config.GAEGmail;
            txtPassword.Text = config.GAEPassword;
            txtSiteURL.Text = config.GAESiteUrl;
            txtAppName.Text = config.GAEAppName;
        }

        private void butSave_Click(object sender, EventArgs e)
        {
            int rowsUpdated = 0;

            config.GAEReadingsServiceUrl = txtServiceURL.Text;
            config.GAEGmail = txtGmail.Text;
            config.GAEPassword = txtPassword.Text;
            config.GAESiteUrl = txtSiteURL.Text;
            config.GAEAppName = txtAppName.Text;

            rowsUpdated = DatabaseController.UpdateGAEConfiguration("default", config);

            this.Close();
        }

        private void butCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
