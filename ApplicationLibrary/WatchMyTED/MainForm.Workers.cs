using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZedGraph;
using Energy.Library;
using Schedule;
using System.Linq;
using System.Threading;
using System.Diagnostics;

namespace Energy.WatchMyTED
{
    public partial class MainForm
    {

        #region Background Worker To Load Data

        private void StartWorkerThreadToLoadData()
        {
            try
            {

                ConsumptionHourlyChart(true);
                zg1.Visible = true;

                if (!workBackgroundMain.IsBusy)
                {
                    this.statusProgress.Visible = true;
                    this.statusProgress.Value = 0;
                    this.statusProgress.Style = ProgressBarStyle.Marquee;
                    this.statusLabel.Text = "Loading recent data...";

                    this.workBackgroundMain.RunWorkerAsync();
                }
                else
                {
                    this.workBackgroundMain.CancelAsync();
                }

                Power900SecondChart(true);
                chart900SecondToolStripMenuItem.Checked = true;
                chart3600SecondToolStripMenuItem.Checked = false;
                chart720MinuteToolStripMenuItem.Checked = false;
                chart1440MinuteToolStripMenuItem.Checked = false;
                chartConsumptionhourlyToolStripMenuItem.Checked = false;

            }
            catch
            {
                this.statusProgress.Visible = false;
                this.statusLabel.Text = "Error loading data.  Check TED settings.";
            }

        }

        private void workBackgroundMain_DoWork(object sender, DoWorkEventArgs e)
        {
            // Load data from TED by calling the timer action functions;  these automatically
            // get older data when first called to initialize the charts;


            workBackgroundMain.ReportProgress(1, "Loading recent TED hours...");
            showConsumptionHourlyChart = true;

            TenMinuteTimerActions();   // hours data

            showConsumptionHourlyChart = false;


            workBackgroundMain.ReportProgress(1, "Loading recent TED minutes...");
            show720minChart = true;
            show1440minChart = true;

            FiveMinuteTimerActions();   // minutes data

            show720minChart = false;
            show1440minChart = false;


            workBackgroundMain.ReportProgress(1, "Loading recent TED seconds...");
            show900secChart = true;
            show3600secChart = true;

            TenSecondTimerActions();    // seconds data

            show3600secChart = false;
            show900secChart = false;

            show3600secChart = true;

            // Start the timers so data is grabbed from TED devices at regular intervals;
            StartTimers();

        }

        private void workBackgroundMain_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.statusProgress.Value = e.ProgressPercentage;
            this.statusLabel.Text = (string)e.UserState;
        }

