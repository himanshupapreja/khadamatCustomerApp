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
using Khadamat_CustomerApp.DependencyInterface;
using Khadamat_CustomerApp.Droid.FirebaseService;
using Xamarin.Forms;

[assembly: Dependency(typeof(FirebaseTokenUpdate))]
namespace Khadamat_CustomerApp.Droid.FirebaseService
{
    public class FirebaseTokenUpdate : IFirebaseTokenUpdate
    {
        public async Task<string> GetNewFirebaseToken()
        {
            MyFirebaseIIDService service = new MyFirebaseIIDService();
            return await service.GenerateNewToken();


        }
    }
}