using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Energy.Library
{
    [DataContract]
    public class GAERead
    {
        private int _meterid;
        private string _name;
        private Enums.GAETargetStorage _store;
        private Enums.UnitOfMeasure _uom;
        private Enums.Fuel _fuel;
        private string _stamp;      //datetime string
        private string _blob;
        private string _storeAsString;
        private string _uomAsString;
        private string _fuelAsString;

        public GAERead()
        {
        }

        public GAERead(int meterid, string name, Enums.GAETargetStorage store, Enums.UnitOfMeasure uom, Enums.Fuel fuel, string stamp, string blob)
        {
            _meterid = meterid;
            _name = name;
            _store = store;
            _uom = uom;
            _fuel = fuel;
            _stamp = stamp;
            _blob = blob;

            _storeAsString = _store.ToString();
            _uomAsString = _uom.ToString();
            _fuelAsString = _fuel.ToString();
        }

        #region Properties

        [DataMember]
        public string Blob
        {
            get { return _blob; }
            set { _blob = value; }
        }

        [DataMember]
        public string Stamp
        {
            get { return _stamp; }
            set { _stamp = value; }
        }

        [DataMember(Name = "Uom")]
        public string UomAsString
        {
            get { return _uomAsString; }
            set { _uomAsString = value; }
        }

        public Enums.UnitOfMeasure Uom
        {
            get { return _uom; }
            set
            {
                _uomAsString = _uom.ToString();
                _uom = value;
            }
        }

        [DataMember(Name = "Fuel")]
        public string FuelAsString
        {
            get { return _fuelAsString; }
            set { _fuelAsString = value; }
        }

        public Enums.Fuel Fuel
        {
            get { return _fuel; }
            set
            {
                _fuelAsString = _fuel.ToString();
                _fuel = value;
            }
        }

        [DataMember(Name = "Store")]
        public string StoreAsString
        {
            get { return _storeAsString; }
            set { _storeAsString = value; }
        }

        public Enums.GAETargetStorage Store
        {
            get { return _store; }
            set 
            {
                _storeAsString = _store.ToString();
                _store = value; 
            }
        }

        [DataMember]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        [DataMember]
        public int MeterID
        {
            get { return _meterid; }
            set { _meterid = value; }
        }


        #endregion

    }
}
