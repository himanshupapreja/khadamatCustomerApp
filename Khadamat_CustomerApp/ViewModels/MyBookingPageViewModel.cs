using Khadamat_CustomerApp.Helpers;
using Khadamat_CustomerApp.Models;
using Khadamat_CustomerApp.Resources;
using Khadamat_CustomerApp.Views;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;
using static Khadamat_CustomerApp.Helpers.Enums;

namespace Khadamat_CustomerApp.ViewModels
{
    public class MyBookingPageViewModel : BaseViewModel
    {
        private readonly INavigationService NavigationService;
        private bool IsChatOpen;
        JobRequestData jobcancelclickdata;
        #region IsLoaderBusy Field
        private bool _IsLoaderBusy = true;

        public bool IsLoaderBusy
        {
            get { return _IsLoaderBusy; }
            set { SetProperty(ref _IsLoaderBusy, value); }
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

        #region ServiceCancelMessage Field
        private string _ServiceCancelMessage;

        public string ServiceCancelMessage
        {
            get { return _ServiceCancelMessage; }
            set { SetProperty(ref _ServiceCancelMessage, value); }
        }
        #endregion

        #region IsPopupVisible Field
        private bool _IsPopupVisible;

        public bool IsPopupVisible
        {
            get { return _IsPopupVisible; }
            set { SetProperty(ref _IsPopupVisible, value); }
        }
        #endregion

        #region IsJobPending
        private bool _IsJobPending;

        public bool IsJobPending
        {
            get { return _IsJobPending; }
            set { SetProperty(ref _IsJobPending, value); }
        }
        #endregion

        #region IsJobInprogress
        private bool _IsJobInprogress;

        public bool IsJobInprogress
        {
            get { return _IsJobInprogress; }
            set { SetProperty(ref _IsJobInprogress, value); }
        }
        #endregion

        #region IsJobCompleted
        private bool _IsJobCompleted;

        public bool IsJobCompleted
        {
            get { return _IsJobCompleted; }
            set { SetProperty(ref _IsJobCompleted, value); }
        }
        #endregion

        #region IsJobCancelled
        private bool _IsJobCancelled;

        public bool IsJobCancelled
        {
            get { return _IsJobCancelled; }
            set { SetProperty(ref _IsJobCancelled, value); }
        }
        #endregion

        #region PendingTextColor
        private Color _PendingTextColor;

        public Color PendingTextColor
        {
            get { return _PendingTextColor; }
            set { SetProperty(ref _PendingTextColor, value); }
        }
        #endregion

        #region InProgressTextColor
        private Color _InProgressTextColor;

        public Color InProgressTextColor
        {
            get { return _InProgressTextColor; }
            set { SetProperty(ref _InProgressTextColor, value); }
        }
        #endregion

        #region CompletedTextColor
        private Color _CompletedTextColor;

        public Color CompletedTextColor
        {
            get { return _CompletedTextColor; }
            set { SetProperty(ref _CompletedTextColor, value); }
        }
        #endregion

        #region CancelledTextColor
        private Color _CancelledTextColor;

        public Color CancelledTextColor
        {
            get { return _CancelledTextColor; }
            set { SetProperty(ref _CancelledTextColor, value); }
        }
        #endregion

        #region UpComingDecoration
        private TextDecorations _UpComingDecoration;

        public TextDecorations UpComingDecoration
        {
            get { return _UpComingDecoration; }
            set { SetProperty(ref _UpComingDecoration, value); }
        }
        #endregion

        #region CompletedDecoration
        private TextDecorations _CompletedDecoration;

        public TextDecorations CompletedDecoration
        {
            get { return _CompletedDecoration; }
            set { SetProperty(ref _CompletedDecoration, value); }
        }
        #endregion

        #region CancelledDecoration
        private TextDecorations _CancelledDecoration;

        public TextDecorations CancelledDecoration
        {
            get { return _CancelledDecoration; }
            set { SetProperty(ref _CancelledDecoration, value); }
        }
        #endregion

        #region MyBookingList
        public ObservableCollection<JobRequestData> AllBookingList = new ObservableCollection<JobRequestData>();
        private ObservableCollection<JobRequestData> _MyBookingList = new ObservableCollection<JobRequestData>();
        public ObservableCollection<JobRequestData> MyBookingList
        {
            get { return _MyBookingList; }
            set { SetProperty(ref _MyBookingList, value); }
        }
        #endregion

        #region JobDetailSelected
        public JobRequestData _JobDetailSelected;

        public JobRequestData JobDetailSelected
        {
            get { return _JobDetailSelected; }
            set
            {
                SetProperty(ref _JobDetailSelected, value);
                if (JobDetailSelected != null)
                {
                    JobDetail(JobDetailSelected);
                }
            }
        }

        private bool _IsListVisible = false;
        public bool IsListVisible
        {
            get { return _IsListVisible; }
            set
            {
                SetProperty(ref _IsListVisible, value);
            }
        }
        #endregion

        #region Constructor
        public MyBookingPageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
            IsNoDataFoundView = false;
            IsChatOpen = false;
            IsNoInternetView = false;
        }
        #endregion

