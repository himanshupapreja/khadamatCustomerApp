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
    public partial class SupportPage : ContentPage
    {
        bool IsSubscribe;
        SupportPageViewModel SupportPageViewModel;
        public SupportPage()
        {
            InitializeComponent(); 

            MessagingCenter.Subscribe<string>(this, "ScrollToEnd", (sender) =>
            {
                var v = supportChat.ItemsSource.Cast<object>().LastOrDefault();
                if (v != null)
                {
                    supportChat.ScrollTo(v, null, ScrollToPosition.End, true);
                }
            });
            IsSubscribe = true;
            SupportPageViewModel = this.BindingContext as SupportPageViewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            SupportPageViewModel.OnAppearing();

            MessagingCenter.Unsubscribe<ImagesModel>(this, "ProfilePicture");

            if (!IsSubscribe)
            {
                MessagingCenter.Subscribe<string>(this, "ScrollToEnd", (sender) =>
                {
                    var v = supportChat.ItemsSource.Cast<object>().LastOrDefault();
                    if (v != null)
                    {
                        supportChat.ScrollTo(v, null, ScrollToPosition.End, true);
                    }
                });
                IsSubscribe = true;
            }
        }

        protected override bool OnBackButtonPressed()
        {
            if (!SupportPageViewModel.IsBackPress)
            {
                //MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_backpress, msDuration: 3000);
                return true;
            }
            else
            {
                return false;
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            SupportPageViewModel.OnDisappearing();
            MessagingCenter.Unsubscribe<string>(this, "ScrollToEnd");
            IsSubscribe = false;
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            //var data = ((TappedEventArgs)e).Parameter as SingleChatListModel;
            var data = ((Button)sender).CommandParameter as SingleChatListModel;
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
