using Khadamat_CustomerApp.Helpers;
using Khadamat_CustomerApp.Models;
using Khadamat_CustomerApp.Resources;
using Khadamat_CustomerApp.Views;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;
using static Khadamat_CustomerApp.Helpers.Enums;

namespace Khadamat_CustomerApp.ViewModels
{
    public class NotificationPageViewModel : BaseViewModel, INavigationAware
    {
        private readonly INavigationService NavigationService;
        private static int PageNumber;
        #region IsLoaderBusy Field
        private bool _IsLoaderBusy;

        public bool IsLoaderBusy
        {
            get { return _IsLoaderBusy; }
            set { SetProperty(ref _IsLoaderBusy, value); }
        }
        #endregion

        #region IsNoInternetView Field
        private bool _IsNoInternetView;

        public bool IsNoInternetView
        {
            get { return _IsNoInternetView; }
            set { SetProperty(ref _IsNoInternetView, value); }
        }
        #endregion

        #region IsNoDataFoundView Field
        private bool _IsNoDataFoundView;

        public bool IsNoDataFoundView
        {
            get { return _IsNoDataFoundView; }
            set { SetProperty(ref _IsNoDataFoundView, value); }
        }
        #endregion

        #region IsRefreshing Field
        private bool _IsRefreshing;

        public bool IsRefreshing
        {
            get { return _IsRefreshing; }
            set { SetProperty(ref _IsRefreshing, value); }
        }
        #endregion

        #region NotificationList
        private ObservableCollection<NotificationsModel> AllNotificationList = new ObservableCollection<NotificationsModel>();
        private ObservableCollection<NotificationsModel> _NotificationList = new ObservableCollection<NotificationsModel>();
        public ObservableCollection<NotificationsModel> NotificationList
        {
            get { return _NotificationList; }
            set { SetProperty(ref _NotificationList, value); }
        }
        #endregion

        #region ItemTreshold
        private int _itemTreshold;
        public int ItemTreshold
        {
            get { return _itemTreshold; }
            set { SetProperty(ref _itemTreshold, value); }
        } 
        #endregion

        #region Constructor
        public NotificationPageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
            ItemTreshold = 1;
            PageNumber = 1;
        }
        #endregion

        #region ItemTresholdReachedCommand
        public Command ItemTresholdReachedCommand
        {
            get
            {
                return new Command(async() =>
                {
                    IsLoaderBusy = true;

                    try
                    {
                        PageNumber = PageNumber + 1;
                        Getnotification(PageNumber);
                        //MessagingCenter.Send<object, Item>(this, ScrollToPreviousLastItem, previousLastItem);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex);
                    }
                    finally
                    {
                        IsLoaderBusy = false;
                    }
                });
            }
        }
        #endregion

