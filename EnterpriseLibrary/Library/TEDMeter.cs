using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Energy.Library
{
    /// <summary>
    /// For TED
    /// </summary>
    public class TEDMeter
    {
        private int _id;
        private string _name;
        private DateTime _addedTimestamp;
        private TEDSecondList _seconds = new TEDSecondList();
        private TEDMinuteList _minutes = new TEDMinuteList();
        private TEDHourList _hours = new TEDHourList();
        private TEDMinuteList _derived15Minutes = new TEDMinuteList();

        public TEDMeter()
        {
        }

        public DateTime AddedTimestamp
        {
            get { return _addedTimestamp; }
            set { _addedTimestamp = value; }
        }

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public TEDSecondList SecondList
        {
            get { return _seconds; }
            set { _seconds = value; }
        }

        public TEDMinuteList MinuteList
        {
            get { return _minutes; }
            set { _minutes = value; }
        }

        public TEDHourList HourList
        {
            get { return _hours; }
            set { _hours = value; }
        }

        public TEDMinuteList Derived15Minutes
        {
            get { return _derived15Minutes; }
            set { _derived15Minutes = value; }
        }
    }
}
