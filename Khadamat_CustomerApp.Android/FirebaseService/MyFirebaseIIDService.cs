using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Firebase.Iid;

namespace Khadamat_CustomerApp.Droid.FirebaseService
{
    [Service]
    [IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
    public class MyFirebaseIIDService : FirebaseInstanceIdService
    {
        const string TAG = "MyFirebaseIIDService";
        public override void OnTokenRefresh()
        {
            try
            {
                var refreshedToken = FirebaseInstanceId.Instance.Token;
                Xamarin.Forms.Application.Current.Properties["AppFirebaseToken"] = refreshedToken;
                Xamarin.Forms.Application.Current.SavePropertiesAsync();
            }
            catch (Exception e)
            {
            }
        }

        public async Task<string> GenerateNewToken()
        {
            //var instanceId = await FirebaseInstanceId.Instance.GetToken();
            //var token = instanceId.Class.GetMethod("getToken").Invoke(instanceId).ToString();
            string token = "";

            await Task.Run(() =>
            {
                //String authorizedEntity = "sjmc-256608"; 
                String authorizedEntity = "266362631613";
                String scope = "GCM";
                token = FirebaseInstanceId.Instance.GetToken(authorizedEntity, scope);
            });
            return token;
        }

        private void Disconnect()
        {
            FirebaseInstanceId.Instance.DeleteInstanceId();
        }

    }
}