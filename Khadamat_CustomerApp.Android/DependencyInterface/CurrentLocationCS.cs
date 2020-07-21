using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Khadamat_CustomerApp.Droid.DependencyInterface;
using Xamarin.Forms;
using Plugin.Geolocator.Abstractions;
using Khadamat_CustomerApp.DependencyInterface;

[assembly: Dependency(typeof(CurrentLocationCS))]
namespace Khadamat_CustomerApp.Droid.DependencyInterface
{
    public class CurrentLocationCS : ICurrentLocationCS
    {
        async Task<Position> ICurrentLocationCS.getLocation()
        {
            Location locationGPS = MainActivity.locationManager.GetLastKnownLocation(LocationManager.GpsProvider);
            if (locationGPS != null)
            {
                double lat = locationGPS.Latitude;
                double longi = locationGPS.Longitude;
                Position location = new Position()
                {
                    Latitude = lat,
                    Longitude = longi
                };
                return location;
            }
            else
            {
                return null;
            }
        }
    }
}