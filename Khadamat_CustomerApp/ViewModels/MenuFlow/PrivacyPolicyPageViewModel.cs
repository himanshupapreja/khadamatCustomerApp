using Khadamat_CustomerApp.CustomControls;
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
using XF.Material.Forms.UI.Dialogs;

namespace Khadamat_CustomerApp.ViewModels
{
    public class PrivacyPolicyPageViewModel : BaseViewModel
    {
        private readonly INavigationService NavigationService;
        private HtmlToText htmlToText;
        #region IsLoaderBusy Field
        private bool _IsLoaderBusy;

        public bool IsLoaderBusy
        {
            get { return _IsLoaderBusy; }
            set { SetProperty(ref _IsLoaderBusy, value); }
        }
        #endregion

        #region PolicyWebViewSource
        private HtmlWebViewSource _PolicyWebViewSource;
        public HtmlWebViewSource PolicyWebViewSource
        {
            get { return _PolicyWebViewSource; }
            set { SetProperty(ref _PolicyWebViewSource, value); }
        }
        #endregion        

        #region IsNodataFound
        private bool _IsNodataFound;
        public bool IsNodataFound
        {
            get { return _IsNodataFound; }
            //set
            //{
            //    _IsNodataFound = value;
            //    OnPropertyChanged(nameof(IsNodataFound));
            //}
            set { SetProperty(ref _IsNodataFound, value); }
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


        #region PrivacyPolicyText
        private string _PrivacyPolicyText;

        public string PrivacyPolicyText
        {
            get { return _PrivacyPolicyText; }
            set { SetProperty(ref _PrivacyPolicyText, value); }
        }
        #endregion

        #region Constructor
        public PrivacyPolicyPageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
            htmlToText = new HtmlToText();
            IsNodataFound = true;
            PrivacyPolicyApi();
        }
        #endregion

        #region Api Call
        private async void PrivacyPolicyApi()
        {
            try
            {
                IsLoaderBusy = true;
                if (Common.CheckConnection())
                {
                    PrivacyPolicyResponseModel response;
                    try
                    {
                        response = await _webApiRestClient.GetAsync<PrivacyPolicyResponseModel>(ApiUrl.GetPrivacyPolicy);
                    }
                    catch (Exception ex)
                    {
                        response = null;
                        await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_ServerError, msDuration: 3000);
                        IsLoaderBusy = false;
                    }
                    if (response != null)
                    {
                        if (response.status)
                        {
                            Application.Current.Properties["PrivacyPolicyData"] = response.PrivacyPolicyData;
                            Application.Current.SavePropertiesAsync();
                            PrivacyPolicyText = htmlToText.Convert(Common.GetLanguage() == "ar-AE" ? response.PrivacyPolicyData.text_arabic : response.PrivacyPolicyData.text);
                            IsNodataFound = false;
                        }
                        else
                        {
                            await MaterialDialog.Instance.SnackbarAsync(message: response.message, msDuration: 3000);
                        }
                    }
                    else
                    {
                        if (Application.Current.Properties.ContainsKey("PrivacyPolicyData"))
                        {
                            var privacyPolicyData = (PrivacyPolicyData)Application.Current.Properties["PrivacyPolicyData"];
                            PrivacyPolicyText = htmlToText.Convert(Common.GetLanguage() == "ar-AE" ? privacyPolicyData.text_arabic : privacyPolicyData.text);
                            IsNodataFound = false;
                        }
                        else
                        {
                            IsNodataFound = true;
                        }
                    }
                }
                else
                {
                    if (Application.Current.Properties.ContainsKey("PrivacyPolicyData"))
                    {
                        var privacyPolicyData = (PrivacyPolicyData)Application.Current.Properties["PrivacyPolicyData"];
                        PrivacyPolicyText = htmlToText.Convert(Common.GetLanguage() == "ar-AE" ? privacyPolicyData.text_arabic : privacyPolicyData.text);
                        IsNodataFound = false;
                    }
                    else
                    {
                        IsNoInternetView = true;
                        IsNodataFound = false;
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                IsLoaderBusy = false;
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
