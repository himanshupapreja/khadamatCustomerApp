using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Provider;
using Android.Views;
using Android.Widget;
using Khadamat_CustomerApp.DependencyInterface;
using Khadamat_CustomerApp.Droid.DependencyInterface;
using Xamarin.Forms;

[assembly: Dependency(typeof(DeviceLocationService_Droid))]
namespace Khadamat_CustomerApp.Droid.DependencyInterface
{
    public class DeviceLocationService_Droid : IDeviceLocationService
    {
        public bool CheckDeviceLocation()
        {
            LocationManager locationManager = (LocationManager)Android.App.Application.Context.GetSystemService(Context.LocationService);
            return locationManager.IsProviderEnabled(LocationManager.GpsProvider);
        }

        public void OpenDeviceSetting()
        {
            LocationManager locationManager = (LocationManager)Android.App.Application.Context.GetSystemService(Context.LocationService);
            //Toast.MakeText(Android.App.Application.Context, "Please enable GPS!", ToastLength.Long).Show();
            Intent gpsSettingIntent = new Intent(Settings.ActionLocationSourceSettings);
            gpsSettingIntent.AddFlags(ActivityFlags.NewTask);
            gpsSettingIntent.AddFlags(ActivityFlags.MultipleTask);
            Android.App.Application.Context.StartActivity(gpsSettingIntent);
        }
    }
}