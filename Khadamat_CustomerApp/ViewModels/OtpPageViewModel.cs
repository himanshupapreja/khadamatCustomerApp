using Khadamat_CustomerApp.Helpers;
using Khadamat_CustomerApp.Models;
using Khadamat_CustomerApp.Resources;
using Khadamat_CustomerApp.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;
using static Khadamat_CustomerApp.Helpers.Enums;

namespace Khadamat_CustomerApp.ViewModels
{
    public class OtpPageViewModel : BaseViewModel, INavigatedAware
    {
        private readonly INavigationService NavigationService;
        string PhoneNumber;
        public static bool ForgotPassword;
        public static bool ProfilePage;

        public string PhoneNumber_One;
        public string PhoneNumber_Two;
        public string PhoneNumber_Three;

        public SendOtpResponseModel sendOtpModel;

        #region ResendOtpEnabled Field
        private bool _ResendOtpEnabled;

        public bool ResendOtpEnabled
        {
            get { return _ResendOtpEnabled; }
            set { SetProperty(ref _ResendOtpEnabled, value); }
        }
        #endregion

        #region IsCallErrorvisible Field
        private bool _IsCallErrorvisible;

        public bool IsCallErrorvisible
        {
            get { return _IsCallErrorvisible; }
            set { SetProperty(ref _IsCallErrorvisible, value); }
        }
        #endregion

        #region IsCallButtonvisible Field
        private bool _IsCallButtonvisible;

        public bool IsCallButtonvisible
        {
            get { return _IsCallButtonvisible; }
            set { SetProperty(ref _IsCallButtonvisible, value); }
        }
        # endregion

        #region IsLoaderBusy Field
        private bool _IsLoaderBusy;

        public bool IsLoaderBusy
        {
            get { return _IsLoaderBusy; }
            set { SetProperty(ref _IsLoaderBusy, value); }
        }
        #endregion

        #region resendOtpCount Field
        private int _resendOtpCount;

        public int resendOtpCount
        {
            get { return _resendOtpCount; }
            set
            {
                SetProperty(ref _resendOtpCount, value);
                if (resendOtpCount >= 3)
                {
                    ResendOtpEnabled = false;
                    IsCallButtonvisible = true;
                    IsCallErrorvisible = true;
                }
            }
        }
        #endregion

        #region OTP1 Entry Field
        public Entry otpEntry1;
        private string _OTP1;

        public string OTP1
        {
            get { return _OTP1; }
            set
            {
                SetProperty(ref _OTP1, value);
                if (!string.IsNullOrEmpty(OTP1) && !string.IsNullOrWhiteSpace(OTP1))
                {
                    if (OTP1.Length > 1)
                    {
                        var dummyEntry = OTP1;
                        OTP1 = OTP1.Remove(OTP1.Length - 1);
                        otpEntry2.Focus();
                        OTP2 = dummyEntry.ElementAt(dummyEntry.Length - 1).ToString();
                    }
                }
            }
        }
        #endregion

        #region OTP2 Entry Field
        public Entry otpEntry2;
        private string _OTP2;

        public string OTP2
        {
            get { return _OTP2; }
            set
            {
                SetProperty(ref _OTP2, value);
                if (!string.IsNullOrEmpty(OTP2) && !string.IsNullOrWhiteSpace(OTP2))
                {
                    if (OTP2.Length > 1)
                    {
                        var dummyEntry = OTP2;
                        OTP2 = OTP2.Remove(OTP2.Length - 1);
                        otpEntry3.Focus();
                        OTP3 = dummyEntry.ElementAt(dummyEntry.Length - 1).ToString();
                    }
                }
                else
                {
                    otpEntry1.Focus();
                }
            }
        }
        #endregion

        #region OTP3 Entry Field
        public Entry otpEntry3;
        private string _OTP3;

        public string OTP3
        {
            get { return _OTP3; }
            set
            {
                SetProperty(ref _OTP3, value);
                if (!string.IsNullOrEmpty(OTP3) && !string.IsNullOrWhiteSpace(OTP3))
                {
                    if (OTP3.Length > 1)
                    {
                        var dummyEntry = OTP3;
                        OTP3 = OTP3.Remove(OTP3.Length - 1);
                        otpEntry4.Focus();
                        OTP4 = dummyEntry.ElementAt(dummyEntry.Length - 1).ToString();
                    }
                }
                else
                {
                    otpEntry2.Focus();
                }
            }
        }
        #endregion

