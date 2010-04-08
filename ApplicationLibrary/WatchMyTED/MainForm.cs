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

namespace Energy.EnergyWatcher
{
    /// <summary>
    /// Main application form with all controls and access to functionality
    /// </summary>
    public partial class MainForm : Form
    {
        private TEDMeterList tedMeters = new TEDMeterList();
        private bool logDataToDatabase = true;
        private bool show900secChart = false;
        private bool show3600secChart = false;
        private bool show720minChart = false;
        private bool show1440minChart = false;
        private bool showConsumptionHourlyChart = false;

        private static Schedule.ScheduleTimer TickTenSecondTimer;
        private bool tenSecondTimerStarted = false;

        private static Schedule.ScheduleTimer TickFiveMinuteTimer;
        private bool fiveMinuteTimerStarted = false;

        private static Schedule.ScheduleTimer TickTenMinuteTimer;
        private bool tenMinuteTimerStarted = false;

        private static Schedule.ScheduleTimer TickFifteenMinuteTimer;
        private bool fifteenMinuteTimerStarted = false;

        public delegate void HourAddedEventHandler(object sender, HourAddedEventArgs e);
        public event HourAddedEventHandler HourAdded;

        public delegate void Minute15AddedEventHandler(object sender, Minute15AddedEventArgs e);
        public event Minute15AddedEventHandler Minute15Added;

        GAEClient gaeClientReadingForDayByHour;
        GAEClient gaeClientReadingForDayBy15Minute;

        public MainForm()
        {
            DateTime dt;
            DateTime today = DateTime.Now;

            InitializeComponent();

            // Check database and initialize of needed
            if (!DatabaseController.IsDatabaseValid())
            {
                DatabaseController.InitializeDatabase();
            }

            // initialize GAE clients
            gaeClientReadingForDayByHour = new GAEClient();
            gaeClientReadingForDayBy15Minute = new GAEClient();

            // Handlers for events that add to database, and send to GAE
            HourAdded += new HourAddedEventHandler(HourAddedHandler);
            Minute15Added += new Minute15AddedEventHandler(Minute15AddedHandler);

            // 10-Second Timer setup (RIGHT NOW SET TO 3 SECONDS)
            TickTenSecondTimer = new Schedule.ScheduleTimer();
            TickTenSecondTimer.Elapsed += new ScheduledEventHandler(TickTenSecondTimer_Elapsed);
            dt = DateTime.Parse(today.Month + "/" + today.Day + "/" + today.Year);
            TickTenSecondTimer.AddEvent(new Schedule.SimpleInterval(dt, TimeSpan.FromSeconds(3)));

            // 5-Minute Timer setup
            TickFiveMinuteTimer = new Schedule.ScheduleTimer();
            TickFiveMinuteTimer.Elapsed += new ScheduledEventHandler(TickFiveMinuteTimer_Elapsed);
            dt = DateTime.Parse(today.Month + "/" + today.Day + "/" + today.Year);
            TickFiveMinuteTimer.AddEvent(new Schedule.SimpleInterval(dt, TimeSpan.FromMinutes(5)));

            // 10-Minute Timer setup (to check for hours to save and query)
            TickTenMinuteTimer = new Schedule.ScheduleTimer();
            TickTenMinuteTimer.Elapsed += new ScheduledEventHandler(TickTenMinuteTimer_Elapsed);
            dt = DateTime.Parse(today.Month + "/" + today.Day + "/" + today.Year);
            TickTenMinuteTimer.AddEvent(new Schedule.SimpleInterval(dt, TimeSpan.FromMinutes(10)));

            // 15-Minute Timer setup (to collect 15-min power; do so 2 min after each 15 minutes from top of hour)
            TickFifteenMinuteTimer = new Schedule.ScheduleTimer();
            TickFifteenMinuteTimer.Elapsed += new ScheduledEventHandler(TickFifteenMinuteTimer_Elapsed);
            dt = DateTime.Parse(today.Month + "/" + today.Day + "/" + today.Year + " 00:06:00 AM");
            TickFifteenMinuteTimer.AddEvent(new Schedule.SimpleInterval(dt, TimeSpan.FromMinutes(15)));

            InitializeTEDMeters();

            StartWorkerThreadToLoadData();

        }

