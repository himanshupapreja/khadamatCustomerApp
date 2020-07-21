using Khadamat_CustomerApp.ViewModels;
using Xamarin.Forms;

namespace Khadamat_CustomerApp.Views
{
    public partial class CreatePasswordPage : ContentPage
    {
        public CreatePasswordPage()
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

            CreatePasswordPageViewModel.newPswdEnrty = newPswdEnrty;
            CreatePasswordPageViewModel.rePswdEnrty = rePswdEnrty;
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}
