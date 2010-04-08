using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Energy.Library
{
    /// <summary>
    /// Meter class
    /// </summary>
    public class Meter
    {
        private int id;
        private Enums.MeterType type;
        private string name;
        private string description;

        public Meter()
        {
        }

        public Meter(int meterID, Enums.MeterType meterType, string meterName, string meterDesc)
        {
            id = meterID;
            type = meterType;
            name = meterName;
            description = meterDesc;
        }

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public Enums.MeterType Type
        {
            get { return type; }
            set { type = value; }
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

    }
}
