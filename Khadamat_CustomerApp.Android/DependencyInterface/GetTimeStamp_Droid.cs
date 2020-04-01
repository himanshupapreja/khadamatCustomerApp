using Firebase.Database;
using Khadamat_CustomerApp.DependencyInterface;
using Khadamat_CustomerApp.Droid.DependencyInterface;
using Java.Lang;
using Xamarin.Forms;

[assembly: Dependency(typeof(GetTimeStamp_Droid))]
namespace Khadamat_CustomerApp.Droid.DependencyInterface
{
    public class GetTimeStamp_Droid : IGetTimeStamp
    {
        public object TimeStamp()
        {
            return ServerValue.Timestamp as Object;
        }
    }
}