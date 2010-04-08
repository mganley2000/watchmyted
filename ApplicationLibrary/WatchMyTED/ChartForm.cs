using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Energy.Library;
using ZedGraph;

namespace Energy.EnergyWatcher
{
    /// <summary>
    /// Historical Chart viewing
    /// </summary>
    public partial class ChartForm : Form
    {
        bool initialized = false;
        MeterCollection databaseMeters;
        DataTable chartList;
        DataTable dateList;

        int selectedMeter;
        Enums.SelectChart selectedChart;
        Enums.SelectDate selectedDate;

        Configuration config;
        Rate currentRate;

        ReadingCollection readings;             //hourly
        ReadingCollection hourlyPeakReadings;   //peak hourly
        ReadingCollection hourlyOffPeakReadings;//OffPeak hourly
        ReadingCollection dailyreadings;        //generated from hourly when needed
        ReadingCollection dailyPeakReadings;    //if TOU, this is used
        ReadingCollection dailyOffPeakReadings;
        ReadingCollection dailyCosts;           //generated from hourly when needed
        ReadingCollection dailyPeakCosts;       //if TOU, this is used
        ReadingCollection dailyOffPeakCosts;

        public ChartForm()
        {
            InitializeComponent();

            config = DatabaseController.GetConfiguration();
            currentRate = DatabaseController.GetRate(config.RateID);

            databaseMeters = DatabaseController.GetMeters();
            LoadMeterDropDown();

            chartList = DatabaseController.GetSelectChart();
            LoadChartDropDown();
            ddChart.SelectedIndex = 0;

            dateList = DatabaseController.GetSelectDate();
            LoadDateDropDown();
            ddDateSelect.SelectedIndex = 0;

            initialized = true;

            // First time loading, load default chart
            ReadSelectLoad();
        }


        private void LoadMeterDropDown()
        {
            int i = 0;

            ddMeter.Items.Clear();

            DataTable list = new DataTable();
            list.Columns.Add(new DataColumn("Display", typeof(string)));
            list.Columns.Add(new DataColumn("Id", typeof(int)));

            foreach (Meter m in databaseMeters)
            {
                list.Rows.Add(list.NewRow());
                list.Rows[i][0] = m.Name;
                list.Rows[i][1] = m.ID;
                i++;
            }

            ddMeter.DisplayMember = "Display";
            ddMeter.ValueMember = "Id";
            ddMeter.DataSource = list;
        }

        private void LoadChartDropDown()
        {
            ddChart.Items.Clear();

            ddChart.DisplayMember = "Display";
            ddChart.ValueMember = "Id";
            ddChart.DataSource = chartList;
        }

        private void LoadDateDropDown()
        {
            ddDateSelect.Items.Clear();

            ddDateSelect.DisplayMember = "Display";
            ddDateSelect.ValueMember = "Id";
            ddDateSelect.DataSource = dateList;
        }

        private void butClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        // On change, grab values and reload chart
        private void ddMeter_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReadSelectLoad();
        }

