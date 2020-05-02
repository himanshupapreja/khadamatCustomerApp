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
using XF.Material.Forms.UI.Dialogs;
using static Khadamat_CustomerApp.Helpers.Enums;

namespace Khadamat_CustomerApp.ViewModels
{
    public class RegisterationPageViewModel : BaseViewModel
    {
        private readonly INavigationService NavigationService;

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

        #region Constructor
        public RegisterationPageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;

            IsLoaderBusy = false;
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
                            SendOtpResponseModel responseModel;
                            int phonenumber;
                            var data = int.TryParse(PhoneNumber, out phonenumber);
                            if (!string.IsNullOrEmpty(PhoneNumber) && !string.IsNullOrWhiteSpace(PhoneNumber) && PhoneNumber.Length > 8 && Common.StringStartValue(PhoneNumber, PhoneStartingValue) && data)
                            {
                                IsLoaderBusy = true;
                                SendOtpRequestModel requestModel = new SendOtpRequestModel()
                                {
                                    phone_number = PhoneNumber,
                                    user_type = Convert.ToInt32(UserTypeEnum.Customer)
                                };
                                try
                                {
                                    responseModel = await _webApiRestClient.PostAsync<SendOtpRequestModel, SendOtpResponseModel>(ApiUrl.SendOtp, requestModel);
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
                                        user_id = responseModel.otpData.user_id.Value;
                                        if (responseModel.otpData.otp_attempts.HasValue && responseModel.otpData.otp_attempts.Value <= 3)
                                        {
                                            //await App.Current.MainPage.DisplayAlert("Otp Received", responseModel.otpData.otp.Value.ToString(), "Ok"); 
                                        }
                                        //Common.CustomNavigation(_navigation, new OtpPage(PhoneNumber));
                                        var param = new NavigationParameters();
                                        param.Add("PhoneNumber", PhoneNumber);
                                        param.Add("OtpAttemps", responseModel);
                                        await NavigationService.NavigateAsync(nameof(OtpPage),param);
                                    }
                                    else
                                    {
                                        await MaterialDialog.Instance.SnackbarAsync(message: responseModel.message, msDuration: 3000);
                                    }
                                }
                            }
                            else
                            {
                                if (string.IsNullOrEmpty(PhoneNumber) || string.IsNullOrWhiteSpace(PhoneNumber))
                                {
                                    await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_BlankPhone, msDuration: 3000);
                                }
                                else if (PhoneNumber.Length < 9 || !Common.StringStartValue(PhoneNumber, PhoneStartingValue) || !data)
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
                    }
                    finally
                    {
                        IsLoaderBusy = false;
                    }
                });
            }
        }
        #endregion
    }
}