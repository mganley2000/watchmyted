using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Energy.Library
{
    /// <summary>
    /// For Hour Handling
    /// </summary>
    public class HourAddedEventArgs : BaseAddedEventArgs
    {
        private TEDHour _hour;

        public HourAddedEventArgs(int meterid, string meterName, TEDHour hour, Configuration config, GAEClient gaeClient)
            : base(meterid, meterName, config, gaeClient)
        {
            _hour = hour;

        }

        #region Properties

        public TEDHour Hour
        {
            get { return _hour; }
            set { _hour = value; }
        }

        #endregion

    }
}
