using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace Energy.Library
{
    /// <summary>
    /// Reading collection class
    /// </summary>
    public class ReadingCollection : System.Collections.CollectionBase
    {
        public ReadingCollection()
        {
        }

        public Reading this[int index]
        {
            get
            {
                return (this.InnerList[index] as Reading);
            }
        }

        public int Add(Reading val)
        {
            return (this.InnerList.Add(val));
        }

        public void Insert(int index, Reading val)
        {
            this.InnerList.Insert(index, val);
        }

        public double Sum()
        {
            double total = 0.0;

            if (this.InnerList.Count > 0)
            {
                for (int i = 0; i <= this.InnerList.Count - 1; i++)
                {
                    total += ((Reading)(this.InnerList[i])).Quantity;
                }
            }

            return (total);
        }


        public bool FillAbsentReadings(DateTime dtStartDate, DateTime dtEndDate, Enums.Interval interval)
        {
            int i, ii, readingIndex, nCount, maxDays;
            Reading reading = null;
            Enums.UnitOfMeasure tempUOM;
            DateTime tempDate;

            switch (interval)
            {
                case Enums.Interval.Hourly:
                    nCount = this.InnerList.Count;
                    maxDays = dtEndDate.Subtract(dtStartDate).Days + 1;

                    for (i = 0; i <= (maxDays - 1); i++)
                    {
                        tempDate = dtStartDate.AddDays(i);

                        for (ii = 0; ii <= 23; ii++)
                        {
                            readingIndex = (i * 24) + ii;

                            if (readingIndex < nCount)
                            {
                                reading = (Reading)(this.InnerList[readingIndex]);

                                if (tempDate.Day != reading.Datestamp.Day || tempDate.Month != reading.Datestamp.Month || tempDate.AddHours(ii).Hour != reading.Datestamp.Hour)
                                {
                                    // Day or hour does not match, so insert at this index
                                    this.InnerList.Insert(readingIndex, new Reading(true, reading.DataInterval, reading.Units, tempDate.AddHours(ii), 0));
                                    // Insert into inner list moves the list up one notch, hence increase the count
                                    nCount = nCount + 1;
                                }
                            }
                            else
                            {
                                // over the top, add to end
                                if (reading != null)
                                {
                                    tempUOM = reading.Units;
                                }
                                else
                                {
                                    tempUOM = Enums.UnitOfMeasure.kWh;
                                }
                                this.InnerList.Add(new Reading(true, interval, tempUOM, tempDate.AddHours(ii), 0));
                                nCount = nCount + 1;
                            }
                        }

                    }


                    break;

                case Enums.Interval.Daily:
                    nCount = this.InnerList.Count;
                    maxDays = dtEndDate.Subtract(dtStartDate).Days + 1;

                    for (i = 0; i <= (maxDays - 1); i++)
                    {
                        tempDate = dtStartDate.AddDays(i);

                        if (i < nCount)
                        {
                            reading = (Reading)(this.InnerList[i]);

                            if ((tempDate.Day != reading.Datestamp.Day) || (tempDate.Day == tempDate.Day && tempDate.Month != reading.Datestamp.Month))
                            {
                                // Day or hour does not match, so insert at this index
                                this.InnerList.Insert(i, new Reading(true, reading.DataInterval, reading.Units, tempDate, 0));
                                // Insert into inner list moves the list up one notch, hence increase the count
                                nCount = nCount + 1;
                            }
                        }
                        else
                        {
                            // over the top, add to end
                            if (reading != null)
                            {
                                tempUOM = reading.Units;
                            }
                            else
                            {
                                tempUOM = Enums.UnitOfMeasure.kWh;
                            }
                            this.InnerList.Add(new Reading(true, interval, tempUOM, tempDate, 0));
                            nCount = nCount + 1;
                        }


                    }
                    break;
            }

            return (true);
        }

        public bool AssignDayType()
        {
            int i;
            Reading r;

            if (InnerList.Count > 0)
            {
                for (i = 0; i <= InnerList.Count - 1; i++)
                {
                    r = (Reading)InnerList[i];
                    r.DayType = GetWeekdayWeekend(r.Datestamp.DayOfWeek);
                }
            }

            return (true);
        }

        public bool AssignSeasonAndBasicCharges(Rate rate)
        {
            bool retval = true;
            int m;
            int i;
            Reading r;
            DateTime startDate;
            DateTime endDate;
            int startMonth;
            int endMonth;
            int tempMonth;
            bool swap = false;
            Enums.Season[] seasons = new Enums.Season[13];

            if (rate.IsSeasonal)
            {
                //seasonal
                startDate = DateTime.Parse(rate.SummerStartMonth + " 1 " + DateTime.Now.Year);
                endDate = DateTime.Parse(rate.SummerEndMonth + " 1 " + DateTime.Now.Year);

                startMonth = startDate.Month;
                endMonth = endDate.Month;

                // swap start and end for southern hemisphere seasons;
                if (startMonth > endMonth)
                {
                    tempMonth = startMonth;
                    startMonth = endMonth;
                    endMonth = tempMonth;
                    swap = true;
                }

                // create lookup for season;
                for (m = 1; m <= 12; m++)
                {
                    if (!swap)
                    {
                        if (m >= startMonth && m <= endMonth)
                        {
                            seasons[m] = Enums.Season.Summer;
                        }
                        else
                        {
                            seasons[m] = Enums.Season.Winter;
                        }
                    }
                    else
                    {
                        if (m >= startMonth && m <= endMonth)
                        {
                            seasons[m] = Enums.Season.Winter;
                        }
                        else
                        {
                            seasons[m] = Enums.Season.Summer;
                        }
                    }
                }

                //use season lookup table here;
                if (InnerList.Count > 0)
                {
                    for (i = 0; i <= InnerList.Count - 1; i++)
                    {
                        r = (Reading)InnerList[i];
                        r.Season = seasons[r.Datestamp.Month];

                        if (rate.HasBasicCharges)
                        {
                            r.Detail.BasicCharge = rate.BasicCharge;

                            switch (r.Season)
                            {
                                case Enums.Season.Summer:
                                    r.Detail.BasicSeasonalCharge = rate.SummerBasicCharge;
                                    break;

                                case Enums.Season.Winter:
                                    r.Detail.BasicSeasonalCharge = rate.WinterBasicCharge;
                                    break;
                            }
                        }

                    }
                }
            }
            else
            {
                // not seasonal
                if (InnerList.Count > 0)
                {
                    for (i = 0; i <= InnerList.Count - 1; i++)
                    {
                        r = (Reading)InnerList[i];
                        r.Season = Enums.Season.None;

                        if (rate.HasBasicCharges)
                        {
                            r.Detail.BasicCharge = rate.BasicCharge;
                        }

                    }
                }
            }

            return (retval);
        }

        public bool AssignTOUAndTOUCharges(Rate rate)
        {
            bool retval = true;
            Reading r;
            Enums.TimeOfUse[, ,] seasonWdWeHourTOU = new Enums.TimeOfUse[3, 2, 24];   //[season,WeekdayOrWeekend,hour]
            double[, ,] seasonWdWeHourCharge = new double[3, 2, 24];   //[season,WeekdayOrWeekend,hour]
            int i;
            Enums.Season s;
            int h;
            Enums.WeekdayWeekend w;

            if (rate.HasTimeOfUseCharges)
            {
                //TOU

                // create TOU lookup tables
                for (s = Enums.Season.None; s <= Enums.Season.Winter; s++)
                {
                    for (w = Enums.WeekdayWeekend.Weekday; w <= Enums.WeekdayWeekend.Weekend; w++)
                    {
                        for (h = 0; h <= 23; h++)
                        {
                            switch (s)
                            {
                                case Enums.Season.None:

                                    switch (w)
                                    {
                                        case Enums.WeekdayWeekend.Weekday:
                                            if (h >= rate.PeakStartHour && h <= rate.PeakEndHour)
                                            {
                                                seasonWdWeHourTOU[(int)s, (int)w, h] = Enums.TimeOfUse.Peak;
                                                seasonWdWeHourCharge[(int)s, (int)w, h] = rate.PeakCharge;
                                            }
                                            else
                                            {
                                                seasonWdWeHourTOU[(int)s, (int)w, h] = Enums.TimeOfUse.OffPeak;
                                                seasonWdWeHourCharge[(int)s, (int)w, h] = rate.OffPeakCharge;
                                            }
                                            break;

                                        case Enums.WeekdayWeekend.Weekend:
                                            if (h >= rate.PeakWeekendStartHour && h <= rate.PeakWeekendEndHour)
                                            {
                                                seasonWdWeHourTOU[(int)s, (int)w, h] = Enums.TimeOfUse.Peak;
                                                seasonWdWeHourCharge[(int)s, (int)w, h] = rate.PeakWeekendCharge;
                                            }
                                            else
                                            {
                                                seasonWdWeHourTOU[(int)s, (int)w, h] = Enums.TimeOfUse.OffPeak;
                                                seasonWdWeHourCharge[(int)s, (int)w, h] = rate.OffPeakWeekendCharge;
                                            }
                                            break;

                                    }

                                    break;

                                case Enums.Season.Summer:

                                    switch (w)
                                    {
                                        case Enums.WeekdayWeekend.Weekday:
                                            if (h >= rate.SummerPeakStartHour && h <= rate.SummerPeakEndHour)
                                            {
                                                seasonWdWeHourTOU[(int)s, (int)w, h] = Enums.TimeOfUse.Peak;
                                                seasonWdWeHourCharge[(int)s, (int)w, h] = rate.SummerPeakCharge;
                                            }
                                            else
                                            {
                                                seasonWdWeHourTOU[(int)s, (int)w, h] = Enums.TimeOfUse.OffPeak;
                                                seasonWdWeHourCharge[(int)s, (int)w, h] = rate.SummerOffPeakCharge;
                                            }
                                            break;

                                        case Enums.WeekdayWeekend.Weekend:
                                            if (h >= rate.SummerPeakWeekendStartHour && h <= rate.SummerPeakWeekendEndHour)
                                            {
                                                seasonWdWeHourTOU[(int)s, (int)w, h] = Enums.TimeOfUse.Peak;
                                                seasonWdWeHourCharge[(int)s, (int)w, h] = rate.SummerPeakWeekendCharge;
                                            }
                                            else
                                            {
                                                seasonWdWeHourTOU[(int)s, (int)w, h] = Enums.TimeOfUse.OffPeak;
                                                seasonWdWeHourCharge[(int)s, (int)w, h] = rate.SummerOffPeakWeekendCharge;
                                            }
                                            break;

                                    }

                                    break;

                                case Enums.Season.Winter:

                                    switch (w)
                                    {
                                        case Enums.WeekdayWeekend.Weekday:
                                            if (h >= rate.WinterPeakStartHour && h <= rate.WinterPeakEndHour)
                                            {
                                                seasonWdWeHourTOU[(int)s, (int)w, h] = Enums.TimeOfUse.Peak;
                                                seasonWdWeHourCharge[(int)s, (int)w, h] = rate.WinterPeakCharge;
                                            }
                                            else
                                            {
                                                seasonWdWeHourTOU[(int)s, (int)w, h] = Enums.TimeOfUse.OffPeak;
                                                seasonWdWeHourCharge[(int)s, (int)w, h] = rate.WinterOffPeakCharge;
                                            }
                                            break;

                                        case Enums.WeekdayWeekend.Weekend:
                                            if (h >= rate.WinterPeakWeekendStartHour && h <= rate.WinterPeakWeekendEndHour)
                                            {
                                                seasonWdWeHourTOU[(int)s, (int)w, h] = Enums.TimeOfUse.Peak;
                                                seasonWdWeHourCharge[(int)s, (int)w, h] = rate.WinterPeakWeekendCharge;
                                            }
                                            else
                                            {
                                                seasonWdWeHourTOU[(int)s, (int)w, h] = Enums.TimeOfUse.OffPeak;
                                                seasonWdWeHourCharge[(int)s, (int)w, h] = rate.WinterOffPeakWeekendCharge;
                                            }
                                            break;

                                    }
                                    break;
                            }
                        }
                    }
                }

                //use TOU lookup table here;
                if (InnerList.Count > 0)
                {
                    for (i = 0; i <= InnerList.Count - 1; i++)
                    {
                        r = (Reading)InnerList[i];
                        r.TimeOfUse = seasonWdWeHourTOU[(int)r.Season, (int)r.DayType, r.Datestamp.Hour];

                        // charges

                        if (rate.IsSeasonal)
                        {
                            r.Detail.TOUCharge = 0.0;
                            r.Detail.TOUSeasonalCharge = seasonWdWeHourCharge[(int)r.Season, (int)r.DayType, r.Datestamp.Hour];
                        }
                        else
                        {
                            r.Detail.TOUSeasonalCharge = 0.0;
                            r.Detail.TOUCharge = seasonWdWeHourCharge[(int)Enums.Season.None, (int)r.DayType, r.Datestamp.Hour];
                        }
                    }
                }

            }
            else
            {
                // No TOU
            }

            return (retval);
        }

        private Enums.WeekdayWeekend GetWeekdayWeekend(System.DayOfWeek dayOfWeek)
        {
            Enums.WeekdayWeekend w = Enums.WeekdayWeekend.Weekday;

            switch (dayOfWeek)
            {
                case DayOfWeek.Monday:
                case DayOfWeek.Tuesday:
                case DayOfWeek.Wednesday:
                case DayOfWeek.Thursday:
                case DayOfWeek.Friday:
                    w = Enums.WeekdayWeekend.Weekday;
                    break;

                case DayOfWeek.Saturday:
                case DayOfWeek.Sunday:
                    w = Enums.WeekdayWeekend.Weekend;
                    break;
            }

            return (w);
        }

        public double SumForDate(DateTime d)
        {
            double total = 0;
            int i;
            Reading r;

            if (InnerList.Count > 0)
            {
                for (i = 0; i <= InnerList.Count - 1; i++)
                {
                    r = (Reading)InnerList[i];

                    if (r.Datestamp.Day == d.Day && r.Datestamp.Month == d.Month && r.Datestamp.Year == d.Year)
                    {
                        total = total + r.Quantity;
                    }
                }
            }

            return (total);
        }

        public double SumForDate(DateTime d, Enums.TimeOfUse tou)
        {
            double total = 0;
            int i;
            Reading r;

            if (InnerList.Count > 0)
            {
                for (i = 0; i <= InnerList.Count - 1; i++)
                {
                    r = (Reading)InnerList[i];

                    if (r.Datestamp.Day == d.Day && r.Datestamp.Month == d.Month && r.Datestamp.Year == d.Year && r.TimeOfUse == tou)
                    {
                        total = total + r.Quantity;
                    }
                }
            }

            return (total);
        }

        public double CostForDate(DateTime d, Enums.TimeOfUse tou)
        {
            double totalCost = 0.0;
            int i;
            Reading r;

            if (InnerList.Count > 0)
            {
                for (i = 0; i <= InnerList.Count - 1; i++)
                {
                    r = (Reading)InnerList[i];

                    if (r.Datestamp.Day == d.Day && r.Datestamp.Month == d.Month && r.Datestamp.Year == d.Year && r.TimeOfUse == tou)
                    {

                        totalCost = totalCost + (r.Detail.BasicCharge * r.Quantity);
                        totalCost = totalCost + (r.Detail.BasicSeasonalCharge * r.Quantity);
                        totalCost = totalCost + (r.Detail.TOUCharge * r.Quantity);
                        totalCost = totalCost + (r.Detail.TOUSeasonalCharge * r.Quantity);
                        //totalCost = totalCost + r.Quantity;
                    }
                }
            }

            return (totalCost);
        }

        // iterate self, and create collection of daily readings
        // from hourly or any other interval 
        public ReadingCollection CreateDailyReadings()
        {
            return (CreateDailyReadings(Enums.TimeOfUse.None));
        }

        public ReadingCollection CreateDailyReadings(Enums.TimeOfUse tou)
        {
            ReadingCollection dailyReadings = new ReadingCollection();
            int i;
            int dayOfYear;
            int previousDayOfYear = -1;
            DateTime date;
            Reading r;
            Reading newReading;
            double dailyTotalQuantity;

            if (InnerList.Count > 0)
            {
                // only continue if we have hourly, and are creating daily from hourly
                if (((Reading)InnerList[0]).DataInterval == Enums.Interval.Hourly)
                {
                    for (i = 0; i <= InnerList.Count - 1; i++)
                    {
                        r = (Reading)InnerList[i];
                        date = r.Datestamp;
                        dayOfYear = date.DayOfYear;

                        if (dayOfYear != previousDayOfYear)
                        {
                            dailyTotalQuantity = this.SumForDate(date, tou);
                            newReading = new Reading(false, Enums.Interval.Daily, r.Units, tou, r.Season, DateTime.Parse(date.Month + "/" + date.Day + "/" + date.Year), dailyTotalQuantity);
                            dailyReadings.Add(newReading);
                            previousDayOfYear = dayOfYear;
                        }
                    }

                }
            }

            return (dailyReadings);
        }

        public ReadingCollection CreateHourlyReadings(Enums.TimeOfUse tou)
        {
            ReadingCollection hourlyReadings = new ReadingCollection();
            int i;
            Reading r;
            Reading newReading;

            if (InnerList.Count > 0)
            {
                // only continue if we have hourly, and are creating daily from hourly
                if (((Reading)InnerList[0]).DataInterval == Enums.Interval.Hourly)
                {
                    for (i = 0; i <= InnerList.Count - 1; i++)
                    {
                        r = (Reading)InnerList[i];

                        if (r.TimeOfUse == tou)
                        {
                            newReading = new Reading(false, Enums.Interval.Hourly, r.Units, tou, r.Season, r.Datestamp, r.Quantity);
                            newReading.Detail.BasicCharge = r.Detail.BasicCharge;
                            newReading.Detail.BasicSeasonalCharge = r.Detail.BasicSeasonalCharge;
                            newReading.Detail.TOUCharge = r.Detail.TOUCharge;
                            newReading.Detail.TOUSeasonalCharge = r.Detail.TOUSeasonalCharge;
                        }
                        else
                        {
                            // empty and zero the other tou
                            newReading = new Reading(true, Enums.Interval.Hourly, r.Units, tou, r.Season, r.Datestamp, 0.0);
                        }

                        hourlyReadings.Add(newReading);
                    }

                }
            }

            return (hourlyReadings);
        }


        public ReadingCollection CreateDailyCosts()
        {
            return (CreateDailyCosts(Enums.TimeOfUse.None));
        }

        public ReadingCollection CreateDailyCosts(Enums.TimeOfUse tou)
        {
            ReadingCollection dailyCosts = new ReadingCollection();
            int i;
            int dayOfYear;
            int previousDayOfYear = -1;
            DateTime date;
            Reading r;
            Reading newReading;
            double dailyTotalCost;

            if (InnerList.Count > 0)
            {
                // only continue if we have hourly, and are creating daily from hourly
                if (((Reading)InnerList[0]).DataInterval == Enums.Interval.Hourly)
                {
                    for (i = 0; i <= InnerList.Count - 1; i++)
                    {
                        r = (Reading)InnerList[i];
                        date = r.Datestamp;
                        dayOfYear = date.DayOfYear;

                        if (dayOfYear != previousDayOfYear)
                        {
                            dailyTotalCost = this.CostForDate(date, tou);
                            newReading = new Reading(false, Enums.Interval.Daily, r.Units, tou, r.Season, DateTime.Parse(date.Month + "/" + date.Day + "/" + date.Year), dailyTotalCost);
                            dailyCosts.Add(newReading);
                            previousDayOfYear = dayOfYear;
                        }
                    }

                }
            }

            return (dailyCosts);
        }

    }

}
