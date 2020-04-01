using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;
using Xamarin.Forms.Platform.Android.AppCompat;
using Android.Graphics;
using Android.Support.V4.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Khadamat_CustomerApp.Droid.CustomRenderers;
using Khadamat_CustomerApp.CustomControls;
using Android.Graphics.Drawables;
using Android.Content.Res;
using Android.Support.V4.Graphics.Drawable;
using Android.Util;

//[assembly: ExportRenderer(typeof(TabbedPage), typeof(IconTabbedPageRenderer))]
//namespace Khadamat_CustomerApp.Droid.CustomRenderers
//{
//    public class CustomTabbedPageRenderer : TabbedPageRenderer
//    {
//        //private bool _isConfigured = false;
//        //private ViewPager _pager;
//        //private TabLayout _layout;

//        public CustomTabbedPageRenderer(Context context) : base(context) { }

//        //protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
//        //{
//        //    base.OnElementPropertyChanged(sender, e);

//        //    try
//        //    {
//        //        //_pager = (ViewPager)ViewGroup.GetChildAt(0);
//        //        //_layout = (TabLayout)ViewGroup.GetChildAt(1);
//        //        var pager = ViewGroup.GetChildAt(0);
//        //        var layout = ViewGroup.GetChildAt(1);

//        //        var control = (TabbedPage)sender;
//        //        Android.Graphics.Color selectedColor;
//        //        Android.Graphics.Color unselectedColor;
//        //        selectedColor = new Android.Graphics.Color(ContextCompat.GetColor(Context, Resource.Color.tabBarSelected));
//        //        unselectedColor = new Android.Graphics.Color(ContextCompat.GetColor(Context, Resource.Color.tabBarUnselected));

//        //        for (int i = 0; i < _layout.TabCount; i++)
//        //        {
//        //            var tab = _layout.GetTabAt(i);
//        //            var icon = tab.Icon;
//        //            if (icon != null)
//        //            {
//        //                var color = tab.IsSelected ? selectedColor : unselectedColor;
//        //                icon = Android.Support.V4.Graphics.Drawable.DrawableCompat.Wrap(icon);
//        //                icon.SetColorFilter(color, PorterDuff.Mode.SrcIn);
//        //            }
//        //        }
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //    }
//        //}

//        private Activity activity;
//        private TabbedPage _tabbedPage;
//        private const string COLOR = "#00796B";

//        protected override void OnElementChanged(ElementChangedEventArgs<TabbedPage> e)
//        {
//            base.OnElementChanged(e);
//            activity = this.Context as Activity;

//            _tabbedPage = e.NewElement as TabbedPage;


//        }

//        protected override void DispatchDraw(Canvas canvas)
//        {

//            ActionBar actionBar = activity.ActionBar;


//            if (actionBar != null && actionBar.TabCount > 0)
//            {
//                ColorDrawable colorDrawable = new ColorDrawable(global::Android.Graphics.Color.ParseColor(COLOR));
//                actionBar.SetStackedBackgroundDrawable(colorDrawable);
//                ActionBarTabsSetup(actionBar);

//            }

//            base.DispatchDraw(canvas);
//        }

//        private void ActionBarTabsSetup(ActionBar actionBar)
//        {
//            try
//            {
//                //_tabbedPage.Children[0].IC
//                for (int i = 0; i < actionBar.TabCount; i++)
//                {
//                    Android.App.ActionBar.Tab dashboardTab = actionBar.GetTabAt(i);
//                    if (TabIsEmpty(dashboardTab))
//                    {

//                        int id = Resources.GetIdentifier(_tabbedPage.Children[i].Icon.File, "drawable", Context.PackageName);
//                        TabSetup(dashboardTab, id);
//                    }

//                }

//            }
//            catch (Exception)
//            {

//            }

//        }

//        private bool TabIsEmpty(ActionBar.Tab tab)
//        {
//            if (tab != null)
//                if (tab.CustomView == null)
//                    return true;
//            return false;
//        }

//        private void TabSetup(ActionBar.Tab tab, int resourceID)
//        {
//            ImageView iv = new ImageView(activity);
//            iv.SetImageResource(resourceID);
//            iv.SetPadding(0, 10, 0, 0);

//            tab.SetCustomView(iv);
//        }
//    }
//}