        #region OTP4 Entry Field
        public Entry otpEntry4;
        private string _OTP4;

        public string OTP4
        {
            get { return _OTP4; }
            set
            {
                SetProperty(ref _OTP4, value);
                if (!string.IsNullOrEmpty(OTP4) && !string.IsNullOrWhiteSpace(OTP4))
                {
                    if (OTP4.Length > 1)
                    {
                        var dummyEntry = OTP4;
                        OTP4 = OTP4.Remove(OTP4.Length - 1);
                        otpEntry5.Focus();
                        OTP5 = dummyEntry.ElementAt(dummyEntry.Length - 1).ToString();
                    }
                }
                else
                {
                    otpEntry3.Focus();
                }
            }
        }
        #endregion

        #region OTP5 Entry Field
        public Entry otpEntry5;
        private string _OTP5;

        public string OTP5
        {
            get { return _OTP5; }
            set
            {
                SetProperty(ref _OTP5, value);
                if (!string.IsNullOrEmpty(OTP5) && !string.IsNullOrWhiteSpace(OTP5))
                {
                    if (OTP5.Length > 1)
                    {
                        OTP5 = OTP5.Remove(OTP5.Length - 1);
                    }
                }
                else
                {
                    otpEntry4.Focus();
                }
            }
        }
        #endregion

        public OtpPageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;

