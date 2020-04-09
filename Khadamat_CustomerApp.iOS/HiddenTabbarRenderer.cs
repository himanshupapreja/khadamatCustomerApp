using System;
using Khadamat_CustomerApp.iOS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(HiddenTabbedPageRenderer))]

namespace Khadamat_CustomerApp.iOS
{
    public class HiddenTabbarRenderer : TabbedRenderer
    {
        private UITabBarController tabbarController { get; set; }
        private HiddenTabbedPage CurrentTabbedPage { get; set; }

        //-------------------------------------------------------------
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        //-------------------------------------------------------------
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                //release any stuff here
            }

            if (e.NewElement != null)
            {
                tabbarController = (UITabBarController)this.ViewController;
                CurrentTabbedPage = (HiddenTabbedPage)e.NewElement;
            }
            else
            {
                CurrentTabbedPage = (HiddenTabbedPage)e.OldElement;
            }

            //the following commented code is not working
            //as Forms as it just leaves empty white space
            //instead of hidden tabbedbar:     
            //       if (tabbarController != null)
            //         tabbarController.TabBar.Hidden = true;
        }

        //just hide tabbar by setting its height to zero
        // credits:
        // https://stackoverflow.com/a/26255545/7149454
        // "how to change UITabBar height"
        private nfloat newHeight = 0; //change tabbed bar height to this value
        //-------------------------------------------------------------------
        public override void ViewWillLayoutSubviews()
        //-------------------------------------------------------------------
        {
            if (tabbarController != null)
            {
                var tabFrame = tabbarController.TabBar.Frame; //self.TabBar is IBOutlet of your TabBar
                tabFrame.Height = newHeight;
                tabFrame.Offset(0, tabbarController.View.Frame.Height - newHeight);
                tabbarController.TabBar.Frame = tabFrame;
            }
            base.ViewWillLayoutSubviews();
        }
    }
}