        #region Getnotification
        private async void Getnotification(int pageNumber)
        {
            try
            {
                if (Common.CheckConnection())
                {
                    IsLoaderBusy = true;
                    NotificationsResponseModel response;
                    try
                    {
                        response = await _webApiRestClient.GetAsync<NotificationsResponseModel>(string.Format(ApiUrl.GetNotificationsWithPaging, pageNumber, user_id));
                    }
                    catch (Exception ex)
                    {
                        response = null;
                        IsLoaderBusy = false;
                        IsRefreshing = false;
                        await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_ServerError, msDuration: 3000);
                    }
                    if (response != null)
                    {
                        if (response.status)
                        {
                            if (pageNumber == 1)
                            {
                                AllNotificationList.Clear(); 
                            }
                            var notificationdata = response.data;
                            foreach (var item in notificationdata)
                            {
                                item.display_text = Common.GetLanguage() != "ar-AE" ? item.text : item.arabic_text;
                                item.display_created_date = Common.GetLanguage() != "ar-AE" ? item.created_date_str : item.created_date_str_arabic;
                                item.UserPic = item.notification_status != Convert.ToInt32(NotificationStatus.AcceptedQuotation) && item.notification_status != Convert.ToInt32(NotificationStatus.RejectedQuotation) ? Common.IsImagesValid(item.from_user_image, ApiUrl.BaseUrl) : Common.IsImagesValid(item.to_user_image, ApiUrl.BaseUrl);
                                item.IsQuoteSent = item.notification_status == Convert.ToInt32(NotificationStatus.SentQuotation) ? true : false;
                                item.IsViewDetail = item.job_request_id.HasValue && item.job_request_id.Value > 0 ? true : false;
                                item.ViewNotificationBtn = !string.IsNullOrEmpty(item.pdf_file) && !string.IsNullOrWhiteSpace(item.pdf_file) ? AppResource.notification_ViewInvoice : AppResource.notification_ViewDetail;
                                AllNotificationList.Add(item);
                            }
                            NotificationList = AllNotificationList;

                            if(AllNotificationList.Count > 0)
                            {
                                IsNoDataFoundView = false;
                                IsNoInternetView = false;
                            }
                            else
                            {
                                IsNoDataFoundView = true;
                                IsNoInternetView = false;
                            }
                            
                            if (notificationdata == null || notificationdata.Count == 0)
                            {
                                ItemTreshold = -1;
                                return;
                            }

                            Application.Current.Properties["NotificationData"] = JsonConvert.SerializeObject(NotificationList);
                            Application.Current.SavePropertiesAsync();
                        }
                        else
                        {
                            await MaterialDialog.Instance.SnackbarAsync(message: response.message,
                                        msDuration: 3000);
                        }
                    }
                    else
                    {
                        if (Application.Current.Properties.ContainsKey("NotificationData") && !string.IsNullOrEmpty(Application.Current.Properties["NotificationData"].ToString()) && !string.IsNullOrWhiteSpace(Application.Current.Properties["NotificationData"].ToString()))
                        {
                            OfflineData();
                        }
                        else
                        {
                            IsNoDataFoundView = true;
                        }
                    }
                }
                else
                {
                    if (Application.Current.Properties.ContainsKey("NotificationData") && !string.IsNullOrEmpty(Application.Current.Properties["NotificationData"].ToString()) && !string.IsNullOrWhiteSpace(Application.Current.Properties["NotificationData"].ToString()))
                    {
                        OfflineData();
                    }
                    else
                    {
                        IsNoInternetView = true;
                        //await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_InternetError,
                        //                        msDuration: 3000);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsLoaderBusy = false;
                IsRefreshing = false;
            }
        }
        #endregion

        #region DataWhennoInternet
        private void OfflineData()
        {
            var notificationData = JsonConvert.DeserializeObject<ObservableCollection<NotificationsModel>>(Application.Current.Properties["NotificationData"].ToString());

            NotificationList = notificationData;
            if (notificationData.Count > 0)
            {
                IsNoDataFoundView = false;
                IsNoInternetView = false;
            }
            else
            {
                IsNoDataFoundView = true;
                IsNoInternetView = false;
            }
        }
        #endregion

        #region RefreshCommand
        public Command RefreshCommand
        {
            get
            {
                return new Command(() =>
                {
                    ItemTreshold = 1;
                    PageNumber = 1;
                    Getnotification(PageNumber);
                });
            }
        }
        #endregion

        #region ViewDetailCommand
        public Command ViewDetailCommand
        {
            get
            {
                return new Command((e) =>
                {
                    var item = (NotificationsModel)e;
                    if (string.IsNullOrEmpty(item.pdf_file) || string.IsNullOrWhiteSpace(item.pdf_file))
                    {
                        var param = new NavigationParameters();
                        param.Add("JobID", item.job_request_id.Value);
                        NavigationService.NavigateAsync(nameof(JobDetailPage), param);

                        //IsLoaderBusy = true;
                        //await App.Current.MainPage.Navigation.PushPopupAsync(new LoaderPopup());
                        //JobDetailResponseModel response;
                        //try
                        //{
                        //    if (Common.CheckConnection())
                        //    {
                        //        try
                        //        {
                        //            response = await _webApiRestClient.GetAsync<JobDetailResponseModel>(string.Format(ApiUrl.GetJobRequestDetail, item.job_request_id.Value));
                        //        }
                        //        catch (Exception ex)
                        //        {
                        //            response = null;
                        //            await MaterialDialog.Instance.SnackbarAsync(AppResource.error_ServerError, 3000);
                        //        }
                        //        if (response != null)
                        //        {
                        //            if (response.status)
                        //            {
                        //                //Common.CustomNavigation(_navigation, new JobDetailPage(response.jobRequestData));
                        //                var param = new NavigationParameters();
                        //                param.Add("JobData", response.jobRequestData);
                        //                await NavigationService.NavigateAsync(nameof(JobDetailPage), param);
                        //            }
                        //            else
                        //            {
                        //                await MaterialDialog.Instance.SnackbarAsync(response.message, 3000);
                        //            }
                        //        }
                        //    }
                        //    else
                        //    {
                        //        await MaterialDialog.Instance.SnackbarAsync(AppResource.error_InternetError, 3000);
                        //    }
                        //}
                        //catch (Exception ex)
                        //{
                        //}
                        //finally
                        //{
                        //    //IsLoaderBusy = false;
                        //    await Task.Delay(1500);
                        //    LoaderPopup.CloseAllPopup();
                        //}
                    }
                    else
                    {
                        //IsLoaderBusy = true;
                        //await App.Current.MainPage.Navigation.PushPopupAsync(new LoaderPopup());
                        try
                        {
                            Launcher.OpenAsync(new Uri(ApiUrl.PdfBaseUrl + item.pdf_file));
                        }
                        catch (Exception ex)
                        {
                        }
                        finally
                        {
                            //await Task.Delay(1500);
                            //LoaderPopup.CloseAllPopup();
                        }
                    }
                });
            }
        }
        #endregion

        #region QuoteAcceptCommand
        public Command QuoteAcceptCommand
        {
            get
            {
                return new Command(async(e) =>
                {
                    if (Common.CheckConnection())
                    {
                        var item = (NotificationsModel)e;
                        AcceptrejectQuote(true, item);
                    }
                    else
                    {
                        await MaterialDialog.Instance.SnackbarAsync(AppResource.error_InternetError, msDuration: 3000);
                    }
                });
            }
        }
        #endregion

        #region QuoteCancelCommand
        public Command QuoteCancelCommand
        {
            get
            {
                return new Command(async (e) =>
                {
                    if (Common.CheckConnection())
                    {
                        var item = (NotificationsModel)e;
                        AcceptrejectQuote(false, item);
                    }
                    else
                    {
                        await MaterialDialog.Instance.SnackbarAsync(AppResource.error_InternetError, msDuration: 3000);
                    }
                });
            }
        }
        #endregion

        #region AcceptrejectQuote
        private async void AcceptrejectQuote(bool notificationStatusvalue, NotificationsModel item)
        {
            try
            {
                IsLoaderBusy = true;
                //await App.Current.MainPage.Navigation.PushPopupAsync(new LoaderPopup());
                AcceptRejectQuoteModel request = new AcceptRejectQuoteModel()
                {
                    is_accept = notificationStatusvalue,
                    job_request_id = item.job_request_id.Value,
                    notification_id = item.id,
                    user_id = user_id
                };
                AcceptRejectQuoteResponseModel response;
                try
                {
                    response = await _webApiRestClient.PostAsync<AcceptRejectQuoteModel, AcceptRejectQuoteResponseModel>(ApiUrl.AcceptRejectQuote, request);
                }
                catch (Exception ex)
                {
                    response = null;
                    IsLoaderBusy = false;
                    //LoaderPopup.CloseAllPopup();
                    await MaterialDialog.Instance.SnackbarAsync(ex.Message, 3000);
                    return;
                }
                if (response != null)
                {
                    await MaterialDialog.Instance.SnackbarAsync(response.message, 3000);
                    if (response.status)
                    {
                        PageNumber = 1;
                        ItemTreshold = 1;
                        Getnotification(PageNumber);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                IsLoaderBusy = false;
                //LoaderPopup.CloseAllPopup();
            }
        }
        #endregion

        #region BackIconCommand
        public ICommand BackIconCommand
        {
            get
            {
                return new DelegateCommand(async() =>
                {
                    await NavigationService.GoBackAsync();
                });
            }
        }
        #endregion

        #region Navigation Aware
        public void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            IsLoaderBusy = false;
            IsNoDataFoundView = false;
            IsNoInternetView = false;
            if (parameters.ContainsKey("Notification"))
            {
                PageNumber = 1;
                ItemTreshold = 1;
                Getnotification(PageNumber);
            }
        }
        #endregion
    }
}
