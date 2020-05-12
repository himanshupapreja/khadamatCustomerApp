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
using System.Linq;
using System.Threading;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;
using static Khadamat_CustomerApp.Helpers.Enums;

namespace Khadamat_CustomerApp.ViewModels
{
    public class HomePageViewModel : BaseViewModel
    {
        public static INavigationService NavigationService;
        bool IsNotificationClick;
        bool IsChatClick;
        LoaderPopup LoaderPopup;

        public Timer notificationTimer;
        public Timer chatTimer;
        #region HeaderBanner
        private string _HeaderBanner;

        public string HeaderBanner
        {
            get { return _HeaderBanner; }
            set 
            { 
                SetProperty(ref _HeaderBanner, value);
            }
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

        #region HeaderBannerTitle
        private string _HeaderBannerTitle;

        public string HeaderBannerTitle
        {
            get { return _HeaderBannerTitle; }
            set 
            { 
                SetProperty(ref _HeaderBannerTitle, value);
            }
        }
        #endregion

        #region MenuIcon
        private string _MenuIcon;

        public string MenuIcon
        {
            get { return _MenuIcon; }
            set { SetProperty(ref _MenuIcon, value); }
        }
        #endregion

        #region NotificationIcon
        private string _NotificationIcon;

        public string NotificationIcon
        {
            get { return _NotificationIcon; }
            set { SetProperty(ref _NotificationIcon, value); }
        }
        #endregion

        #region ChatIcon
        private string _ChatIcon;

        public string ChatIcon
        {
            get { return _ChatIcon; }
            set { SetProperty(ref _ChatIcon, value); }
        }
        #endregion

        #region IsNodataFound
        private bool _IsNodataFound;
        public bool IsNodataFound
        {
            get { return _IsNodataFound; }
            set { SetProperty(ref _IsNodataFound, value); }
        }
        #endregion

        #region SearchServiceEntry
        private string _SearchServiceEntry;

        public string SearchServiceEntry
        {
            get { return _SearchServiceEntry; }
            set
            {
                SetProperty(ref _SearchServiceEntry, value);
                if (AllMainCategoryList != null && AllMainCategoryList.Count > 0)
                {
                    SearchMainCategoryList = new ObservableCollection<MainCategory>();
                    if (string.IsNullOrEmpty(SearchServiceEntry) || string.IsNullOrWhiteSpace(SearchServiceEntry))
                    {
                        MainCategoryList = AllMainCategoryList;
                        if (MainCategoryList.Count > 0)
                        {
                            IsNodataFound = false;
                        }
                        else
                        {
                            IsNodataFound = true;
                        }
                    }
                    else
                    {
                        foreach (var item in AllMainCategoryList)
                        {
                            //searchedCategory = item;
                            var searchinglist = item.Categories.ToList().FindAll(a => a.service_category_name.ToLower().StartsWith(SearchServiceEntry.ToLower())).ToList();
                            if (searchinglist != null && searchinglist.Count > 0)
                            {
                                searchedCategory = new MainCategory();
                                searchedCategory.main_category_name = item.main_category_name;
                                searchedCategory.main_category_id = item.main_category_id;
                                searchedCategory.LowerBanner = item.LowerBanner;
                                searchedCategory.IsImageFound = item.IsImageFound;
                                searchedCategory.is_active = item.is_active;
                                searchedCategory.created_date = item.created_date;
                                searchedCategory.Categories = searchinglist;

                                searchedCategory.ServiceListHeight = searchedCategory.Categories.Count % 2 == 0 ? ((searchedCategory.Categories.Count / 2) * 200 + (searchedCategory.Categories.Count / 2) * 2 * 5) : (((searchedCategory.Categories.Count / 2) + 1) * 200 + ((searchedCategory.Categories.Count / 2) + 1) * 2 * 5);

                                SearchMainCategoryList.Add(searchedCategory);

                            }
                        }
                        MainCategoryList = SearchMainCategoryList;
                        if (MainCategoryList.Count > 0)
                        {
                            IsNodataFound = false;
                        }
                        else
                        {
                            IsNodataFound = true;
                        }
                        //SearchMainCategoryList[0].Categories = aa;
                        //MainCategoryList = SearchMainCategoryList;
                        //MainCategoryList = new ObservableCollection<MainCategory>(SearchMainCategoryList.Contains(y => y.Categories.Where(x => x.service_category_name.ToLower().Contains(SearchServiceEntry.ToLower())).ToList()));
                    }
                }
            }
        }
        #endregion

        #region IsLoaderBusy Field
        private bool _IsLoaderBusy;

        public bool IsLoaderBusy
        {
            get { return _IsLoaderBusy; }
            set { SetProperty(ref _IsLoaderBusy, value); }
        }
        #endregion

        #region MainCategoryList
        public MainCategory searchedCategory = new MainCategory();
        public ObservableCollection<MainCategory> SearchMainCategoryList = new ObservableCollection<MainCategory>();
        public ObservableCollection<MainCategory> AllMainCategoryList = new ObservableCollection<MainCategory>();
        private ObservableCollection<MainCategory> _MainCategoryList = new ObservableCollection<MainCategory>();
        public ObservableCollection<MainCategory> MainCategoryList
        {
            get { return _MainCategoryList; }
            set { SetProperty(ref _MainCategoryList, value); }
        }
        #endregion

        #region Constructor
        public HomePageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
            //GetServiceData();
            ChatIcon = ImageHelpers.ChatIcon;
            NotificationIcon = ImageHelpers.NotificationIcon;
            MenuIcon = ImageHelpers.MenuIcon;
            IsNotificationClick = false;
            IsChatClick = false;

            //Device.BeginInvokeOnMainThread(() =>
            //{
            //    UpdateNotificationCount();
            //});
            Device.BeginInvokeOnMainThread(() =>
            {
                GetServiceData();
            });
        }
        #endregion

