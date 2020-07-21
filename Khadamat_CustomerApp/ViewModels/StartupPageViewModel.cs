using Khadamat_CustomerApp.Helpers;
using Khadamat_CustomerApp.Models;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace Khadamat_CustomerApp.ViewModels
{
    public class StartupPageViewModel : BaseViewModel, INavigationAware
    {
        private readonly INavigationService NavigationService;
        //private string _CurrentOperation;
        //public string CurrentOperation
        //{
        //    get { return _CurrentOperation; }
        //    set { SetProperty(ref _CurrentOperation, value); }
        //}
        public StartupPageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        #region GetServiceData Api Method
        private async void GetServiceData()
        {
            try
            {
                if (Common.CheckConnection())
                {
                    CategoryListResponseModel response;
                    try
                    {
                        response = await _webApiRestClient.GetAsync<CategoryListResponseModel>(string.Format(ApiUrl.GetCategories, province_id));
                    }
                    catch (Exception ex)
                    {
                        response = null;
                    }
                    if (response != null)
                    {
                        if (response.status)
                        {
                            Application.Current.Properties["HomePageData"] = JsonConvert.SerializeObject(response.CategoryData);
                            Application.Current.SavePropertiesAsync();
                        }
                    }

                    var param = new NavigationParameters();
                    param.Add("HomePageData", response);
                    await NavigationService.NavigateAsync(new Uri("/MasterPage/NavigationPage/HomePage", UriKind.Absolute), param);
                }
                else
                {
                    var param = new NavigationParameters();
                    param.Add("HomePageData", null);
                    await NavigationService.NavigateAsync(new Uri("/MasterPage/NavigationPage/HomePage", UriKind.Absolute), param);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("featching Category data ::-->> " + ex.Message);
                await MaterialDialog.Instance.SnackbarAsync("Location Error Page", 2000);
            }
            finally
            {
            }
        }
        #endregion

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            
            Device.BeginInvokeOnMainThread(() =>
            {
                GetServiceData();
            });

        }
    }
}
