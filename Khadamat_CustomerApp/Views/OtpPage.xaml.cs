using Khadamat_CustomerApp.ViewModels;
using Xamarin.Forms;

namespace Khadamat_CustomerApp.Views
{
    public partial class OtpPage : ContentPage
    {
        private OtpPageViewModel OtpPageViewModel;
        public OtpPage()
        {
            InitializeComponent();
            //if (Application.Current.Properties.ContainsKey("AppLocale") && !string.IsNullOrEmpty(Application.Current.Properties["AppLocale"].ToString()))
            //{
            //    var languageculture = Application.Current.Properties["AppLocale"].ToString();
            //    if (languageculture == "ar-AE")
            //    {
            //        this.FlowDirection = FlowDirection.RightToLeft;
            //    }
            //    else
            //    {
            //        this.FlowDirection = FlowDirection.LeftToRight;
            //    }
            //}
            //else
            //{
            //    this.FlowDirection = FlowDirection.LeftToRight;
            //}

            OtpPageViewModel = this.BindingContext as OtpPageViewModel;
            OtpPageViewModel.otpEntry1 = otp1;
            OtpPageViewModel.otpEntry2 = otp2;
            OtpPageViewModel.otpEntry3 = otp3;
            OtpPageViewModel.otpEntry4 = otp4;
            OtpPageViewModel.otpEntry5 = otp5;
        }

        protected override bool OnBackButtonPressed()
        {
            //if (!OtpPageViewModel.ProfilePage)
            //{
            return true;
            //}
            //else
            //{
            //return false;
            //}
        }
    }
}
