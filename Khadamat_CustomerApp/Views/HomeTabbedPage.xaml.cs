using Khadamat_CustomerApp.CustomControls;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace Khadamat_CustomerApp.Views
{
    public partial class HomeTabbedPage : HiddenTabbedPage
    {
        public HomeTabbedPage()
        {
            InitializeComponent();
            MessagingCenter.Subscribe<string>(this, "TabSelected", (sender) =>
            {
                if (sender.Equals("HomeTab"))
                {
                    this.CurrentPage = Children[0];
                }
                else if (sender.Equals("ExpressTab"))
                {
                    this.CurrentPage = Children[1];
                }
            });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
        }
    }
}
