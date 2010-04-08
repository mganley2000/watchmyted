using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace Energy.Library
{
    /// <summary>
    /// Configuration class stores basic configuration information
    /// COM port, default rate, TED settings, GAE settings.
    /// </summary>
    public class Configuration
    {
        private string name;
        private string description;
        private string comPort;
        private int baud;
        private Parity parity;
        private int databits;
        private int stopbits;
        private int rateid;
        private bool tedMTU0Enabled;            //
        private string tedUrl1;                 //for The Energy Detective (TED)
        private string tedUrl2;                 //for The Energy Detective (TED)
        private string tedUrl3;                 //for The Energy Detective (TED)
        private string gaeReadingsServiceUrl;   //for Google App Engine (GAE)
        private string gaeGmail;                //
        private string gaePassword;             //
        private string gaeSiteUrl;              //
        private string gaeAppName;              //

        public Configuration()
        {
            name = string.Empty;
            description = string.Empty;
            comPort = string.Empty;
            tedMTU0Enabled = false;
            tedUrl1 = string.Empty;
            tedUrl2 = string.Empty;
            tedUrl3 = string.Empty;
            gaeReadingsServiceUrl = string.Empty;
            gaeGmail = string.Empty;
            gaePassword = string.Empty;
        }

        public string GAESiteUrl
        {
            get { return gaeSiteUrl; }
            set { gaeSiteUrl = value; }
        }

        public string GAEAppName
        {
            get { return gaeAppName; }
            set { gaeAppName = value; }
        }

        public bool TEDMTU0Enabled
        {
            get { return tedMTU0Enabled; }
            set { tedMTU0Enabled = value; }
        }

        public string GAEPassword
        {
            get { return gaePassword; }
            set { gaePassword = value; }
        }

        public string GAEGmail
        {
            get { return gaeGmail; }
            set { gaeGmail = value; }
        }

        public string GAEReadingsServiceUrl
        {
            get { return gaeReadingsServiceUrl; }
            set { gaeReadingsServiceUrl = value; }
        }

        public string TEDUrl1
        {
            get { return tedUrl1; }
            set { tedUrl1 = value; }
        }

        public string TEDUrl2
        {
            get { return tedUrl2; }
            set { tedUrl2 = value; }
        }

        public string TEDUrl3
        {
            get { return tedUrl3; }
            set { tedUrl3 = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public string COMPort
        {
            get { return comPort; }
            set { comPort = value; }
        }

        public int Baud
        {
            get { return baud; }
            set { baud = value; }
        }

        public Parity Parity
        {
            get { return parity; }
            set { parity = value; }
        }

        public int DataBits
        {
            get { return databits; }
            set { databits = value; }
        }

        public int StopBits
        {
            get { return stopbits; }
            set { stopbits = value; }
        }

        public int RateID
        {
            get { return rateid; }
            set { rateid = value; }
        }

    }
}
