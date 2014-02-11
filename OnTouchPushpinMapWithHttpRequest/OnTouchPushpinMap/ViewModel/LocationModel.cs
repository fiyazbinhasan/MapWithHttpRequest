using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Device.Location;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Microsoft.Phone.Maps.Controls;
using OnTouchPushpinMap.Annotations;

namespace OnTouchPushpinMap.ViewModel
{
    public class LocationModel : ViewModelBase
    {
        private string _address;

        public string Address
        {
            get
            {
                return _address;
            }
            set
            {
                if (value != _address)
                {
                    _address = value;
                    RaisePropertyChanged(() => Address);
                }
            }
        }

        private GeoCoordinate _geoCoordinate;
        public GeoCoordinate GeoCoordinate
        {
            get
            {
                return _geoCoordinate;
            }
            set
            {
                if (value != _geoCoordinate)
                {
                    _geoCoordinate = value;
                    RaisePropertyChanged(() => GeoCoordinate);
                }
            }
        }
    }
}
