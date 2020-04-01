
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Android.Support.V4.Widget;
using Android.Views;
using Android.Support.V4.App;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android.AppCompat;
using Khadamat_CustomerApp.Droid.CustomRenderers;

[assembly: ExportRenderer(typeof(MasterDetailPage), typeof(MyMasterDetailPageRenderer))]

namespace Khadamat_CustomerApp.Droid.CustomRenderers
{
	[Obsolete]
	public class MyMasterDetailPageRenderer : MasterDetailPageRenderer
	{
		protected override void OnElementChanged(VisualElement oldElement, VisualElement newElement)
		{
			base.OnElementChanged(oldElement, newElement);

			var fieldInfo = GetType().BaseType.GetField("_masterLayout", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
			var _masterLayout = (ViewGroup)fieldInfo.GetValue(this);
			var lp = new DrawerLayout.LayoutParams(_masterLayout.LayoutParameters);
            if (Application.Current.Properties.ContainsKey("AppLocale") && !string.IsNullOrEmpty(Application.Current.Properties["AppLocale"].ToString()))
            {
                var languageculture = Application.Current.Properties["AppLocale"].ToString();
                if (languageculture == "ar-AE")
                {
                    lp.Gravity = (int)GravityFlags.Right;
                }
                else
                {
                    lp.Gravity = (int)GravityFlags.Left;
                }
            }
            else
            {
                lp.Gravity = (int)GravityFlags.Left;
            }
			_masterLayout.LayoutParameters = lp;
		}
	}
}