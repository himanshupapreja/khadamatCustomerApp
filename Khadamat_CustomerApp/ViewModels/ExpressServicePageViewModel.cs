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
namespace Khadamat_CustomerApp.ViewModels
{

    public class ExpressServicePageViewModel : BaseViewModel
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
            set { SetProperty(ref _HeaderBanner, value); }
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
            set { SetProperty(ref _HeaderBannerTitle, value); }
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


        public double _ServiceListHeight;
        //[JsonIgnore]
        public double ServiceListHeight
        {
            get { return _ServiceListHeight; }
            set { SetProperty(ref _ServiceListHeight, value); }
        }

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

        #region ExpressCategories
        public CategoryListModel searchedCategory = new CategoryListModel();
        public ObservableCollection<ExpressData2> SearchCategoriesList = new ObservableCollection<ExpressData2>();
        public ObservableCollection<ExpressData2> AllCategoriesList = new ObservableCollection<ExpressData2>();
        private ObservableCollection<ExpressData2> _Categories = new ObservableCollection<ExpressData2>();
        public ObservableCollection<ExpressData2> Categories
        {
            get { return _Categories; }
            set { SetProperty(ref _Categories, value); }
        }
        #endregion

        #region Constructor
        public ExpressServicePageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
            //GetExpressServiceData();
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
                GetExpressServiceData();
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
                    GetExpressServiceData();
                });
            }
        }
        #endregion

        #region GetServiceData Api Method
        private async void GetExpressServiceData()
        {
            try
            {
                if (Common.CheckConnection())
                {
                    IsLoaderBusy = true;
                    ExpressDataResponseModel response;
                    try
                    {
                        response = await _webApiRestClient.GetAsync<ExpressDataResponseModel>(ApiUrl.GetExpressList);
                    }
                    catch (Exception ex)
                    {
                        response = null;
                        IsLoaderBusy = false;
                        IsRefreshing = false;
                        ////await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_ServerError, msDuration: 3000);
                        //return;
                    }
                    if (response != null)
                    {

                        if (response.status)
                        {
                            Application.Current.Properties["ExpressPageData"] = JsonConvert.SerializeObject(response);
                            Application.Current.SavePropertiesAsync();

                            HeaderBanner = Common.IsImagesValid(response.ExpressData.upper_banner_image, ApiUrl.BaseUrl);
                            HeaderBannerTitle = Common.GetLanguage() != "ar-AE" ? Common.FirstCharToUpper(response.ExpressData.upper_banner_title) : Common.FirstCharToUpper(response.ExpressData.upper_banner_title_arabic);
                            if (response.ExpressData.ExpressData != null && response.ExpressData.ExpressData.Count > 0)
                            {
                                AllCategoriesList = new ObservableCollection<ExpressData2>(response.ExpressData.ExpressData);
                                foreach (var item in AllCategoriesList)
                                {
                                    var index = AllCategoriesList.IndexOf(item);
                                    AllCategoriesList[index].service_category_name = Common.GetLanguage() != "ar-AE" ? item.title : item.title_arabic;
                                    AllCategoriesList[index].IsEnglishView = Common.GetLanguage() != "ar-AE" ? true : false;
                                    AllCategoriesList[index].picture = Common.IsImagesValid(item.picture, ApiUrl.ServiceImageBaseUrl);
                                    AllCategoriesList[index].icon = Common.IsImagesValid(item.icon, ApiUrl.ServiceImageBaseUrl);
                                }


                                Categories = AllCategoriesList;

                                ServiceListHeight = Categories.Count % 2 == 0 ? ((Categories.Count / 2) * 210 + (Categories.Count / 2) * 2 * 5) : (((Categories.Count / 2) + 1) * 210 + ((Categories.Count / 2) + 1) * 2 * 5);
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
                        if (Application.Current.Properties.ContainsKey("ExpressPageData") && !string.IsNullOrEmpty(Application.Current.Properties["ExpressPageData"].ToString()) && !string.IsNullOrWhiteSpace(Application.Current.Properties["ExpressPageData"].ToString()))
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
                    if (Application.Current.Properties.ContainsKey("ExpressPageData") && !string.IsNullOrEmpty(Application.Current.Properties["ExpressPageData"].ToString()) && !string.IsNullOrWhiteSpace(Application.Current.Properties["ExpressPageData"].ToString()))
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
            //try
            //{
            //    if (Common.CheckConnection())
            //    {
            //        IsLoaderBusy = true;
            //        CategoryListResponseModel response;
            //        try
            //        {
            //            response = await _webApiRestClient.GetAsync<CategoryListResponseModel>(string.Format(ApiUrl.GetCategories, user_id));
            //        }
            //        catch (Exception ex)
            //        {
            //            response = null;
            //            IsLoaderBusy = false;
            //            IsRefreshing = false;
            //            ////await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_ServerError, msDuration: 3000);
            //            //return;
            //        }
            //        if (response != null)
            //        {

            //            if (response.status)
            //            {
            //                Application.Current.Properties["ExpressPageData"] = JsonConvert.SerializeObject(response.CategoryData);
            //                Application.Current.SavePropertiesAsync();

            //                HeaderBanner = Common.IsImagesValid(response.CategoryData.upper_banner_image, ApiUrl.BaseUrl);
            //                HeaderBannerTitle = Common.GetLanguage() != "ar-AE" ? Common.FirstCharToUpper(response.CategoryData.upper_banner_title) : Common.FirstCharToUpper(response.CategoryData.upper_banner_title_arabic);
            //                if (response.CategoryData.AppData != null && response.CategoryData.AppData.Count > 0)
            //                {
            //                    AllMainCategoryList = new ObservableCollection<MainCategory>(response.CategoryData.AppData.Where(x => x.Categories != null && x.Categories.Count > 0).ToList());
            //                    foreach (var item in AllMainCategoryList)
            //                    {
            //                        var index = AllMainCategoryList.IndexOf(item);
            //                        AllMainCategoryList[index].ServiceListHeight = item.Categories.Count % 2 == 0 ? ((item.Categories.Count / 2) * 200 + (item.Categories.Count / 2) * 2 * 5) : (((item.Categories.Count / 2) + 1) * 200 + ((item.Categories.Count / 2) + 1) * 2 * 5);
            //                        AllMainCategoryList[index].main_category_name = Common.GetLanguage() != "ar-AE" ? item.main_category_name : item.main_category_name_arabic;

            //                        foreach (var categoryItem in item.Categories)
            //                        {
            //                            var categoryItemIndex = AllMainCategoryList[index].Categories.IndexOf(categoryItem);
            //                            AllMainCategoryList[index].Categories[categoryItemIndex].service_category_name = Common.GetLanguage() != "ar-AE" ? categoryItem.service_category_name : categoryItem.service_category_name_arabic;
            //                            AllMainCategoryList[index].Categories[categoryItemIndex].terms_conditions = Common.GetLanguage() != "ar-AE" ? categoryItem.terms_conditions : categoryItem.terms_conditions_arabic;
            //                        }
            //                    }
            //                    AllMainCategoryList[0].LowerBanner = Common.IsImagesValid(response.CategoryData.lower_banner_image, ApiUrl.BaseUrl);
            //                    AllMainCategoryList[0].IsImageFound = true;

            //                    MainCategoryList = AllMainCategoryList;
            //                    IsNodataFound = false;
            //                }
            //                else
            //                {
            //                    IsNodataFound = true;
            //                }
            //            }
            //            else
            //            {
            //                await MaterialDialog.Instance.SnackbarAsync(message: response.message,
            //                            msDuration: 3000);
            //            }
            //        }
            //        else
            //        {
            //            if (Application.Current.Properties.ContainsKey("HomePageData") && !string.IsNullOrEmpty(Application.Current.Properties["HomePageData"].ToString()) && !string.IsNullOrWhiteSpace(Application.Current.Properties["HomePageData"].ToString()))
            //            {
            //                OfflineData();
            //            }
            //            else
            //            {
            //                IsNodataFound = true;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        if (Application.Current.Properties.ContainsKey("HomePageData") && !string.IsNullOrEmpty(Application.Current.Properties["HomePageData"].ToString()) && !string.IsNullOrWhiteSpace(Application.Current.Properties["HomePageData"].ToString()))
            //        {
            //            OfflineData();
            //        }
            //        else
            //        {
            //            await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_InternetError,
            //                                    msDuration: 3000);
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    System.Diagnostics.Debug.WriteLine("featching Category data ::-->> " + ex.Message);
            //}
            //finally
            //{
            //    IsLoaderBusy = false;
            //    IsRefreshing = false;
            //}
        }

        private void OfflineData()
        {
            var response = JsonConvert.DeserializeObject<ExpressDataResponseModel>(Application.Current.Properties["ExpressPageData"].ToString());
            if (response.ExpressData != null && response.ExpressData.ExpressData != null && response.ExpressData.ExpressData.Count > 0)
            {
                AllCategoriesList = new ObservableCollection<ExpressData2>(response.ExpressData.ExpressData);
                foreach (var item in AllCategoriesList)
                {
                    var index = AllCategoriesList.IndexOf(item);
                    AllCategoriesList[index].service_category_name = Common.GetLanguage() != "ar-AE" ? item.title : item.title_arabic;
                    AllCategoriesList[index].IsEnglishView = Common.GetLanguage() != "ar-AE" ? true : false;
                    AllCategoriesList[index].picture = Common.IsImagesValid(item.picture, ApiUrl.ServiceImageBaseUrl);
                    AllCategoriesList[index].icon = Common.IsImagesValid(item.icon, ApiUrl.ServiceImageBaseUrl);
                }


                Categories = AllCategoriesList;

                ServiceListHeight = Categories.Count % 2 == 0 ? ((Categories.Count / 2) * 210 + (Categories.Count / 2) * 2 * 5) : (((Categories.Count / 2) + 1) * 210 + ((Categories.Count / 2) + 1) * 2 * 5);
                IsNodataFound = false;
            }
            else
            {
                IsNodataFound = true;
            }
        }
        #endregion

        //#region DataWhennoInternet
        //private void OfflineData()
        //{
        //    var responseCategoryData = JsonConvert.DeserializeObject<CategoryData>(Application.Current.Properties["HomePageData"].ToString());

        //    HeaderBanner = Common.IsImagesValid(responseCategoryData.upper_banner_image, ApiUrl.BaseUrl);
        //    HeaderBannerTitle = Common.GetLanguage() != "ar-AE" ? Common.FirstCharToUpper(responseCategoryData.upper_banner_title) : Common.FirstCharToUpper(responseCategoryData.upper_banner_title_arabic);
        //    if (responseCategoryData.AppData != null && responseCategoryData.AppData.Count > 0)
        //    {
        //        AllMainCategoryList = new ObservableCollection<MainCategory>(responseCategoryData.AppData.Where(x => x.Categories != null && x.Categories.Count > 0).ToList());
        //        foreach (var item in AllMainCategoryList)
        //        {
        //            var index = AllMainCategoryList.IndexOf(item);
        //            AllMainCategoryList[index].ServiceListHeight = item.Categories.Count % 2 == 0 ? ((item.Categories.Count / 2) * 200 + (item.Categories.Count / 2) * 2 * 5) : (((item.Categories.Count / 2) + 1) * 200 + ((item.Categories.Count / 2) + 1) * 2 * 5);
        //            AllMainCategoryList[index].main_category_name = Common.GetLanguage() != "ar-AE" ? item.main_category_name : item.main_category_name_arabic;

        //            foreach (var categoryItem in item.Categories)
        //            {
        //                var categoryItemIndex = AllMainCategoryList[index].Categories.IndexOf(categoryItem);
        //                AllMainCategoryList[index].Categories[categoryItemIndex].service_category_name = Common.GetLanguage() != "ar-AE" ? categoryItem.service_category_name : categoryItem.service_category_name_arabic;
        //            }
        //        }
        //        AllMainCategoryList[0].LowerBanner = Common.IsImagesValid(responseCategoryData.lower_banner_image, ApiUrl.BaseUrl);
        //        AllMainCategoryList[0].IsImageFound = true;

        //        MainCategoryList = AllMainCategoryList;
        //        IsNodataFound = false;
        //    }
        //    else
        //    {
        //        IsNodataFound = true;
        //    }
        //}
        //#endregion

        #region Right1IconCommand
        public Command Right1IconCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (!IsChatClick)
                    {
                        IsChatClick = true;
                        try
                        {
                            LoaderPopup = new LoaderPopup();
                            await App.Current.MainPage.Navigation.PushPopupAsync(LoaderPopup);
                            //Common.CustomNavigation(_navigation, new ChatListPage());
                            await NavigationService.NavigateAsync(nameof(ChatListPage));
                        }
                        catch (Exception ex)
                        {

                        }
                        finally
                        {
                            IsChatClick = false;
                            LoaderPopup.ClosePopup();
                        }
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
                            LoaderPopup = new LoaderPopup();
                            await App.Current.MainPage.Navigation.PushPopupAsync(LoaderPopup);
                            //Common.CustomNavigation(_navigation, new NotificationPage());
                            await NavigationService.NavigateAsync(nameof(NotificationPage));
                        }
                        catch (Exception ex)
                        {

                        }
                        finally
                        {
                            IsNotificationClick = false;
                            LoaderPopup.ClosePopup();
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
                            var supportChatData = response.data.Where(x => x.user_id == 5 && x.un_read == true).ToList();
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
                            var chatData = response.data.Where(x => x.user_id != 5 && x.un_read == true).ToList();
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
            Device.BeginInvokeOnMainThread(() =>
            {
                notificationTimer = new Timer(_ => UpdateNotificationCount(), null, 0, 10000);
            });
            Device.BeginInvokeOnMainThread(() =>
            {
                chatTimer = new Timer(_ => GetChat(), null, 0, 10000);
            });
        }
        #endregion

        #region Disappearing View Method
        public void OnDisappearing()
        {
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
    }
}
