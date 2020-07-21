using Khadamat_CustomerApp.Helpers;
using Khadamat_CustomerApp.Models;
using Khadamat_CustomerApp.Resources;
using Khadamat_CustomerApp.Views;
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
    public class CreatePasswordPageViewModel : BaseViewModel, INavigatedAware
    {
        private readonly INavigationService NavigationService;

        bool ForgotPassword;
        public static Entry newPswdEnrty;
        public static Entry rePswdEnrty;
        #region IsLoaderBusy Field
        private bool _IsLoaderBusy;

        public bool IsLoaderBusy
        {
            get { return _IsLoaderBusy; }
            set { SetProperty(ref _IsLoaderBusy, value); }
        }
        #endregion

        #region NewPassword Entry Field
        private string _NewPassword;

        public string NewPassword
        {
            get { return _NewPassword; }
            set { SetProperty(ref _NewPassword, value); }
        }
        #endregion

        #region ConfirmPassword Entry Field
        private string _ConfirmPassword;

        public string ConfirmPassword
        {
            get { return _ConfirmPassword; }
            set { SetProperty(ref _ConfirmPassword, value); }
        }
        #endregion

        #region IsNewPassword Field
        private bool _IsNewPassword;

        public bool IsNewPassword
        {
            get { return _IsNewPassword; }
            set { SetProperty(ref _IsNewPassword, value); }
        }
        #endregion

        #region NewPswdShowHidePswdIcon Field
        private string _NewPswdShowHidePswdIcon;

        public string NewPswdShowHidePswdIcon
        {
            get { return _NewPswdShowHidePswdIcon; }
            set { SetProperty(ref _NewPswdShowHidePswdIcon, value); }
        }
        #endregion

        #region IsConfirmPassword Field
        private bool _IsConfirmPassword;

        public bool IsConfirmPassword
        {
            get { return _IsConfirmPassword; }
            set { SetProperty(ref _IsConfirmPassword, value); }
        }
        #endregion

        #region ConfirmPswdShowHidePswdIcon Field
        private string _ConfirmPswdShowHidePswdIcon;

        public string ConfirmPswdShowHidePswdIcon
        {
            get { return _ConfirmPswdShowHidePswdIcon; }
            set { SetProperty(ref _ConfirmPswdShowHidePswdIcon, value); }
        }
        #endregion

        #region Constructor
        public CreatePasswordPageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;

            //ForgotPassword = forgotPassword;
            IsLoaderBusy = false;
            NewPswdShowHidePswdIcon = "resource://Khadamat_CustomerApp.SvgImages.hidepswd.svg";
            IsNewPassword = true;
            ConfirmPswdShowHidePswdIcon = "resource://Khadamat_CustomerApp.SvgImages.hidepswd.svg";
            IsConfirmPassword = true;
        }
        #endregion

        #region ShowHidePswdCommand
        public ICommand ShowHidePswdCommand
        {
            get
            {
                return new Command((e) =>
                {
                    try
                    {
                        switch (e.ToString().ToLower())
                        {
                            case "newpswd":
                                if (IsNewPassword)
                                {
                                    NewPswdShowHidePswdIcon = "resource://Khadamat_CustomerApp.SvgImages.showpswd.svg";
                                    IsNewPassword = false;
                                }
                                else
                                {
                                    NewPswdShowHidePswdIcon = "resource://Khadamat_CustomerApp.SvgImages.hidepswd.svg";
                                    IsNewPassword = true;
                                }

                                newPswdEnrty.Focus();
                                break;
                            case "confirmpswd":
                                if (IsConfirmPassword)
                                {
                                    ConfirmPswdShowHidePswdIcon = "resource://Khadamat_CustomerApp.SvgImages.showpswd.svg";
                                    IsConfirmPassword = false;
                                }
                                else
                                {
                                    ConfirmPswdShowHidePswdIcon = "resource://Khadamat_CustomerApp.SvgImages.hidepswd.svg";
                                    IsConfirmPassword = true;
                                }
                                rePswdEnrty.Focus();
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        //Console.Clear();
                        Debug.WriteLine("Hideshow Password error:-> " + ex.Message);
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
                        CreatePasswordResponseModel responseModel;
                        if (Common.CheckConnection())
                        {
                            if (!string.IsNullOrEmpty(NewPassword) && !string.IsNullOrWhiteSpace(NewPassword))
                            {
                                if (ConfirmPassword == NewPassword)
                                {
                                    IsLoaderBusy = true;
                                    CreatePasswordRequestModel requestModel = new CreatePasswordRequestModel()
                                    {
                                        user_id = user_id,
                                        password = NewPassword,
                                        is_approved = !ForgotPassword
                                    };
                                    try
                                    {
                                        responseModel = await _webApiRestClient.PostAsync<CreatePasswordRequestModel, CreatePasswordResponseModel>(ApiUrl.CreatePassword, requestModel);
                                    }
                                    catch (Exception ex)
                                    {
                                        responseModel = null;
                                        //await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_ServerError, msDuration: 3000);
                                        IsLoaderBusy = false;
                                        return;
                                    }
                                    if (responseModel != null)
                                    {
                                        if (responseModel.status)
                                        {
                                            if (!ForgotPassword)
                                            {
                                                //Common.CustomNavigation(_navigation, new CompleteProfilePage());
                                                await NavigationService.NavigateAsync(nameof(CompleteProfilePage));
                                            }
                                            else
                                            {
                                                //App.Current.MainPage = new NavigationPage(new LoginPage());
                                                await NavigationService.NavigateAsync(nameof(LoginPage));
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
                                    await MaterialDialog.Instance.SnackbarAsync(message: AppResource.err_ConfirmPassword,
                                            msDuration: 3000);
                                }
                            }
                            else
                            {
                                await MaterialDialog.Instance.SnackbarAsync(message: AppResource.err_BlankPassword,
                                            msDuration: 3000);
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

        public void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("IsForgotPassword"))
            {
                ForgotPassword = (bool)parameters["IsForgotPassword"];
            }
            else
            {
                ForgotPassword = false;
            }
        }

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