        #region RefreshCommand
        public Command RefreshCommand
        {
            get
            {
                return new Command(() =>
                {
                    SearchServiceEntry = string.Empty;
                    GetServiceData();
                });
            }
        }
        #endregion

        #region GetServiceData Api Method
        private async void GetServiceData()
        {
            try
            {
                if (Common.CheckConnection())
                {
                    IsLoaderBusy = true;
                    CategoryListResponseModel response;
                    try
                    {
                        response = await _webApiRestClient.GetAsync<CategoryListResponseModel>(string.Format(ApiUrl.GetCategories, province_id));
                    }
                    catch (Exception ex)
                    {
                        response = null;
                        IsLoaderBusy = false;
                        IsRefreshing = false;
                        //await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_ServerError, msDuration: 3000);
                        //return;
                    }
                    if (response != null)
                    {

                        if (response.status)
                        {
                            Application.Current.Properties["HomePageData"] = JsonConvert.SerializeObject(response.CategoryData);
                            Application.Current.SavePropertiesAsync();

                            HeaderBanner = Common.IsImagesValid(response.CategoryData.upper_banner_image, ApiUrl.BaseUrl);
                            HeaderBannerTitle = Common.GetLanguage() != "ar-AE" ? Common.FirstCharToUpper(response.CategoryData.upper_banner_title) : Common.FirstCharToUpper(response.CategoryData.upper_banner_title_arabic);
                            if (response.CategoryData.AppData != null && response.CategoryData.AppData.Count > 0)
                            {
                                AllMainCategoryList = new ObservableCollection<MainCategory>(response.CategoryData.AppData.Where(x => x.Categories != null && x.Categories.Count > 0).ToList());
                                foreach (var item in AllMainCategoryList)
                                {
                                    var index = AllMainCategoryList.IndexOf(item);
                                    AllMainCategoryList[index].ServiceListHeight = item.Categories.Count % 2 == 0 ? ((item.Categories.Count / 2) * 200 + (item.Categories.Count / 2) * 2 * 5) : (((item.Categories.Count / 2) + 1) * 200 + ((item.Categories.Count / 2) + 1) * 2 * 5);
                                    AllMainCategoryList[index].main_category_name = Common.GetLanguage() != "ar-AE" ? item.main_category_name : item.main_category_name_arabic;

                                    foreach (var categoryItem in item.Categories)
                                    {
                                        var categoryItemIndex = AllMainCategoryList[index].Categories.IndexOf(categoryItem);
                                        AllMainCategoryList[index].Categories[categoryItemIndex].service_category_name = Common.GetLanguage() != "ar-AE" ? categoryItem.service_category_name : categoryItem.service_category_name_arabic;
                                        AllMainCategoryList[index].Categories[categoryItemIndex].picture = Common.IsImagesValid(categoryItem.picture,ApiUrl.ServiceImageBaseUrl);
                                        AllMainCategoryList[index].Categories[categoryItemIndex].icon = Common.IsImagesValid(categoryItem.icon,ApiUrl.ServiceImageBaseUrl);
                                        AllMainCategoryList[index].Categories[categoryItemIndex].terms_conditions = Common.GetLanguage() != "ar-AE" ? categoryItem.terms_conditions : categoryItem.terms_conditions_arabic;
                                    }
                                }
                                AllMainCategoryList[0].LowerBanner = Common.IsImagesValid(response.CategoryData.lower_banner_image, ApiUrl.BaseUrl);
                                AllMainCategoryList[0].IsImageFound = true;

                                MainCategoryList = AllMainCategoryList;
                                IsNodataFound = false;
                            }
                            else
                            {
                                IsNodataFound = true;
                            }
                        }
                        else
                        {
                            await MaterialDialog.Instance.SnackbarAsync(message: response.message,
                                        msDuration: 3000);
                        }
                    }
                    else
                    {
                        if (Application.Current.Properties.ContainsKey("HomePageData") && !string.IsNullOrEmpty(Application.Current.Properties["HomePageData"].ToString()) && !string.IsNullOrWhiteSpace(Application.Current.Properties["HomePageData"].ToString()))
                        {
                            OfflineData();
                        }
                        else
                        {
                            IsNodataFound = true;
                        }
                    }
                }
                else
                {
                    if(Application.Current.Properties.ContainsKey("HomePageData") && !string.IsNullOrEmpty(Application.Current.Properties["HomePageData"].ToString()) && !string.IsNullOrWhiteSpace(Application.Current.Properties["HomePageData"].ToString()))
                    {
                        OfflineData();
                    }
                    else
                    {
                        await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_InternetError,
                                                msDuration: 3000);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("featching Category data ::-->> " + ex.Message);
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
            var responseCategoryData = JsonConvert.DeserializeObject<CategoryData>(Application.Current.Properties["HomePageData"].ToString());

            HeaderBanner = Common.IsImagesValid(responseCategoryData.upper_banner_image, ApiUrl.BaseUrl);
            HeaderBannerTitle = Common.GetLanguage() != "ar-AE" ? Common.FirstCharToUpper(responseCategoryData.upper_banner_title) : Common.FirstCharToUpper(responseCategoryData.upper_banner_title_arabic);
            if (responseCategoryData.AppData != null && responseCategoryData.AppData.Count > 0)
            {
                AllMainCategoryList = new ObservableCollection<MainCategory>(responseCategoryData.AppData.Where(x => x.Categories != null && x.Categories.Count > 0).ToList());
                foreach (var item in AllMainCategoryList)
                {
                    var index = AllMainCategoryList.IndexOf(item);
                    AllMainCategoryList[index].ServiceListHeight = item.Categories.Count % 2 == 0 ? ((item.Categories.Count / 2) * 200 + (item.Categories.Count / 2) * 2 * 5) : (((item.Categories.Count / 2) + 1) * 200 + ((item.Categories.Count / 2) + 1) * 2 * 5);
                    AllMainCategoryList[index].main_category_name = Common.GetLanguage() != "ar-AE" ? item.main_category_name : item.main_category_name_arabic;

                    foreach (var categoryItem in item.Categories)
                    {
                        var categoryItemIndex = AllMainCategoryList[index].Categories.IndexOf(categoryItem);
                        AllMainCategoryList[index].Categories[categoryItemIndex].service_category_name = Common.GetLanguage() != "ar-AE" ? categoryItem.service_category_name : categoryItem.service_category_name_arabic;
                        AllMainCategoryList[index].Categories[categoryItemIndex].picture = Common.IsImagesValid(categoryItem.picture, ApiUrl.ServiceImageBaseUrl);
                        AllMainCategoryList[index].Categories[categoryItemIndex].icon = Common.IsImagesValid(categoryItem.icon, ApiUrl.ServiceImageBaseUrl);
                        AllMainCategoryList[index].Categories[categoryItemIndex].terms_conditions = Common.GetLanguage() != "ar-AE" ? categoryItem.terms_conditions : categoryItem.terms_conditions_arabic;
                    }
                }
                AllMainCategoryList[0].LowerBanner = Common.IsImagesValid(responseCategoryData.lower_banner_image, ApiUrl.BaseUrl);
                AllMainCategoryList[0].IsImageFound = true;

                MainCategoryList = AllMainCategoryList;
                IsNodataFound = false;
            }
            else
            {
                IsNodataFound = true;
            }
        } 
        #endregion

        #region Right1IconCommand
        public Command Right1IconCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        if (!IsChatClick)
                        {
                            IsChatClick = true;
                            try
                            {
                                //LoaderPopup = new LoaderPopup();
                                //await App.Current.MainPage.Navigation.PushPopupAsync(LoaderPopup);
                                IsLoaderBusy = true;
                                //Common.CustomNavigation(_navigation, new ChatListPage());
                                await NavigationService.NavigateAsync(nameof(ChatListPage));
                            }
                            catch (Exception ex)
                            {

                            }
                            finally
                            {
                                IsChatClick = false;
                                IsLoaderBusy = false;
                                //LoaderPopup.ClosePopup();
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                });
            }
        }
        #endregion

