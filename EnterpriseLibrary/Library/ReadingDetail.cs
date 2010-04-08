using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Energy.Library
{
    /// <summary>
    /// The charge details of a particular reading
    /// </summary>
    public class ReadingDetail
    {
        private double basicCharge;
        private double basicSeasonalCharge;
        private double touCharge;
        private double touSeasonalCharge;

        public ReadingDetail()
        {
            basicCharge = 0.0;
            basicSeasonalCharge = 0.0;
            touCharge = 0.0;
            touSeasonalCharge = 0.0;
        }

        public double BasicCharge
        {
            get { return basicCharge; }
            set { basicCharge = value; }
        }

        public double BasicSeasonalCharge
        {
            get { return basicSeasonalCharge; }
            set { basicSeasonalCharge = value; }
        }

        public double TOUCharge
        {
            get { return touCharge; }
            set { touCharge = value; }
        }

        public double TOUSeasonalCharge
        {
            get { return touSeasonalCharge; }
            set { touSeasonalCharge = value; }
        }

    }
}
