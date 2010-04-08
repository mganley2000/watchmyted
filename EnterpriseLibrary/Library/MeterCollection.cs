using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace Energy.Library
{
    /// <summary>
    /// Meter collection class
    /// Contains functions to operate on the collection
    /// </summary>
    public class MeterCollection: System.Collections.CollectionBase
    {
        public MeterCollection()
        {
        }

        public Meter this[int index]
        {
            get
            {
                return (this.InnerList[index] as Meter);
            }
        }

        public int Add(Meter val)
        {
            return (this.InnerList.Add(val));
        }

        public void Insert(int index, Reading val)
        {
            this.InnerList.Insert(index, val);
        }


        public Meter FindMeter(int meterid)
        {
            Meter meter = null;
            bool found = false;

            if (this.InnerList.Count > 0)
            {
                for (int i = 0; i <= this.InnerList.Count - 1; i++)
                {
                    meter = (Meter)(this.InnerList[i]);
                    if (meter.ID == meterid)
                    {
                        found = true;
                        break;
                    }
                }
            }

            if (!found)
            {
                meter = null;
            }

            return (meter);
        }

    }
}