            IsLoaderBusy = false;
            IsCallButtonvisible = false;
            IsCallErrorvisible = false;
            //PhoneNumber = phoneNumber;
            //ForgotPassword = forgotPassword;
            //ProfilePage = profilepage;
            resendOtpCount = 0;
            ResendOtpEnabled = true;
        }

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
                            VerifyOtpResponseModel responseModel;
                            if (!string.IsNullOrEmpty(OTP1) && !string.IsNullOrWhiteSpace(OTP1) && !string.IsNullOrEmpty(OTP2) && !string.IsNullOrWhiteSpace(OTP2) && !string.IsNullOrEmpty(OTP3) && !string.IsNullOrWhiteSpace(OTP3) && !string.IsNullOrEmpty(OTP4) && !string.IsNullOrWhiteSpace(OTP4) && !string.IsNullOrEmpty(OTP5) && !string.IsNullOrWhiteSpace(OTP5))
                            {
                                IsLoaderBusy = true;
                                VerifyOtpRequestModel requestModel = new VerifyOtpRequestModel()
                                {
                                    user_id = user_id,
                                    phone_number = PhoneNumber,
                                    otp = int.Parse(OTP1 + OTP2 + OTP3 + OTP4 + OTP5)
                                };
                                try
                                {
                                    responseModel = await _webApiRestClient.PostAsync<VerifyOtpRequestModel, VerifyOtpResponseModel>(ApiUrl.VerifyOtp, requestModel);
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
                                        if (!ProfilePage)
                                        {
                                            //Common.CustomNavigation(_navigation, new CreatePasswordPage(ForgotPassword));
                                            var param = new NavigationParameters();
                                            param.Add("IsForgotPassword", ForgotPassword);
                                            await NavigationService.NavigateAsync(nameof(CreatePasswordPage),param);
                                        }
                                        else
                                        {
                                            var param = new NavigationParameters();
                                            param.Add("UpdateProfileData", true);
                                            await NavigationService.GoBackAsync(param);
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
                                await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_BlankOtp, msDuration: 3000);
                            }
                        }
                        else
                        {
                            await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_InternetError, msDuration: 3000);
                        }
                    }
                    catch (Exception)
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

        #region ResendOtpApiCall
        private async void ResendOtpApiCall(VerifyResendOtpModel requestModel)
        {
            try
            {
                if (Common.CheckConnection())
                {
                    IsLoaderBusy = true;
                    SendOtpResponseModel response;
                    try
                    {
                        response = await _webApiRestClient.PostAsync<VerifyResendOtpModel, SendOtpResponseModel>(ApiUrl.ResendOtp, requestModel);
                    }
                    catch (Exception ex)
                    {
                        response = null;
                        await MaterialDialog.Instance.SnackbarAsync(AppResource.error_ServerError, 3000);
                        IsLoaderBusy = false;
                        return;
                    }
                    if (response != null)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            if (response.status)
                            {
                                //await MaterialDialog.Instance.SnackbarAsync(response.message, 3000);
                                resendOtpCount = response.otpData.otp_attempts.HasValue ? response.otpData.otp_attempts.Value : 0;
                                OTP1 = OTP2 = OTP3 = OTP4 = OTP5 = string.Empty;
                                //if (response.otpData.otp_attempts.HasValue && response.otpData.otp_attempts.Value <= 3)
                                //{
                                //    await App.Current.MainPage.DisplayAlert("Otp Received", response.otpData.otp.Value.ToString(), "Ok");
                                //}

                                PhoneNumber_One = response.phone_number_one;
                                PhoneNumber_Two = response.phone_number_two;
                                PhoneNumber_Three = response.phone_number_three;
                            }
                            else
                            {
                                //await MaterialDialog.Instance.SnackbarAsync(response.message, 3000);
                                await App.Current.MainPage.DisplayAlert("", response.message, AppResource.Ok);
                            }
                        });
                    }
                }
                else
                {
                    await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_InternetError, msDuration: 3000);
                }
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

        #region ResendOtpCommand
        public ICommand ResendOtpCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    try
                    {
                        if (resendOtpCount < 3)
                        {
                            VerifyResendOtpModel VerifyResendOtpModel = new VerifyResendOtpModel()
                            {
                                phone_number = PhoneNumber,
                                user_id = user_id,
                                page_type = !ProfilePage && !ForgotPassword ? OTPPageEnum.NewUser.GetHashCode() : ProfilePage ? OTPPageEnum.Profile.GetHashCode() : OTPPageEnum.ForgotPswd.GetHashCode()
                            };
                            ResendOtpApiCall(VerifyResendOtpModel);
                        }
                    }
                    catch (Exception)
                    {

                    }
                    finally
                    {
                    }
                });
            }
        }
        #endregion

        #region CallCommand
        public ICommand CallCommand
        {
            get
            {
                return new DelegateCommand(async() =>
                {
                    try
                    {
                        var result = await App.Current.MainPage.DisplayActionSheet(AppResource.otp_SupportCall, null,null,PhoneNumber_One, PhoneNumber_Two, PhoneNumber_Three);
                        PhoneDialer.Open(result);
                    }
                    catch (Exception)
                    {

                    }
                    finally
                    {
                    }
                });
            }
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

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("PhoneNumber"))
            {
                PhoneNumber = (string)parameters["PhoneNumber"];
            }
            if (parameters.ContainsKey("OtpAttemps"))
            {
                sendOtpModel = (SendOtpResponseModel)parameters["OtpAttemps"];
            }

            if(sendOtpModel != null)
            {
                resendOtpCount = sendOtpModel.otpData.otp_attempts.HasValue ? sendOtpModel.otpData.otp_attempts.Value : 0;
                PhoneNumber_One = sendOtpModel.phone_number_one;
                PhoneNumber_Two = sendOtpModel.phone_number_two;
                PhoneNumber_Three = sendOtpModel.phone_number_three;
            }


            if (parameters.ContainsKey("IsForgotPassword"))
            {
                ForgotPassword = (bool)parameters["IsForgotPassword"];
                IsCallButtonvisible = true;
                IsCallErrorvisible = false;

                if (parameters.ContainsKey("ForgotData"))
                {
                    var data = (ForgotPasswordResponseModel)parameters["ForgotData"];
                    PhoneNumber_One = data.phone_number_one;
                    PhoneNumber_Two = data.phone_number_two;
                    PhoneNumber_Three = data.phone_number_three;


                    Device.BeginInvokeOnMainThread(async() =>
                    {
                        if (data.userData.otp.Value <=0)
                        {
                            await App.Current.MainPage.DisplayAlert("", data.message, "Ok"); 
                        }
                    });
                }
            }
            else
            {
                ForgotPassword = false;
                IsCallButtonvisible = false;
            }
            if (parameters.ContainsKey("IsProfilePage"))
            {
                ProfilePage = (bool)parameters["IsProfilePage"];
                IsCallButtonvisible = true;
                IsCallErrorvisible = false;

                if (parameters.ContainsKey("ProfileData"))
                {
                    var data = (EditPhoneResponseModel)parameters["ProfileData"];
                    PhoneNumber_One = data.phone_number_one;
                    PhoneNumber_Two = data.phone_number_two;
                    PhoneNumber_Three = data.phone_number_three;

                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        if (data.otpCode.otp <= 0)
                        {
                            await App.Current.MainPage.DisplayAlert("", data.message, "Ok");
                        }
                    });
                }
            }
            else
            {
                ProfilePage = false;
                IsCallButtonvisible = false;
            }
        }
    }
}