        #region JobDetail
        private async void JobDetail(JobRequestData jobDetailSelected)
        {
            IsLoaderBusy = true;
            var param = new NavigationParameters();
            param.Add("JobData", jobDetailSelected);
            await NavigationService.NavigateAsync(new Uri("/NavigationPage/JobDetailPage", UriKind.Relative),param);

            IsLoaderBusy = false;
        }
        #endregion

        #region RefreshCommand
        public Command RefreshCommand
        {
            get
            {
                return new Command(() =>
                {
                    IsPopupVisible = false;
                    GetBookingByStatus();
                });
            }
        }
        #endregion

        #region GetBookingData
        private void GetBookingData()
        {
            try
            {
                PendingTextColor = Color.FromHex(ColorHelpers.LightYellowColor);
                InProgressTextColor = Color.FromHex(ColorHelpers.Black2Color);
                CompletedTextColor = Color.FromHex(ColorHelpers.Black2Color);
                CancelledTextColor = Color.FromHex(ColorHelpers.Black2Color);

                IsJobPending = PendingTextColor == Color.FromHex(ColorHelpers.LightYellowColor) ? true : false;
                IsJobInprogress = InProgressTextColor == Color.FromHex(ColorHelpers.LightYellowColor) ? true : false;
                IsJobCompleted = CompletedTextColor == Color.FromHex(ColorHelpers.LightYellowColor) ? true : false;
                IsJobCancelled = CancelledTextColor == Color.FromHex(ColorHelpers.LightYellowColor) ? true : false;

                GetBookingByStatus();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                IsLoaderBusy = false;
            }
        }
        #endregion

        #region DataWhennoInternet
        private void OfflineData()
        {
            AllBookingList = JsonConvert.DeserializeObject<ObservableCollection<JobRequestData>>(Application.Current.Properties["MyBookingData"].ToString());
        }
        #endregion

