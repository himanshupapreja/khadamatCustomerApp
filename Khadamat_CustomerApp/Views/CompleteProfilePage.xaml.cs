using System;
using Xamarin.Forms;

namespace Khadamat_CustomerApp.Views
{
    public partial class CompleteProfilePage : ContentPage
    {
        public CompleteProfilePage()
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

        protected override bool OnBackButtonPressed()
        {
            return true;
        }

        private void YYYY_TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            yyyy_Picker.Focus();
        }

        private void DD_TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            dd_Picker.Focus();
        }

        private void MMM_TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            mmm_Picker.Focus();
        }
    }
}