        private void ddChart_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReadSelectLoad();
        }

        private void ddDateSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReadSelectLoad();
        }


        private void ReadSelectLoad()
        {
            if (initialized)
            {
                ReadSelectionsFromDropdowns();

                SelectReadings();

                switch (selectedChart)
                {
                    case Enums.SelectChart.HourlyEnergy:

                        if (currentRate.HasTimeOfUseCharges)
                        {
                            hourlyPeakReadings = readings.CreateHourlyReadings(Enums.TimeOfUse.Peak);
                            hourlyOffPeakReadings = readings.CreateHourlyReadings(Enums.TimeOfUse.OffPeak);
                        }

                        LoadHourlyChart();

                        break;

                    case Enums.SelectChart.DailyEnergy:

                        if (dailyreadings != null)
                        {
                            dailyreadings.Clear();
                            dailyreadings = null;
                        }

                        // Make separate calls here for two collections, OnPeak and OffPeak;
                        if (!currentRate.HasTimeOfUseCharges)
                        {
                            dailyreadings = readings.CreateDailyReadings();
                        }
                        else
                        {
                            dailyPeakReadings = readings.CreateDailyReadings(Enums.TimeOfUse.Peak);
                            dailyOffPeakReadings = readings.CreateDailyReadings(Enums.TimeOfUse.OffPeak);
                        }

                        LoadDailyChart();

                        break;

                    case Enums.SelectChart.DailyCost:

                        if (dailyCosts != null)
                        {
                            dailyCosts.Clear();
                            dailyCosts = null;
                        }

                        // Make separate calls here for two collections, OnPeak and OffPeak;
                        if (!currentRate.HasTimeOfUseCharges)
                        {
                            dailyCosts = readings.CreateDailyCosts();
                        }
                        else
                        {
                            dailyPeakCosts = readings.CreateDailyCosts(Enums.TimeOfUse.Peak);
                            dailyOffPeakCosts = readings.CreateDailyCosts(Enums.TimeOfUse.OffPeak);
                        }

                        LoadDailyCostChart();

                        break;
                }

            }
        }


        private void ReadSelectionsFromDropdowns()
        {
            selectedMeter = (int)(((System.Data.DataRowView)(ddMeter.SelectedItem)).Row.ItemArray[1]);
            selectedChart = (Enums.SelectChart)(((System.Data.DataRowView)(ddChart.SelectedItem)).Row.ItemArray[1]);
            selectedDate = (Enums.SelectDate)(((System.Data.DataRowView)(ddDateSelect.SelectedItem)).Row.ItemArray[1]);
        }

        private void SelectReadings()
        {
            string startDate;
            string endDate;
            DateTime now;
            DateTime dtStart, dtEnd, dtEndDayOnly;
            int daysInMonth;
            bool retval;

            now = DateTime.Now;
            daysInMonth = DateTime.DaysInMonth(now.Year, now.Month);
            dtStart = DateTime.Parse(now.Month + "/" + "1" + "/" + now.Year + " 00:00:00");
            startDate = dtStart.ToString("yyyy/MM/dd HH:mm:ss");
            dtEnd = DateTime.Parse(now.Month + "/" + daysInMonth + "/" + now.Year + " 23:00:00");
            endDate = dtEnd.ToString("yyyy/MM/dd HH:mm:ss");

            switch (selectedDate)
            {
                case Enums.SelectDate.CurrentMonth:
                    // nothing more to do
                    break;

                case Enums.SelectDate.Days10:
                    // not month specific, so recalculate start and end
                    dtEnd = DateTime.Parse(now.Month + "/" + now.Day + "/" + now.Year + " 23:00:00");
                    endDate = dtEnd.ToString("yyyy/MM/dd HH:mm:ss");
                    dtEndDayOnly = DateTime.Parse(now.Month + "/" + now.Day + "/" + now.Year + " 00:00:00");
                    dtStart = dtEndDayOnly.AddDays(-10);
                    startDate = dtStart.ToString("yyyy/MM/dd HH:mm:ss");
                    break;

                case Enums.SelectDate.Months3:
                    dtStart = dtStart.AddMonths(-2);    // current month inclusive, only subtract 2
                    startDate = dtStart.ToString("yyyy/MM/dd HH:mm:ss");
                    break;

                case Enums.SelectDate.Months6:
                    dtStart = dtStart.AddMonths(-5);    // current month inclusive, only subtract 5
                    startDate = dtStart.ToString("yyyy/MM/dd HH:mm:ss");
                    break;

                case Enums.SelectDate.Months12:
                    dtStart = dtStart.AddMonths(-11);   // curretn month inclusive, only subtract 11
                    startDate = dtStart.ToString("yyyy/MM/dd HH:mm:ss");
                    break;

                default:
                    // noop
                    break;
            }

            readings = DatabaseController.GetReadings(selectedMeter, Enums.Interval.Hourly, Enums.UnitOfMeasure.kWh, startDate, endDate);

            // Fill readings over selected range that are absent
            retval = readings.FillAbsentReadings(dtStart, dtEnd, Enums.Interval.Hourly);

            //Assign DayType as Weekend or Weekday
            retval = readings.AssignDayType();

            //Assign Season to readings; checks if rate is seasonal
            retval = readings.AssignSeasonAndBasicCharges(currentRate);

            //Assign TOU to readings; checks if TOU
            retval = readings.AssignTOUAndTOUCharges(currentRate);

        }


        private void LoadHourlyChart()
        {
            GraphPane pane;
            BarItem curve;
            double[] q;
            string[] categories;
            double[] placeholder;
            int i = 0;

            zedChart.MasterPane.PaneList.Clear();

            pane = new GraphPane();
            pane.Title.Text = "Hourly Energy Consumption";
            pane.XAxis.Title.Text = "Month/Day Hour";
            pane.YAxis.Title.Text = "Energy (kWh)";
            pane.XAxis.Scale.FontSpec.Angle = 90;

            if (!currentRate.HasTimeOfUseCharges)
            {

                q = new double[readings.Count];
                categories = new string[readings.Count];
                placeholder = new double[readings.Count];

                foreach (Reading r in readings)
                {
                    q[i] = r.Quantity;
                    categories[i] = r.Datestamp.ToString("M/dd HH:00");
                    placeholder[i] = (double)i;
                    i++;
                }

                curve = pane.AddBar("Energy", placeholder, q, Color.LightGreen);
                pane.XAxis.Scale.TextLabels = categories;
                curve.Bar.Border.IsVisible = true;
                curve.Bar.Border.Color = Color.LightGreen;
           
            }
            else
            {
                // OffPeak Hourly
                i = 0;
                q = new double[hourlyOffPeakReadings.Count];
                categories = new string[hourlyOffPeakReadings.Count];
                placeholder = new double[hourlyOffPeakReadings.Count];

                foreach (Reading r in hourlyOffPeakReadings)
                {
                    q[i] = r.Quantity;
                    categories[i] = r.Datestamp.ToString("M/dd HH:00");
                    placeholder[i] = (double)i;
                    i++;
                }

                curve = pane.AddBar("OffPeak Energy", placeholder, q, Color.LightGreen);
                pane.XAxis.Scale.TextLabels = categories;
                curve.Bar.Border.IsVisible = true;
                curve.Bar.Border.Color = Color.LightGreen;
                

                // Peak Hourly
                i = 0;
                q = new double[hourlyPeakReadings.Count];
                categories = new string[hourlyPeakReadings.Count];
                placeholder = new double[hourlyPeakReadings.Count];

                foreach (Reading r in hourlyPeakReadings)
                {
                    q[i] = r.Quantity;
                    categories[i] = r.Datestamp.ToString("M/dd HH:00");
                    placeholder[i] = (double)i;
                    i++;
                }

                curve = pane.AddBar("Peak Energy", placeholder, q, Color.Red);
                pane.XAxis.Scale.TextLabels = categories;
                curve.Bar.Border.IsVisible = true;
                curve.Bar.Border.Color = Color.Red;
                

                pane.BarSettings.Type = BarType.Stack;  // not really stacking; union of OffPeak and OnPeak datasets
            }


            pane.XAxis.Type = AxisType.Text;

            pane.XAxis.Scale.MajorStep = 12;
            pane.XAxis.Scale.MinorStep = 2;
            pane.Chart.Fill = new Fill(Color.White, Color.FromArgb(220, 220, 225), 45);

            this.zedChart.MasterPane.Add(pane);

            // Tell ZedGraph to calculate the axis ranges, and layout
            zedChart.AxisChange();
            using (Graphics g = this.CreateGraphics())
            {
                zedChart.MasterPane.SetLayout(g, PaneLayout.SingleColumn);
            }
            zedChart.Invalidate();
        }

        private void LoadDailyChart()
        {
            GraphPane pane;
            BarItem curve;
            double[] q;
            string[] categories;
            double[] placeholder;
            int i = 0;

            zedChart.MasterPane.PaneList.Clear();

            pane = new GraphPane();
            pane.Title.Text = "Daily Energy Consumption";
            pane.XAxis.Title.Text = "Month/Day";
            pane.YAxis.Title.Text = "Energy (kWh)";
            pane.XAxis.Scale.FontSpec.Angle = 90;

            if (!currentRate.HasTimeOfUseCharges)
            {
                q = new double[dailyreadings.Count];
                categories = new string[dailyreadings.Count];
                placeholder = new double[dailyreadings.Count];

                foreach (Reading r in dailyreadings)
                {
                    q[i] = r.Quantity;
                    categories[i] = r.Datestamp.ToString("M/dd");
                    placeholder[i] = (double)i;
                    i++;
                }

                curve = pane.AddBar("Energy", placeholder, q, Color.LightGreen);
                pane.XAxis.Scale.TextLabels = categories;
                curve.Bar.Border.Color = Color.LightGreen;
            }
            else
            {
                //OffPeak
                i = 0;
                q = new double[dailyOffPeakReadings.Count];
                categories = new string[dailyOffPeakReadings.Count];
                placeholder = new double[dailyOffPeakReadings.Count];

                foreach (Reading r in dailyOffPeakReadings)
                {
                    q[i] = r.Quantity;
                    categories[i] = r.Datestamp.ToString("M/dd");
                    placeholder[i] = (double)i;
                    i++;
                }
                curve = pane.AddBar("OffPeak Energy", placeholder, q, Color.LightGreen);
                curve.Bar.Border.Color = Color.LightGreen;

                //Peak
                i = 0;
                q = new double[dailyPeakReadings.Count];
                categories = new string[dailyPeakReadings.Count];
                placeholder = new double[dailyPeakReadings.Count];

                foreach (Reading r in dailyPeakReadings)
                {
                    q[i] = r.Quantity;
                    categories[i] = r.Datestamp.ToString("M/dd");
                    placeholder[i] = (double)i;
                    i++;
                }
                curve = pane.AddBar("Peak Energy", placeholder, q, Color.Red);
                curve.Bar.Border.Color = Color.Red;

                pane.XAxis.Scale.TextLabels = categories;
                pane.BarSettings.Type = BarType.Stack;  //accumulating stack
            }

            pane.XAxis.Type = AxisType.Text;
            pane.Chart.Fill = new Fill(Color.White, Color.FromArgb(220, 220, 225), 45);

            this.zedChart.MasterPane.Add(pane);

            // Tell ZedGraph to calculate the axis ranges, and layout
            zedChart.AxisChange();
            using (Graphics g = this.CreateGraphics())
            {
                zedChart.MasterPane.SetLayout(g, PaneLayout.SingleColumn);
            }
            zedChart.Invalidate();
        }

        private void LoadDailyCostChart()
        {
            GraphPane pane;
            BarItem curve;
            double[] q;
            string[] categories;
            double[] placeholder;
            int i = 0;

            zedChart.MasterPane.PaneList.Clear();

            pane = new GraphPane();
            pane.Title.Text = "Daily Cost";
            pane.XAxis.Title.Text = "Month/Day";
            pane.YAxis.Title.Text = "Cost $";
            pane.XAxis.Scale.FontSpec.Angle = 90;

            if (!currentRate.HasTimeOfUseCharges)
            {
                q = new double[dailyCosts.Count];
                categories = new string[dailyCosts.Count];
                placeholder = new double[dailyCosts.Count];

                foreach (Reading r in dailyCosts)
                {
                    q[i] = r.Quantity;
                    categories[i] = r.Datestamp.ToString("M/dd");
                    placeholder[i] = (double)i;
                    i++;
                }

                curve = pane.AddBar("Cost", placeholder, q, Color.LightGreen);
                curve.Bar.Border.Color = Color.LightGreen;
                pane.XAxis.Scale.TextLabels = categories;

            }
            else
            {
                //OffPeak
                i = 0;
                q = new double[dailyOffPeakCosts.Count];
                categories = new string[dailyOffPeakCosts.Count];
                placeholder = new double[dailyOffPeakCosts.Count];

                foreach (Reading r in dailyOffPeakCosts)
                {
                    q[i] = r.Quantity;
                    categories[i] = r.Datestamp.ToString("M/dd");
                    placeholder[i] = (double)i;
                    i++;
                }
                curve = pane.AddBar("Off Peak Cost", placeholder, q, Color.LightGreen);
                curve.Bar.Border.Color = Color.LightGreen;

                //Peak
                i = 0;
                q = new double[dailyPeakCosts.Count];
                categories = new string[dailyPeakCosts.Count];
                placeholder = new double[dailyPeakCosts.Count];

                foreach (Reading r in dailyPeakCosts)
                {
                    q[i] = r.Quantity;
                    categories[i] = r.Datestamp.ToString("M/dd");
                    placeholder[i] = (double)i;
                    i++;
                }
                curve = pane.AddBar("Peak Cost", placeholder, q, Color.Red);
                curve.Bar.Border.Color = Color.Red;

                pane.XAxis.Scale.TextLabels = categories;
                pane.BarSettings.Type = BarType.Stack;  //accumulating stack
            }

            pane.XAxis.Type = AxisType.Text;
            pane.Chart.Fill = new Fill(Color.White, Color.FromArgb(220, 220, 225), 45);

            this.zedChart.MasterPane.Add(pane);

            // Tell ZedGraph to calculate the axis ranges, and layout
            zedChart.AxisChange();
            using (Graphics g = this.CreateGraphics())
            {
                zedChart.MasterPane.SetLayout(g, PaneLayout.SingleColumn);
            }
            zedChart.Invalidate();
        }

    }

}
