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
    public class TermConditionPageViewModel : BaseViewModel, INavigationAware
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

        #region TermWebViewSource
        private HtmlWebViewSource _TermWebViewSource;
        public HtmlWebViewSource TermWebViewSource
        {
            get { return _TermWebViewSource; }
            set { SetProperty(ref _TermWebViewSource, value); }
        }
        #endregion        

        #region TermConditionText
        private string _TermConditionText;

        public string TermConditionText
        {
            get { return _TermConditionText; }
            set { SetProperty(ref _TermConditionText, value); }
        }
        #endregion

        #region Constructor
        public TermConditionPageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
            htmlToText = new HtmlToText();
            IsNodataFound = false;
        }
        #endregion

        #region Api Call
        private async void TermConditionApi()
        {
            try
            {
                IsLoaderBusy = true;
                if (Common.CheckConnection())
                {
                    TermConditionResponseModel response;
                    try
                    {
                        response = await _webApiRestClient.GetAsync<TermConditionResponseModel>(ApiUrl.GetTermsConditions);
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
                            Application.Current.Properties["TermsConditionsData"] = response.TermsConditionsData;
                            Application.Current.SavePropertiesAsync();
                            TermConditionText = htmlToText.Convert(Common.GetLanguage() == "ar-AE" ? response.TermsConditionsData.text_arabic : response.TermsConditionsData.text);
                            //var htmltext = new HtmlWebViewSource
                            //{
                            //    Html = response.TermsConditionsData.text
                            //};
                            //TermWebViewSource = htmltext;
                            IsNodataFound = false;
                        }
                        else
                        {
                            await MaterialDialog.Instance.SnackbarAsync(message: response.message,
                                        msDuration: 3000);
                            IsNodataFound = true;
                        }
                    }
                    else
                    {
                        if (Application.Current.Properties.ContainsKey("TermsConditionsData"))
                        {
                            var termConditionData = (TermsConditionsData)Application.Current.Properties["TermsConditionsData"];
                            TermConditionText = htmlToText.Convert(Common.GetLanguage() == "ar-AE" ? termConditionData.text_arabic : termConditionData.text);
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
                    if (Application.Current.Properties.ContainsKey("TermsConditionsData"))
                    {
                        var termConditionData = (TermsConditionsData)Application.Current.Properties["TermsConditionsData"];
                        TermConditionText = htmlToText.Convert(Common.GetLanguage() == "ar-AE" ? termConditionData.text_arabic : termConditionData.text);
                        IsNodataFound = false;
                    }
                    else
                    {
                        IsNoInternetView = true;
                        IsNodataFound = false;
                    }
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

        #region Navigation Aware Parameters
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("CategoryTermCondition"))
            {
                TermConditionText = htmlToText.Convert((string)parameters["CategoryTermCondition"]);
            }
            else
            {
                TermConditionApi();
            }
        } 
        #endregion
    }
}