//[Obsolete]
//public class IconTabbedPageRenderer : TabbedPageRenderer
//{
//    private ColorStateList _colors;

//    // Set text colour and remove the indicator strip
//    protected override void OnVisibilityChanged(Android.Views.View changedView, [GeneratedEnum] ViewStates visibility)
//    {
//        base.OnVisibilityChanged(changedView, visibility);

//        if (visibility == ViewStates.Visible)
//        {
//            TabbedPage element = (TabbedPage)Element;

//            int[][] states = new int[][]
//            {
//                        new int[] { Android.Resource.Attribute.StateSelected },
//                        new int[] { -Android.Resource.Attribute.StateSelected }
//            };

//            int[] colors = new int[]
//            {
//                        //element.SelectedColor.ToAndroid(),
//                        //element.UnselectedColor.ToAndroid()                        
//                Xamarin.Forms.Color.FromHex("#eeaf1c").ToAndroid(),
//                Xamarin.Forms.Color.FromHex("#1964c3").ToAndroid()
//            };

//            _colors = new ColorStateList(states, colors);

//            TabLayout tabLayout = null;

//            for (int i = 0; i < ChildCount; ++i)
//            {
//                Android.Views.View view = GetChildAt(i);
//                if (view is TabLayout) tabLayout = (TabLayout)view;
//            }

//            //tabLayout.SetTabTextColors(element.UnselectedColor.ToAndroid(), element.SelectedColor.ToAndroid());
//            tabLayout.SetSelectedTabIndicatorHeight(0);
//        }
//    }

//    // Set icon colours and select the first tab
//    protected override void OnLayout(bool changed, int l, int t, int r, int b)
//    {
//        TabLayout _tabs = null;

//        for (int i = 0; i < ChildCount; ++i)
//        {
//            Android.Views.View view = GetChildAt(i);
//            if (view is TabLayout) _tabs = (TabLayout)view;
//        }

//        for (int i = 0; i < _tabs.TabCount; i++)
//        {
//            TabLayout.Tab tab = _tabs.GetTabAt(i);
//            var icon = tab.Icon;
//            if (icon != null)
//            {
//                icon = DrawableCompat.Wrap(icon);
//                DrawableCompat.SetTintList(icon, _colors);
//            }
//        }

//        if (_tabs.TabCount > 0)
//        {
//            TabbedPage element = (TabbedPage)Element;
//            TabLayout.Tab tab = _tabs.GetTabAt(0);
//            var icon = tab.Icon;
//            icon.SetState(new int[] { Android.Resource.Attribute.StateSelected });
//        }


//        int height = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, 40, Resources.DisplayMetrics);

//        ViewGroup vg = (ViewGroup)_tabs.GetChildAt(0);
//        int tabsCount = vg.ChildCount;
//        for (int j = 0; j < tabsCount; j++)
//        {
//            ViewGroup vgTab = (ViewGroup)vg.GetChildAt(j);
//            int tabChildsCount = vgTab.ChildCount;
//            for (int i = 0; i < tabChildsCount; i++)
//            {
//                Android.Views.View tabViewChild = vgTab.GetChildAt(i);
//                if (tabViewChild is Android.Widget.TextView)
//                {
//                    TextView tv = (TextView)tabViewChild;
//                    tv.SetAllCaps(false);
//                    LinearLayout.LayoutParams llp = new LinearLayout.LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent);
//                    int size = (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, -10, Resources.DisplayMetrics);
//                    llp.SetMargins(0, size, 0, 0);
//                    tv.LayoutParameters = llp;
//                }
//            }
//        }

//        InvertLayoutThroughScale();

//        base.OnLayout(changed, l, t, r, b);
//    }


//    // Move tabs to the bottom of the page
//    private void InvertLayoutThroughScale()
//    {
//        ViewGroup.ScaleY = -1;

//        TabLayout tabLayout = null;
//        ViewPager viewPager = null;

//        for (int i = 0; i < ChildCount; ++i)
//        {
//            Android.Views.View view = GetChildAt(i);
//            if (view is TabLayout) tabLayout = (TabLayout)view;
//            else if (view is ViewPager) viewPager = (ViewPager)view;
//        }

//        tabLayout.ScaleY = viewPager.ScaleY = -1;
//        viewPager.SetPadding(0, -tabLayout.MeasuredHeight, 0, 0);
//    }
//}