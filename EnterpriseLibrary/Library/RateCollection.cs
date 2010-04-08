using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace Energy.Library
{
    /// <summary>
    /// Rate collection class
    /// </summary>
    class RateCollection: System.Collections.CollectionBase
    {
        public RateCollection()
        {
        }

        public Rate this[int index]
        {
            get
            {
                return (this.InnerList[index] as Rate);
            }
        }

        public int Add(Rate val)
        {
            return (this.InnerList.Add(val));
        }

        public void Insert(int index, Rate val)
        {
            this.InnerList.Insert(index, val);
        }
    }

}
