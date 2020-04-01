using Android.Content;
using Android.Graphics;
using Khadamat_CustomerApp.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Picker), typeof(BorderlessPickerRenderer_Droid))]
namespace Khadamat_CustomerApp.Droid.CustomRenderers
{
    public class BorderlessPickerRenderer_Droid : PickerRenderer
    {
        public BorderlessPickerRenderer_Droid(Context context) : base(context)
        {

        }

        public static void Init() { }
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                Control.Background = null;
            }
            if (Control != null)
            {
                // stop double triggering 
                Control.Focusable = false;
                Control.FocusableInTouchMode = false;
            }

            var layoutParams = new MarginLayoutParams(Control.LayoutParameters);
            layoutParams.SetMargins(0, 0, 0, 0);
            LayoutParameters = layoutParams;
            Control.LayoutParameters = layoutParams;
            Control.SetPadding(0, 0, 0, 0);
            SetPadding(0, 0, 0, 0);

            try
            {
                if (!string.IsNullOrEmpty(e.NewElement?.FontFamily))
                {
                    var font = Typeface.CreateFromAsset(Android.App.Application.Context.ApplicationContext.Assets, e.NewElement.FontFamily + ".ttf");
                    Control.Typeface = font;
                    //if (e.NewElement?.FontFamily == "Raleway-ExtraBold")
                    //{
                    //    var font = Typeface.CreateFromAsset(Android.App.Application.Context.ApplicationContext.Assets,
                    //        e.NewElement.FontFamily + ".ttf");
                    //    Control.Typeface = font;
                    //}
                    //else
                    //{
                    //    var font = Typeface.CreateFromAsset(Android.App.Application.Context.ApplicationContext.Assets, e.NewElement.FontFamily + ".otf");
                    //    Control.Typeface = font;
                    //}
                }
            }
            catch (System.Exception)
            {
            }
        }
    }
}