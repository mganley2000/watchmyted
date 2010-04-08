using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Energy.Library
{
    public class BaseAddedEventArgs : System.EventArgs
    {
        private int _meterid;
        private string _meterName;
        private Configuration _config;
        private GAEClient _gaeClient;

        public BaseAddedEventArgs(int meterid, string meterName,Configuration config, GAEClient gaeClient) : base()
        {
            _meterid = meterid;
            _meterName = meterName;
            _config = config;
            _gaeClient = gaeClient;
        }

        #region Properties

        public int MeterID
        {
            get { return _meterid; }
            set { _meterid = value; }
        }

        public string MeterName
        {
            get { return _meterName; }
            set { _meterName = value; }
        }

        public GAEClient GaeClient
        {
            get { return _gaeClient; }
            set { _gaeClient = value; }
        }

        public Configuration Config
        {
            get { return _config; }
            set { _config = value; }
        }

        #endregion

    }
}