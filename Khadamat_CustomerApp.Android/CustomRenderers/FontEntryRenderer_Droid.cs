using System;
using Android.Content;
using Android.Graphics;
using Khadamat_CustomerApp.Droid.CustomRenderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Entry), typeof(FontEntryRenderer_Droid))]
namespace Khadamat_CustomerApp.Droid.CustomRenderers
{
    public class FontEntryRenderer_Droid : EntryRenderer
    {
        public FontEntryRenderer_Droid(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement == null)
            {
                Control.Background = null;
                Control.SetPadding(0, 0, 0, 0);
            }

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
            catch (Exception)
            {
            }
        }
    }
}