        private void InitializeTEDMeters()
        {
            TEDMeter tedMeter = null;
            Meter databaseMeter = null;

            // MTU0 is meter 0, MTU1 is meter 1, etc.
            // Currently, assume meterid = 0; ONLY ONE METER RIGHT NOW
            try
            {
                if (DatabaseController.DoesMeterIDExist(0))
                {
                    databaseMeter = DatabaseController.GetMeter(0);
                }
                else
                {
                    databaseMeter = new Meter(0, Enums.MeterType.ted_v1, "MTU 0", "TED Meter #1");
                    DatabaseController.InsertMeter(databaseMeter);
                }
            }
            catch
            {
                // in case there is a database problem
                throw;
            }

            // Set the tedMeter and add to master list of meters
            tedMeter = new TEDMeter();

            tedMeter.ID = databaseMeter.ID;
            tedMeter.Name = databaseMeter.Name;
            tedMeter.AddedTimestamp = DateTime.Now.AddMinutes(-1440);   // Set back 24 hours (for all - do not change)

            tedMeters.Add(tedMeter);

        }

        private void loggingPowerToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (logDataToDatabase)
            {
                loggingPowerToolStripMenuItem.Checked = false;
                logDataToDatabase = false;
            }
            else
            {
                loggingPowerToolStripMenuItem.Checked = true;
                logDataToDatabase = true;
            }
        }

        /// <summary>
        /// This needs to query seconds feed from TED Gateway for each MTU
        /// and add power values to the list for each MTU
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void TickTenSecondTimer_Elapsed(object source, ScheduledEventArgs e)
        {
            TenSecondTimerActions();
        }

        private void TenSecondTimerActions()
        {
            Configuration config;
            TEDSecondList seconds = null;
            int meterCount = 0;
            const int itemIndex = 1;
            const int itemCount = 10;            // use 20 when timer is 10 sec, use 10 when timer is 3 sec!
            const int itemCountMax = 3600;       // 900 seconds = 15 minutes
            int queryItemCount = 0;
            DateTime recentDate = DateTime.MinValue;
            int matchIndex = 0;
            bool isInitial = false;
            bool isValid = false;

            try
            {

                //TODO: make this global rather than here soon; TEMPORARY
                config = DatabaseController.GetConfiguration();

                if (tedMeters != null)
                {
                    if (tedMeters.Count > 0)
                    {

                        foreach (TEDMeter meter in tedMeters)
                        {

                            if (meter.SecondList.Count > 0)
                            {
                                queryItemCount = itemCount;
                                isInitial = false;
                            }
                            else
                            {
                                queryItemCount = itemCountMax;
                                isInitial = true;
                            }

                            try
                            {
                                seconds = TEDController.GetSeconds(config.TEDUrl2, meterCount, itemIndex, queryItemCount);
                                isValid = true;
                            }
                            catch
                            {
                                isValid = false;
                            }

                            if (isValid)
                            {
                                if (!isInitial)
                                {
                                    // Use Linq to find index of matching timestamp
                                    recentDate = meter.SecondList[meter.SecondList.Count() - 1].Timestamp;
                                    //Debug.Write("lastKnown = " + recentDate.ToString("yyyy/MM/dd HH:mm:ss"));
                                    matchIndex = seconds.FindIndex(w => w.Timestamp == recentDate);
                                    //Debug.Write(", matchIndex = " + matchIndex.ToString() );
                                    //Debug.WriteLine("");

                                    // no overlap, so add all seconds to meter.SecondList
                                    if (matchIndex == -1)
                                    {
                                        matchIndex = seconds.Count() - 1;
                                    }

                                    if (matchIndex > 0)
                                    {
                                        // add all the new seconds to meters list of seconds
                                        for (int i = matchIndex; i >= 1; i--)
                                        {
                                            meter.SecondList.Add(seconds[i - 1]);
                                            //Debug.WriteLine("TEDSecond = " + seconds[i-1].Timestamp.ToString("yyyy/MM/dd HH:mm:ss") + ", " + seconds[i-1].POWER);
                                        }
                                    }

                                }
                                else
                                {
                                    seconds.Reverse();

                                    foreach (TEDSecond s in seconds)
                                    {
                                        meter.SecondList.Add(s);
                                    }
                                }
                            }

                            // We always want at least 3600 seconds minimum after list grows for a while;
                            // When it reaches 4600, cut back to 3800 
                            CheckAndHandleSizeOfList(meter.SecondList, 4600, 800);

                            meterCount++;
                        }

                        // two charts need updating of pane collections;
                        Power900SecondChart(show900secChart);
                        Power3600SecondChart(show3600secChart);
                    }
                }
            }
            catch
            {
                // communication error
            }
        }

