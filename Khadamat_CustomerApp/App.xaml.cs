using Prism;
using Prism.Ioc;
using Khadamat_CustomerApp.ViewModels;
using Khadamat_CustomerApp.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Khadamat_CustomerApp.Services.ApiService;
using Khadamat_CustomerApp.Services.DBService.LiteDB.ModelDB;
using Khadamat_CustomerApp.Models;
using Khadamat_CustomerApp.Helpers;
using System.Linq;
using Khadamat_CustomerApp.Resources;
using System;
using XF.Material.Forms.UI.Dialogs;
using System.Globalization;
using Plugin.FirebasePushNotification;
using Newtonsoft.Json;
using System.Collections.Generic;
using LiteDB;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Khadamat_CustomerApp
{
    public partial class App
    {
        WebApiRestClient _webApiRestClient;
        public UserDataDbService userDataDbService;
        UserModel userdataindb;
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();
            _webApiRestClient = new WebApiRestClient();
            XF.Material.Forms.Material.Init(this);
            userDataDbService = new UserDataDbService();

            //Xamarin.Essentials.Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;

            try
            {
                if (Application.Current.Properties.ContainsKey("AppLocale") && !string.IsNullOrEmpty(Application.Current.Properties["AppLocale"].ToString()))
                {
                    var languageculture = Application.Current.Properties["AppLocale"].ToString();
                    Setlanguage(languageculture);
                    Application.Current.Properties["IsAppAlreadyInstalled"] = true;
                    Application.Current.SavePropertiesAsync();
                }
                else
                {
                    Application.Current.Properties["AppLocale"] = "ar-AE";
                    Setlanguage("ar-AE");
                    Application.Current.Properties["IsAppAlreadyInstalled"] = true;
                    Application.Current.SavePropertiesAsync();
                }
            }
            catch (Exception ex)
            {

            }


            //await NavigationService.NavigateAsync("NavigationPage/LoginPage");

            //if (Application.Current.Properties.ContainsKey("CountryList"))
            //{
            //    BaseViewModel.countryDataModels = JsonConvert.DeserializeObject<List<CountryDataModel>>(Application.Current.Properties["CountryList"].ToString());
            //}
            //if (Application.Current.Properties.ContainsKey("ProvinceList"))
            //{
            //    BaseViewModel.provienceDataModels = JsonConvert.DeserializeObject<List<ProvienceDataModel>>(Application.Current.Properties["ProvinceList"].ToString());
            //}

            if (userDataDbService.IsUserDbPresentInDB())
            {
                userdataindb = userDataDbService.ReadAllItems().FirstOrDefault();
                BaseViewModel.user_id = userdataindb.user_id;
                BaseViewModel.province_id = userdataindb.province.Value;
                BaseViewModel.user_name = userdataindb.name;
                BaseViewModel.user_pic = Common.IsImagesValid(userdataindb.profile_pic, ApiUrl.BaseUrl);
                BaseViewModel.email_verified = userdataindb.email_verified.HasValue ? userdataindb.email_verified.Value : false;

                Device.BeginInvokeOnMainThread(async() =>
                {
                    await NavigationService.NavigateAsync(new Uri("/MasterPage/NavigationPage/HomePage", UriKind.Absolute));
                });
            }
            else
            {
                await NavigationService.NavigateAsync("NavigationPage/LoginPage");
            }

            //GetCountriesApi();


            BaseViewModel.app = this;

            if (Application.Current.Properties.ContainsKey("CountryList"))
            {
                BaseViewModel.countryDataModels = JsonConvert.DeserializeObject<List<CountryDataModel>>(Application.Current.Properties["CountryList"].ToString());
            }
            if (Application.Current.Properties.ContainsKey("ProvinceList"))
            {
                BaseViewModel.provienceDataModels = JsonConvert.DeserializeObject<List<ProvienceDataModel>>(Application.Current.Properties["ProvinceList"].ToString());
            }

            //if (Device.RuntimePlatform == Device.Android || Device.RuntimePlatform == Device.iOS)
            //{

            //    await CrossFirebasePushNotification.Current.RegisterForPushNotifications();

            //    CrossFirebasePushNotification.Current.Subscribe("general");
            //    CrossFirebasePushNotification.Current.OnTokenRefresh += (s, p) =>
            //    {
            //        System.Diagnostics.Debug.WriteLine($"TOKEN REC: {p.Token}");
            //        Application.Current.Properties["AppFirebaseToken"] = p.Token;
            //        Application.Current.SavePropertiesAsync();

            //    };
            //    System.Diagnostics.Debug.WriteLine($"TOKEN: {CrossFirebasePushNotification.Current.Token}");

            //    CrossFirebasePushNotification.Current.OnNotificationReceived += (s, p) =>
            //    {
            //        try
            //        {
            //            if (Device.RuntimePlatform == Device.iOS)
            //            {
            //                if (p.Data.ContainsKey("aps.alert"))
            //                {
            //                    Device.BeginInvokeOnMainThread(async () =>
            //                    {
            //                        var data = $"{p.Data["aps.alert"]}";
            //                        if (data.Contains("Your password has been reset by the admin, In order to continue please contact administrator.") || data.Contains("تمت إعادة تعيين كلمة المرور الخاصة بك من قبل المشرف ، للمتابعة ، يرجى الاتصال بالمسؤول") || data.Contains("Your account has been de-activated by the admin, in order to continue please contact administrator") || data.Contains("تم إلغاء تنشيط حسابك من قبل المشرف ، للمتابعة ، يرجى الاتصال بالمسؤول"))
            //                        {
            //                            if (userDataDbService.IsUserDbPresentInDB())
            //                            {
            //                                var item = userDataDbService.ReadAllItems().FirstOrDefault();
            //                                BsonValue id = item.ID;
            //                                userDataDbService.DeleteItemFromDB(id, item);

            //                                //App.Current.MainPage = new NavigationPage(new LoginPage());
            //                                await NavigationService.NavigateAsync(new Uri("/NavigationPage/LoginPage", UriKind.Absolute));
            //                            }
            //                        }
            //                    });

            //                }
            //            }
            //            else if (Device.RuntimePlatform == Device.Android)
            //            {
            //                if (p.Data.ContainsKey("body"))
            //                {
            //                    Device.BeginInvokeOnMainThread(async () =>
            //                    {
            //                        var data = $"{p.Data["body"]}";

            //                        if (data.Contains("Your password has been reset by the admin, In order to continue please contact administrator.") || data.Contains("تمت إعادة تعيين كلمة المرور الخاصة بك من قبل المشرف ، للمتابعة ، يرجى الاتصال بالمسؤول") || data.Contains("Your account has been de-activated by the admin, in order to continue please contact administrator") || data.Contains("تم إلغاء تنشيط حسابك من قبل المشرف ، للمتابعة ، يرجى الاتصال بالمسؤول"))
            //                        {
            //                            if (userDataDbService.IsUserDbPresentInDB())
            //                            {
            //                                var item = userDataDbService.ReadAllItems().FirstOrDefault();
            //                                BsonValue id = item.ID;
            //                                userDataDbService.DeleteItemFromDB(id, item);

            //                                //App.Current.MainPage = new NavigationPage(new LoginPage());
            //                                await NavigationService.NavigateAsync(new Uri("/NavigationPage/LoginPage", UriKind.Absolute));
            //                            }
            //                        }
            //                    });

            //                }
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            // Some other exception occurred
            //        }

            //    };

            //    CrossFirebasePushNotification.Current.OnNotificationOpened += async (s, p) =>
            //    {
            //        //System.Diagnostics.Debug.WriteLine(p.Identifier);

            //        try
            //        {
            //            if (Device.RuntimePlatform == Device.iOS)
            //            {
            //                if (p.Data.ContainsKey("aps.alert"))
            //                {
            //                    Device.BeginInvokeOnMainThread(async () =>
            //                    {
            //                        var data = $"{p.Data["aps.alert"]}";
            //                        if (data.Contains("You have new message regarding your job request") || data.Contains("لديك رسالة جديدة بخصوص طلب عملك.") || data.Contains("You have new message") || data.Contains("لديك رسالة جديدة"))
            //                        {
            //                            await NavigationService.NavigateAsync(new Uri("/MasterPage/NavigationPage/HomePage", UriKind.Absolute));
            //                            await NavigationService.NavigateAsync(nameof(ChatListPage));
            //                            //App.Current.MainPage = new NavigationPage(new MasterPage());
            //                            //App.Current.MainPage.Navigation.PushAsync(new ChatListPage());
            //                        }
            //                        else if (data.Contains("You have new message from support team") || data.Contains("لديك رسالة جديدة من فريق الدعم"))
            //                        {
            //                            await NavigationService.NavigateAsync(new Uri("/MasterPage/NavigationPage/HomePage", UriKind.Absolute));
            //                            await NavigationService.NavigateAsync(nameof(SupportPage));
            //                        }
            //                        else if (data.Contains("Your password has been reset by the admin, In order to continue please contact administrator.") || data.Contains("تمت إعادة تعيين كلمة المرور الخاصة بك من قبل المشرف ، للمتابعة ، يرجى الاتصال بالمسؤول") || data.Contains("Your account has been de-activated by the admin, in order to continue please contact administrator") || data.Contains("تم إلغاء تنشيط حسابك من قبل المشرف ، للمتابعة ، يرجى الاتصال بالمسؤول"))
            //                        {
            //                            if (userDataDbService.IsUserDbPresentInDB())
            //                            {
            //                                var item = userDataDbService.ReadAllItems().FirstOrDefault();
            //                                BsonValue id = item.ID;
            //                                userDataDbService.DeleteItemFromDB(id, item);

            //                                //App.Current.MainPage = new NavigationPage(new LoginPage());
            //                                await NavigationService.NavigateAsync(new Uri("/NavigationPage/LoginPage", UriKind.Absolute));
            //                            }
            //                        }
            //                        else
            //                        {
            //                            await NavigationService.NavigateAsync(new Uri("/MasterPage/NavigationPage/HomePage", UriKind.Absolute));
            //                            await NavigationService.NavigateAsync(nameof(NotificationPage));
            //                            //if (Application.Current.Properties.ContainsKey("AppStatus"))
            //                            //{
            //                            //if (Convert.ToInt32(Application.Current.Properties["AppStatus"]) == Convert.ToInt32(AppStatus.Foreground))
            //                            //{
            //                            //    App.Current.MainPage.Navigation.PushAsync(new NotificationPage());
            //                            //}
            //                            //else if (Convert.ToInt32(Application.Current.Properties["AppStatus"]) == Convert.ToInt32(AppStatus.Background))
            //                            //{
            //                            //    App.Current.MainPage = new NavigationPage(new MasterPage());
            //                            //    App.Current.MainPage.Navigation.PushAsync(new NotificationPage());
            //                            //}
            //                            //}
            //                        }
            //                    });

            //                }
            //            }
            //            else if (Device.RuntimePlatform == Device.Android)
            //            {
            //                if (p.Data.ContainsKey("body"))
            //                {
            //                    Device.BeginInvokeOnMainThread(async () =>
            //                    {
            //                        var data = $"{p.Data["body"]}";

            //                        if (data.Contains("You have new message regarding your job request") || data.Contains("لديك رسالة جديدة بخصوص طلب عملك.") || data.Contains("You have new message") || data.Contains("لديك رسالة جديدة"))
            //                        {
            //                            await NavigationService.NavigateAsync(new Uri("/MasterPage/NavigationPage/HomePage", UriKind.Absolute));
            //                            await NavigationService.NavigateAsync(nameof(ChatListPage));
            //                            //App.Current.MainPage = new NavigationPage(new MasterPage());
            //                            //App.Current.MainPage.Navigation.PushAsync(new ChatListPage());
            //                        }
            //                        else if (data.Contains("You have new message from support team") || data.Contains("لديك رسالة جديدة من فريق الدعم"))
            //                        {
            //                            await NavigationService.NavigateAsync(new Uri("/MasterPage/NavigationPage/HomePage", UriKind.Absolute));
            //                            await NavigationService.NavigateAsync(nameof(SupportPage));
            //                        }
            //                        else if (data.Contains("Your password has been reset by the admin, In order to continue please contact administrator.") || data.Contains("تمت إعادة تعيين كلمة المرور الخاصة بك من قبل المشرف ، للمتابعة ، يرجى الاتصال بالمسؤول") || data.Contains("Your account has been de-activated by the admin, in order to continue please contact administrator") || data.Contains("تم إلغاء تنشيط حسابك من قبل المشرف ، للمتابعة ، يرجى الاتصال بالمسؤول"))
            //                        {
            //                            if (userDataDbService.IsUserDbPresentInDB())
            //                            {
            //                                var item = userDataDbService.ReadAllItems().FirstOrDefault();
            //                                BsonValue id = item.ID;
            //                                userDataDbService.DeleteItemFromDB(id, item);

            //                                //App.Current.MainPage = new NavigationPage(new LoginPage());
            //                                await NavigationService.NavigateAsync(new Uri("/NavigationPage/LoginPage", UriKind.Absolute));
            //                            }
            //                        }
            //                        else
            //                        {
            //                            await NavigationService.NavigateAsync(new Uri("/MasterPage/NavigationPage/HomePage", UriKind.Absolute));
            //                            await NavigationService.NavigateAsync(nameof(NotificationPage));
            //                            //if (Application.Current.Properties.ContainsKey("AppStatus"))
            //                            //{
            //                            //if (Convert.ToInt32(Application.Current.Properties["AppStatus"]) == Convert.ToInt32(AppStatus.Foreground))
            //                            //{
            //                            //    App.Current.MainPage.Navigation.PushAsync(new NotificationPage());
            //                            //}
            //                            //else if (Convert.ToInt32(Application.Current.Properties["AppStatus"]) == Convert.ToInt32(AppStatus.Background))
            //                            //{
            //                            //    App.Current.MainPage = new NavigationPage(new MasterPage());
            //                            //    App.Current.MainPage.Navigation.PushAsync(new NotificationPage());
            //                            //}
            //                            //}
            //                        }
            //                    });

            //                }
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            // Some other exception occurred
            //        }
            //    };

            //    CrossFirebasePushNotification.Current.OnNotificationAction += (s, p) =>
            //    {
            //        System.Diagnostics.Debug.WriteLine("Action");

            //        if (!string.IsNullOrEmpty(p.Identifier))
            //        {
            //            System.Diagnostics.Debug.WriteLine($"ActionId: {p.Identifier}");
            //            foreach (var data in p.Data)
            //            {
            //                System.Diagnostics.Debug.WriteLine($"{data.Key} : {data.Value}");
            //            }

            //        }

            //    };

            //    CrossFirebasePushNotification.Current.OnNotificationDeleted += (s, p) =>
            //    {
            //        System.Diagnostics.Debug.WriteLine("Dismissed");
            //    };
            //}
        }

        public async void UpdateLanguageServer(ChangeLanguagesModel request)
        {
            CommonResponseModel resonse;
            try
            {
                resonse = await _webApiRestClient.PostAsync<ChangeLanguagesModel, CommonResponseModel>(ApiUrl.ChangeLanguage, request);
            }
            catch (Exception ex)
            {

            }
        }

        #region Setlanguage
        public static void Setlanguage(string culturevalue)
        {

            var netLanguage = L10n.SetLocale(culturevalue);
            AppResource.Culture = new CultureInfo(netLanguage);
        }
        #endregion

        #region GetProvienceApi Api Call Method
        public async void GetProvienceApi(int country_id)
        {
            if (Common.CheckConnection())
            {
                ProvienceModel response;
                try
                {
                    response = await _webApiRestClient.GetAsync<ProvienceModel>(string.Format(ApiUrl.GetProvinces, country_id));
                }
                catch (Exception ex)
                {
                    response = null;
                    await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_ServerError, msDuration: 3000);
                    //countryDetailData = null;
                }
                if (response != null)
                {
                    if (response.status)
                    {
                        //foreach (var item in response.data)
                        //{
                        //    ViewModelBase.provienceDataModels.Add(item);
                        //}
                        BaseViewModel.provienceDataModels = response.data;
                        Application.Current.Properties["ProvinceList"] = JsonConvert.SerializeObject(response.data);
                        App.Current.SavePropertiesAsync();
                        //countryDetailData = response.data;
                        //foreach (var data in countryDetailData)
                        //{
                        //    CountryPickerList.Add(data);
                        //    CountryCodeList.Add(data);
                        //}
                    }
                    else
                    {
                        await MaterialDialog.Instance.SnackbarAsync(message: response.message,
                                    msDuration: 3000);
                        //countryDetailData = null;
                    }
                }
            }
            else
            {
                await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_InternetError,
                                        msDuration: 3000);
            }
        }
        #endregion

        #region GetCountriesApi Api Call Method
        public async void GetCountriesApi()
        {
            if (Common.CheckConnection())
            {
                CountriesModel response;
                try
                {
                    response = await _webApiRestClient.GetAsync<CountriesModel>(ApiUrl.GetCountries);
                }
                catch (Exception ex)
                {
                    response = null;
                    //await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_ServerError, msDuration: 3000);
                    //countryDetailData = null;
                }
                if (response != null)
                {
                    if (response.status)
                    {
                        BaseViewModel.countryDataModels = response.data;
                        Application.Current.Properties["CountryList"] = JsonConvert.SerializeObject(response.data);
                        App.Current.SavePropertiesAsync();
                        GetProvienceApi(BaseViewModel.countryDataModels.FirstOrDefault().country_id);
                    }
                    else
                    {
                        await MaterialDialog.Instance.SnackbarAsync(message: response.message,
                                    msDuration: 3000);
                        //countryDetailData = null;
                    }
                }
            }
            else
            {
                await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_InternetError,
                                        msDuration: 3000);
            }
        }
        #endregion

        #region UpdateDeviceInfo
        public async void UpdateDeviceInfo()
        {
            if (userdataindb != null)
            {
                var deviceRequestmodel = new UpdateDeviceInfoModel()
                {
                    device_id = Device.RuntimePlatform == Device.Android ? 1 : Device.RuntimePlatform == Device.iOS ? 2 : 0,
                    user_id = userdataindb.user_id,
                    device_token = Application.Current.Properties.ContainsKey("AppFirebaseToken") ? Application.Current.Properties["AppFirebaseToken"].ToString() : null
                };
                UpdateDeviceInfoResponse deviceInfoResponse;
                try
                {
                    deviceInfoResponse = await _webApiRestClient.PostAsync<UpdateDeviceInfoModel, UpdateDeviceInfoResponse>(ApiUrl.UpdateDeviceInfo, deviceRequestmodel);
                }
                catch (Exception ex)
                {
                    deviceInfoResponse = null;
                }
                if (deviceInfoResponse != null)
                {
                    //if (deviceInfoResponse.status)
                    //{
                    //    //await MaterialDialog.Instance.SnackbarAsync(message: deviceInfoResponse.message, msDuration: 3000);
                    //}
                    //else
                    //{
                    //    await MaterialDialog.Instance.SnackbarAsync(message: deviceInfoResponse.message, msDuration: 3000);
                    //}
                } 
            }
        }
        #endregion

        #region RegisterTypes
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<ForgotPasswordPage, ForgotPasswordPageViewModel>();
            containerRegistry.RegisterForNavigation<RegisterationPage, RegisterationPageViewModel>();
            containerRegistry.RegisterForNavigation<OtpPage, OtpPageViewModel>();
            containerRegistry.RegisterForNavigation<CreatePasswordPage, CreatePasswordPageViewModel>();
            containerRegistry.RegisterForNavigation<CompleteProfilePage, CompleteProfilePageViewModel>();
            containerRegistry.RegisterForNavigation<MasterPage, MenuPageViewModel>();
            containerRegistry.RegisterForNavigation<AboutUsPage, AboutUsPageViewModel>();
            containerRegistry.RegisterForNavigation<ContactUsPage, ContactUsPageViewModel>();
            containerRegistry.RegisterForNavigation<PrivacyPolicyPage, PrivacyPolicyPageViewModel>();
            containerRegistry.RegisterForNavigation<TermConditionPage, TermConditionPageViewModel>();
            containerRegistry.RegisterForNavigation<HomePage, HomePageViewModel>();
            containerRegistry.RegisterForNavigation<CustomMapPage, CustomMapPageViewModel>();
            containerRegistry.RegisterForNavigation<JobDetailPage, JobDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<JobRequestPage, JobRequestPageViewModel>();
            containerRegistry.RegisterForNavigation<NoInternetPage, NoInternetPageViewModel>();
            containerRegistry.RegisterForNavigation<NotificationPage, NotificationPageViewModel>();
            containerRegistry.RegisterForNavigation<ReviewPage, ReviewPageViewModel>();
            containerRegistry.RegisterForNavigation<ServiceDetailPage, ServiceDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<ServiceDetailPage, ServiceDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<ChatListPage, ChatListPageViewModel>();
            containerRegistry.RegisterForNavigation<ChatDetailPage, ChatDetailPageViewModel>();
            containerRegistry.RegisterForNavigation<ChangeLanguagePage, ChangeLanguagePageViewModel>();
            containerRegistry.RegisterForNavigation<ChangePasswordPage, ChangePasswordPageViewModel>();
            containerRegistry.RegisterForNavigation<FaqPage, FaqPageViewModel>();
            containerRegistry.RegisterForNavigation<ProfilePage, ProfilePageViewModel>();
            containerRegistry.RegisterForNavigation<SettingPage, SettingPageViewModel>();
            containerRegistry.RegisterForNavigation<SupportPage, SupportPageViewModel>();
            containerRegistry.RegisterForNavigation<EditProfilePage, EditProfilePageViewModel>();
            containerRegistry.RegisterForNavigation<MyBookingPage, MyBookingPageViewModel>();
            containerRegistry.RegisterForNavigation<MenuPage, MenuPageViewModel>();
            containerRegistry.RegisterForNavigation<HomeTabbedPage, HomeTabbedPageViewModel>();
            containerRegistry.RegisterForNavigation<ExpressServicePage, ExpressServicePageViewModel>();
            containerRegistry.RegisterForNavigation<SplashPage, SplashPageViewModel>();
            containerRegistry.RegisterForNavigation<ExpressServiceDetailPage, ExpressServiceDetailPageViewModel>();
        } 
        #endregion
    }
}
