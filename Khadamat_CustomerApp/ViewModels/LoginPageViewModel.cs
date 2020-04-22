using Khadamat_CustomerApp.Helpers;
using Khadamat_CustomerApp.Models;
using Khadamat_CustomerApp.Resources;
using Khadamat_CustomerApp.Views;
using LiteDB;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace Khadamat_CustomerApp.ViewModels
{
    public class LoginPageViewModel : BaseViewModel
    {
        private readonly INavigationService NavigationService;
        bool IsForgotClick;
        bool IsSignupClick;

        List<string> PhoneStartingValue = new List<string>()
        {
            "77","71","73","70"
        };

        #region IsLoaderBusy Field
        private bool _IsLoaderBusy;

        public bool IsLoaderBusy
        {
            get { return _IsLoaderBusy; }
            set { SetProperty(ref _IsLoaderBusy, value); }
        }
        #endregion

        #region IsPassword Field
        private bool _IsPassword;

        public bool IsPassword
        {
            get { return _IsPassword; }
            set { SetProperty(ref _IsPassword, value); }
        }
        #endregion

        #region ShowHidePswdIcon Field
        private string _ShowHidePswdIcon;

        public string ShowHidePswdIcon
        {
            get { return _ShowHidePswdIcon; }
            set { SetProperty(ref _ShowHidePswdIcon, value); }
        }
        #endregion

        #region PhoneNumber Entry Field
        private string _PhoneNumber;

        public string PhoneNumber
        {
            get { return _PhoneNumber; }
            set
            {
                SetProperty(ref _PhoneNumber, value);
                if (!string.IsNullOrEmpty(PhoneNumber) && !string.IsNullOrWhiteSpace(PhoneNumber) && PhoneNumber.Length > 9)
                {
                    PhoneNumber = Common.StringBuilderChars(PhoneNumber.Take(9));
                }
            }
        }
        #endregion

        #region Password Entry Field
        private string _Password;

        public string Password
        {
            get { return _Password; }
            set { SetProperty(ref _Password, value); }
        }
        #endregion

        #region Constructor
        public LoginPageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
            IsLoaderBusy = false;
            IsSignupClick = false;
            IsForgotClick = false;
            ShowHidePswdIcon = "resource://Khadamat_CustomerApp.SvgImages.hidepswd.svg";
            IsPassword = true;
        }
        #endregion

        #region ShowHidePswdCommand
        public ICommand ShowHidePswdCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    try
                    {
                        if (IsPassword)
                        {
                            ShowHidePswdIcon = "resource://Khadamat_CustomerApp.SvgImages.showpswd.svg";
                            IsPassword = false;
                        }
                        else
                        {
                            ShowHidePswdIcon = "resource://Khadamat_CustomerApp.SvgImages.hidepswd.svg";
                            IsPassword = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        //Console.Clear();
                        Debug.WriteLine("DB error:-> " + ex.Message);
                    }
                    finally
                    {
                    }
                });
            }
        }
        #endregion

        #region SubmitBtnCommand
        public ICommand SubmitBtnCommand
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    try
                    {
                        if (Common.CheckConnection())
                        {
                            LoginResponseModel responseModel;
                            if (!string.IsNullOrEmpty(PhoneNumber) && !string.IsNullOrWhiteSpace(PhoneNumber) && !string.IsNullOrWhiteSpace(Password) && !string.IsNullOrEmpty(Password) && PhoneNumber.Length > 8 && Common.StringStartValue(PhoneNumber, PhoneStartingValue))
                            {
                                IsLoaderBusy = true;
                                LoginRequestModel requestModel = new LoginRequestModel()
                                {
                                    phone_number = PhoneNumber,
                                    password = Password,
                                    user_type = Convert.ToInt32(Enums.UserTypeEnum.Customer)
                                };
                                try
                                {
                                    responseModel = await _webApiRestClient.PostAsync<LoginRequestModel, LoginResponseModel>(ApiUrl.Login, requestModel);
                                }
                                catch (Exception ex)
                                {
                                    responseModel = null;
                                    await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_ServerError, msDuration: 3000);
                                    IsLoaderBusy = false;
                                    return;
                                }
                                if (responseModel != null)
                                {
                                    if (responseModel.status)
                                    {
                                        user_id = responseModel.userData.user_id;
                                        user_name = responseModel.userData.name;
                                        user_pic = Common.IsImagesValid(responseModel.userData.profile_pic, ApiUrl.BaseUrl);

                                        Device.BeginInvokeOnMainThread(() =>
                                        {
                                            var request = new ChangeLanguagesModel();
                                            if (Application.Current.Properties.ContainsKey("AppLocale") && (Application.Current.Properties["AppLocale"].ToString()).Contains("en"))
                                            {
                                                request.language = "en";
                                                request.user_id = user_id;
                                            }
                                            else
                                            {
                                                request.language = "ar";
                                                request.user_id = user_id;
                                            }

                                            UpdateLanguageServer(request);
                                        });
                                        if (!string.IsNullOrEmpty(responseModel.userData.name) && !string.IsNullOrWhiteSpace(responseModel.userData.name))
                                        {
                                            var deviceRequestmodel = new UpdateDeviceInfoModel()
                                            {
                                                device_id = Device.RuntimePlatform == Device.Android ? 1 : Device.RuntimePlatform == Device.iOS ? 2 : 0,
                                                user_id = responseModel.userData.user_id,
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
                                            //if (deviceInfoResponse != null)
                                            //{
                                            //    if (deviceInfoResponse.status)
                                            //    {
                                            //        await MaterialDialog.Instance.SnackbarAsync(message: deviceInfoResponse.message, msDuration: 3000);
                                            //    }
                                            //    else
                                            //    {
                                            //        await MaterialDialog.Instance.SnackbarAsync(message: deviceInfoResponse.message, msDuration: 3000);
                                            //    }
                                            //}
                                            if (userDataDbService.IsUserDbPresentInDB())
                                            {
                                                var item = userDataDbService.ReadAllItems().FirstOrDefault();
                                                BsonValue id = item.ID;
                                                userDataDbService.UpdateUserDataInDb(id, responseModel.userData);
                                            }
                                            else
                                            {
                                                userDataDbService.CreateUserDataInDB(responseModel.userData);
                                            }

                                            //App.Current.MainPage = new NavigationPage(new MasterPage());
                                            await NavigationService.NavigateAsync(new Uri("/MasterPage/NavigationPage/HomePage", UriKind.Absolute));
                                        }
                                        else
                                        {
                                            await NavigationService.NavigateAsync(nameof(CompleteProfilePage));

                                        }
                                    }
                                    else
                                    {
                                        await MaterialDialog.Instance.SnackbarAsync(message: responseModel.message, msDuration: 3000);
                                    }
                                }
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(PhoneNumber) || string.IsNullOrWhiteSpace(PhoneNumber) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrEmpty(Password))
                                {
                                    await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_BlankPhonePassword, msDuration: 3000);
                                }
                                else if (PhoneNumber.Length < 9 || !Common.StringStartValue(PhoneNumber, PhoneStartingValue))
                                {
                                    await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_ValidPhone, msDuration: 3000);
                                }
                            }
                        }
                        else
                        {
                            await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_InternetError, msDuration: 3000);
                        }
                    }
                    catch (Exception ex)
                    {
                        //Console.Clear();
                        Debug.WriteLine("DB error:-> " + ex.Message);
                    }
                    finally
                    {
                        IsLoaderBusy = false;
                    }
                });
            }
        }
        #endregion

        #region UpdateLanguageServer
        private async void UpdateLanguageServer(ChangeLanguagesModel request)
        {
            var result = await _webApiRestClient.PostAsync<ChangeLanguagesModel, CommonResponseModel>(ApiUrl.ChangeLanguage, request);
        } 
        #endregion

        #region SignupCommand
        public ICommand SignupCommand
        {
            get
            {
                return new Command(async() =>
                {
                    try
                    {
                        if (!IsSignupClick)
                        {
                            IsSignupClick = true;
                            await NavigationService.NavigateAsync(nameof(RegisterationPage)); 
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                        IsSignupClick = false;
                    }
                });
            }
        }
        #endregion

        #region ForgotPasswordCommand
        public ICommand ForgotPasswordCommand
        {
            get
            {
                return new Command(async() =>
                {
                    try
                    {
                        if (!IsForgotClick)
                        {
                            IsForgotClick = true;
                            await NavigationService.NavigateAsync(nameof(ForgotPasswordPage)); 
                        }
                    }
                    catch (Exception)
                    {

                    }
                    finally
                    {
                        IsForgotClick = false;
                    }
                });
            }
        }
        #endregion

        #region OnAppearing
        public void OnAppearing()
        {
            Xamarin.Essentials.Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
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

        #region OnDisappearing
        public void OnDisappearing()
        {
            Xamarin.Essentials.Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
        }
        #endregion

        private void Connectivity_ConnectivityChanged(object sender, Xamarin.Essentials.ConnectivityChangedEventArgs e)
        {
            if ((e.ConnectionProfiles.Contains(Xamarin.Essentials.ConnectionProfile.WiFi) || e.ConnectionProfiles.Contains(Xamarin.Essentials.ConnectionProfile.Cellular)) && e.NetworkAccess == Xamarin.Essentials.NetworkAccess.Internet)
            {

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

                    UpdateLanguageServer(request);
                });

                Device.BeginInvokeOnMainThread(() =>
                {
                    app.GetCountriesApi();
                });
            }
        }
    }
}
