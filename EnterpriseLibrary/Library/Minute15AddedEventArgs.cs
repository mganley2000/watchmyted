using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Energy.Library
{
    public class Minute15AddedEventArgs : BaseAddedEventArgs
    {
        private TEDMinute _minute;
        private List<TEDMinute> _minuteList;

        public Minute15AddedEventArgs(int meterid, string meterName, TEDMinute minute, List<TEDMinute> minuteList, Configuration config, GAEClient gaeClient)
            : base(meterid, meterName, config, gaeClient)
        {
            _minute = minute;
            _minuteList = minuteList;
        }

        #region Properties

        public TEDMinute Minute
        {
            get { return _minute; }
            set { _minute = value; }
        }

        public List<TEDMinute> MinuteList
        {
            get { return _minuteList; }
            set { _minuteList = value; }
        }

        #endregion

    }
}
