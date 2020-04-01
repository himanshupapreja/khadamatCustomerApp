using Khadamat_CustomerApp.ViewModels;
using Xamarin.Forms;

namespace Khadamat_CustomerApp.Views
{
    public partial class MyBookingPage : ContentPage
    {
        MyBookingPageViewModel MyBookingPageViewModel;
        public MyBookingPage()
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
            MyBookingPageViewModel = this.BindingContext as MyBookingPageViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            Device.BeginInvokeOnMainThread(() =>
            {
                MyBookingPageViewModel.OnAppearing();
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            Device.BeginInvokeOnMainThread(() =>
            {
                MyBookingPageViewModel.OnDisappearing();
            });
        }

        //private void myBookingList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    myBookingList.SelectedItem = null;
        //}

        //private void myBookingList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        //{
        //}

        private void myBookingList_ItemSelected(object sender, SelectionChangedEventArgs e)
        {

            myBookingList.SelectedItem = null;
        }
    }
}
