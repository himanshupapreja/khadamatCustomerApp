using Khadamat_CustomerApp.Resources;
using Khadamat_CustomerApp.ViewModels;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Khadamat_CustomerApp.Views
{
    public partial class JobRequestPage : ContentPage
    {
        private string CategoryName;
        private string ServiceName;
        private int CategoryId;
        private int ServiceId;

        public static DatePicker newdatePicker;
        private DateTime datetimeSelected;
        private JobRequestPageViewModel jobRequestViewModel;
        public JobRequestPage()
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
            newdatePicker = datePicker;
            newdatePicker.MinimumDate = DateTime.Now;
            jobRequestViewModel = this.BindingContext as JobRequestPageViewModel;
        }

        public static void DatePickerOpen()
        {
            newdatePicker.Focus();
        }

        private void DatePicker_Unfocused(object sender, FocusEventArgs e)
        {
            datetimeSelected = ((DatePicker)sender).Date;
            timePicker.Focus();
        }

        private async void TimePicker_Unfocused(object sender, FocusEventArgs e)
        {
            datetimeSelected = datetimeSelected.Add(((TimePicker)sender).Time);
            if (datetimeSelected > DateTime.Now)
            {
                jobRequestViewModel.JobDateTime = datetimeSelected;
                jobRequestViewModel.JobDateTimeValue = datetimeSelected.ToString("dd-MMM-yyyy, hh:mm tt");
                jobRequestViewModel.JobDateValue = datetimeSelected.ToString("dd-MMM-yyyy");
                jobRequestViewModel.JobTimeValue = datetimeSelected.ToString("hh:mm tt");
            }
            else
            {
                var answer = await App.Current.MainPage.DisplayAlert(AppResource.error, AppResource.error_jobdatetime, AppResource.Ok, AppResource.Cancel);
                if (answer)
                {
                    datePicker.Focus();
                }
            }
        }
    }
}
