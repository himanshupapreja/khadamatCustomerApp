using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Common;
using Android.Gms.Location;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using FFImageLoading.Forms.Platform;
using FFImageLoading.Svg.Forms;
using Firebase;
using HockeyApp.Android;
using ImageCircle.Forms.Plugin.Droid;
using Khadamat_CustomerApp.DependencyInterface;
using Khadamat_CustomerApp.Droid;
using Lottie.Forms.Droid;
using Plugin.CurrentActivity;
using Plugin.FirebasePushNotification;
using Prism;
using Prism.Ioc;
using System.IO;
using Xamarin;
using Xamarin.Forms;

[assembly: Dependency(typeof(MainActivity))]
namespace Khadamat_CustomerApp.Droid
{
    [Activity(Label = "أي خدمات", 
        Icon = "@drawable/logo", MainLauncher = false, 
        ScreenOrientation = ScreenOrientation.Portrait, 
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, 
        LaunchMode = LaunchMode.SingleTop)]

    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, ICameraPermission
    {
        //string HOCKEYAPP_APPID = "febab6eb36ed4e3bb55ba976df3d2df4";
        //protected override void OnResume()
        //{
        //    base.OnResume();
        //    CrashManager.Register(this, HOCKEYAPP_APPID);
        //}

        public static LocationManager locationManager;
        public static MainActivity activity;
        public static bool isForeground;
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            base.SetTheme(Resource.Style.MainTheme);
            base.OnCreate(bundle);

            activity = this;
            
            Forms.SetFlags("CollectionView_Experimental");
            Forms.SetFlags("FastRenderers_Experimental");
            var intent = this.Intent;
            var message = intent.Action;
            Xamarin.Essentials.Platform.Init(this, bundle);
            global::Xamarin.Forms.Forms.Init(this, bundle);


            CachedImageRenderer.Init(true);
            var ignore = typeof(SvgCachedImage);

            AnimationViewRenderer.Init();
            //locationManager = (LocationManager)this.GetSystemService(LocationService);
            //if (string.IsNullOrEmpty(message) || string.IsNullOrWhiteSpace(message))
            //{
                LoadApplication(new App(new AndroidInitializer())); 
            //}
            XF.Material.Droid.Material.Init(this, bundle);

            Fabric.Fabric.With(this, new Crashlytics.Crashlytics());
            Crashlytics.Crashlytics.HandleManagedExceptions();

            //Forms.SetFlags("CollectionView_Experimental");
            //Forms.SetFlags("FastRenderers_Experimental");
            CrossCurrentActivity.Current.Init(this, bundle);
            FormsMaps.Init(this, bundle);
            Rg.Plugins.Popup.Popup.Init(this, bundle);
            ImageCircleRenderer.Init();
            FirebaseApp.InitializeApp(this);
            GetLocationPermissions();

            if (!string.IsNullOrEmpty(message) && !string.IsNullOrWhiteSpace(message))
            {
                NotificationClickedEvent(message, true); 
            }

            //CheckForGoogleServices();
            //FirebasePushNotificationManager.ProcessIntent(this, Intent);
        }

        public bool CheckForGoogleServices()
        {
            var resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
            if (resultCode != ConnectionResult.Success)
            {
                if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
                {
                    Toast.MakeText(Android.App.Application.Context, GoogleApiAvailability.Instance.GetErrorString(resultCode), ToastLength.Long);
                }
                else
                {
                    Toast.MakeText(Android.App.Application.Context, " This device does not support Google Play Services ", ToastLength.Long);
                }
                return false;
            }
            return true;
        }
        protected override void OnNewIntent(Intent intent)
        {
            base.OnNewIntent(intent);
            var message=intent.Action;
            NotificationClickedEvent(message, true);
        }

        protected override void OnPause()
        {
            base.OnPause();
            isForeground = false;
        }

        protected override void OnResume()
        {
            base.OnResume();
            isForeground = true;
        }



        void NotificationClickedEvent(string _msg, bool isForeground)
        {
            MessagingCenter.Send(_msg, "NotificationOpen",isForeground);
        }
        //public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        //{
        //    Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

        //    base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        //}

        const string permissionRS = Manifest.Permission.ReadExternalStorage;
        const string permissionWS = Manifest.Permission.WriteExternalStorage;
        const string permissionCM = Manifest.Permission.Camera;
        const string permissionCL = Manifest.Permission.AccessCoarseLocation;
        const string permissionFL = Manifest.Permission.AccessFineLocation;
        const int RequestLocationId = 0;
        readonly string[] locationpermissions =
        {
           Manifest.Permission.AccessCoarseLocation,
           Manifest.Permission.AccessFineLocation
        };
        readonly string[] camerapermissions =
        {
           Manifest.Permission.ReadExternalStorage,
           Manifest.Permission.WriteExternalStorage,
           Manifest.Permission.Camera,
        };
        public void GetLocationPermissions()
        {
            if ((int)Build.VERSION.SdkInt >= 23 && (this.CheckSelfPermission(permissionCL) != (int)Permission.Granted) || this.CheckSelfPermission(permissionFL) != (int)Permission.Granted)
            {
                this.RequestPermissions(locationpermissions, RequestLocationId);
            }
        }
        public void GetGalleryPermissions()
        {
            if ((int)Build.VERSION.SdkInt >= 23 && this.CheckSelfPermission(permissionRS) != (int)Permission.Granted)
            {
                this.RequestPermissions(camerapermissions, RequestLocationId);
            }
            if ((int)Build.VERSION.SdkInt >= 23 && this.CheckSelfPermission(permissionWS) != (int)Permission.Granted)
            {
                this.RequestPermissions(camerapermissions, RequestLocationId);
            }
            if ((int)Build.VERSION.SdkInt >= 23 && this.CheckSelfPermission(permissionCM) != (int)Permission.Granted)
            {
                this.RequestPermissions(camerapermissions, RequestLocationId);
            }
        }

    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
        }
    }
}

