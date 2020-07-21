using Khadamat_CustomerApp.Helpers;
using Khadamat_CustomerApp.Models;
using Khadamat_CustomerApp.Resources;
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
    public class ChangePasswordPageViewModel : BaseViewModel
    {
        private readonly INavigationService NavigationService;
        public static Entry oldPswdEnrty;
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

        #region IsOldPassword Field
        private bool _IsOldPassword;

        public bool IsOldPassword
        {
            get { return _IsOldPassword; }
            set { SetProperty(ref _IsOldPassword, value); }
        }
        #endregion

        #region OldPswdShowHidePswdIcon Field
        private string _OldPswdShowHidePswdIcon;

        public string OldPswdShowHidePswdIcon
        {
            get { return _OldPswdShowHidePswdIcon; }
            set { SetProperty(ref _OldPswdShowHidePswdIcon, value); }
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

        #region OldPassword Entry Field
        private string _OldPassword;

        public string OldPassword
        {
            get { return _OldPassword; }
            set { SetProperty(ref _OldPassword, value); }
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

        #region Constructor
        public ChangePasswordPageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
            IsLoaderBusy = false;
            OldPswdShowHidePswdIcon = "resource://Khadamat_CustomerApp.SvgImages.hidepswd.svg";
            IsOldPassword = true;
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
                return new DelegateCommand<string>((e) =>
                {
                    try
                    {
                        switch (e.ToString().ToLower())
                        {
                            case "oldpswd":
                                if (IsOldPassword)
                                {
                                    OldPswdShowHidePswdIcon = "resource://Khadamat_CustomerApp.SvgImages.showpswd.svg";
                                    IsOldPassword = false;
                                }
                                else
                                {
                                    OldPswdShowHidePswdIcon = "resource://Khadamat_CustomerApp.SvgImages.hidepswd.svg";
                                    IsOldPassword = true;
                                }
                                oldPswdEnrty.Focus();
                                break;
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
                        if (Common.CheckConnection())
                        {
                            if (!string.IsNullOrEmpty(OldPassword) && !string.IsNullOrWhiteSpace(OldPassword) && !string.IsNullOrEmpty(NewPassword) && !string.IsNullOrWhiteSpace(NewPassword) && !string.IsNullOrEmpty(ConfirmPassword) && !string.IsNullOrWhiteSpace(ConfirmPassword))
                            {
                                if (ConfirmPassword == NewPassword)
                                {
                                    IsLoaderBusy = true;
                                    ChangePasswordModel request = new ChangePasswordModel()
                                    {
                                        old_password = OldPassword,
                                        password = ConfirmPassword,
                                        user_id = user_id
                                    };
                                    ChangePasswordResponseModel response;
                                    try
                                    {
                                        response = await _webApiRestClient.PostAsync<ChangePasswordModel, ChangePasswordResponseModel>(ApiUrl.ChangePassword, request);
                                    }
                                    catch (Exception ex)
                                    {
                                        response = null;
                                        IsLoaderBusy = false;
                                        //await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_ServerError, msDuration: 3000);
                                        return;
                                    }
                                    if (response != null)
                                    {
                                        if (response.status)
                                        {
                                            await MaterialDialog.Instance.SnackbarAsync(response.message, 3000);
                                            //Common.PopPage(_navigation);
                                            await NavigationService.GoBackAsync();
                                            //App.Current.MainPage = new NavigationPage(new MasterPage());
                                        }
                                        else
                                        {
                                            await MaterialDialog.Instance.SnackbarAsync(response.message, 3000);
                                        }
                                    }
                                }
                                else
                                {
                                    await MaterialDialog.Instance.SnackbarAsync(AppResource.err_ConfirmPassword, 3000);
                                }
                            }
                            else
                            {
                                await MaterialDialog.Instance.SnackbarAsync(AppResource.err_BlankPassword, 3000);
                            }
                        }
                        else
                        {
                            await MaterialDialog.Instance.SnackbarAsync(AppResource.error_InternetError, 3000);
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