        #region RightIconCommand
        public Command RightIconCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (!IsNotificationClick)
                    {
                        IsNotificationClick = true;
                        try
                        {
                            //LoaderPopup = new LoaderPopup();
                            //await App.Current.MainPage.Navigation.PushPopupAsync(LoaderPopup);
                            IsLoaderBusy = true;
                            //Common.CustomNavigation(_navigation, new NotificationPage());
                            await NavigationService.NavigateAsync(nameof(NotificationPage));
                        }
                        catch (Exception ex)
                        {

                        }
                        finally
                        {
                            IsNotificationClick = false;
                            IsLoaderBusy = false;
                            //LoaderPopup.ClosePopup();
                        } 
                    }
                });
            }
        }
        #endregion

        #region UpdateNotificationCount

        private async void UpdateNotificationCount()
        {
            if (Common.CheckConnection())
            {
                NotificationCountResponseModel response;
                try
                {
                    response = await _webApiRestClient.GetAsync<NotificationCountResponseModel>(string.Format(ApiUrl.GetNotificationUnreadCount, user_id));
                }
                catch (Exception ex)
                {
                    response = null;
                    return;
                }
                if (response != null)
                {
                    if (response.status)
                    {
                        if (response.notificationUnreadCount.HasValue && response.notificationUnreadCount.Value > 0)
                        {
                            NotificationIcon = ImageHelpers.NewNotificationIcon;
                        }
                        else
                        {
                            NotificationIcon = ImageHelpers.NotificationIcon;
                        }
                    }
                } 
            }
        }
        #endregion

        #region GetChat
        private async void GetChat()
        {
            try
            {
                if (Common.CheckConnection())
                {
                    ChatUserResponseModel response;
                    try
                    {
                        response = await _webApiRestClient.GetAsync<ChatUserResponseModel>(string.Format(ApiUrl.GetChat, user_id, Enums.UserTypeEnum.Customer.GetHashCode()));
                    }
                    catch (Exception ex)
                    {
                        response = null;
                        return;
                    }
                    if (response != null)
                    {
                        if (response.status)
                        {
                            var supportChatData = response.data.Where(x => x.user_type == UserTypeEnum.Admin.GetHashCode() && x.un_read == true).ToList();
                            if (supportChatData != null && supportChatData.Count > 0)
                            {
                                MessagingCenter.Send(true.ToString(), "NewSupportChat");
                                MenuIcon = ImageHelpers.NewMenuIcon;
                            }
                            else
                            {
                                MessagingCenter.Send(false.ToString(), "NewSupportChat");
                                MenuIcon = ImageHelpers.MenuIcon;
                            }
                            var chatData = response.data.Where(x => x.user_type != UserTypeEnum.Admin.GetHashCode() && x.un_read == true).ToList();
                            if (chatData != null && chatData.Count > 0)
                            {
                                ChatIcon = ImageHelpers.NewChatIcon;
                            }
                            else
                            {
                                ChatIcon = ImageHelpers.ChatIcon;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
            }
        }
        #endregion

        #region Appearing View Method
        public void OnAppearing()
        {
            Xamarin.Essentials.Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            Device.BeginInvokeOnMainThread(() =>
            {
                notificationTimer = new Timer(_ => UpdateNotificationCount(), null, 0, 10000);
            });
            Device.BeginInvokeOnMainThread(() =>
            {
                chatTimer = new Timer(_ => GetChat(), null, 0, 10000);
            });
            Device.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    app.UpdateDeviceInfo();
                }
                catch (Exception ex)
                {
                }
            });
            Device.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    app.GetCountriesApi();
                }
                catch (Exception ex)
                {
                }
            });
            Device.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    var request = new ChangeLanguagesModel();
                    if (Application.Current.Properties.ContainsKey("AppLocale") && (Application.Current.Properties["AppLocale"].ToString()).Contains("en"))
                    {
                        request.language = "en";
                        request.user_id = BaseViewModel.user_id;
                    }
                    else
                    {
                        request.language = "ar";
                        request.user_id = BaseViewModel.user_id;
                    }

                    app.UpdateLanguageServer(request);
                }
                catch (Exception ex)
                {
                }
            });
        }
        #endregion

        #region Disappearing View Method
        public void OnDisappearing()
        {
            Xamarin.Essentials.Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
            if (notificationTimer != null)
            {
                notificationTimer.Dispose();
            }
            if (chatTimer != null)
            {
                chatTimer.Dispose();
            }
        }
        #endregion

        private void Connectivity_ConnectivityChanged(object sender, Xamarin.Essentials.ConnectivityChangedEventArgs e)
        {
            if ((e.ConnectionProfiles.Contains(Xamarin.Essentials.ConnectionProfile.WiFi) || e.ConnectionProfiles.Contains(Xamarin.Essentials.ConnectionProfile.Cellular)) && e.NetworkAccess == Xamarin.Essentials.NetworkAccess.Internet)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    app.UpdateDeviceInfo();
                });

                Device.BeginInvokeOnMainThread(() =>
                {
                    var request = new ChangeLanguagesModel();
                    if (Application.Current.Properties.ContainsKey("AppLocale") && (Application.Current.Properties["AppLocale"].ToString()).Contains("en"))
                    {
                        request.language = "en";
                        request.user_id = BaseViewModel.user_id;
                    }
                    else
                    {
                        request.language = "ar";
                        request.user_id = BaseViewModel.user_id;
                    }

                    app.UpdateLanguageServer(request);
                });

                Device.BeginInvokeOnMainThread(() =>
                {
                    app.GetCountriesApi();
                });
            }
        }
    }
}
