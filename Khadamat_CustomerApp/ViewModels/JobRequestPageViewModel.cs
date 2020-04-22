using Khadamat_CustomerApp.DependencyInterface;
using Khadamat_CustomerApp.Helpers;
using Khadamat_CustomerApp.Models;
using Khadamat_CustomerApp.Resources;
using Khadamat_CustomerApp.Views;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using XF.Material.Forms.UI.Dialogs;

namespace Khadamat_CustomerApp.ViewModels
{
    public class JobRequestPageViewModel : BaseViewModel, INavigationAware
    {
        bool IsJobRequested;
        private readonly INavigationService NavigationService;
        public static string CategoryTermCondition;
        #region SelectedSubSubService
        private SubSubCategory SelectedSubSubService;
        public Location locationpoint;
        #endregion

        #region IsLoaderBusy Field
        private bool _IsLoaderBusy;

        public bool IsLoaderBusy
        {
            get { return _IsLoaderBusy; }
            set { SetProperty(ref _IsLoaderBusy, value); }
        }
        #endregion

        #region TermConditionCheck
        private string _TermConditionCheck;

        public string TermConditionCheck
        {
            get { return _TermConditionCheck; }
            set { SetProperty(ref _TermConditionCheck, value); }
        }
        #endregion

        #region IsPopupVisible
        private bool _IsPopupVisible;

        public bool IsPopupVisible
        {
            get { return _IsPopupVisible; }
            set { SetProperty(ref _IsPopupVisible, value); }
        }
        #endregion

        #region CategoryName
        private string _CategoryName;

        public string CategoryName
        {
            get { return _CategoryName; }
            set { SetProperty(ref _CategoryName, value); }
        }
        #endregion

        #region CategoryServiceName
        private string _CategoryServiceName;

        public string CategoryServiceName
        {
            get { return _CategoryServiceName; }
            set { SetProperty(ref _CategoryServiceName, value); }
        }
        #endregion

        #region JobDateValue
        private string _JobDateValue;

        public string JobDateValue
        {
            get { return _JobDateValue; }
            set { SetProperty(ref _JobDateValue, value); }
        }
        #endregion

        #region JobTimeValue
        private string _JobTimeValue;

        public string JobTimeValue
        {
            get { return _JobTimeValue; }
            set { SetProperty(ref _JobTimeValue, value); }
        }
        #endregion

        #region JobDateTimeValue
        private string _JobDateTimeValue;

        public string JobDateTimeValue
        {
            get { return _JobDateTimeValue; }
            set { SetProperty(ref _JobDateTimeValue, value); }
        }
        #endregion

        #region JobDateTime
        private DateTime _JobDateTime;

        public DateTime JobDateTime
        {
            get { return _JobDateTime; }
            set { SetProperty(ref _JobDateTime, value); }
        }
        #endregion

        #region Location
        private string _Location;

        public string Location
        {
            get { return _Location; }
            set { SetProperty(ref _Location, value); }
        }
        #endregion

        #region VenueName
        private string _VenueName;

        public string VenueName
        {
            get { return _VenueName; }
            set { SetProperty(ref _VenueName, value); }
        }
        #endregion

        #region ServiceDescriptionValue
        private string _ServiceDescriptionValue;

        public string ServiceDescriptionValue
        {
            get { return _ServiceDescriptionValue; }
            set { SetProperty(ref _ServiceDescriptionValue, value); }
        }
        #endregion

        #region Constructor
        public JobRequestPageViewModel(INavigationService navigationService)
        {
            IsJobRequested = false;
            NavigationService = navigationService;
            IsLoaderBusy = false;
            IsPopupVisible = false;
            Location = AppResource.cyp_StreetPlaceholder;
            //GetCurrentlocation();
            TermConditionCheck = "resource://Khadamat_CustomerApp.SvgImages.blank_check_box.svg";
            JobDateTimeValue = DateTime.Now.ToString("dd-MMM-yyyy, hh:mm tt");
            JobDateTime = DateTime.Now;
            JobDateValue = DateTime.Now.ToString("dd-MMM-yyyy");
            JobTimeValue = DateTime.Now.ToString("hh:mm tt");
        }
        #endregion

        #region GetCurrentlocation
        private async void GetCurrentlocation()
        {
            IsLoaderBusy = true;
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                locationpoint = await Geolocation.GetLocationAsync(request);
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }

