using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Microsoft.Phone.Maps.Controls;

namespace OnTouchPushpinMap.ViewModel
{
    public class MapMode : ViewModelBase
    {
        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    RaisePropertyChanged(() => Name);
                }
            }
        }

        private MapCartographicMode cartographicMode;
        public MapCartographicMode CartographicMode
        {
            get { return cartographicMode; }
            set
            {
                if (cartographicMode != value)
                {
                    cartographicMode = value;
                    RaisePropertyChanged(() => CartographicMode);
                }
            }
        }
    }
}
