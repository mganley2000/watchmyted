using System;
using System.Text;
using System.Data.SQLite;
using System.Data;
using System.Configuration;
using System.IO;
using System.IO.Ports;

namespace Energy.Library
{
    /// <summary>
    /// Global variables available to share between all forms
    /// </summary>
    public static class Global
    {
        private static string _googleCookie = string.Empty;

        public static string GoogleCookie
        {
            get { return _googleCookie; }
            set { _googleCookie = value; }
        }
    }
}
