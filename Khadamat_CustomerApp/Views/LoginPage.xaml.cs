using Khadamat_CustomerApp.ViewModels;
using Xamarin.Forms;

namespace Khadamat_CustomerApp.Views
{
    public partial class LoginPage : ContentPage
    {
        LoginPageViewModel loginPageViewModel;
        public LoginPage()
        {
            try
            {
                InitializeComponent();
            }
            catch (System.Exception ex)
            {
            }
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
            loginPageViewModel = this.BindingContext as LoginPageViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            loginPageViewModel.OnAppearing();
        }
        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            loginPageViewModel.OnDisappearing();
        }
    }
}
