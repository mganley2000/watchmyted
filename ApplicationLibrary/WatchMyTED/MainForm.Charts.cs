using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using ZedGraph;
using Energy.Library;
using System.Linq;

namespace Energy.WatchMyTED
{

    /// <summary>
    /// Charting details present in this partial class
    /// The real time rolling charts
    /// </summary>
    public partial class MainForm
    {
        GraphPane pane;
        private List<GraphPaneMeter> pane900secPowerList = new List<GraphPaneMeter>();
        private List<GraphPaneMeter> pane3600secPowerList = new List<GraphPaneMeter>();
        private List<GraphPaneMeter> pane720minPowerList = new List<GraphPaneMeter>();
        private List<GraphPaneMeter> pane1440minPowerList = new List<GraphPaneMeter>();
        private List<GraphPaneMeter> pane24HourConsumptionList = new List<GraphPaneMeter>();

        private void Power900SecondChart(bool showMe)
        {
            int count = 0;
            bool foundPaneMeter;
            bool meterIsNew;
            LineItem curve;
            RollingPointPairList list;
            IPointListEdit editList;

            // Get a reference to the GraphPane instance in the ZedGraphControl
            MasterPane master = zg1.MasterPane;

            if (showMe)
            {
                master.PaneList.Clear();
                master.Title.IsVisible = false;
                master.Title.Text = "Charts";
                master.Margin.All = 5;
                master.Border.IsVisible = false;
            }

            foreach (TEDMeter meter in tedMeters)
            {
                // create new or grab existing pane per meter
                meterIsNew = false;
                foundPaneMeter = false;
                foreach (GraphPaneMeter p in pane900secPowerList)
                {
                    if (p.MeterID == meter.ID)
                    {
                        pane = p.Pane;
                        foundPaneMeter = true;
                        break;
                    }
                }
                if (!foundPaneMeter)
                {
                    pane = new GraphPane();
                    GraphPaneMeter newPane = new GraphPaneMeter();
                    newPane.MeterID = meter.ID;
                    newPane.Pane = pane;
                    pane900secPowerList.Add(newPane);
                    meterIsNew = true;
                }

                if (meterIsNew)
                {
                    // Set the titles and axis labels
                    pane.Title.Text = meter.Name + "\n\r" + "Power per second (last 15-min)";
                    pane.XAxis.Title.Text = "min:sec";
                    pane.YAxis.Title.Text = "Power (W)";

                    // 1 sec power; rolling point pair, add with no points to start; 
                    list = new RollingPointPairList(910);
                    curve = pane.AddCurve("Power", list, Color.Purple, SymbolType.None);
                    editList = list;
                    curve.Symbol.Fill = new Fill(Color.White);
                    pane.XAxis.MajorGrid.IsVisible = true;
                    pane.YAxis.Scale.FontSpec.FontColor = Color.Purple;
                    pane.YAxis.Title.FontSpec.FontColor = Color.Purple;
                    pane.YAxis.MajorGrid.IsZeroLine = false;
                    pane.YAxis.Scale.Align = AlignP.Inside;
                    pane.YAxis.Scale.Min = 0;
                    pane.XAxis.Type = AxisType.Date;
                    pane.XAxis.Scale.Format = "mm:ss";
                    pane.XAxis.Scale.MajorUnit = DateUnit.Minute;
                    pane.XAxis.Scale.MinorUnit = DateUnit.Second;
                    pane.XAxis.Scale.MinorStep = 30;
                    pane.XAxis.Scale.MajorStep = 1;
                    pane.XAxis.Scale.Max = meter.AddedTimestamp.AddSeconds(900).ToOADate();
                    pane.XAxis.Scale.Min = meter.AddedTimestamp.ToOADate();

                    pane.Chart.Fill = new Fill(Color.White, Color.LightGray, 45.0f);
                }

                count = meter.SecondList.Count;

                if (count > 0)
                {
                    List<TEDSecond> newSeconds = meter.SecondList.FindAll(m => m.IsNewFor900sec == true);

                    curve = pane.CurveList[0] as LineItem;
                    if (curve != null)
                    {
                        editList = curve.Points as IPointListEdit;
                        if (editList != null)
                        {

                            foreach (TEDSecond sec in newSeconds)
                            {
                                editList.Add(sec.Timestamp.ToOADate(), sec.POWER);
                                sec.IsNewFor900sec = false;
                            }

                        }

                    }

                    if (meter.AddedTimestamp.AddSeconds(900) < meter.SecondList[meter.SecondList.Count - 1].Timestamp)
                    {
                        pane.XAxis.Scale.Max = meter.SecondList[meter.SecondList.Count - 1].Timestamp.AddSeconds(5).ToOADate();
                        pane.XAxis.Scale.Min = meter.SecondList[meter.SecondList.Count - 1].Timestamp.AddSeconds(-900).ToOADate();
                    }

                }

                if (showMe) { master.Add(pane); }

            }

            if (showMe)
            {
                zg1.AxisChange();
                using (Graphics g = this.CreateGraphics())
                {
                    master.SetLayout(g, PaneLayout.SingleColumn);
                }
                zg1.Invalidate();
                zg1.IsShowPointValues = true;
      
            }

        }