        /// <summary>
        /// This needs to query minutes feed from TED Gateway for each MTU
        /// and add power values to the list for each MTU
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void TickFiveMinuteTimer_Elapsed(object source, ScheduledEventArgs e)
        {
            FiveMinuteTimerActions();
        }

        private void FiveMinuteTimerActions()
        {
            Configuration config;
            TEDMinuteList minutes = null;
            int meterCount = 0;
            const int itemIndex = 1;
            const int itemCount = 20;
            const int itemCountMax = 1440;       // 120 minutes = 2 hours
            int queryItemCount = 0;
            DateTime recentDate = DateTime.MinValue;
            int matchIndex = 0;
            bool isInitial = false;
            bool isValid = false;

            try
            {
                //TODO: make this global rather than here soon; TEMPORARY
                config = DatabaseController.GetConfiguration();

                if (tedMeters != null)
                {
                    if (tedMeters.Count > 0)
                    {

                        foreach (TEDMeter meter in tedMeters)
                        {

                            if (meter.MinuteList.Count > 0)
                            {
                                queryItemCount = itemCount;
                                isInitial = false;
                            }
                            else
                            {
                                queryItemCount = itemCountMax;
                                isInitial = true;
                            }

                            try
                            {
                                minutes = TEDController.GetMinutes(config.TEDUrl1, meterCount, itemIndex, queryItemCount);
                                isValid = true;
                            }
                            catch
                            {
                                isValid = false;
                            }

                            if (isValid)
                            {
                                if (!isInitial)
                                {
                                    // Use Linq to find index of matching timestamp
                                    recentDate = meter.MinuteList[meter.MinuteList.Count() - 1].Timestamp;
                                    matchIndex = minutes.FindIndex(w => w.Timestamp == recentDate);

                                    // no overlap, so add all minutes to meter.MinuteList
                                    if (matchIndex == -1)
                                    {
                                        matchIndex = minutes.Count() - 1;
                                    }

                                    if (matchIndex > 0)
                                    {
                                        // add all the new minutes to meters list of minutes
                                        for (int i = matchIndex; i >= 1; i--)
                                        {
                                            meter.MinuteList.Add(minutes[i - 1]);
                                        }
                                    }
                                }
                                else
                                {
                                    minutes.Reverse();

                                    foreach (TEDMinute m in minutes)
                                    {
                                        meter.MinuteList.Add(m);
                                    }
                                }
                            }

                            // We always want at least 1440 minutes minimum after list grows for a while;
                            // When it reaches 2000, cut back to 1600 
                            CheckAndHandleSizeOfList(meter.MinuteList, 2000, 400);

                            meterCount++;
                        }

                        // two charts need updating of pane collections;
                        Power720MinuteChart(show720minChart);
                        Power1440MinuteChart(show1440minChart);
                    }
                }
            }
            catch
            {
                //communication error
            }
        }

        /// <summary>
        /// This timer is for the HOUR list
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void TickTenMinuteTimer_Elapsed(object source, ScheduledEventArgs e)
        {
            TenMinuteTimerActions();
        }

