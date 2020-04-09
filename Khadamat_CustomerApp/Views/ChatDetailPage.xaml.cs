using Khadamat_CustomerApp.DependencyInterface;
using Khadamat_CustomerApp.Models;
using Khadamat_CustomerApp.Resources;
using Khadamat_CustomerApp.ViewModels;
using System;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace Khadamat_CustomerApp.Views
{
    public partial class ChatDetailPage : ContentPage
    {
        ChatDetailPageViewModel ChatDetailPageViewModel;
        public ChatDetailPage()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {

            }
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
            this.FlowDirection = FlowDirection.LeftToRight;
            //}

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            MessagingCenter.Subscribe<string>(this, "ScrollToEnd", (sender) =>
            {
                try
                {
                    var v = chatdetailList.ItemsSource.Cast<object>().LastOrDefault();
                    if (v != null)
                    {
                        chatdetailList.ScrollTo(v, null, ScrollToPosition.End, true);
                    }
                }
                catch (System.Exception ex)
                {
                }
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            ChatDetailPageViewModel = this.BindingContext as ChatDetailPageViewModel;
            ChatDetailPageViewModel.OnDisappearing();
            MessagingCenter.Unsubscribe<string>(this, "ScrollToEnd");
        }

        protected override bool OnBackButtonPressed()
        {
            if (!ChatDetailPageViewModel.IsBackPress)
            {
                //MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_backpress, msDuration: 3000);
                return true;
            }
            else
            {
                return base.OnBackButtonPressed();
            }
        }

        private void TapGestureRecognizer_Tapped(object sender, System.EventArgs e)
        {
            var data = ((Button)sender).CommandParameter as ChatDetailListModel;
            if (!string.IsNullOrEmpty(data.file_url) && !string.IsNullOrWhiteSpace(data.file_url))
            {
                try
                {
                    Launcher.OpenAsync(new Uri(data.file_url));
                }
                catch (Exception ex)
                {
                }
                finally
                {
                }
            }
            else if (!string.IsNullOrWhiteSpace(data.image_url) && !string.IsNullOrEmpty(data.image_url))
            {
                try
                {
                    Launcher.OpenAsync(new Uri(data.image_url));
                }
                catch (Exception ex)
                {
                }
                finally
                {
                }
            }
        }

        private void Downloader_OnFileDownloaded(object sender, DownloadEventArgs e)
        {
            //if (e.FileSaved)
            //{
            //    App.Current.MainPage.DisplayAlert(AppResource.downloadFile_Title, AppResource.downloadFile_Success, AppResource.Ok);
            //}
            //else
            //{
            //    App.Current.MainPage.DisplayAlert(AppResource.downloadFile_Title, AppResource.downloadFile_Error, AppResource.Ok);
            //}
        }
    }
}