        private void Power3600SecondChart(bool showMe)
        {
            int count = 0;
            bool foundPaneMeter;
            bool meterIsNew;
            LineItem curve;
            RollingPointPairList list;
            IPointListEdit editList;

            // Get a reference to the GraphPane instance in the ZedGraphControl
            MasterPane master = zg1.MasterPane;

            if (showMe)
            {
                master.PaneList.Clear();
                master.Title.IsVisible = false;
                master.Title.Text = "Charts";
                master.Margin.All = 5;
                master.Border.IsVisible = false;
            }

            foreach (TEDMeter meter in tedMeters)
            {
                // create new or grab existing pane per meter
                meterIsNew = false;
                foundPaneMeter = false;
                foreach (GraphPaneMeter p in pane3600secPowerList)
                {
                    if (p.MeterID == meter.ID)
                    {
                        pane = p.Pane;
                        foundPaneMeter = true;
                        break;
                    }
                }
                if (!foundPaneMeter)
                {
                    pane = new GraphPane();
                    GraphPaneMeter newPane = new GraphPaneMeter();
                    newPane.MeterID = meter.ID;
                    newPane.Pane = pane;
                    pane3600secPowerList.Add(newPane);
                    meterIsNew = true;
                }

                if (meterIsNew)
                {
                    // Set the titles and axis labels
                    pane.Title.Text = meter.Name + "\n\r" + "Power per second (last 60-min)";
                    pane.XAxis.Title.Text = "min:sec";
                    pane.YAxis.Title.Text = "Power (W)";

                    // 1 sec power; rolling point pair, add with no points to start; 
                    list = new RollingPointPairList(3610);
                    curve = pane.AddCurve("Power", list, Color.Purple, SymbolType.None);
                    editList = list;
                    curve.Symbol.Fill = new Fill(Color.White);
                    pane.XAxis.MajorGrid.IsVisible = true;
                    pane.YAxis.Scale.FontSpec.FontColor = Color.Purple;
                    pane.YAxis.Title.FontSpec.FontColor = Color.Purple;
                    pane.YAxis.MajorGrid.IsZeroLine = false;
                    pane.YAxis.Scale.Align = AlignP.Inside;
                    pane.YAxis.Scale.Min = 0;
                    pane.XAxis.Type = AxisType.Date;
                    pane.XAxis.Scale.Format = "mm:ss";
                    pane.XAxis.Scale.MajorUnit = DateUnit.Minute;
                    pane.XAxis.Scale.MinorUnit = DateUnit.Second;
                    pane.XAxis.Scale.MinorStep = 30;
                    pane.XAxis.Scale.MajorStep = 1;
                    pane.XAxis.Scale.Max = meter.AddedTimestamp.AddSeconds(3600).ToOADate();
                    pane.XAxis.Scale.Min = meter.AddedTimestamp.ToOADate();

                    pane.Chart.Fill = new Fill(Color.White, Color.LightGray, 45.0f);
                }

                count = meter.SecondList.Count;

                if (count > 0)
                {
                    List<TEDSecond> newSeconds = meter.SecondList.FindAll(m => m.IsNewFor3600sec == true);

                    curve = pane.CurveList[0] as LineItem;
                    if (curve != null)
                    {
                        editList = curve.Points as IPointListEdit;
                        if (editList != null)
                        {

                            foreach (TEDSecond sec in newSeconds)
                            {
                                editList.Add(sec.Timestamp.ToOADate(), sec.POWER);
                                sec.IsNewFor3600sec = false;
                            }

                        }

                    }

                    if (meter.AddedTimestamp.AddSeconds(3600) < meter.SecondList[meter.SecondList.Count - 1].Timestamp)
                    {
                        pane.XAxis.Scale.Max = meter.SecondList[meter.SecondList.Count - 1].Timestamp.AddSeconds(5).ToOADate();
                        pane.XAxis.Scale.Min = meter.SecondList[meter.SecondList.Count - 1].Timestamp.AddSeconds(-3600).ToOADate();
                    }

                }

                if (showMe) { master.Add(pane); }

            }

            if (showMe)
            {
                zg1.AxisChange();
                using (Graphics g = this.CreateGraphics())
                {
                    master.SetLayout(g, PaneLayout.SingleColumn);
                }
                zg1.Invalidate();
                zg1.IsShowPointValues = true;
            }

        }