            try
            {
                Geocoder gc = new Geocoder();

                IEnumerable<string> pickedaddress = await gc.GetAddressesForPositionAsync(new Position(locationpoint.Latitude, locationpoint.Longitude));

                Location = pickedaddress.FirstOrDefault().ToString();

                if (!string.IsNullOrEmpty(Location) && !string.IsNullOrWhiteSpace(Location) && Location != AppResource.cyp_StreetPlaceholder)
                {
                    //await App.Current.MainPage.DisplayAlert("", AppResource.cyp_LocationPickupAlert, "OK");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("GetAddressCurrent2_CurrentLocation" + ex.Message);
            }
            finally
            {
                IsLoaderBusy = false;
            }
        }
        #endregion

        #region LocationCommand
        public Command LocationCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        if (Common.CheckConnection())
                        {
                            if (DependencyService.Get<IDeviceLocationService>().CheckDeviceLocation())
                            {
                                try
                                {
                                    var resultrs = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);

                                    if (resultrs != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                                    {
                                        var result = await App.Current.MainPage.DisplayAlert("", AppResource.error_AppLocationPermissionDisable, "Yes", "No");
                                        if (result)
                                        {
                                            CrossPermissions.Current.OpenAppSettings();
                                        }

                                    }
                                    else
                                    {
                                        //IsLoaderBusy = true;
                                        GetCurrentlocation();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //IsLoaderBusy = false;
                                }
                            }
                            else
                            {
                                var result = await App.Current.MainPage.DisplayAlert("", AppResource.error_DeviceLocationDisable, "Yes", "No");
                                if (result)
                                {
                                    DependencyService.Get<IDeviceLocationService>().OpenDeviceSetting();
                                }
                            }
                        }
                        else
                        {
                            await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_InternetError,
                                                    msDuration: 3000);
                        }
                    }
                    catch (Exception)
                    {
                        //IsLoaderBusy = false;
                    }
                    finally
                    {
                        //IsLoaderBusy = false;
                    }
                });
            }
        }
        #endregion

        #region TermConditionCheckCommand
        public Command TermConditionCheckCommand
        {
            get
            {
                return new Command(() =>
                {
                    try
                    {
                        switch (TermConditionCheck.Contains("blank_check_box"))
                        {
                            case true:
                                TermConditionCheck = "resource://Khadamat_CustomerApp.SvgImages.ic_checkbox.svg";
                                break;
                            case false:
                                TermConditionCheck = "resource://Khadamat_CustomerApp.SvgImages.blank_check_box.svg";
                                break;
                        }
                    }
                    catch (Exception)
                    {

                    }
                    finally
                    {

                    }
                });
            }
        }
        #endregion

        #region TermConditionCommand
        public Command TermConditionCommand
        {
            get
            {
                return new Command(() =>
                {
                    try
                    {
                        try
                        {
                            var param = new NavigationParameters();
                            param.Add("CategoryTermCondition", CategoryTermCondition);
                            NavigationService.NavigateAsync(nameof(TermConditionPage), param);
                        }
                        catch (Exception)
                        {
                        }

                    }
                    catch (Exception)
                    {

                    }
                    finally
                    {

                    }
                });
            }
        }
        #endregion

        #region DateTimePickerCommand
        public Command DateTimePickerCommand
        {
            get
            {
                return new Command((e) =>
                {
                    try
                    {
                        //MessagingCenter.Send("DateTimePicker", "DateTimePicker");
                        JobRequestPage.newdatePicker.Focus();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("DateTimePickerCommand_Exception:- " + ex.Message);
                    }
                });
            }
        }
        #endregion

        #region SubmitBtnCommand
        public Command SubmitBtnCommand
        {
            get
            {
                return new Command(async (e) =>
                {
                    if (!IsJobRequested)
                    {
                        IsJobRequested = true;
                        try
                        {
                            if (Common.CheckConnection())
                            {
                                await App.Current.MainPage.Navigation.PushPopupAsync(new LoaderPopup());
                                if (!string.IsNullOrEmpty(VenueName) && !string.IsNullOrEmpty(ServiceDescriptionValue) && !string.IsNullOrWhiteSpace(VenueName) && !string.IsNullOrWhiteSpace(ServiceDescriptionValue))
                                {
                                    if (TermConditionCheck.Contains("ic_checkbox"))
                                    {
                                        var jobrequest = new AddJobRequestModel()
                                        {
                                            category_id = SelectedSubSubService.category_id,
                                            description = ServiceDescriptionValue,
                                            job_date_time = JobDateTime,
                                            job_date_time_str = JobDateTime.ToString("dd-MM-yyyy, hh:mm tt"),
                                            sub_sub_category_id = SelectedSubSubService.sub_sub_category_id,
                                            user_id = (int)user_id,
                                            venue = VenueName,
                                            location = !string.IsNullOrEmpty(Location) && !string.IsNullOrWhiteSpace(Location) && Location != AppResource.cyp_StreetPlaceholder ? Location : null,
                                            latitude = locationpoint != null ? locationpoint.Latitude : 0,
                                            longitude = locationpoint != null ? locationpoint.Longitude : 0,
                                        };
                                        AddJobRequestResponseModel response;
                                        try
                                        {
                                            response = await _webApiRestClient.PostAsync<AddJobRequestModel, AddJobRequestResponseModel>(ApiUrl.AddJobRequest, jobrequest);
                                        }
                                        catch (Exception ex)
                                        {
                                            response = null;
                                            await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_ServerError, msDuration: 3000);
                                            await Task.Delay(1500);
                                            LoaderPopup.CloseAllPopup();
                                            return;
                                        }
                                        if (response != null)
                                        {
                                            if (response.status)
                                            {
                                                //IsLoaderBusy = false;
                                                IsPopupVisible = true;
                                            }
                                            else
                                            {
                                                //IsLoaderBusy = false;
                                                await MaterialDialog.Instance.SnackbarAsync(message: response.message,
                                                            msDuration: 3000);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_TermCondition, msDuration: 3000);
                                    }
                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(VenueName) || string.IsNullOrWhiteSpace(VenueName))
                                    {
                                        await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_JobRequestVenueBlank,
                                                        msDuration: 3000);
                                    }
                                    else if (string.IsNullOrEmpty(ServiceDescriptionValue) || string.IsNullOrWhiteSpace(ServiceDescriptionValue))
                                    {
                                        await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_JobRequestServiceDescriptionBlank,
                                                        msDuration: 3000);
                                    }
                                }
                            }
                            else
                            {
                                await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_InternetError,
                                                        msDuration: 3000);
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                        finally
                        {
                            await Task.Delay(1500);
                            IsJobRequested = false;
                            LoaderPopup.CloseAllPopup();
                        } 
                    }
                });
            }
        }
        #endregion

        #region MapLocationCommand
        public Command MapLocationCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(Location) && !string.IsNullOrWhiteSpace(Location) && Location != AppResource.cyp_StreetPlaceholder)
                        {
                            //Common.CustomNavigation(_navigation, new CustomMapPage(locationpoint));
                            var param = new NavigationParameters();
                            param.Add("CurrentLocationPoints", locationpoint);
                            await NavigationService.NavigateAsync(nameof(CustomMapPage), param);
                        }
                    }
                    catch (Exception)
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

        #region OkBtnCommand
        public Command OkBtnCommand
        {
            get
            {
                return new Command(() =>
                {
                    try
                    {
                        //App.Current.MainPage = new NavigationPage(new MasterPage());
                        NavigationService.GoBackToRootAsync();
                    }
                    catch (Exception ex)
                    {

                    }
                });
            }
        }
        #endregion

        #region PopupCloseCommand
        public Command PopupCloseCommand
        {
            get
            {
                return new Command(() =>
                {
                    try
                    {
                        //IsPopupVisible = false;
                    }
                    catch (Exception ex)
                    {

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
            if (parameters.ContainsKey("SelectedCategories"))
            {
                SelectedSubSubService = (SubSubCategory)parameters["SelectedCategories"];
                CategoryName = Common.GetLanguage() != "ar-AE" ? SelectedSubSubService.category_name : SelectedSubSubService.category_name_arabic;
                CategoryServiceName = SelectedSubSubService.sub_sub_category_name;
            }
            if (parameters.ContainsKey("CategoryTermCondition"))
            {
                CategoryTermCondition = (string)parameters["CategoryTermCondition"];
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

