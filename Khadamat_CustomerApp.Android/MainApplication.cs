//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//using Android.App;
//using Android.Content;
//using Android.OS;
//using Android.Runtime;
//using Android.Views;
//using Android.Widget;
//using Plugin.CurrentActivity;
//using Plugin.FirebasePushNotification;

//namespace Khadamat_CustomerApp.Droid
//{
//    [Application]
//    public class MainApplication : Application, Application.IActivityLifecycleCallbacks
//    {
//        public MainApplication(IntPtr handle, JniHandleOwnership transer)
//          : base(handle, transer)
//        {
//        }

//        public override void OnCreate()
//        {
//            base.OnCreate();

//            FirebasePushNotificationManager.Initialize(this, false, true);

//            RegisterActivityLifecycleCallbacks(this);


//            //Set the default notification channel for your app when running Android Oreo
//            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.O)
//            {
//                //Change for your default notification channel id here
//                FirebasePushNotificationManager.DefaultNotificationChannelId = "DefaultChannel";

//                //Change for your default notification channel name here
//                FirebasePushNotificationManager.DefaultNotificationChannelName = "General";
//            }


//            //If debug you should reset the token each time.

//#if DEBUG
//            FirebasePushNotificationManager.Initialize(this, false);
//#else
//              FirebasePushNotificationManager.Initialize(this,false);
//#endif


//            CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
//            {
//                System.Diagnostics.Debug.WriteLine($"TOKEN : {p.Token}");
//            };

//            //Handle notification when app is closed here
//            CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
//            {
//                System.Diagnostics.Debug.WriteLine("NOTIFICATION RECEIVED", p.Data);
//                //MainActivity.webContentList = "webContentList";
//            };


//            CrossFirebasePushNotification.Current.OnNotificationOpened += (s, p) =>
//            {
//                System.Diagnostics.Debug.WriteLine("Opened");
//                foreach (var data in p.Data)
//                {
//                    System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
//                }


//            };


//            CrossFirebasePushNotification.Current.OnNotificationAction += (s, p) =>
//            {
//                System.Diagnostics.Debug.WriteLine("Action");

//                if (!string.IsNullOrEmpty(p.Identifier))
//                {
//                    System.Diagnostics.Debug.WriteLine($"ActionId: {p.Identifier}");
//                    foreach (var data in p.Data)
//                    {
//                        System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
//                    }

//                }

//            };

//            CrossFirebasePushNotification.Current.OnNotificationDeleted += (s, p) =>
//            {

//                System.Diagnostics.Debug.WriteLine("Deleted");

//            };


//            //#if DEBUG
//            //            FirebasePushNotificationManager.Initialize(this, new NotificationUserCategory[]
//            //            {
//            //            new NotificationUserCategory("message",new List<NotificationUserAction> {
//            //                new NotificationUserAction("Reply","Reply",NotificationActionType.Foreground),
//            //                new NotificationUserAction("Forward","Forward",NotificationActionType.Foreground)

//            //            }),
//            //            new NotificationUserCategory("request",new List<NotificationUserAction> {
//            //                new NotificationUserAction("Accept","Accept",NotificationActionType.Default,"check"),
//            //                new NotificationUserAction("Reject","Reject",NotificationActionType.Default,"cancel")
//            //            })

//            //            }, true);
//            //#else
//            //	            FirebasePushNotificationManager.Initialize(this,new NotificationUserCategory[]
//            //		    {
//            //			new NotificationUserCategory("message",new List<NotificationUserAction> {
//            //			    new NotificationUserAction("Reply","Reply",NotificationActionType.Foreground),
//            //			    new NotificationUserAction("Forward","Forward",NotificationActionType.Foreground)

//            //			}),
//            //			new NotificationUserCategory("request",new List<NotificationUserAction> {
//            //			    new NotificationUserAction("Accept","Accept",NotificationActionType.Default,"check"),
//            //			    new NotificationUserAction("Reject","Reject",NotificationActionType.Default,"cancel")
//            //			})

//            //		    },false);
//            //#endif

//            //CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
//            //{



//            //};



//        }

//        public override void OnTerminate()
//        {
//            base.OnTerminate();
//            UnregisterActivityLifecycleCallbacks(this);
//        }

//        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
//        {
//            CrossCurrentActivity.Current.Activity = activity;
//        }

//        public void OnActivityDestroyed(Activity activity)
//        {
//        }

//        public void OnActivityPaused(Activity activity)
//        {
//        }

//        public void OnActivityResumed(Activity activity)
//        {
//            CrossCurrentActivity.Current.Activity = activity;
//        }

//        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
//        {
//        }

//        public void OnActivityStarted(Activity activity)
//        {
//            CrossCurrentActivity.Current.Activity = activity;
//        }

//        public void OnActivityStopped(Activity activity)
//        {
//        }
//    }
//}