        private void Power720MinuteChart(bool showMe)
        {
            int count;
            bool foundPaneMeter;
            bool meterIsNew;
            LineItem curve;
            RollingPointPairList list;
            IPointListEdit editList;

            // Get a reference to the GraphPane instance in the ZedGraphControl
            MasterPane master = zg1.MasterPane;

            if (showMe)
            {
                master.PaneList.Clear();
                master.Title.IsVisible = false;
                master.Title.Text = "Charts";
                master.Margin.All = 5;
                master.Border.IsVisible = false;
            }

            foreach (TEDMeter meter in tedMeters)
            {
                // create new or grab existing pane per meter
                meterIsNew = false;
                foundPaneMeter = false;
                foreach (GraphPaneMeter p in pane720minPowerList)
                {
                    if (p.MeterID == meter.ID)
                    {
                        pane = p.Pane;
                        foundPaneMeter = true;
                        break;
                    }
                }
                if (!foundPaneMeter)
                {
                    pane = new GraphPane();
                    GraphPaneMeter newPane = new GraphPaneMeter();
                    newPane.MeterID = meter.ID;
                    newPane.Pane = pane;
                    pane720minPowerList.Add(newPane);
                    meterIsNew = true;
                }

                if (meterIsNew)
                {
                    // Set the titles and axis labels
                    pane.Title.Text = meter.Name + "\n\r" + "Power per minute (last 12-hours)";
                    pane.XAxis.Title.Text = "hour:min";
                    pane.YAxis.Title.Text = "Power (W)";

                    // 1 sec power; rolling point pair, add with no points to start; 
                    list = new RollingPointPairList(730);
                    curve = pane.AddCurve("Power", list, Color.Purple, SymbolType.None);
                    editList = list;
                    curve.Symbol.Fill = new Fill(Color.White);
                    pane.XAxis.MajorGrid.IsVisible = true;
                    pane.YAxis.Scale.FontSpec.FontColor = Color.Purple;
                    pane.YAxis.Title.FontSpec.FontColor = Color.Purple;
                    pane.YAxis.MajorGrid.IsZeroLine = false;
                    pane.YAxis.Scale.Align = AlignP.Inside;
                    pane.YAxis.Scale.Min = 0;
                    pane.XAxis.Type = AxisType.Date;
                    pane.XAxis.Scale.Format = "hh:mm";
                    pane.XAxis.Scale.MajorUnit = DateUnit.Hour;
                    pane.XAxis.Scale.MinorUnit = DateUnit.Minute;
                    pane.XAxis.Scale.MinorStep = 15;
                    pane.XAxis.Scale.MajorStep = 1;
                    pane.XAxis.Scale.Max = meter.AddedTimestamp.AddMinutes(720).ToOADate();
                    pane.XAxis.Scale.Min = meter.AddedTimestamp.ToOADate();

                    pane.Chart.Fill = new Fill(Color.White, Color.LightGray, 45.0f);
                }

                count = meter.MinuteList.Count;

                if (count > 0)
                {
                    List<TEDMinute> newMinutes = meter.MinuteList.FindAll(m => m.IsNewFor720min == true);

                    curve = pane.CurveList[0] as LineItem;
                    if (curve != null)
                    {
                        editList = curve.Points as IPointListEdit;
                        if (editList != null)
                        {

                            foreach (TEDMinute min in newMinutes)
                            {
                                editList.Add(min.Timestamp.ToOADate(), min.POWER);
                                min.IsNewFor720min = false;
                            }

                        }

                    }

                    if (meter.AddedTimestamp.AddMinutes(720) < meter.MinuteList[meter.MinuteList.Count - 1].Timestamp)
                    {
                        pane.XAxis.Scale.Max = meter.MinuteList[meter.MinuteList.Count - 1].Timestamp.AddMinutes(5).ToOADate();
                        pane.XAxis.Scale.Min = meter.MinuteList[meter.MinuteList.Count - 1].Timestamp.AddMinutes(-720).ToOADate();
                    }

                }

                if (showMe) { master.Add(pane); }

            }

            if (showMe)
            {
                zg1.AxisChange();
                using (Graphics g = this.CreateGraphics())
                {
                    master.SetLayout(g, PaneLayout.SingleColumn);
                }
                zg1.Invalidate();
                zg1.IsShowPointValues = true;
            }

        }

