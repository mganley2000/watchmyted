using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Energy.Library
{
    public class TEDMinute : TEDBaseInterval
    {
        private bool _newFor720min;
        private bool _newFor1440min;

        public TEDMinute() : base()
        {
            _newFor720min = true;
            _newFor1440min = true;
        }

        public bool IsNewFor720min
        {
            get { return _newFor720min; }
            set { _newFor720min = value; }
        }

        public bool IsNewFor1440min
        {
            get { return _newFor1440min; }
            set { _newFor1440min = value; }
        }

    }
}
