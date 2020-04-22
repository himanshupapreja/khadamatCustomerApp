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
    public class AboutUsPageViewModel : BaseViewModel
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

        #region AboutWebViewSource
        private HtmlWebViewSource _AboutWebViewSource;
        public HtmlWebViewSource AboutWebViewSource
        {
            get { return _AboutWebViewSource; }
            set { SetProperty(ref _AboutWebViewSource, value); }
        }
        #endregion        

        #region IsNodataFound
        private bool _IsNodataFound = true;
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

        #region AboutUsText
        private string _AboutUsText;

        public string AboutUsText
        {
            get { return _AboutUsText; }
            set { SetProperty(ref _AboutUsText, value); }
        }
        #endregion

        #region Constructor
        public AboutUsPageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
            htmlToText = new HtmlToText();
            if (Application.Current.Properties.ContainsKey("AboutUsData"))
            {
                var aboutusData = (AboutUsData)Application.Current.Properties["AboutUsData"];
                AboutUsText = htmlToText.Convert(Common.GetLanguage() == "ar-AE" ? aboutusData.text_arabic : aboutusData.text);
                IsNodataFound = false;
            }
            AboutUsDataApi();
        }
        #endregion

        #region AboutUsDataApi
        private async void AboutUsDataApi()
        {
            if (!Application.Current.Properties.ContainsKey("AboutUsData"))
            {
                IsLoaderBusy = true;
            }
            try
            {
                if (Common.CheckConnection())
                {
                    AboutUsResponseModel response;
                    try
                    {
                        response = await _webApiRestClient.GetAsync<AboutUsResponseModel>(ApiUrl.GetAboutus);
                    }
                    catch (Exception ex)
                    {
                        response = null;
                        IsLoaderBusy = false;
                    }
                    if (response != null)
                    {
                        if (response.status)
                        {
                            if (Application.Current.Properties.ContainsKey("AboutUsData"))
                            {
                                var aboutusData = (AboutUsData)Application.Current.Properties["AboutUsData"];
                                if(response.AboutUsData != aboutusData)
                                {
                                    Application.Current.Properties["AboutUsData"] = response.AboutUsData;
                                    Application.Current.SavePropertiesAsync();
                                    AboutUsText = htmlToText.Convert(Common.GetLanguage() == "ar-AE" ? response.AboutUsData.text_arabic : response.AboutUsData.text);
                                    IsNodataFound = false;
                                }
                            }
                            else
                            {
                                Application.Current.Properties["AboutUsData"] = response.AboutUsData;
                                Application.Current.SavePropertiesAsync();
                                AboutUsText = htmlToText.Convert(Common.GetLanguage() == "ar-AE" ? response.AboutUsData.text_arabic : response.AboutUsData.text);
                                IsNodataFound = false;
                            }
                        }
                        else
                        {

                            if (!Application.Current.Properties.ContainsKey("AboutUsData"))
                            {
                                await MaterialDialog.Instance.SnackbarAsync(message: response.message, msDuration: 3000);
                            }
                        }
                    }
                    else
                    {
                        if (Application.Current.Properties.ContainsKey("AboutUsData"))
                        {
                            var aboutusData = (AboutUsData)Application.Current.Properties["AboutUsData"];
                            AboutUsText = htmlToText.Convert(Common.GetLanguage() == "ar-AE" ? aboutusData.text_arabic : aboutusData.text);
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
                    if (Application.Current.Properties.ContainsKey("AboutUsData"))
                    {
                        var aboutusData = (AboutUsData)Application.Current.Properties["AboutUsData"];
                        AboutUsText = htmlToText.Convert(Common.GetLanguage() == "ar-AE" ? aboutusData.text_arabic : aboutusData.text);
                        IsNodataFound = false;
                    }
                    else
                    {
                        IsNodataFound = false;
                        IsNoInternetView = true;
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
    }
}
