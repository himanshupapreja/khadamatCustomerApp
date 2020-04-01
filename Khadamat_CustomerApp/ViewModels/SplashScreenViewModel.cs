using Khadamat_CustomerApp.Helpers;
using Khadamat_CustomerApp.Models;
using Khadamat_CustomerApp.Resources;
using Khadamat_CustomerApp.Services.ApiService;
using Khadamat_CustomerApp.Services.DBService.LiteDB.ModelDB;
using Khadamat_CustomerApp.Views;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace Khadamat_CustomerApp.ViewModels
{
    public class SplashScreenViewModel : BaseViewModel, INavigationAware
    {
        private readonly INavigationService NavigationService;
        UserModel userdataindb;
        public SplashScreenViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
            Xamarin.Essentials.Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
            
        }
        private void Connectivity_ConnectivityChanged(object sender, Xamarin.Essentials.ConnectivityChangedEventArgs e)
        {
            if ((e.ConnectionProfiles.Contains(Xamarin.Essentials.ConnectionProfile.WiFi) || e.ConnectionProfiles.Contains(Xamarin.Essentials.ConnectionProfile.Cellular)) && e.NetworkAccess == Xamarin.Essentials.NetworkAccess.Internet)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    if (userdataindb != null)
                    {
                        UpdateDeviceInfo();
                    }
                });
                Device.BeginInvokeOnMainThread(() =>
                {
                    GetCountriesApi();
                });
            }
        }


        #region GetProvienceApi Api Call Method
        private async void GetProvienceApi(int country_id)
        {
            if (Common.CheckConnection())
            {
                ProvienceModel response;
                try
                {
                    response = await _webApiRestClient.GetAsync<ProvienceModel>(string.Format(ApiUrl.GetProvinces, country_id));
                }
                catch (Exception ex)
                {
                    response = null;
                    await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_ServerError, msDuration: 3000);
                    //countryDetailData = null;
                }
                if (response != null)
                {
                    if (response.status)
                    {
                        //foreach (var item in response.data)
                        //{
                        //    ViewModelBase.provienceDataModels.Add(item);
                        //}
                        BaseViewModel.provienceDataModels = response.data;
                        Application.Current.Properties["ProvinceList"] = JsonConvert.SerializeObject(response.data);
                        App.Current.SavePropertiesAsync();
                        //countryDetailData = response.data;
                        //foreach (var data in countryDetailData)
                        //{
                        //    CountryPickerList.Add(data);
                        //    CountryCodeList.Add(data);
                        //}
                    }
                    else
                    {
                        await MaterialDialog.Instance.SnackbarAsync(message: response.message,
                                    msDuration: 3000);
                        //countryDetailData = null;
                    }
                }
            }
            else
            {
                await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_InternetError,
                                        msDuration: 3000);
            }
        }
        #endregion

        #region GetCountriesApi Api Call Method
        private async void GetCountriesApi()
        {
            if (Common.CheckConnection())
            {
                CountriesModel response;
                try
                {
                    response = await _webApiRestClient.GetAsync<CountriesModel>(ApiUrl.GetCountries);
                }
                catch (Exception ex)
                {
                    response = null;
                    await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_ServerError, msDuration: 3000);
                    //countryDetailData = null;
                }
                if (response != null)
                {
                    if (response.status)
                    {
                        BaseViewModel.countryDataModels = response.data;
                        Application.Current.Properties["CountryList"] = JsonConvert.SerializeObject(response.data);
                        App.Current.SavePropertiesAsync();
                        GetProvienceApi(BaseViewModel.countryDataModels.FirstOrDefault().country_id);
                    }
                    else
                    {
                        await MaterialDialog.Instance.SnackbarAsync(message: response.message,
                                    msDuration: 3000);
                        //countryDetailData = null;
                    }
                }
            }
            else
            {
                await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_InternetError,
                                        msDuration: 3000);
            }
        }
        #endregion

        #region UpdateDeviceInfo
        private async void UpdateDeviceInfo()
        {
            var deviceRequestmodel = new UpdateDeviceInfoModel()
            {
                device_id = Device.RuntimePlatform == Device.Android ? 1 : Device.RuntimePlatform == Device.iOS ? 2 : 0,
                user_id = userdataindb.user_id,
                device_token = Application.Current.Properties.ContainsKey("AppFirebaseToken") ? Application.Current.Properties["AppFirebaseToken"].ToString() : null
            };
            UpdateDeviceInfoResponse deviceInfoResponse;
            try
            {
                deviceInfoResponse = await _webApiRestClient.PostAsync<UpdateDeviceInfoModel, UpdateDeviceInfoResponse>(ApiUrl.UpdateDeviceInfo, deviceRequestmodel);
            }
            catch (Exception ex)
            {
                deviceInfoResponse = null;
            }
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            //Task.Delay(TimeSpan.FromSeconds(30));

            //if (Application.Current.Properties.ContainsKey("CountryList"))
            //{
            //    BaseViewModel.countryDataModels = JsonConvert.DeserializeObject<List<CountryDataModel>>(Application.Current.Properties["CountryList"].ToString());
            //}
            //if (Application.Current.Properties.ContainsKey("ProvinceList"))
            //{
            //    BaseViewModel.provienceDataModels = JsonConvert.DeserializeObject<List<ProvienceDataModel>>(Application.Current.Properties["ProvinceList"].ToString());
            //}

            //if (userDataDbService.IsUserDbPresentInDB())
            //{
            //    userdataindb = userDataDbService.ReadAllItems().FirstOrDefault();
            //    BaseViewModel.user_id = userdataindb.user_id;
            //    BaseViewModel.user_name = userdataindb.name;
            //    BaseViewModel.user_pic = Common.IsImagesValid(userdataindb.profile_pic, ApiUrl.BaseUrl);
            //    BaseViewModel.email_verified = userdataindb.email_verified.HasValue ? userdataindb.email_verified.Value : false;

            //    //MainPage = userdataindb.email_verified.HasValue && userdataindb.email_verified.Value ? new NavigationPage(new MasterPage()) : new NavigationPage(new LoginPage());
            //    //MainPage = new NavigationPage(new MasterPage());
            //    //await NavigationService.NavigateAsync(new Uri("/"+nameof(MasterPage)+"/" + nameof(NavigationPage)+"/"+nameof(HomePage), UriKind.Absolute));
            //    NavigationService.NavigateAsync(new Uri("/MasterPage/NavigationPage/HomePage", UriKind.Absolute));
            //}
            //else
            //{
            //    //MainPage = new NavigationPage(new LoginPage());
            //    NavigationService.NavigateAsync("NavigationPage/LoginPage");
            //}

            //Device.BeginInvokeOnMainThread(() =>
            //{
            //    UpdateDeviceInfo();
            //});

            //Device.BeginInvokeOnMainThread(() =>
            //{
            //    GetCountriesApi();
            //});
        }
        #endregion
    }
}
