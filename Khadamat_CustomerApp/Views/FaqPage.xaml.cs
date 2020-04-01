using Khadamat_CustomerApp.Models;
using Khadamat_CustomerApp.ViewModels;
using System;
using Xamarin.Forms;

namespace Khadamat_CustomerApp.Views
{
    public partial class FaqPage : ContentPage
    {
        public FaqPage()
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

        private void Question_Tapped(object sender, EventArgs e)
        {
            var view = sender as View;

            var getData = view?.BindingContext as FaqData;

            var vm = BindingContext as FaqPageViewModel;

            vm.Drop_Image_Command.Execute(getData);
        }
    }
}