        #region GetBookingByStatus Method
        private async void GetBookingByStatus()
        {
            try
            {
                if (Common.CheckConnection())
                {
                    IsLoaderBusy = true;
                    BookingResponseModel response;
                    try
                    {
                        response = await _webApiRestClient.GetAsync<BookingResponseModel>(string.Format(ApiUrl.GetCustomerJobRequests, user_id));
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
                            AllBookingList.Clear();
                            if (response.jobRequestData.Count > 0)
                            {
                                var bookingdata = response.jobRequestData.OrderByDescending(x => x.id).ToList();
                                foreach (var item in bookingdata)
                                {
                                    item.display_category_name = Common.GetLanguage() != "ar-AE" ? item.category_name : item.category_name_arabic;
                                    item.display_sub_category_name = Common.GetLanguage() != "ar-AE" ? item.sub_category_name : item.sub_category_name_arabic;
                                    item.IsNoJobStatusPending = item.status == Convert.ToInt32(JobRequestEnum.Pending) || (string.IsNullOrEmpty(item.coordinator_name) && string.IsNullOrEmpty(item.coordinator_name_one)) ? false : true;
                                    item.IsChatVisible = item.status == Convert.ToInt32(JobRequestEnum.Pending) || item.status == Convert.ToInt32(JobRequestEnum.Completed) || (string.IsNullOrEmpty(item.coordinator_name) && string.IsNullOrEmpty(item.coordinator_name_one) && string.IsNullOrEmpty(item.service_provider_name)) ? false : true;
                                    item.IsJobCancelled = item.status == Convert.ToInt32(JobRequestEnum.Canceled) || item.status == Convert.ToInt32(JobRequestEnum.QuoteCanceled) ? true : false;
                                    item.IsJobCompleted = item.status == Convert.ToInt32(JobRequestEnum.Completed) ? true : false;
                                    item.IsJobPending = item.status == Convert.ToInt32(JobRequestEnum.Pending) || item.status == Convert.ToInt32(JobRequestEnum.Accepted) ? true : false;
                                    item.job_date = item.job_date_time.HasValue ? item.job_date_time.Value.ToString("ddd, MMMM dd, yyyy") : null;
                                    item.job_time = item.job_date_time.HasValue ? item.job_date_time.Value.ToString("HH:mm") : null;
                                    item.WorkerImage = !string.IsNullOrEmpty(item.service_provider_name) && !string.IsNullOrWhiteSpace(item.service_provider_name) ? Common.IsImagesValid(item.service_provider_image, ApiUrl.BaseUrl) : item.IsJobCancelled ? Common.IsImagesValid(item.coordinator_image_one, ApiUrl.BaseUrl) : Common.IsImagesValid(item.coordinator_image, ApiUrl.BaseUrl);
                                    item.WorkerName = !string.IsNullOrEmpty(item.service_provider_name) && !string.IsNullOrWhiteSpace(item.service_provider_name) ? item.service_provider_name : item.IsJobCancelled ? item.coordinator_name_one : item.coordinator_name;
                                    item.WorkerServiceName = !string.IsNullOrEmpty(item.service_provider_name) && !string.IsNullOrWhiteSpace(item.service_provider_name) ? item.category_name : AppResource.mybooking_Coordinator;
                                    item.IsRatingVisible = !string.IsNullOrEmpty(item.service_provider_name) && !string.IsNullOrWhiteSpace(item.service_provider_name) ? true : false;
                                    item.WorkerReviewText = item.user_rating.HasValue && item.user_rating.Value > 0.0 ? AppResource.mybooking_CheckReview : AppResource.mybooking_GiveReview;
                                    AllBookingList.Add(item);
                                }
                                //IsListVisible = true;
                                Application.Current.Properties["MyBookingData"] = JsonConvert.SerializeObject(AllBookingList);
                                Application.Current.SavePropertiesAsync();
                            }
                            else
                            {
                                IsNoDataFoundView = true;
                                IsNoInternetView = false;
                            }

                        }
                        else
                        {
                            await MaterialDialog.Instance.SnackbarAsync(message: response.message,
                                        msDuration: 3000);
                            if (Application.Current.Properties.ContainsKey("MyBookingData"))
                            {
                                OfflineData();
                            }
                            else
                            {
                                IsNoDataFoundView = true;
                                IsNoInternetView = false;
                            }
                        }
                    }
                    else
                    {
                        if (Application.Current.Properties.ContainsKey("MyBookingData"))
                        {
                            OfflineData();
                        }
                        else
                        {
                            IsNoDataFoundView = false;
                            IsNoInternetView = true;
                        }
                    }
                }
                else
                {
                    if (Application.Current.Properties.ContainsKey("MyBookingData"))
                    {
                        OfflineData();
                    }
                    else
                    {
                        IsNoDataFoundView = false;
                        IsNoInternetView = true;
                        return;
                    }
                }


                if (PendingTextColor == Color.FromHex(ColorHelpers.LightYellowColor))
                {
                    MyBookingList = new ObservableCollection<JobRequestData>(AllBookingList.Where(x => x.status == Convert.ToInt32(JobRequestEnum.Pending) || x.status == Convert.ToInt32(JobRequestEnum.Accepted)).ToList());
                }
                else if (InProgressTextColor == Color.FromHex(ColorHelpers.LightYellowColor))
                {
                    MyBookingList = new ObservableCollection<JobRequestData>(AllBookingList.Where(x => x.status == Convert.ToInt32(JobRequestEnum.InProgress) || x.status == Convert.ToInt32(JobRequestEnum.Assigned)).ToList());
                }
                else if (CompletedTextColor == Color.FromHex(ColorHelpers.LightYellowColor))
                {
                    MyBookingList = new ObservableCollection<JobRequestData>(AllBookingList.Where(x => x.status == Convert.ToInt32(JobRequestEnum.Completed)).ToList());
                }
                else if (CancelledTextColor == Color.FromHex(ColorHelpers.LightYellowColor))
                {
                    MyBookingList = new ObservableCollection<JobRequestData>(AllBookingList.Where(x => x.status == Convert.ToInt32(JobRequestEnum.Canceled) || x.status == Convert.ToInt32(JobRequestEnum.QuoteCanceled) || x.status == Convert.ToInt32(JobRequestEnum.Closed)).ToList());
                }

                if (MyBookingList.Count > 0)
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

        #region JobStatusCommand
        public Command JobStatusCommand
        {
            get
            {
                return new Command((e) =>
                {
                    switch (e.ToString().ToLower())
                    {
                        case "pending":
                            PendingTextColor = Color.FromHex(ColorHelpers.LightYellowColor);
                            InProgressTextColor = Color.FromHex(ColorHelpers.Black2Color);
                            CompletedTextColor = Color.FromHex(ColorHelpers.Black2Color);
                            CancelledTextColor = Color.FromHex(ColorHelpers.Black2Color);

                            IsJobPending = PendingTextColor == Color.FromHex(ColorHelpers.LightYellowColor) ? true : false;
                            IsJobInprogress = InProgressTextColor == Color.FromHex(ColorHelpers.LightYellowColor) ? true : false;
                            IsJobCompleted = CompletedTextColor == Color.FromHex(ColorHelpers.LightYellowColor) ? true : false;
                            IsJobCancelled = CancelledTextColor == Color.FromHex(ColorHelpers.LightYellowColor) ? true : false;

                            GetBookingByStatus();
                            break;
                        case "inprogress":
                            PendingTextColor = Color.FromHex(ColorHelpers.Black2Color);
                            InProgressTextColor = Color.FromHex(ColorHelpers.LightYellowColor);
                            CompletedTextColor = Color.FromHex(ColorHelpers.Black2Color);
                            CancelledTextColor = Color.FromHex(ColorHelpers.Black2Color);

                            IsJobPending = PendingTextColor == Color.FromHex(ColorHelpers.LightYellowColor) ? true : false;
                            IsJobInprogress = InProgressTextColor == Color.FromHex(ColorHelpers.LightYellowColor) ? true : false;
                            IsJobCompleted = CompletedTextColor == Color.FromHex(ColorHelpers.LightYellowColor) ? true : false;
                            IsJobCancelled = CancelledTextColor == Color.FromHex(ColorHelpers.LightYellowColor) ? true : false;

                            GetBookingByStatus();
                            break;
                        case "completed":
                            PendingTextColor = Color.FromHex(ColorHelpers.Black2Color);
                            InProgressTextColor = Color.FromHex(ColorHelpers.Black2Color);
                            CompletedTextColor = Color.FromHex(ColorHelpers.LightYellowColor);
                            CancelledTextColor = Color.FromHex(ColorHelpers.Black2Color);

                            IsJobPending = PendingTextColor == Color.FromHex(ColorHelpers.LightYellowColor) ? true : false;
                            IsJobInprogress = InProgressTextColor == Color.FromHex(ColorHelpers.LightYellowColor) ? true : false;
                            IsJobCompleted = CompletedTextColor == Color.FromHex(ColorHelpers.LightYellowColor) ? true : false;
                            IsJobCancelled = CancelledTextColor == Color.FromHex(ColorHelpers.LightYellowColor) ? true : false;

                            GetBookingByStatus();
                            break;
                        case "cancelled":
                            PendingTextColor = Color.FromHex(ColorHelpers.Black2Color);
                            InProgressTextColor = Color.FromHex(ColorHelpers.Black2Color);
                            CompletedTextColor = Color.FromHex(ColorHelpers.Black2Color);
                            CancelledTextColor = Color.FromHex(ColorHelpers.LightYellowColor);

                            IsJobPending = PendingTextColor == Color.FromHex(ColorHelpers.LightYellowColor) ? true : false;
                            IsJobInprogress = InProgressTextColor == Color.FromHex(ColorHelpers.LightYellowColor) ? true : false;
                            IsJobCompleted = CompletedTextColor == Color.FromHex(ColorHelpers.LightYellowColor) ? true : false;
                            IsJobCancelled = CancelledTextColor == Color.FromHex(ColorHelpers.LightYellowColor) ? true : false;

                            GetBookingByStatus();
                            break;
                    }
                });
            }
        }
        #endregion

        #region BookAgainCommand
        public Command BookAgainCommand
        {
            get
            {
                return new Command(async (e) =>
                {
                    var item = (JobRequestData)e;
                    try
                    {
                        if (Common.CheckConnection())
                        {
                            IsLoaderBusy = true;
                            JobRequestBookCancelResponseModel response;
                            try
                            {
                                response = await _webApiRestClient.GetAsync<JobRequestBookCancelResponseModel>(string.Format(ApiUrl.BookJobRequest, item.id, user_id));
                            }
                            catch (Exception ex)
                            {
                                response = null;
                                IsLoaderBusy = false;
                                await MaterialDialog.Instance.SnackbarAsync(message: ex.Message, msDuration: 3000);
                                return;
                            }
                            if (response != null)
                            {
                                if (response.status)
                                {
                                    await MaterialDialog.Instance.SnackbarAsync(message: response.message,
                                                msDuration: 3000);
                                    //MessagingCenter.Send("BookJob", "BookCancelJob");
                                    GetBookingData();
                                }
                                else
                                {
                                    await MaterialDialog.Instance.SnackbarAsync(message: response.message,
                                                msDuration: 3000);
                                }
                            }
                        }
                        else
                        {
                            await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_InternetError,
                                        msDuration: 3000);
                        }
                    }
                    catch (Exception ex)
                    {
                        await MaterialDialog.Instance.SnackbarAsync(message: ex.Message, msDuration: 3000);
                    }
                    finally
                    {
                        IsLoaderBusy = false;
                    }
                });
            }
        }
        #endregion

