using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Device.Location;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Windows.Devices.Geolocation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Maps.Controls;
using Microsoft.Phone.Maps.Services;
using Microsoft.Phone.Maps.Toolkit;
using Microsoft.Phone.Shell;
using OnTouchPushpinMap.Resources;
using OnTouchPushpinMap.ViewModel;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;

namespace OnTouchPushpinMap
{
    public partial class MainPage : PhoneApplicationPage
    {
        private HttpWebRequest _httpWebRequest;
        
        private readonly double userLocationMarkerZoomLevel = 16;
        MainViewModel _viewModel = new MainViewModel();
        private MapRoute MapRoute { get; set; }
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            this.MapExtensionsSetup(this.Map);
            this.DataContext = _viewModel;
            this.Loaded += MainPage_Loaded;

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void MapExtensionsSetup(Map map)
        {
            ObservableCollection<DependencyObject> children = MapExtensions.GetChildren(map);
            var runtimeFields = this.GetType().GetRuntimeFields();

            foreach (DependencyObject i in children)
            {
                var info = i.GetType().GetProperty("Name");

                if (info != null)
                {
                    string name = (string)info.GetValue(i);

                    if (name != null)
                    {
                        foreach (FieldInfo j in runtimeFields)
                        {
                            if (j.Name == name)
                            {
                                j.SetValue(this, i);
                                break;
                            }
                        }
                    }
                }
            }
        }

        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            Map.SetView(new GeoCoordinate(23.7426, 90, 3918), 12);
            MapExtensions.GetChildren(Map)
            .OfType<MapItemsControl>().First()
            .ItemsSource = this._viewModel.Locations;
        }

        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
        private async void Map_OnHold(object sender, GestureEventArgs e)
        {
            ReverseGeocodeQuery query;
            List<MapLocation> mapLocations;
            string pushpinContent;
            MapLocation mapLocation;

            query = new ReverseGeocodeQuery();
            query.GeoCoordinate = this.Map.ConvertViewportPointToGeoCoordinate(e.GetPosition(this.Map));

            mapLocations = (List<MapLocation>)await query.GetMapLocationsAsync();
            mapLocation = mapLocations.FirstOrDefault();

            if (mapLocation != null)
            {
                this.RouteDirectionsPushPin.GeoCoordinate = mapLocation.GeoCoordinate;

                pushpinContent = mapLocation.Information.Name;
                pushpinContent = string.IsNullOrEmpty(pushpinContent) ? mapLocation.Information.Description : null;
                pushpinContent = string.IsNullOrEmpty(pushpinContent) ? string.Format("{0} {1}", mapLocation.Information.Address.Street, mapLocation.Information.Address.City) : null;

                this.RouteDirectionsPushPin.Content = pushpinContent.Trim();
                this.RouteDirectionsPushPin.Visibility = Visibility.Visible;
                this.SendHttpRequest(mapLocation);
            }
        }

        private void SendHttpRequest(MapLocation mapLocation)
        {
            string url = string.Format("http://locmailer.aws.af.cm/?PlaceName={0}&Longitude={1}&Latitude={2}",mapLocation.Information.Address,mapLocation.GeoCoordinate.Longitude,mapLocation.GeoCoordinate.Latitude);

            // HTTP web request
            _httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            _httpWebRequest.BeginGetResponse(FinishWebRequest, null);
        }

        private void FinishWebRequest(IAsyncResult result)
        {
           _httpWebRequest.EndGetResponse(result);
        }


        private async void ApplicationBarMeButton_OnClick(object sender, EventArgs e)
        {
            await this.ShowUserLocation();

            this.Map.SetView(this.UserLocationMarker.GeoCoordinate, this.userLocationMarkerZoomLevel);
        
        }

        private async Task ShowUserLocation()
        {
            Geolocator geolocator;
            Geoposition geoposition;

            this.UserLocationMarker = (UserLocationMarker)this.FindName("UserLocationMarker");

            geolocator = new Geolocator();

            geoposition = await geolocator.GetGeopositionAsync();

            this.UserLocationMarker.GeoCoordinate = geoposition.Coordinate.ToGeoCoordinate();
            this.UserLocationMarker.Visibility = System.Windows.Visibility.Visible;
        }

        private async void ApplicationBarShowRouteButton_OnClick(object sender, EventArgs e)
        {
            if (this.RouteDirectionsPushPin.GeoCoordinate == null || this.RouteDirectionsPushPin.Visibility == Visibility.Collapsed)
            {
                MessageBox.Show("Tap and hold somewhere in the map to show a Pushpin. After that you can show route from your location to the destination");
            }
            else
            {
                RouteQuery query;
                List<GeoCoordinate> wayPoints;
                Route route;

                if (this.MapRoute != null)
                {
                    this.Map.RemoveRoute(this.MapRoute);
                }

                await this.ShowUserLocation();

                query = new RouteQuery();
                wayPoints = new List<GeoCoordinate>();

                wayPoints.Add(this.UserLocationMarker.GeoCoordinate);
                wayPoints.Add(this.RouteDirectionsPushPin.GeoCoordinate);

                query.Waypoints = wayPoints;

                route = await query.GetRouteAsync();
                this.MapRoute = new MapRoute(route);

                this.Map.SetView(route.BoundingBox);
                this.Map.AddRoute(this.MapRoute);

            }
        }
    }
}