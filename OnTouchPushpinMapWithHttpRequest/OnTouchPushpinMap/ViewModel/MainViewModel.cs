using System.Collections.ObjectModel;
using System.Device.Location;
using GalaSoft.MvvmLight;

namespace OnTouchPushpinMap.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public ObservableCollection<LocationModel> Locations { get; private set; }

        public MainViewModel()
        {
            this.Locations = new ObservableCollection<LocationModel>();
            this.LoadData();
        }
        public void LoadData()
        {
            // Sample data; replace with real data
            this.Locations.Add(new LocationModel { GeoCoordinate = new GeoCoordinate(23.7445, 90.3848), Address = "Green Road" });
            this.Locations.Add(new LocationModel { GeoCoordinate = new GeoCoordinate(23.7457, 90.3925), Address = "Kataban Road" });
            this.Locations.Add(new LocationModel { GeoCoordinate = new GeoCoordinate(23.7517, 90.3975), Address = "Sonargaon Road" });
        }
    }
}