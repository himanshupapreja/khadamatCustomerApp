using Khadamat_CustomerApp.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace Khadamat_CustomerApp.ViewModels
{
    public class SettingPageViewModel : BindableBase
    {
        private readonly INavigationService NavigationService;

        bool IsChangePswd;
        bool IsChangeLang;
        bool IsFAQ;

        #region Constructor
        public SettingPageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
            IsChangeLang = false;
            IsChangePswd = false;
            IsFAQ = false;
        }
        #endregion

        #region SettingCommand
        public ICommand SettingCommand
        {
            get
            {
                return new DelegateCommand<string>(async(e) =>
                {
                    switch (e.ToString().ToLower())
                    {
                        case "changepassword":
                            if (!IsChangePswd)
                            {
                                IsChangePswd = true;
                                await NavigationService.NavigateAsync(nameof(ChangePasswordPage));
                                IsChangePswd = false;
                            }
                            break;
                        case "changelanguage":
                            if (!IsChangeLang)
                            {
                                IsChangeLang = true;
                                await NavigationService.NavigateAsync(nameof(ChangeLanguagePage));
                                IsChangeLang = false;
                            }
                            break;
                        case "rateapp":
                            break;
                        case "faq":
                            if (!IsFAQ)
                            {
                                IsFAQ = true;
                                await NavigationService.NavigateAsync(nameof(FaqPage));
                                IsFAQ = false;
                            }
                            break;
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
