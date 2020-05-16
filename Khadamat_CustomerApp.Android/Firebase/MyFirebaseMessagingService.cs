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
using Android.Graphics;
using Android.Support.V4.App;
using Firebase.Messaging;
using System.Text.RegularExpressions;

namespace Khadamat_CustomerApp.Droid.Firebase
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
    public class MyFirebaseMessagingService : FirebaseMessagingService
    {
        const string TAG = "MyFirebaseMsgService";
        public const string PRIMARY_CHANNEL = "default";
        // [START receive_message]
        public override void OnMessageReceived(RemoteMessage message)
        {
            try
            {
                Android.Util.Log.Debug(TAG, "From: " + message.From);
                Android.Util.Log.Debug(TAG, "Notification Message Body: " + message.GetNotification().Body);
                SendNotifications(message);
            }
            catch (Exception ex)
            {
            }

        }
        // [END receive_message]



        public void SendNotifications(RemoteMessage message)
        {
            try
            {
                NotificationManager manager = (NotificationManager)GetSystemService(NotificationService);
                var seed = Convert.ToInt32(Regex.Match(Guid.NewGuid().ToString(), @"\d+").Value);
                int id = new Random(seed).Next(000000000, 999999999);
                var push = new Intent();
                push.SetAction(message.GetNotification().Body);
                var fullScreenPendingIntent = PendingIntent.GetActivity(this, 0, push, PendingIntentFlags.UpdateCurrent);
                NotificationCompat.Builder notification;
                if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                {
                    var chan1 = new NotificationChannel(PRIMARY_CHANNEL,
                     new Java.Lang.String("Primary"), NotificationImportance.High);
                    chan1.LightColor = Color.Green;
                    manager.CreateNotificationChannel(chan1);
                    notification = new NotificationCompat.Builder(this, PRIMARY_CHANNEL);
                }
                else
                {
                    notification = new NotificationCompat.Builder(this);
                }
                notification.SetContentIntent(fullScreenPendingIntent)
                         .SetContentTitle(message.GetNotification().Title)
                         .SetContentText(message.GetNotification().Body)
                         .SetLargeIcon(BitmapFactory.DecodeResource(Resources, Resource.Drawable.logo))
                         .SetSmallIcon(Resource.Drawable.logo)
                         .SetStyle((new NotificationCompat.BigTextStyle()))
                         .SetPriority(NotificationCompat.PriorityHigh)
                         .SetColor(0x9c6114)
                         .SetAutoCancel(true);
                manager.Notify(id, notification.Build());
            }
            catch (Exception ex)
            {
            }
        }

        public override void HandleIntent(Intent p0)
        {
            try
            {
                if (p0.Extras != null)
                {
                    var builder = new RemoteMessage.Builder("FirebaseMessagingService");

                    foreach (string key in p0.Extras.KeySet())
                    {
                        builder.AddData(key, p0.Extras.Get(key).ToString());
                    }

                    OnMessageReceived(builder.Build());
                }
                else
                {
                    base.HandleIntent(p0);
                }
            }
            catch (Exception)
            {
                base.HandleIntent(p0);
            }
        }
    }
}