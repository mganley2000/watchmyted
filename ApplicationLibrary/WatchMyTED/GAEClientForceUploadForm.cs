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
    /// This form used as a testing tool to send data to myenergyuse GAE site
    /// In future this will be expanded to pull from database and upload readings by request
    /// </summary>
    public partial class GAEClientForceUploadForm : Form
    {
        public GAEClientForceUploadForm()
        {
            InitializeComponent();
        }

        private void butDone_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void butUpload_Click(object sender, EventArgs e)
        {
            Configuration config;
            GAERead gaeRead;
            GAEReadContainer gaeReadContainer;
            GAEClient gaeClient;
            string blob;
            string stamp;

            // use Start and End datetimes, execute query for readings

            config = DatabaseController.GetConfiguration();
            gaeClient = new GAEClient(config, false);

            gaeClient.Mode = GoogleAppEngineMode.Production;    //DEBUG: Development || Production
            gaeClient.ApplicationName = config.GAEAppName;
            gaeClient.ApplicationURL = config.GAESiteUrl;
            gaeClient.GoogleUsername = config.GAEGmail;
            gaeClient.GooglePassword = config.GAEPassword;

            bool debug = false;
            if (debug)
            {
                if (gaeClient.Authorize())
                {
                    // test object
                    TEDHour hour = new TEDHour();
                    hour.MTU = 0;
                    hour.POWER = 1850;
                    hour.DATE = DateTime.Now.ToString("yyyy/MM/dd HH:00:00");
                    hour.VOLTAGE = 166;
                    hour.Timestamp = DateTime.Parse(hour.DATE);

                    // Create JSON serializable class from data
                    // Note that TEDHour POWER is implied to have units of "Wh"
                    blob = hour.Timestamp.Hour.ToString() + "=" + hour.POWER.ToString();
                    stamp = hour.Timestamp.ToString("yyyy-MM-dd");
                    gaeRead = new GAERead(0, "test name", Enums.GAETargetStorage.ReadingForDayByHour, Enums.UnitOfMeasure.Wh, Enums.Fuel.Electric, stamp, blob);
                    gaeReadContainer = new GAEReadContainer(gaeRead);

                    // Send this data to Google App Engine Site
                    gaeClient.SendJSONToGAEService(gaeReadContainer, config.GAEReadingsServiceUrl);

                    labStatus.Text = "JSON sent";
                }
                else
                {
                    // failed to authorize
                    // TODO: log error to event log or application log
                    labStatus.Text = "failed to authorize";
                }
            
            }

        }
    }
}