        #region JobCancelCommand
        public Command JobCancelCommand
        {
            get
            {
                return new Command((e) =>
                {
                    jobcancelclickdata = (JobRequestData)e;
                    IsPopupVisible = true; 
                    ServiceCancelMessage = string.Empty;
                });
            }
        }
        #endregion

        #region PopupCloseCommand
        public Command PopupCloseCommand
        {
            get
            {
                return new Command((e) =>
                {
                    jobcancelclickdata = null;
                    ServiceCancelMessage = string.Empty;
                    IsPopupVisible = false;
                });
            }
        }
        #endregion

        #region SubmitBtnCommand
        public Command SubmitBtnCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        if (Common.CheckConnection())
                        {
                            if (!string.IsNullOrEmpty(ServiceCancelMessage) && !string.IsNullOrWhiteSpace(ServiceCancelMessage))
                            {
                                IsLoaderBusy = true;
                                var request = new JobRequestCancelData
                                {
                                    cancel_reason = ServiceCancelMessage,
                                    job_request_id = jobcancelclickdata.id,
                                    user_id = user_id
                                };
                                JobRequestBookCancelResponseModel response;
                                try
                                {
                                    response = await _webApiRestClient.PostAsync<JobRequestCancelData, JobRequestBookCancelResponseModel>(ApiUrl.CancelJobRequest, request);
                                }
                                catch (Exception ex)
                                {
                                    response = null;
                                    IsLoaderBusy = false;
                                    await MaterialDialog.Instance.SnackbarAsync(message: ex.Message, msDuration: 3000);
                                    return;
                                }
                                if (response != null)
                                {
                                    if (response.status)
                                    {
                                        IsPopupVisible = false;
                                        ServiceCancelMessage = string.Empty;
                                        await MaterialDialog.Instance.SnackbarAsync(message: response.message,
                                                    msDuration: 3000);
                                        //MessagingCenter.Send("CancelJob", "BookCancelJob");
                                        GetBookingByStatus();
                                    }
                                    else
                                    {
                                        await MaterialDialog.Instance.SnackbarAsync(message: response.message,
                                                    msDuration: 3000);
                                    }
                                }
                            }
                            else
                            {
                                await MaterialDialog.Instance.SnackbarAsync(message: AppResource.spjob_popupErrorMsg,
                                            msDuration: 3000);
                            }
                        }
                        else
                        {
                            await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_InternetError,
                                                    msDuration: 3000);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    finally
                    {
                        IsLoaderBusy = false;
                    }
                });
            }
        }
        #endregion

        #region ReviewCommand
        public Command ReviewCommand
        {
            get
            {
                return new Command((e) =>
                {
                    var item = (JobRequestData)e;
                    //Common.CustomNavigation(_navigation, new ReviewPage(item));
                    var param = new NavigationParameters();
                    param.Add("UserReview", item);
                    NavigationService.NavigateAsync(nameof(ReviewPage), param);
                });
            }
        }
        #endregion

        #region ChatCommand
        public Command ChatCommand
        {
            get
            {
                return new Command(async(e) =>
                {
                    Device.BeginInvokeOnMainThread(async() =>
                    {

                        if (!IsChatOpen)
                        {
                            IsChatOpen = true;
                            var data = (JobRequestData)e;
                            try
                            {
                                if (data.IsJobCancelled)
                                {
                                    JobChatModel chatModel = new JobChatModel()
                                    {
                                        job_id = data.id,
                                        job_Name = "Order " + data.id.ToString(),
                                        coordinator_id = data.coordinator_id_one,
                                        coordinator_Name = data.coordinator_name_one,
                                        worker_id = data.service_provider_id,
                                        worker_Name = data.service_provider_name,
                                        customer_id = user_id,
                                        customer_Name = user_name
                                    };
                                    if (!string.IsNullOrEmpty(data.coordinator_name_one) || !string.IsNullOrEmpty(data.coordinator_name) || !string.IsNullOrEmpty(data.service_provider_name))
                                    {
                                        //Common.CustomNavigation(_navigation, new ChatDetailPage(data.coordinator_name_one, data.id));
                                        //Common.CustomNavigation(_navigation, new ChatDetailPage("Group " + data.id.ToString(), chatModel));
                                        var param = new NavigationParameters();
                                        param.Add("ChatDetailTitle", "Order " + data.id.ToString());
                                        param.Add("ChatDetailData", chatModel);
                                        await NavigationService.NavigateAsync(nameof(ChatDetailPage), param);
                                    }
                                }
                                else
                                {
                                    JobChatModel chatModel = new JobChatModel()
                                    {
                                        job_id = data.id,
                                        job_Name = "Order " + data.id.ToString(),
                                        coordinator_id = data.coordinator_id,
                                        coordinator_Name = data.coordinator_name,
                                        worker_id = data.service_provider_id,
                                        worker_Name = data.service_provider_name,
                                        customer_id = user_id,
                                        customer_Name = user_name
                                    };
                                    //Common.CustomNavigation(_navigation, new ChatDetailPage("Group " + data.id.ToString(), chatModel));

                                    var param = new NavigationParameters();
                                    param.Add("ChatDetailTitle", "Order " + data.id.ToString());
                                    param.Add("ChatDetailData", chatModel);
                                    try
                                    {
                                        await NavigationService.NavigateAsync(nameof(ChatDetailPage), param);
                                    }
                                    catch (Exception ex)
                                    {
                                    }
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                            IsChatOpen = false;
                        }

                    });
                });
            }
        }
        #endregion

        #region OnAppearing
        public void OnAppearing()
        {
            IsChatOpen = false;
            GetBookingData();
        }
        #endregion

        #region OnDisappearing
        public void OnDisappearing()
        {

        }
        #endregion

        #region BackIconCommand
        public ICommand BackIconCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    NavigationService.GoBackAsync();
                });
            }
        }
        #endregion
    }
}