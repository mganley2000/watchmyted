using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Energy.Library
{
    /// <summary>
    /// For TED
    /// </summary>
    public abstract class TEDBaseInterval
    {
        private int _mtu;
        private string _date;
        private int _power;
        private int _cost;
        private int _voltage;
        private DateTime _timestamp;

        public TEDBaseInterval()
        {
        }

        public DateTime Timestamp
        {
            get { return _timestamp; }
            set { _timestamp = value; }
        }

        public virtual int MTU
        {
            get { return _mtu; }
            set { _mtu = value; }
        }

        public virtual string DATE
        {
            get { return _date; }
            set { _date = value; }
        }

        public virtual int POWER
        {
            get { return _power; }
            set { _power = value; }
        }

        public virtual int COST
        {
            get { return _cost; }
            set { _cost = value; }
        }

        public virtual int VOLTAGE
        {
            get { return _voltage; }
            set { _voltage = value; }
        }

    }
}
