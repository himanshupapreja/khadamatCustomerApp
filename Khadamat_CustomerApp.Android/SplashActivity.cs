using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Content.PM;

namespace Khadamat_CustomerApp.Droid
{
    [Activity(Theme = "@style/ThemeSplashHKD", MainLauncher = false, NoHistory = false,
        Label = "Khadamat_CustomerApp", Icon = "@drawable/logo",
          ScreenOrientation = ScreenOrientation.Portrait)]
    //[IntentFilter(new[] { Intent.ActionView },
    //Categories = new[]
    //{
    //    Intent.CategoryDefault,
    //    Intent.CategoryBrowsable
    //},
    //DataScheme = "http", DataHost = "khadamatcustomerapps.page.link", AutoVerify = true)]

    //[IntentFilter(new[] { Intent.ActionView },
    //    Categories = new[]
    //    {
    //        Intent.CategoryDefault,
    //        Intent.CategoryBrowsable
    //},
    //DataScheme = "https", DataHost = "khadamatcustomerapps.page.link", AutoVerify = true)]
    public class SplashActivity : Activity
    {
        public static string DeepLink { get; set; }
        private static int SPLASH_TIME = 1 * 1000;// 1 seconds
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Intent intent = this.Intent;
            DeepLink = intent.DataString;
            try
            {
                new Handler().PostDelayed(() =>
                {
                    var intent = new Intent(this, typeof(MainActivity));
                    StartActivity(intent);
                    Finish();

                }, SPLASH_TIME);

            }
            catch (Exception e) { }

        }
    }
}