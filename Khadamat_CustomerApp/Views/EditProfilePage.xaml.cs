using Khadamat_CustomerApp.Models;
using Khadamat_CustomerApp.ViewModels;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Khadamat_CustomerApp.Views
{
    public partial class EditProfilePage : ContentPage
    {
        List<string> PhoneStartingValue = new List<string>()
        {
            "77","71","73","70"
        };
        private UserModel Profiledata;
        public EditProfilePage()
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

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<ImagesModel>(this, "ProfilePicture");
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

        private void provincePickerOpen_Tapped(object sender, EventArgs e)
        {
            provincePicker.Focus();
        }

        private void maritalPickerOpen_Tapped(object sender, EventArgs e)
        {
            maritalPicker.Focus();
        }
    }
}
