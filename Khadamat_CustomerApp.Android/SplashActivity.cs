﻿using System;
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
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            try
            {
                Finish();
                var intent = new Intent(Application.Context, typeof(MainActivity));
                StartActivity(intent);
                //new Handler().PostDelayed(() =>
                //{
                //    var intent = new Intent(this, typeof(MainActivity));
                //    StartActivity(intent);
                //    Finish();

                //}, SPLASH_TIME);

            }
            catch (Exception e) { }

        }
    }
}