        private void Power1440MinuteChart(bool showMe)
        {
            int count;
            bool foundPaneMeter;
            bool meterIsNew;
            LineItem curve;
            RollingPointPairList list;
            IPointListEdit editList;

            // Get a reference to the GraphPane instance in the ZedGraphControl
            MasterPane master = zg1.MasterPane;

            if (showMe)
            {
                master.PaneList.Clear();
                master.Title.IsVisible = false;
                master.Title.Text = "Charts";
                master.Margin.All = 5;
                master.Border.IsVisible = false;
            }

            foreach (TEDMeter meter in tedMeters)
            {
                // create new or grab existing pane per meter
                meterIsNew = false;
                foundPaneMeter = false;
                foreach (GraphPaneMeter p in pane1440minPowerList)
                {
                    if (p.MeterID == meter.ID)
                    {
                        pane = p.Pane;
                        foundPaneMeter = true;
                        break;
                    }
                }
                if (!foundPaneMeter)
                {
                    pane = new GraphPane();
                    GraphPaneMeter newPane = new GraphPaneMeter();
                    newPane.MeterID = meter.ID;
                    newPane.Pane = pane;
                    pane1440minPowerList.Add(newPane);
                    meterIsNew = true;
                }

                if (meterIsNew)
                {
                    // Set the titles and axis labels
                    pane.Title.Text = meter.Name + "\n\r" + "Power per minute (last 24-hours)";
                    pane.XAxis.Title.Text = "hour:min";
                    pane.YAxis.Title.Text = "Power (W)";

                    // 1 sec power; rolling point pair, add with no points to start; 
                    list = new RollingPointPairList(1450);
                    curve = pane.AddCurve("Power", list, Color.Purple, SymbolType.None);
                    editList = list;
                    curve.Symbol.Fill = new Fill(Color.White);
                    pane.XAxis.MajorGrid.IsVisible = true;
                    pane.YAxis.Scale.FontSpec.FontColor = Color.Purple;
                    pane.YAxis.Title.FontSpec.FontColor = Color.Purple;
                    pane.YAxis.MajorGrid.IsZeroLine = false;
                    pane.YAxis.Scale.Align = AlignP.Inside;
                    pane.YAxis.Scale.Min = 0;
                    pane.XAxis.Type = AxisType.Date;
                    pane.XAxis.Scale.Format = "hh:mm";
                    pane.XAxis.Scale.MajorUnit = DateUnit.Hour;
                    pane.XAxis.Scale.MinorUnit = DateUnit.Minute;
                    pane.XAxis.Scale.MinorStep = 15;
                    pane.XAxis.Scale.MajorStep = 1;
                    pane.XAxis.Scale.Max = meter.AddedTimestamp.AddMinutes(1440).ToOADate();
                    pane.XAxis.Scale.Min = meter.AddedTimestamp.ToOADate();

                    pane.Chart.Fill = new Fill(Color.White, Color.LightGray, 45.0f);
                }

                count = meter.MinuteList.Count;

                if (count > 0)
                {
                    List<TEDMinute> newMinutes = meter.MinuteList.FindAll(m => m.IsNewFor1440min == true);

                    curve = pane.CurveList[0] as LineItem;
                    if (curve != null)
                    {
                        editList = curve.Points as IPointListEdit;
                        if (editList != null)
                        {

                            foreach (TEDMinute min in newMinutes)
                            {
                                editList.Add(min.Timestamp.ToOADate(), min.POWER);
                                min.IsNewFor1440min = false;
                            }

                        }

                    }

                    if (meter.AddedTimestamp.AddMinutes(1440) < meter.MinuteList[meter.MinuteList.Count - 1].Timestamp)
                    {
                        pane.XAxis.Scale.Max = meter.MinuteList[meter.MinuteList.Count - 1].Timestamp.AddMinutes(5).ToOADate();
                        pane.XAxis.Scale.Min = meter.MinuteList[meter.MinuteList.Count - 1].Timestamp.AddMinutes(-1440).ToOADate();
                    }

                }

                if (showMe) { master.Add(pane); }

            }

            if (showMe)
            {
                zg1.AxisChange();
                using (Graphics g = this.CreateGraphics())
                {
                    master.SetLayout(g, PaneLayout.SingleColumn);
                }
                zg1.Invalidate();
                zg1.IsShowPointValues = true;
            }

        }

