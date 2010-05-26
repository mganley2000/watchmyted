using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Energy.Library
{
    /// <summary>
    /// For TED
    /// </summary>
    public class TEDSecond : TEDBaseInterval
    {
        private bool _newFor900sec;
        private bool _newFor3600sec;

        public TEDSecond() : base()
        {
            _newFor900sec = true;
            _newFor3600sec = true;
        }

        public bool IsNewFor900sec
        {
            get { return _newFor900sec; }
            set { _newFor900sec = value; }
        }

        public bool IsNewFor3600sec
        {
            get { return _newFor3600sec; }
            set { _newFor3600sec = value; }
        }

    }
}