        private void TenMinuteTimerActions()
        {
            Configuration config;
            TEDHourList hours = null;
            int meterCount = 0;
            const int itemIndex = 1;
            const int itemCount = 4;
            const int itemCountMax = 24;       // 24 hours
            int queryItemCount = 0;
            DateTime recentDate = DateTime.MinValue;
            int matchIndex = 0;
            bool isInitial = false;
            bool isValid = false;

            try
            {
                //TODO: make this global rather than here soon; TEMPORARY
                config = DatabaseController.GetConfiguration();

                if (tedMeters != null)
                {
                    if (tedMeters.Count > 0)
                    {
                        foreach (TEDMeter meter in tedMeters)
                        {

                            if (meter.HourList.Count > 0)
                            {
                                queryItemCount = itemCount;
                                isInitial = false;
                            }
                            else
                            {
                                queryItemCount = itemCountMax;
                                isInitial = true;
                            }

                            try
                            {
                                hours = TEDController.GetHours(config.TEDUrl3, meterCount, itemIndex, queryItemCount);
                                isValid = true;
                            }
                            catch
                            {
                                isValid = false;
                            }

                            if (isValid)
                            {
                                if (!isInitial)
                                {
                                    // Use Linq to find index of matching timestamp
                                    recentDate = meter.HourList[meter.HourList.Count() - 1].Timestamp;
                                    matchIndex = hours.FindIndex(w => w.Timestamp == recentDate);

                                    // no overlap, so add all hours to meter.HourList
                                    if (matchIndex == -1)
                                    {
                                        matchIndex = hours.Count() - 1;
                                    }


                                    if (matchIndex > 0)
                                    {
                                        // add all the new hours to meters list of hours
                                        for (int i = matchIndex; i >= 1; i--)
                                        {
                                            meter.HourList.Add(hours[i - 1]);

                                            // raise hour added avent
                                            HourAddedEventArgs eventArgs = new HourAddedEventArgs(meter.ID, meter.Name, hours[i - 1], config, gaeClientReadingForDayByHour);
                                            HourAdded(this, eventArgs);
                                        }
                                    }

                                }
                                else
                                {
                                    hours.Reverse();

                                    foreach (TEDHour h in hours)
                                    {
                                        meter.HourList.Add(h);

                                        // raise hour added avent
                                        HourAddedEventArgs eventArgs = new HourAddedEventArgs(meter.ID, meter.Name, h, config, gaeClientReadingForDayByHour);
                                        HourAdded(this, eventArgs);
                                    }
                                }
                            }

                            // We always want at least 24 hours minimum after list grows for a while;
                            // When it reaches 48, cut back to 40 
                            CheckAndHandleSizeOfList(meter.HourList, 48, 8);

                            meterCount++;
                        }

                        // Update pane collection
                        ConsumptionHourlyChart(showConsumptionHourlyChart);

                    }
                }

            }
            catch
            {
                // could not communicate
            }
        }

        /// <summary>
        /// Store minutes and 15-minute derived with lookback
        /// This timer is to lookback and create 15-min average power list to queue for GAE
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void TickFifteenMinuteTimer_Elapsed(object source, ScheduledEventArgs e)
        {
            FifteenMinuteTimerActions();
        }

