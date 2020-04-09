using Khadamat_CustomerApp.Helpers;
using Khadamat_CustomerApp.Models;
using Khadamat_CustomerApp.Resources;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace Khadamat_CustomerApp.ViewModels
{
    public class ChangeLanguagePageViewModel : BaseViewModel
    {
        private readonly INavigationService NavigationService;

        #region LanguageSelected
        private string _LanguageSelected;
        public string LanguageSelected
        {
            get { return _LanguageSelected; }
            set { SetProperty(ref _LanguageSelected, value); }
        }
        #endregion

        #region ChangeLanguage Picker static value
        //public List<string> _ChangeLanguageList = new List<string> {
        //    AppResource.changelang_English,AppResource.changelang_Russian,AppResource.changelang_Hebrew,AppResource.changelang_French
        //};
        public List<LanguagesModel> AvailableLanguages = new List<LanguagesModel> {
            new LanguagesModel {
                LanguageFullName = AppResource.changelang_Arabic, LanguageCultureName = "ar-AE"
            },
            new LanguagesModel {
                LanguageFullName = AppResource.changelang_English, LanguageCultureName = "en-US"
            }
        };
        public List<LanguagesModel> ChangeLanguageList => AvailableLanguages;
        #endregion

        #region ChangeLanguageListSelected SelectedItem property
        private LanguagesModel _ChangeLanguageListSelected;
        public LanguagesModel ChangeLanguageListSelected
        {
            get => _ChangeLanguageListSelected;
            set
            {
                SetProperty(ref _ChangeLanguageListSelected, value);
                if (ChangeLanguageListSelected != null && ChangeLanguageListSelected.LanguageFullName != null && ChangeLanguageListSelected.LanguageCultureName != null)
                {
                    try
                    {
                        App.Setlanguage(ChangeLanguageListSelected.LanguageCultureName);
                        Application.Current.Properties["AppLocale"] = ChangeLanguageListSelected.LanguageCultureName;
                        Application.Current.SavePropertiesAsync();

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

                        NavigationService.NavigateAsync(new Uri("/MasterPage/NavigationPage/HomeTabbedPage", UriKind.Absolute));
                        Application.Current.Properties["IsAppAlreadyInstalled"] = true;
                        Application.Current.SavePropertiesAsync();
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }
        #endregion

        #region Constructor
        public ChangeLanguagePageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
            LanguageSelected = AppResource.changelang_PickerPlaceholder;
        } 
        #endregion

        #region CloseCommand
        public ICommand CloseCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    try
                    {
                        NavigationService.GoBackAsync();
                    }
                    catch (Exception ex)
                    {

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
    }
}
