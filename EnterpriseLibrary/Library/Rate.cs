using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Energy.Library
{
    /// <summary>
    /// Rate class
    /// Big class that holds all the rate information
    /// Could be broken out into smaller pieces in the future
    /// </summary>
    [Serializable()]
    public class Rate : ICloneable
    {
        private int id;                         //
        private string name;                    //
        private string description;             //
        private bool seasonal;                  //
        private bool basicCharges;              //
        private bool touCharges;                //
        private bool current;                   //
        private string summerStartDate;         //
        private string summerEndDate;           //
        private double basicCharge;             //
        private double summerBasicCharge;       //
        private double winterBasicCharge;       //

        private double peakCharge;                     //weekday
        private double offPeakCharge;                  //weekday
        private double peakWeekendCharge;              //weekend
        private double offPeakWeekendCharge;           //weekend
        private int peakStartHour;                     //
        private int peakEndHour;                       //
        private int peakWeekendStartHour;              //
        private int peakWeekendEndHour;                //

        private double summerPeakCharge;               //weekday
        private double summerOffPeakCharge;            //weekday
        private double summerPeakWeekendCharge;        //weekend
        private double summerOffPeakWeekendCharge;     //weekend
        private int summerPeakStartHour;               //
        private int summerPeakEndHour;                 //
        private int summerPeakWeekendStartHour;        //
        private int summerPeakWeekendEndHour;          //

        private double winterPeakCharge;               //weekday
        private double winterOffPeakCharge;            //weekday
        private double winterPeakWeekendCharge;        //weekend
        private double winterOffPeakWeekendCharge;     //weekend
        private int winterPeakStartHour;               //
        private int winterPeakEndHour;                 //
        private int winterPeakWeekendStartHour;        //
        private int winterPeakWeekendEndHour;          //

        public Rate()
        {
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
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

        public bool IsSeasonal
        {
            get { return seasonal; }
            set { seasonal = value; }
        }

        public bool HasBasicCharges
        {
            get { return basicCharges; }
            set { basicCharges = value; }
        }

        public bool HasTimeOfUseCharges
        {
            get { return touCharges; }
            set { touCharges = value; }
        }

        public bool IsCurrentlySelectedRate
        {
            get { return current; }
            set { current = value; }
        }

        public string SummerStartMonth
        {
            get { return summerStartDate; }
            set { summerStartDate = value; }
        }

        public string SummerEndMonth
        {
            get { return summerEndDate; }
            set { summerEndDate = value; }
        }

        public int PeakStartHour
        {
            get { return peakStartHour; }
            set { peakStartHour = value; }
        }

        public int PeakEndHour
        {
            get { return peakEndHour; }
            set { peakEndHour = value; }
        }

        public int SummerPeakStartHour
        {
            get { return summerPeakStartHour; }
            set { summerPeakStartHour = value; }
        }

        public int SummerPeakEndHour
        {
            get { return summerPeakEndHour; }
            set { summerPeakEndHour = value; }
        }

        public int WinterPeakStartHour
        {
            get { return winterPeakStartHour; }
            set { winterPeakStartHour = value; }
        }

        public int WinterPeakEndHour
        {
            get { return winterPeakEndHour; }
            set { winterPeakEndHour = value; }
        }

        public double BasicCharge
        {
            get { return basicCharge; }
            set { basicCharge = value; }
        }

        public double SummerBasicCharge
        {
            get { return summerBasicCharge; }
            set { summerBasicCharge = value; }
        }

        public double WinterBasicCharge
        {
            get { return winterBasicCharge; }
            set { winterBasicCharge = value; }
        }

        public double PeakCharge
        {
            get { return peakCharge; }
            set { peakCharge = value; }
        }

        public double OffPeakCharge
        {
            get { return offPeakCharge; }
            set { offPeakCharge = value; }
        }

        public double SummerPeakCharge
        {
            get { return summerPeakCharge; }
            set { summerPeakCharge = value; }
        }

        public double SummerOffPeakCharge
        {
            get { return summerOffPeakCharge; }
            set { summerOffPeakCharge = value; }
        }

        public double WinterPeakCharge
        {
            get { return winterPeakCharge; }
            set { winterPeakCharge = value; }
        }

        public double WinterOffPeakCharge
        {
            get { return winterOffPeakCharge; }
            set { winterOffPeakCharge = value; }
        }

        public double PeakWeekendCharge
        {
            get { return peakWeekendCharge; }
            set { peakWeekendCharge = value; }
        }

        public double OffPeakWeekendCharge
        {
            get { return offPeakWeekendCharge; }
            set { offPeakWeekendCharge = value; }
        }

        public double SummerPeakWeekendCharge
        {
            get { return summerPeakWeekendCharge; }
            set { summerPeakWeekendCharge = value; }
        }

        public double SummerOffPeakWeekendCharge
        {
            get { return summerOffPeakWeekendCharge; }
            set { summerOffPeakWeekendCharge = value; }
        }

        public double WinterPeakWeekendCharge
        {
            get { return winterPeakWeekendCharge; }
            set { winterPeakWeekendCharge = value; }
        }

        public double WinterOffPeakWeekendCharge
        {
            get { return winterOffPeakWeekendCharge; }
            set { winterOffPeakWeekendCharge = value; }
        }


        public int PeakWeekendStartHour
        {
            get { return peakWeekendStartHour; }
            set { peakWeekendStartHour = value; }
        }

        public int PeakWeekendEndHour
        {
            get { return peakWeekendEndHour; }
            set { peakWeekendEndHour = value; }
        }


        public int SummerPeakWeekendStartHour
        {
            get { return summerPeakWeekendStartHour; }
            set { summerPeakWeekendStartHour = value; }
        }

        public int SummerPeakWeekendEndHour
        {
            get { return summerPeakWeekendEndHour; }
            set { summerPeakWeekendEndHour = value; }
        }

        public int WinterPeakWeekendStartHour
        {
            get { return winterPeakWeekendStartHour; }
            set { winterPeakWeekendStartHour = value; }
        }

        public int WinterPeakWeekendEndHour
        {
            get { return winterPeakWeekendEndHour; }
            set { winterPeakWeekendEndHour = value; }
        }

        public object Clone()
        {
            Rate obj = null;
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            System.IO.MemoryStream ms = new System.IO.MemoryStream();

            bf.Serialize(ms, this);
            ms.Seek(0, System.IO.SeekOrigin.Begin);
            obj = (Rate)(bf.Deserialize(ms));
            ms.Close();

            return (obj);
        }
    }
}