        private void FifteenMinuteTimerActions()
        {
            Configuration config;
            List<TEDMinute> minutes = null;
            TEDMinute recentRead = null;
            TEDMinute derived15Minute = null;
            DateTime recentDate = DateTime.MinValue;
            DateTime mid = DateTime.MinValue;
            DateTime start = DateTime.MinValue;
            DateTime end = DateTime.MinValue;
            int startMinute = 0;
            int startSecond = 0;
            int endMinute = 0;
            int endSecond = 0;
            int sum = 0;
            double power = 0.0;
            List<TEDMinute> minutes2 = null;
            TEDMinute min2 = null;

            try
            {
                //TODO: make this global rather than here soon; TEMPORARY
                config = DatabaseController.GetConfiguration();

                if (tedMeters != null)
                {
                    if (tedMeters.Count > 0)
                    {
                        foreach (TEDMeter meter in tedMeters)
                        {
                            sum = 0;
                            power = 0.0;

                            // Grab newest timestamp from the live list of minutes 
                            recentRead = meter.MinuteList[meter.MinuteList.Count() - 1];
                            recentDate = recentRead.Timestamp;

                            // timer is 6-min after each 15 minutes; 
                            // so use -13 to go back to near-middle of *last* 15 minutes
                            mid = recentDate.AddMinutes(-13);       

                            if (mid.Minute >= 0 && mid.Minute < 15)
                            {
                                startMinute = 0;
                                endMinute = 14;
                            }
                            else if (mid.Minute >= 15 && mid.Minute < 30)
                            {
                                startMinute = 15;
                                endMinute = 29;
                            }
                            else if (mid.Minute >= 30 && mid.Minute < 45)
                            {
                                startMinute = 30;
                                endMinute = 44;
                            }
                            else if (mid.Minute >= 45 && mid.Minute <= 59)
                            {
                                startMinute = 45;
                                endMinute = 59;
                            }

                            // get the range of minutes that we need
                            start = new DateTime(mid.Year, mid.Month, mid.Day, mid.Hour, startMinute, startSecond);
                            end = new DateTime(mid.Year, mid.Month, mid.Day, mid.Hour, endMinute, endSecond);

                            minutes = meter.MinuteList.FindAll(m => (m.Timestamp >= start && m.Timestamp <= end));
                            if (minutes != null)
                            {
                                if (minutes.Count > 0)
                                {
                                    sum = minutes.Sum(m => m.POWER);
                                    power = (double)sum / 15.0;     //15-min power, store w/ the start timestamp;
                                }
                            }

                            // Add to a derived 15-min list for the meter; this is medium term storage
                            derived15Minute = new TEDMinute();
                            derived15Minute.POWER = (int)Math.Round(power);
                            derived15Minute.Timestamp = start;
                            derived15Minute.MTU = recentRead.MTU;
                            derived15Minute.DATE = start.ToString("yyyy/MM/dd HH:mm:ss");
                            meter.Derived15Minutes.Add(derived15Minute);

                            // *** create a collection of minute reads, 15 of them;
                            minutes2 = new List<TEDMinute>();
                            foreach (TEDMinute min in minutes)
                            {
                                min2 = new TEDMinute();
                                min2.MTU = min.MTU;
                                min2.Timestamp = min.Timestamp;
                                min2.DATE = min.DATE;
                                min2.COST = min.COST;
                                min2.POWER = min.POWER;
                                min2.VOLTAGE = min.VOLTAGE;
                                minutes2.Add(min2);
                            }

                            // Raise an event so that another handler can send the 15-min value to GAE
                            Minute15AddedEventArgs eventArgs = new Minute15AddedEventArgs(meter.ID, meter.Name, derived15Minute, minutes2, config, gaeClientReadingForDayBy15Minute);
                            Minute15Added(this, eventArgs);

                        }
                    }
                }
            }
            catch
            {
                // communication error
            }
        }




        private void aboutEnergyWatcherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frmAbout = new AboutForm();

            frmAbout.ShowDialog(this);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void checkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frmContact = new ContactForm();

