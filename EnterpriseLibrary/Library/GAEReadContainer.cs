using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Energy.Library
{
    [DataContract]
    public class GAEReadContainer
    {
        private GAERead _gaeRead;

        public GAEReadContainer()
        {
        }

        public GAEReadContainer(GAERead gaeRead)
        {
            _gaeRead = gaeRead;
        }

        #region Properties

        [DataMember]
        public GAERead ReadData
        {
            get { return _gaeRead; }
            set { _gaeRead = value; }
        }

        #endregion

    }
}