        private void ConsumptionHourlyChart(bool showMe)
        {
            int count;
            bool foundPaneMeter;
            bool meterIsNew;
            BarItem curve;
            RollingPointPairList list;
            IPointListEdit editList;

            // Get a reference to the GraphPane instance in the ZedGraphControl
            MasterPane master = zg1.MasterPane;

            if (showMe)
            {
                master.PaneList.Clear();
                master.Title.IsVisible = false;
                master.Title.Text = "Charts";
                master.Margin.All = 5;
                master.Border.IsVisible = false;
            }

            foreach (TEDMeter meter in tedMeters)
            {
                // create new or grab existing pane per meter
                meterIsNew = false;
                foundPaneMeter = false;
                foreach (GraphPaneMeter p in pane24HourConsumptionList)
                {
                    if (p.MeterID == meter.ID)
                    {
                        pane = p.Pane;
                        foundPaneMeter = true;
                        break;
                    }
                }
                if (!foundPaneMeter)
                {
                    pane = new GraphPane();
                    GraphPaneMeter newPane = new GraphPaneMeter();
                    newPane.MeterID = meter.ID;
                    newPane.Pane = pane;
                    pane24HourConsumptionList.Add(newPane);
                    meterIsNew = true;
                }

                count = meter.HourList.Count;

                if (meterIsNew)
                {
                    // Set the titles and axis labels
                    pane.Title.Text = meter.Name + "\n\r" + "Energy Consumption (Hourly)";
                    pane.XAxis.Title.Text = "Hour";
                    pane.YAxis.Title.Text = "Energy (kWh)";
                    pane.XAxis.Scale.FontSpec.Angle = 90;

                    // Hourly Consumption; rolling point pair, add with no points to start; 
                    list = new RollingPointPairList(24);    // 36 hours
                    curve = pane.AddBar("Energy", list, Color.Blue);

                    editList = list;
                    curve.Bar.Fill = new Fill(Color.LightGreen);
                    curve.Bar.Fill.Type = FillType.Solid;
                    pane.XAxis.Type = AxisType.Date;
                    pane.XAxis.Scale.Format = "M/dd HH:mm";
                    pane.XAxis.Scale.MajorUnit = DateUnit.Day;
                    pane.XAxis.Scale.MinorUnit = DateUnit.Hour;
                    pane.XAxis.Scale.MinorStep = 1;
                    pane.XAxis.Scale.MajorStep = 1;
                    pane.XAxis.Scale.Max = meter.AddedTimestamp.AddHours(24).ToOADate();
                    pane.XAxis.Scale.Min = meter.AddedTimestamp.ToOADate();

                    pane.Chart.Fill = new Fill(Color.White, Color.FromArgb(220, 220, 225), 45);
                }
                else
                {
                    if (count > 0)
                    {
                        List<TEDHour> newHours = meter.HourList.FindAll(m => m.IsNewFor24HourConsumption == true);

                        curve = pane.CurveList[0] as BarItem;
                        if (curve != null)
                        {
                            editList = curve.Points as IPointListEdit;
                            if (editList != null)
                            {

                                foreach (TEDHour hour in newHours)
                                {
                                    editList.Add(hour.Timestamp.ToOADate(), hour.Consumption_kWh());
                                    hour.IsNewFor24HourConsumption = false;
                                }

                            }

                        }

                        if (meter.AddedTimestamp.AddHours(24) < meter.HourList[meter.HourList.Count - 1].Timestamp)
                        {
                            pane.XAxis.Scale.Max = meter.HourList[meter.HourList.Count - 1].Timestamp.AddHours(0).ToOADate();
                            pane.XAxis.Scale.Min = meter.HourList[meter.HourList.Count - 1].Timestamp.AddHours(-24).ToOADate();
                        } 

                    }
                }

                if (showMe) { master.Add(pane); }

            }

            if (showMe)
            {
                zg1.AxisChange();
                using (Graphics g = this.CreateGraphics())
                {
                    master.SetLayout(g, PaneLayout.SingleColumn);
                }

                zg1.Invalidate();
                zg1.IsShowPointValues = true;
            }

        }


