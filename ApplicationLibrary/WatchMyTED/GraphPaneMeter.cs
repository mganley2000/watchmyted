using System;
using System.Collections.Generic;
using System.Text;
using ZedGraph;

namespace Energy.EnergyWatcher
{
    /// <summary>
    /// Class to allow storage of meterID with GraphPane
    /// </summary>
    public class GraphPaneMeter
    {
        private GraphPane pane;
        private int meterID;

        public GraphPane Pane
        {
            get { return pane; }
            set { pane = value; }
        }

        public int MeterID
        {
            get { return meterID; }
            set { meterID = value; }
        }
    }

}
