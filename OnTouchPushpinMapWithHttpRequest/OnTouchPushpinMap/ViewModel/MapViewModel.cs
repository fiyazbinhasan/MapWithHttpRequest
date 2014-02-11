using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Microsoft.Phone.Maps.Controls;

namespace OnTouchPushpinMap.ViewModel
{
    public class MapViewModel : ViewModelBase
    {
        public MapViewModel()
        {
            modes = new ObservableCollection<MapMode>
                {
                  new MapMode {CartographicMode = MapCartographicMode.Road, Name = "Road"},
                  new MapMode {CartographicMode = MapCartographicMode.Aerial, Name = "Aerial"},
                  new MapMode {CartographicMode = MapCartographicMode.Hybrid, Name = "Hybrid"},
                  new MapMode {CartographicMode = MapCartographicMode.Terrain, Name = "Terrain"}
                };
            selectedMode = modes[0];
        }

        private MapMode selectedMode;
        public MapMode SelectedMode
        {
            get { return selectedMode; }
            set
            {
                if (selectedMode != value)
                {
                    selectedMode = value;
                    RaisePropertyChanged(() => SelectedMode);
                }
            }
        }

        private ObservableCollection<MapMode> modes;
        public ObservableCollection<MapMode> Modes
        {
            get { return modes; }
            set
            {
                if (modes != value)
                {
                    modes = value;
                    RaisePropertyChanged(() => Modes);
                }
            }
        }

        private double pitch;
        public double Pitch
        {
            get { return pitch; }
            set
            {
                if (Math.Abs(pitch - value) > 0.05)
                {
                    pitch = value;
                    RaisePropertyChanged(() => Pitch);
                }
            }
        }

        private double heading;
        public double Heading
        {
            get { return heading; }
            set
            {
                if (value > 180) value -= 360;
                if (value < -180) value += 360;
                if (Math.Abs(heading - value) > 0.05)
                {
                    heading = value;
                    RaisePropertyChanged(() => Heading);
                }
            }
        }

        // Center may not be null initially! #WP7toWP8
        private GeoCoordinate mapCenter = new GeoCoordinate(40.712923, -74.013292);
        /// <summary>
        /// Stores the map center
        /// </summary>
        public GeoCoordinate MapCenter
        {
            get { return mapCenter; }
            set
            {
                if (mapCenter != value)
                {
                    mapCenter = value;
                    RaisePropertyChanged(() => MapCenter);
                }
            }
        }

        private double zoomLevel = 15;
        /// <summary>
        /// Stores the map zoomlevel
        /// </summary>
        public double ZoomLevel
        {
            get { return zoomLevel; }
            set
            {
                if (zoomLevel != value)
                {
                    zoomLevel = value;
                    RaisePropertyChanged(() => ZoomLevel);
                }
            }
        }

        private bool landmarks;
        public bool Landmarks
        {
            get
            {
                return landmarks;
            }
            set
            {
                if (landmarks != value)
                {
                    landmarks = value;
                    RaisePropertyChanged(() => Landmarks);
                }
            }
        }

        
    }
}
