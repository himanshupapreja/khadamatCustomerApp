using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using System;
using Android.Content;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Views;
using Khadamat_CustomerApp.Droid.CustomRenderers;
using Xamarin.Forms.Platform.Android.AppCompat;
using Khadamat_CustomerApp.CustomControls;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(HiddenTabbarRenderer))]
namespace Khadamat_CustomerApp.Droid.CustomRenderers
{
    public class HiddenTabbarRenderer : TabbedPageRenderer, TabLayout.IOnTabSelectedListener
    {

        private TabLayout TabsLayout { get; set; }
        private ViewPager PagerLayout { get; set; }
        private HiddenTabbedPage CurrentTabbedPage { get; set; }

        //-------------------------------------------------------------------
        public HiddenTabbarRenderer(Context context) : base(context)
        //-------------------------------------------------------------------
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<TabbedPage> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                //cleanup here
            }

            if (e.NewElement != null)
            {
                CurrentTabbedPage = (HiddenTabbedPage)e.NewElement;
            }
            else
                CurrentTabbedPage = (HiddenTabbedPage)e.OldElement;

            //find the pager and tabs
            for (int i = 0; i < ChildCount; ++i)
            {
                Android.Views.View view = (Android.Views.View)GetChildAt(i);
                if (view is TabLayout)
                    TabsLayout = (TabLayout)view;
                else
                if (view is ViewPager) PagerLayout = (ViewPager)view;
            }

        }


        //-------------------------------------------------------------------------------
        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        //-------------------------------------------------------------------------------    
        {
            TabsLayout.Visibility = ViewStates.Gone;

            base.OnLayout(changed, l, t, r, b);
        }
    }


}