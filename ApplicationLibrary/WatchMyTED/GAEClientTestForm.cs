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
    public partial class GAEClientTestForm : Form
    {
        Configuration config;
        GoogleAppEngineMode mode;

        public GAEClientTestForm( GoogleAppEngineMode appEngineMode )
        {
            InitializeComponent();
            mode = appEngineMode;
        }

        /// <summary>
        /// Test the authentication and retrieval of cookie from App Engine Application
        /// Production and Development modes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            bool isAuthorised = false;
            GAEClient client;

            config = DatabaseController.GetConfiguration();

            client = new GAEClient();

            // change this to Development if developing, and Production if using it for real!
            client.Mode = mode;   
            client.ApplicationName = config.GAEAppName;
            client.ApplicationURL = config.GAESiteUrl;
            client.GoogleUsername = config.GAEGmail;
            client.GooglePassword = config.GAEPassword;

            isAuthorised = client.Authorize();

            txtResponse.Text = isAuthorised.ToString() + 
                Environment.NewLine + 
                Environment.NewLine + 
                ", cookie=" + client.AuthorizedCookie;

        }
    }
}
