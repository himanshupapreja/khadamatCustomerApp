using Xamarin.Forms;

namespace Khadamat_CustomerApp.Views
{
    public partial class ServiceDetailPage : ContentPage
    {
        public ServiceDetailPage()
        {
            InitializeComponent();
            if (Application.Current.Properties.ContainsKey("AppLocale") && !string.IsNullOrEmpty(Application.Current.Properties["AppLocale"].ToString()))
            {
                var languageculture = Application.Current.Properties["AppLocale"].ToString();
                if (languageculture == "ar-AE")
                {
                    this.FlowDirection = FlowDirection.RightToLeft;
                }
                else
                {
                    this.FlowDirection = FlowDirection.LeftToRight;
                }
            }
            else
            {
                this.FlowDirection = FlowDirection.LeftToRight;
            }
        }
    }
}
