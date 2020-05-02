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

namespace Khadamat_CustomerApp.ViewModels
{
    public class ForgotPasswordPageViewModel : BaseViewModel
    {
        private readonly INavigationService NavigationService;

        #region IsLoaderBusy Field
        private bool _IsLoaderBusy;

        public bool IsLoaderBusy
        {
            get { return _IsLoaderBusy; }
            set { SetProperty(ref _IsLoaderBusy, value); }
        }
        #endregion

        #region PhoneNumber Field
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

        #region Toolbar Icon
        private string _svgtool;

        public string svgtool
        {
            get { return _svgtool; }
            set { SetProperty(ref _svgtool, value); }
        }
        #endregion

        #region Constructor
        public ForgotPasswordPageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
            svgtool = "resource://Khadamat_CustomerApp.SvgImages.telephone.svg";
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
                        if (!string.IsNullOrEmpty(PhoneNumber) && !string.IsNullOrWhiteSpace(PhoneNumber))
                        {
                            IsLoaderBusy = true;
                            ForgotPasswordResponseModel response;
                            try
                            {
                                response = await _webApiRestClient.GetAsync<ForgotPasswordResponseModel>(string.Format(ApiUrl.ForgetPassword, PhoneNumber));
                            }
                            catch (Exception ex)
                            {
                                response = null;
                                IsLoaderBusy = false;
                                await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_ServerError, msDuration: 3000);
                                return;
                            }
                            if (response != null)
                            {
                                if (response.status)
                                {
                                    user_id = response.userData.user_id.Value;
                                    if (response.userData.otp.Value > 0)
                                    {
                                        //await App.Current.MainPage.DisplayAlert("", response.userData.otp.Value.ToString(), "Ok");
                                    }
                                    //Common.CustomNavigation(_navigation, new OtpPage(PhoneNumber, true));
                                    var param = new NavigationParameters();
                                    param.Add("PhoneNumber", PhoneNumber);
                                    param.Add("IsForgotPassword", true);
                                    param.Add("IsProfilePage", false);
                                    param.Add("ForgotData", response);
                                    await NavigationService.NavigateAsync(nameof(OtpPage), param);
                                }
                                else
                                {
                                    await MaterialDialog.Instance.SnackbarAsync(response.message, 3000);
                                }
                            }
                        }
                        else
                        {
                            await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_BlankPhone,
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
