using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Web;
using System.Xml;
using System.Diagnostics;

namespace Energy.Library
{
    /// <summary>
    /// Wrapper API to talk to TED Gateway and get usages from that web service
    /// </summary>
    public class TEDController
    {
        const string intervalNameSecond = "SECOND";
        const string intervalNameMinute = "MINUTE";
        const string intervalNameHour = "HOUR";

        #region GetHours

        /// <summary>
        /// Get Hours
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static TEDHourList GetHours(Configuration config)
        {
            return (GetHoursMain(config.TEDUrl1));
        }

        public static TEDHourList GetHours(string url)
        {
            return (GetHoursMain(url));
        }

        /// <summary>
        ///  Get minutes and override the querystring with new values
        ///  http://ted5000/history/hourhistory.xml?MTU=0&INDEX=1&COUNT=10
        /// </summary>
        /// <param name="url"></param>
        /// <param name="mtu"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static TEDHourList GetHours(string url, int mtu, int index, int count)
        {
            StringBuilder sb = new StringBuilder();
            string baseURL = string.Empty;
            string fullURL = string.Empty;
            const string querystring = "?MTU={0}&INDEX={1}&COUNT={2}";

            baseURL = url.Split('?')[0];
            sb.Append(baseURL);
            sb.AppendFormat(querystring, mtu, index, count);

            return (GetHoursMain(sb.ToString()));
        }

        private static TEDHourList GetHoursMain(string url)
        {
            Stream streamXML = null;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = null;
            TEDHourList hours = null;
            bool retval = true;

            req.Method = "GET";
            req.ContentType = "application/x-www-form-urlencoded";
            req.AllowAutoRedirect = false;
            req.Timeout = 10000;

            response = (HttpWebResponse)req.GetResponse();
            streamXML = response.GetResponseStream();

            // Create an XmlReader from stream
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            settings.IgnoreWhitespace = true;
            settings.IgnoreComments = true;
            XmlReader reader = XmlReader.Create(streamXML, settings);

            hours = new TEDHourList();

            retval = GetHoursFromXML(reader, hours);

            reader.Close();

            return (hours);
        }

        public static bool GetHoursFromXML(XmlReader reader, TEDHourList hours)
        {
            string mtu = string.Empty;
            string date = string.Empty;
            string power = string.Empty;
            string cost = string.Empty;
            string vmin = string.Empty;
            string vmax = string.Empty;
            TEDHour hour = null;

            try
            {
                while (reader.Read())
                {

                    if (reader.IsStartElement())
                    {
                        switch (reader.Name)
                        {
                            case "MTU":
                                mtu = reader.ReadString();
                                break;
                            case "DATE":
                                date = reader.ReadString();
                                break;
                            case "POWER":
                                power = reader.ReadString();
                                break;
                            case "COST":
                                cost = reader.ReadString();
                                break;
                            case "VMIN":
                                vmin = reader.ReadString();
                                break;
                            case "VMAX":
                                vmax = reader.ReadString();
                                break;
                        }

                    }

                    if (reader.NodeType == XmlNodeType.EndElement)
                    {
                        if (reader.Name == intervalNameHour)
                        {
                            hour = new TEDHour();
                            hour.MTU = System.Convert.ToInt32(mtu);
                            hour.DATE = date;
                            hour.POWER = System.Convert.ToInt32(power);     // for hourly, also consumption
                            hour.COST = System.Convert.ToInt32(cost);
                            hour.VMIN = System.Convert.ToInt32(vmin);
                            hour.VMAX = System.Convert.ToInt32(vmax);
                            hour.Timestamp = DateTime.Parse(hour.DATE);
                            
                            hours.Add(hour);
                        }
                    }

                }

            }
            catch (XmlException e)
            {
                throw e;
            }

            return (true);
        }

        #endregion

        #region GetMinutes

        /// <summary>
        /// Get Minutes
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static TEDMinuteList GetMinutes(Configuration config)
        {
            return (GetMinutesMain(config.TEDUrl1));
        }

        public static TEDMinuteList GetMinutes(string url)
        {
            return (GetMinutesMain(url));
        }

        /// <summary>
        ///  Get minutes and override the querystring with new values
        ///  http://ted5000/history/minutehistory.xml?MTU=0&INDEX=1&COUNT=10
        /// </summary>
        /// <param name="url"></param>
        /// <param name="mtu"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static TEDMinuteList GetMinutes(string url, int mtu, int index, int count)
        {
            StringBuilder sb = new StringBuilder();
            string baseURL = string.Empty;
            string fullURL = string.Empty;
            const string querystring = "?MTU={0}&INDEX={1}&COUNT={2}";

            baseURL = url.Split('?')[0];
            sb.Append(baseURL);
            sb.AppendFormat(querystring, mtu, index, count);

            return (GetMinutesMain(sb.ToString()));
        }

        private static TEDMinuteList GetMinutesMain(string url)
        {
            Stream streamXML = null;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = null;
            TEDMinuteList minutes = null;
            bool retval = true;

            req.Method = "GET";
            req.ContentType = "application/x-www-form-urlencoded";
            req.AllowAutoRedirect = false;
            req.Timeout = 10000;

            response = (HttpWebResponse)req.GetResponse();
            streamXML = response.GetResponseStream();

            // Create an XmlReader from stream
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            settings.IgnoreWhitespace = true;
            settings.IgnoreComments = true;
            XmlReader reader = XmlReader.Create(streamXML, settings);

            minutes = new TEDMinuteList();

            retval = GetMinutesFromXML(reader, minutes);

            reader.Close();

            return (minutes);
        }