        #region "ZedGraph Helpers"

        private void SetSize()
        {
            zg1.Location = new Point(10, 10);
            // Leave a small margin around the outside of the control
            zg1.Size = new Size(this.ClientRectangle.Width - 10,
                    this.ClientRectangle.Height - 10);
        }


        /// <summary>
        /// Display customized tooltips when the mouse hovers over a point
        /// </summary>
        private string MyPointValueHandler(ZedGraphControl control, GraphPane pane,
                        CurveItem curve, int iPt)
        {
            // Get the PointPair that is under the mouse
            PointPair pt = curve[iPt];

            return curve.Label.Text + " is " + pt.Y.ToString("f2") + " units at " + pt.X.ToString("f1") + " days";
        }

        /// <summary>
        /// Customize the context menu by adding a new item to the end of the menu
        /// </summary>
        private void MyContextMenuBuilder(ZedGraphControl control, ContextMenuStrip menuStrip,
                        Point mousePt, ZedGraphControl.ContextMenuObjectState objState)
        {
            ToolStripMenuItem item = new ToolStripMenuItem();
            item.Name = "add-beta";
            item.Tag = "add-beta";
            item.Text = "Add a new Beta Point";
            item.Click += new System.EventHandler(AddBetaPoint);

            menuStrip.Items.Add(item);
        }

        /// <summary>
        /// Handle the "Add New Beta Point" context menu item.  This finds the curve with
        /// the CurveItem.Label = "Beta", and adds a new point to it.
        /// </summary>
        private void AddBetaPoint(object sender, EventArgs args)
        {
            // Get a reference to the "Beta" curve IPointListEdit
            IPointListEdit ip = zg1.GraphPane.CurveList["Beta"].Points as IPointListEdit;
            if (ip != null)
            {
                double x = ip.Count * 5.0;
                double y = Math.Sin(ip.Count * Math.PI / 15.0) * 16.0 * 13.5;
                ip.Add(x, y);
                zg1.AxisChange();
                zg1.Refresh();
            }
        }

        // Respond to a Zoom Event
        private void MyZoomEvent(ZedGraphControl control, ZoomState oldState,
                    ZoomState newState)
        {
            // Here we get notification everytime the user zooms
        }

        #endregion

    }

}
