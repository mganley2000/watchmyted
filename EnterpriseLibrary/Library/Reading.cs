using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Energy.Library
{
    /// <summary>
    /// Reading information
    /// A reading is a meter reading
    /// </summary>
    public class Reading
    {
        private Enums.Interval interval;
        private Enums.UnitOfMeasure uom;
        private Enums.TimeOfUse tou;
        private Enums.Season season;
        private DateTime datestamp;
        private double quantity;
        private bool empty;
        private Enums.WeekdayWeekend dayType;
        private ReadingDetail detail;

        public Reading()
        {
            detail = new ReadingDetail();
            empty = false;
        }

        public Reading(bool isempty, Enums.Interval v, Enums.UnitOfMeasure u, DateTime dt, double q)
        {
            detail = new ReadingDetail();
            interval = v;
            uom = u;
            datestamp = dt;
            quantity = q;
            empty = isempty;
            tou = Enums.TimeOfUse.None;
            season = Enums.Season.None;
        }

        public Reading(bool isempty, Enums.Interval v, Enums.UnitOfMeasure u, Enums.TimeOfUse t, Enums.Season s, DateTime dt, double q)
        {
            detail = new ReadingDetail();
            interval = v;
            uom = u;
            datestamp = dt;
            quantity = q;
            empty = isempty;
            tou = t;
            season = s;
        }

        public Enums.Interval DataInterval
        {
            get { return interval; }
            set { interval = value; }
        }

        public Enums.UnitOfMeasure Units
        {
            get { return uom; }
            set { uom = value; }
        }

        public Enums.TimeOfUse TimeOfUse
        {
            get { return tou; }
            set { tou = value; }
        }

        public Enums.Season Season
        {
            get { return season; }
            set { season = value; }
        }

        public double Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        public DateTime Datestamp
        {
            get { return datestamp; }
            set { datestamp = value; }
        }

        public bool IsEmpty
        {
            get { return empty; }
        }

        public bool Empty
        {
            set { empty = value; }
        }

        public Enums.WeekdayWeekend DayType
        {
            get { return dayType; }
            set { dayType = value; }
        }

        public ReadingDetail Detail
        {
            get { return detail; }
            set { detail = value; }
        }

    }
}