        public static bool GetMinutesFromXML(XmlReader reader, TEDMinuteList minutes)
        {
            string mtu = string.Empty;
            string date = string.Empty;
            string power = string.Empty;
            string cost = string.Empty;
            string voltage = string.Empty;
            TEDMinute minute = null;

            try
            {
                while (reader.Read())
                {

                    if (reader.IsStartElement())
                    {
                        switch (reader.Name)
                        {
                            case "MTU":
                                mtu = reader.ReadString();
                                break;
                            case "DATE":
                                date = reader.ReadString();
                                break;
                            case "POWER":
                                power = reader.ReadString();
                                break;
                            case "COST":
                                cost = reader.ReadString();
                                break;
                            case "VOLTAGE":
                                voltage = reader.ReadString();
                                break;
                        }

                    }

                    if (reader.NodeType == XmlNodeType.EndElement)
                    {
                        if (reader.Name == intervalNameMinute)
                        {
                            minute = new TEDMinute();
                            minute.MTU = System.Convert.ToInt32(mtu);
                            minute.DATE = date;
                            minute.POWER = System.Convert.ToInt32(power);
                            minute.COST = System.Convert.ToInt32(cost);
                            minute.VOLTAGE = System.Convert.ToInt32(voltage);
                            minute.Timestamp = DateTime.Parse(minute.DATE);

                            minutes.Add(minute);
                        }
                    }

                }

            }
            catch (XmlException e)
            {
                throw e;
            }

            return (true);
        }

        #endregion

        #region GetSeconds

        public static TEDSecondList GetSeconds(Configuration config)
        {
            return (GetSecondsMain(config.TEDUrl2));
        }

        public static TEDSecondList GetSeconds(string url)
        {
            return (GetSecondsMain(url));
        }

        /// <summary>
        ///  Get seconds and override the querystring with new values
        ///  http://ted5000/history/secondhistory.xml?MTU=0&INDEX=1&COUNT=10
        /// </summary>
        /// <param name="url"></param>
        /// <param name="mtu"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static TEDSecondList GetSeconds(string url, int mtu, int index, int count)
        {
            StringBuilder sb = new StringBuilder();
            string baseURL = string.Empty;
            string fullURL = string.Empty;
            const string querystring = "?MTU={0}&INDEX={1}&COUNT={2}";

            baseURL = url.Split('?')[0];
            sb.Append(baseURL);
            sb.AppendFormat(querystring, mtu, index, count);
 
            return (GetSecondsMain(sb.ToString()));
        }

        private static TEDSecondList GetSecondsMain(string url)
        {
            Stream streamXML = null;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = null;
            TEDSecondList seconds = null;
            bool retval = true;

            req.Method = "GET";
            req.ContentType = "application/x-www-form-urlencoded";
            req.AllowAutoRedirect = false;
            req.Timeout = 10000;

            response = (HttpWebResponse)req.GetResponse();
            streamXML = response.GetResponseStream();

            // Create an XmlReader from stream
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            settings.IgnoreWhitespace = true;
            settings.IgnoreComments = true;
            XmlReader reader = XmlReader.Create(streamXML, settings);

            seconds = new TEDSecondList();

            retval = GetSecondsFromXML(reader, seconds);

            reader.Close();

            return (seconds);
        }

        private static bool GetSecondsFromXML(XmlReader reader, TEDSecondList seconds)
        {
            string mtu = string.Empty;
            string date = string.Empty;
            string power = string.Empty;
            string cost = string.Empty;
            string voltage = string.Empty;
            TEDSecond second = null;

            try
            {
                while (reader.Read())
                {

                    if (reader.IsStartElement())
                    {
                        switch (reader.Name)
                        {
                            case "MTU":
                                mtu = reader.ReadString();
                                break;
                            case "DATE":
                                date = reader.ReadString();
                                break;
                            case "POWER":
                                power = reader.ReadString();
                                break;
                            case "COST":
                                cost = reader.ReadString();
                                break;
                            case "VOLTAGE":
                                voltage = reader.ReadString();
                                break;
                        }

                    }

                    if (reader.NodeType == XmlNodeType.EndElement)
                    {
                        if (reader.Name == intervalNameSecond)
                        {
                            second = new TEDSecond();
                            second.MTU = System.Convert.ToInt32(mtu);
                            second.DATE = date;
                            second.POWER = System.Convert.ToInt32(power);
                            second.COST = System.Convert.ToInt32(cost);
                            second.VOLTAGE = System.Convert.ToInt32(voltage);
                            second.Timestamp = DateTime.Parse(second.DATE);

                            seconds.Add(second);
                            //Debug.WriteLine("orig second=" + second.DATE + ", power=" + second.POWER.ToString());
                        }
                    }

                }

            }
            catch (XmlException e)
            {
                throw e;
            }

            return (true);
        }

        #endregion

    }
}