            frmContact.ShowDialog(this);
        }


        private void chart900SecondToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart900SecondToolStripMenuItem.Checked = true;
            chart3600SecondToolStripMenuItem.Checked = false;
            chart720MinuteToolStripMenuItem.Checked = false;
            chart1440MinuteToolStripMenuItem.Checked = false;
            chartConsumptionhourlyToolStripMenuItem.Checked = false;

            show900secChart = true;
            show3600secChart = false;
            show720minChart = false;
            show1440minChart = false;
            showConsumptionHourlyChart = false;

            Power900SecondChart(true);
            zg1.Visible = true;
        }

        private void chart3600SecondToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart900SecondToolStripMenuItem.Checked = false;
            chart3600SecondToolStripMenuItem.Checked = true;
            chart720MinuteToolStripMenuItem.Checked = false;
            chart1440MinuteToolStripMenuItem.Checked = false;
            chartConsumptionhourlyToolStripMenuItem.Checked = false;

            show900secChart = false;
            show3600secChart = true;
            show720minChart = false;
            show1440minChart = false;
            showConsumptionHourlyChart = false;

            Power3600SecondChart(true);
            zg1.Visible = true;
        }

        private void chart720MinuteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart900SecondToolStripMenuItem.Checked = false;
            chart3600SecondToolStripMenuItem.Checked = false;
            chart720MinuteToolStripMenuItem.Checked = true;
            chart1440MinuteToolStripMenuItem.Checked = false;
            chartConsumptionhourlyToolStripMenuItem.Checked = false;

            show900secChart = false;
            show3600secChart = false;
            show720minChart = true;
            show1440minChart = false;
            showConsumptionHourlyChart = false;

            Power720MinuteChart(true);
            zg1.Visible = true;
        }

        private void chart1440MinuteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart900SecondToolStripMenuItem.Checked = false;
            chart3600SecondToolStripMenuItem.Checked = false;
            chart720MinuteToolStripMenuItem.Checked = false;
            chart1440MinuteToolStripMenuItem.Checked = true;
            chartConsumptionhourlyToolStripMenuItem.Checked = false;

            show900secChart = false;
            show3600secChart = false;
            show720minChart = false;
            show1440minChart = true;
            showConsumptionHourlyChart = false;

            Power1440MinuteChart(true);
            zg1.Visible = true;
        }

        private void chartConsumptionhourlyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            chart900SecondToolStripMenuItem.Checked = false;
            chart3600SecondToolStripMenuItem.Checked = false;
            chart720MinuteToolStripMenuItem.Checked = false;
            chart1440MinuteToolStripMenuItem.Checked = false;
            chartConsumptionhourlyToolStripMenuItem.Checked = true;

            show900secChart = false;
            show3600secChart = false;
            show720minChart = false;
            show1440minChart = false;
            showConsumptionHourlyChart = true;

            ConsumptionHourlyChart(true);
            zg1.Visible = true;
        }


        //// 
        //// Re-enable this when additional charts are available in desktop application
        //// and when zigbee devices are supported with the TED devices
        ////
        //private void moreChartsToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    Form frmChartForm = new ChartForm();
        //
        //    frmChartForm.ShowDialog(this);
        //}

        //// 
        //// Re-enable this when rate authoring truly supported in desktop application
        //// Too distracting at this time
        ////
        //private void ratesToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    Form frmRateForm = new RateForm();
        //
        //    frmRateForm.ShowDialog(this);
        //}


        private void clientTesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frmGAETest = new GAEClientTestForm(GoogleAppEngineMode.Production);    // DEBUG: development || production
            frmGAETest.ShowDialog(this);
        }

        private void configurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frmTEDConfiguration = new TEDConfigurationForm();
            frmTEDConfiguration.ShowDialog(this);
        }

        private void clientConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frmGAEClientConfiguration = new GAEClientConfigurationForm();
            frmGAEClientConfiguration.ShowDialog(this);
        }

        private void forceUploadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frmGAEClientForceUpload = new GAEClientForceUploadForm();
            frmGAEClientForceUpload.ShowDialog(this);
        }

        private void viewToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form frmTEDViewer = new TEDViewerForm();
            frmTEDViewer.ShowDialog(this);
        }


        // Removing range of data from a generic list
        private void CheckAndHandleSizeOfList<T>(List<T> list, int maxSize, int removeSize)
        {
            if (list.Count > maxSize)
            {
                list.RemoveRange(0, removeSize);
            }
        }


        // Handler for adding an hour
        // Add to database
        private void HourAddedHandler(object sender, HourAddedEventArgs e)
        {
            bool readingsExists = false;
            Thread thread;

            if (logDataToDatabase)
            {
                // cannot assume that reading is not already present
                readingsExists = DatabaseController.DoesReadingExist(e.MeterID, Enums.Interval.Hourly, Enums.UnitOfMeasure.kWh, e.Hour.Timestamp);

                if (!readingsExists)
                {
                    DatabaseController.InsertReading(e.MeterID, Enums.Interval.Hourly, Enums.UnitOfMeasure.kWh, e.Hour.Timestamp, e.Hour.Consumption_kWh());

                    // Start a new thread that will handle the sending of data
                    thread = new Thread(new ParameterizedThreadStart(GAESend_ReadingForDayByHour));
                    thread.Start(e);

                }

            }
        }

        // Handler for 15-min power
        // This is not saved locally to database
        private void Minute15AddedHandler(object sender, Minute15AddedEventArgs e)
        {
            Thread thread;

            // Start a new thread that will handle the sending of data
            thread = new Thread(new ParameterizedThreadStart(GAESend_ReadingForDayBy15Minute));
            thread.Start(e);

        }



        /// <summary>
        /// Notify icon work for tray
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            //ewNotify.ShowBalloonTip(2000);
            ewNotify.Visible = false;
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
            {
                //Hide();
                ewNotify.Visible = true;
                this.ShowInTaskbar = false;
            }
            else
            {
                //Show();
                this.ShowInTaskbar = true;
            }
        }

        private void ewNotify_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            WindowState = FormWindowState.Normal;
            Show();
        }


    }

}