        private void workBackgroundMain_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.statusProgress.Style = ProgressBarStyle.Blocks;
            this.statusProgress.Value = 0;
            this.statusProgress.Visible = false;
            this.statusLabel.Text = "Ready";
        }

        private void StartTimers()
        {

            if (!tenSecondTimerStarted)
            {
                tenSecondTimerStarted = true;
                TickTenSecondTimer.Start();
            }

            if (!fiveMinuteTimerStarted)
            {
                fiveMinuteTimerStarted = true;
                TickFiveMinuteTimer.Start();
            }

            if (!tenMinuteTimerStarted)
            {
                tenMinuteTimerStarted = true;
                TickTenMinuteTimer.Start();
            }

            if (!fifteenMinuteTimerStarted)
            {
                fifteenMinuteTimerStarted = true;
                TickFifteenMinuteTimer.Start();
            }

        }

        #endregion



        #region THREAD SCOPE - GAESend_ReadingForDayByHour

        private void GAESend_ReadingForDayByHour(object e)
        {
            // this must finish before another thread interrupts
            lock (this)
            {
                SendReadingForDayByHour((HourAddedEventArgs)e);
            }

        }

        private void SendReadingForDayByHour(HourAddedEventArgs data)
        {
            string blob;
            string stamp;
            GAERead gaeRead;
            GAEReadContainer gaeReadContainer;

            try
            {
                // set the GAEClient settings; in case configuration changed along the way
                data.GaeClient.Mode = GoogleAppEngineMode.Production;   //DEBUG: Development || Production
                data.GaeClient.ApplicationName = data.Config.GAEAppName;
                data.GaeClient.ApplicationURL = data.Config.GAESiteUrl;
                data.GaeClient.GoogleUsername = data.Config.GAEGmail;
                data.GaeClient.GooglePassword = data.Config.GAEPassword;

                if (data.GaeClient.Authorize())
                {
                    // Create JSON serializable class from data
                    // Note that for GAE, we send the POWER, which has implied uom of Wh (Watt*hour) in this case
                    blob = data.Hour.Timestamp.Hour.ToString() + "=" + data.Hour.POWER.ToString();
                    stamp = data.Hour.Timestamp.ToString("yyyy-MM-dd");
                    gaeRead = new GAERead(data.MeterID, data.MeterName, Enums.GAETargetStorage.ReadingForDayByHour, Enums.UnitOfMeasure.Wh, Enums.Fuel.Electric, stamp, blob);
                    gaeReadContainer = new GAEReadContainer(gaeRead);

                    // Send this data to Google App Engine Site
                    data.GaeClient.SendJSONToGAEService(gaeReadContainer, data.Config.GAEReadingsServiceUrl);

                }
                else
                {
                    // failed to authorize
                    // TODO: log error to event log or application log
                }
            }
            catch
            {
                // assume that there was an error talking to GAE, un-authorize the GAEClient
                data.GaeClient.Unauthorize();
            }


        }

        #endregion


        #region THREAD SCOPE - GAESend_ReadingForDayBy15Minute - GAESend_ReadingForHourByMinute

        private void GAESend_ReadingForDayBy15Minute(object e)
        {
            // this must finish before another thread interrupts
            lock (this)
            {
                SendReadingForDayBy15Minute((Minute15AddedEventArgs)e);
            }

        }

        private void SendReadingForDayBy15Minute(Minute15AddedEventArgs data)
        {
            string blob;
            string stamp;
            GAERead gaeRead;
            GAEReadContainer gaeReadContainer;
            GAERead gaeRead2;
            GAEReadContainer gaeReadContainer2;
            int hour = 0;
            int minute = 0;
            int blobInterval = 0;
            int count = 0;

            try
            {
                // set the GAEClient settings; in case configuration changed along the way
                data.GaeClient.Mode = GoogleAppEngineMode.Production;   //DEBUG: Development || Production
                data.GaeClient.ApplicationName = data.Config.GAEAppName;
                data.GaeClient.ApplicationURL = data.Config.GAESiteUrl;
                data.GaeClient.GoogleUsername = data.Config.GAEGmail;
                data.GaeClient.GooglePassword = data.Config.GAEPassword;

                if (data.GaeClient.Authorize())
                {
                    // Create JSON serializable class from data
                    // Note that for GAE, we send the POWER in Watts in this case
                    hour = data.Minute.Timestamp.Hour;
                    minute = data.Minute.Timestamp.Minute;
                    blobInterval = (hour * 4) + ((int)((double)minute / 15.0));    // four 15-minute reads per hour
                    blob = blobInterval.ToString() + "=" + data.Minute.POWER.ToString();

                    stamp = data.Minute.Timestamp.ToString("yyyy-MM-dd");   // for DAY, so only stamp the day for this parameter

                    gaeRead = new GAERead(data.MeterID, data.MeterName, Enums.GAETargetStorage.ReadingForDayBy15Minute, Enums.UnitOfMeasure.W, Enums.Fuel.Electric, stamp, blob);
                    gaeReadContainer = new GAEReadContainer(gaeRead);

                    // Send this data to Google App Engine
                    data.GaeClient.SendJSONToGAEService(gaeReadContainer, data.Config.GAEReadingsServiceUrl);


                    // ****
                    //Create another object for minute-by-minute
                    // ****
                    blob = string.Empty;
                    foreach (TEDMinute min in data.MinuteList)
                    {
                        if (count > 0)
                        {
                            blob = blob + ",";
                        }
                        else
                        {
                            // only use hour for stamp; do not use minutes!
                            stamp = min.Timestamp.ToString("yyyy-MM-dd HH:00:00");  
                        }

                        blob = blob + min.Timestamp.Minute + "=" + min.POWER.ToString();
                        count++;
                    }

                    gaeRead2 = new GAERead(data.MeterID, data.MeterName, Enums.GAETargetStorage.ReadingForHourByMinute, Enums.UnitOfMeasure.W, Enums.Fuel.Electric, stamp, blob);
                    gaeReadContainer2 = new GAEReadContainer(gaeRead2);

                    // Send this data to Google App Engine
                    data.GaeClient.SendJSONToGAEService(gaeReadContainer2, data.Config.GAEReadingsServiceUrl);

                }
                else
                {
                    // failed to authorize
                    // TODO: log error to event log or application log
                }

            }
            catch
            {
                // assume that there was an error talking to GAE, un-authorize the GAEClient
                data.GaeClient.Unauthorize();
            }


        }

        #endregion

    }
}
