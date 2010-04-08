using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Energy.Library
{
    public class TEDHour : TEDBaseInterval
    {
        private bool _newFor24HourConsumption;
        private int _vmin;
        private int _vmax;

        public TEDHour() : base()
        {
            _newFor24HourConsumption = true;
        }

        public bool IsNewFor24HourConsumption
        {
            get { return _newFor24HourConsumption; }
            set { _newFor24HourConsumption = value; }
        }


        public int VMIN
        {
            get { return _vmin; }
            set { _vmin = value; }
        }

        public int VMAX
        {
            get { return _vmax; }
            set { _vmax = value; }
        }

        public double Consumption_kWh()
        {
            return( (double)POWER / 1000.0 );
        }

    }
}