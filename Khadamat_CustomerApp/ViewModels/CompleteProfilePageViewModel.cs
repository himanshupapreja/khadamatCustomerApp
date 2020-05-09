using Khadamat_CustomerApp.DependencyInterface;
using Khadamat_CustomerApp.Helpers;
using Khadamat_CustomerApp.Models;
using Khadamat_CustomerApp.Resources;
using Khadamat_CustomerApp.Views;
using LiteDB;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using XF.Material.Forms.UI.Dialogs;
using static Khadamat_CustomerApp.Helpers.Enums;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System.Windows.Input;

namespace Khadamat_CustomerApp.ViewModels
{
    public class CompleteProfilePageViewModel : BaseViewModel
    {
        private readonly INavigationService NavigationService;

        public string AddressfromGPS;
        public Position AddressPositionfromGPS;
        VerifyResendOtpModel VerifyResendOtpModel;
        public Location location;
        public ImagesModel GallData;

        public int user_age;
        #region IsLocationFetch Field
        private bool _IsLocationFetch;

        public bool IsLocationFetch
        {
            get { return _IsLocationFetch; }
            set { SetProperty(ref _IsLocationFetch, value); }
        }
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

        #region UserPic Field
        private ImageSource _UserPic;

        public ImageSource UserPic
        {
            get { return _UserPic; }
            set { SetProperty(ref _UserPic, value); }
        }
        #endregion

        #region UserPicBytes Field
        private byte[] _UserPicBtyes;

        public byte[] UserPicBytes
        {
            get { return _UserPicBtyes; }
            set { SetProperty(ref _UserPicBtyes, value); }
        }
        #endregion

        #region Name Entry Field
        private string _Name;

        public string Name
        {
            get { return _Name; }
            set { SetProperty(ref _Name, value); }
        }
        #endregion

        #region Email Entry Field
        private string _Email;

        public string Email
        {
            get { return _Email; }
            set { SetProperty(ref _Email, value); }
        }
        #endregion

        #region DOB Entry Field
        private string _DOB;

        public string DOB
        {
            get { return _DOB; }
            set { SetProperty(ref _DOB, value); }
        }
        #endregion

        #region Country Entry Field
        private string _Country;

        public string Country
        {
            get { return _Country; }
            set { SetProperty(ref _Country, value); }
        }
        #endregion

        #region CurrentJob Entry Field
        private string _CurrentJob;

        public string CurrentJob
        {
            get { return _CurrentJob; }
            set { SetProperty(ref _CurrentJob, value); }
        }
        #endregion

        #region Marital Picker List
        private ObservableCollection<MaritalStatusPickerModel> _MaritalStatusList = new ObservableCollection<MaritalStatusPickerModel>();

        public ObservableCollection<MaritalStatusPickerModel> MaritalStatusList
        {
            get { return _MaritalStatusList; }
            set { SetProperty(ref _MaritalStatusList, value); }
        }
        #endregion

        #region Country Picker List
        private ObservableCollection<CountryDataModel> _CountryList = new ObservableCollection<CountryDataModel>();

        public ObservableCollection<CountryDataModel> CountryList
        {
            get { return _CountryList; }
            set { SetProperty(ref _CountryList, value); }
        }
        #endregion

        #region ProvinceList Picker
        private ObservableCollection<ProvienceDataModel> _ProvinceList = new ObservableCollection<ProvienceDataModel>();

        public ObservableCollection<ProvienceDataModel> ProvinceList
        {
            get { return _ProvinceList; }
            set { SetProperty(ref _ProvinceList, value); }
        }
        #endregion

        #region DOBYearList Picker
        private ObservableCollection<string> _DOBYearList = new ObservableCollection<string>(Enumerable.Range(DateTime.Now.Year - 70, 70 + 1).ToList().ConvertAll(delegate (int i) { return i.ToString(); }));

        public ObservableCollection<string> DOBYearList
        {
            get { return _DOBYearList; }
            set { SetProperty(ref _DOBYearList, value); }
        }
        #endregion

        #region DOBDateList Picker
        private ObservableCollection<string> _DOBDateList = new ObservableCollection<string>(Enumerable.Range(1, 31).ToList().ConvertAll(delegate (int i) { return string.Format(i < 10 ? "0{0}" : "{0}", i); }));

        public ObservableCollection<string> DOBDateList
        {
            get { return _DOBDateList; }
            set { SetProperty(ref _DOBDateList, value); }
        }
        #endregion

        #region DOBMonthList Picker
        //private ObservableCollection<string> _DOBMonthList = new ObservableCollection<string>(Enumerable.Range(1,12).Select(i=> Common.StringBuilderChars(System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(i).Take(3))).ToList());
        private ObservableCollection<string> _DOBMonthList = new ObservableCollection<string>(Enumerable.Range(1, 12).ToList().ConvertAll(delegate (int i) { return string.Format(i < 10 ? "0{0}" : "{0}", i); }));

        public ObservableCollection<string> DOBMonthList
        {
            get { return _DOBMonthList; }
            set { SetProperty(ref _DOBMonthList, value); }
        }
        #endregion

        #region Provience Picker Selectedindex
        private int _ProviencePickerSelectedindex;

        public int ProviencePickerSelectedindex
        {
            get { return _ProviencePickerSelectedindex; }
            set { SetProperty(ref _ProviencePickerSelectedindex, value); }
        }
        #endregion

        #region MaritalStatus Selected Field
        private MaritalStatusPickerModel _MaritalStatus = new MaritalStatusPickerModel();

        public MaritalStatusPickerModel MaritalStatus
        {
            get { return _MaritalStatus; }
            set { SetProperty(ref _MaritalStatus, value); }
        }
        #endregion

        #region Province Selected Field
        private ProvienceDataModel _Province;

        public ProvienceDataModel Province
        {
            get { return _Province; }
            set { SetProperty(ref _Province, value); }
        }
        #endregion

        #region MonthSelected Selected Field
        private string _MonthSelected;

        public string MonthSelected
        {
            get { return _MonthSelected; }
            set { SetProperty(ref _MonthSelected, value); }
        }
        #endregion

        #region DateSelected Selected Field
        private string _DateSelected;

        public string DateSelected
        {
            get { return _DateSelected; }
            set { SetProperty(ref _DateSelected, value); }
        }
        #endregion

        #region YearSelected Selected Field
        private string _YearSelected;

        public string YearSelected
        {
            get { return _YearSelected; }
            set { SetProperty(ref _YearSelected, value); }
        }
        #endregion

        #region Street Entry Field
        private string _Street;

        public string Street
        {
            get { return _Street; }
            set { SetProperty(ref _Street, value); }
        }
        #endregion

        #region Location Entry Field
        private string _Location;

        public string Location
        {
            get { return _Location; }
            set { SetProperty(ref _Location, value); }
        }
        #endregion

        #region Constructor
        public CompleteProfilePageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;

            MessagingCenter.Subscribe<string, Position>(this, "LocationAddress", (sender, pickedposition) =>
            {
                Street = AddressfromGPS = sender.ToString();
                AddressPositionfromGPS = pickedposition;
            });
            IsLocationFetch = false;
            IsLoaderBusy = false;

            //UserPic = "logo.png";
            IsLoaderBusy = false;
            Street = AppResource.cyp_StreetPlaceholder;
            DOB = AppResource.cyp_DOBPlaceholder;
            TermConditionCheck = "resource://Khadamat_CustomerApp.SvgImages.blank_check_box.svg";

            //GetCurrentlocation();
            MaritalStatusList.Add(new MaritalStatusPickerModel
            {
                MaritalStatusDisplay = Common.GetLanguage() != "ar-AE" ? Common.GetEnumDescription(MartialStatusEnum.Single) : Common.GetEnumDescription(MartialStatusArabicEnum.Single),
                MaritalStatusEnumValue = Convert.ToInt32(MartialStatusEnum.Single)
            });
            //MaritalStatusList.Add(new MaritalStatusPickerModel
            //{
            //    MaritalStatusDisplay = Common.GetLanguage() != "ar-AE" ? Common.GetEnumDescription(MartialStatusEnum.InARelationship) : Common.GetEnumDescription(MartialStatusArabicEnum.InARelationship),
            //    MaritalStatusEnumValue = Convert.ToInt32(MartialStatusEnum.InARelationship)
            //});
            //MaritalStatusList.Add(new MaritalStatusPickerModel
            //{
            //    MaritalStatusDisplay = Common.GetLanguage() != "ar-AE" ? Common.GetEnumDescription(MartialStatusEnum.Engaged) : Common.GetEnumDescription(MartialStatusArabicEnum.Engaged),
            //    MaritalStatusEnumValue = Convert.ToInt32(MartialStatusEnum.Engaged)
            //});
            MaritalStatusList.Add(new MaritalStatusPickerModel
            {
                MaritalStatusDisplay = Common.GetLanguage() != "ar-AE" ? Common.GetEnumDescription(MartialStatusEnum.Married) : Common.GetEnumDescription(MartialStatusArabicEnum.Married),
                MaritalStatusEnumValue = Convert.ToInt32(MartialStatusEnum.Married)
            });
            //MaritalStatusList.Add(new MaritalStatusPickerModel
            //{
            //    MaritalStatusDisplay = Common.GetLanguage() != "ar-AE" ? Common.GetEnumDescription(MartialStatusEnum.ItsComplicated) : Common.GetEnumDescription(MartialStatusArabicEnum.ItsComplicated),
            //    MaritalStatusEnumValue = Convert.ToInt32(MartialStatusEnum.ItsComplicated)
            //});
            //MaritalStatusList.Add(new MaritalStatusPickerModel
            //{
            //    MaritalStatusDisplay = Common.GetLanguage() != "ar-AE" ? Common.GetEnumDescription(MartialStatusEnum.InAnOpenRelationship) : Common.GetEnumDescription(MartialStatusArabicEnum.InAnOpenRelationship),
            //    MaritalStatusEnumValue = Convert.ToInt32(MartialStatusEnum.InAnOpenRelationship)
            //});
            MaritalStatusList.Add(new MaritalStatusPickerModel
            {
                MaritalStatusDisplay = Common.GetLanguage() != "ar-AE" ? Common.GetEnumDescription(MartialStatusEnum.Divorced) : Common.GetEnumDescription(MartialStatusArabicEnum.Divorced),
                MaritalStatusEnumValue = Convert.ToInt32(MartialStatusEnum.Divorced)
            });
            if (BaseViewModel.countryDataModels != null && BaseViewModel.countryDataModels.Count > 0 && BaseViewModel.provienceDataModels != null && BaseViewModel.provienceDataModels.Count > 0)
            {
                if (Application.Current.Properties.ContainsKey("AppLocale") && !string.IsNullOrEmpty(Application.Current.Properties["AppLocale"].ToString()))
                {
                    var languageculture = Application.Current.Properties["AppLocale"].ToString();
                    Country = languageculture.Equals("en-US") ? BaseViewModel.countryDataModels.FirstOrDefault().country_name : BaseViewModel.countryDataModels.FirstOrDefault().arabic_country_name;
                }
                else
                {
                    Country = BaseViewModel.countryDataModels.FirstOrDefault().country_name;
                }

                foreach (var item in BaseViewModel.provienceDataModels)
                {
                    if (Application.Current.Properties.ContainsKey("AppLocale") && !string.IsNullOrEmpty(Application.Current.Properties["AppLocale"].ToString()))
                    {
                        var languageculture = Application.Current.Properties["AppLocale"].ToString();
                        item.display_province_name = languageculture.Equals("en-US") ? item.province_name : item.arabic_province_name;
                    }
                    else
                    {
                        item.display_province_name = item.province_name;
                    }
                    ProvinceList.Add(item);
                }


                ProviencePickerSelectedindex = ProvinceList.IndexOf(ProvinceList.Where(x => x.province_name.ToLower().Contains("sana") || x.arabic_province_name.Contains("sana")).ToList().FirstOrDefault());
            }
            //GetCountriesApi();

            MessagingCenter.Subscribe<ImagesModel>(this, "ProfilePicture", (sender) =>
            {
                GallData = sender;
                UserPic = GallData.Image;
            });

            MessagingCenter.Send("CompleteProfilePage", "CompleteProfilePage");
        }
        #endregion

        #region ResendOtpApiCall
        private async void ResendOtpApiCall(VerifyResendOtpModel requestModel)
        {
            try
            {
                if (Common.CheckConnection())
                {

                }
                else
                {
                    await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_InternetError, msDuration: 3000);
                }
            }
            catch (Exception ec)
            {
            }
            finally
            {
                IsLoaderBusy = false;
            }
        }
        #endregion

        #region GetCurrentlocation
        private async void GetCurrentlocation()
        {
            IsLoaderBusy = true;
            IsLocationFetch = true;
            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                location = await Geolocation.GetLocationAsync(request);
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

                IEnumerable<string> pickedaddress = await gc.GetAddressesForPositionAsync(new Position(location.Latitude, location.Longitude));

                Street = pickedaddress.FirstOrDefault().ToString();

                //if (!string.IsNullOrEmpty(Street) && !string.IsNullOrWhiteSpace(Street) && Street != AppResource.cyp_StreetPlaceholder)
                //{
                //    await App.Current.MainPage.DisplayAlert("", AppResource.cyp_LocationPickupAlert, "OK");
                //}
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("GetAddressCurrent2_CurrentLocation" + ex.Message);
            }

            IsLoaderBusy = false;
            IsLocationFetch = false;
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
                        //Common.CustomNavigation(_navigation, new TermConditionPage());
                        NavigationService.NavigateAsync(nameof(TermConditionPage));
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

        #region MapLocationCommand
        public Command MapLocationCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(Street) && !string.IsNullOrWhiteSpace(Street) && Street != AppResource.cyp_StreetPlaceholder)
                        {
                            //Common.CustomNavigation(_navigation, new CustomMapPage(locationpoint));
                            var param = new NavigationParameters();
                            param.Add("CurrentLocationPoints", location);
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
                                        var result = await App.Current.MainPage.DisplayAlert("", AppResource.error_AppLocationPermissionDisable, AppResource.Yes, AppResource.No);
                                        if (result)
                                        {
                                            CrossPermissions.Current.OpenAppSettings();
                                        }

                                    }
                                    else
                                    {
                                        GetCurrentlocation();
                                    }
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                            else
                            {
                                var result = await App.Current.MainPage.DisplayAlert("", AppResource.error_DeviceLocationDisable, AppResource.Yes, AppResource.No);
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

                    }
                    finally
                    {
                        //IsLoaderBusy = false;
                    }
                });
            }
        }
        #endregion

        #region ChangeUserPicCommand
        public Command ChangeUserPicCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        var action = await App.Current.MainPage.DisplayActionSheet(AppResource.AddPhoto, AppResource.Cancel, null, AppResource.Camera, AppResource.Gallery);

                        if (action == AppResource.Camera)
                        {
                            await CameraCommand();
                        }
                        else if (action == AppResource.Gallery)
                        {
                            await GalleryCommand();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("ProfilePicCommand_Exception:- " + ex.Message);
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
                return new Command(async () =>
                {
                    try
                    {
                        if (Common.CheckConnection())
                        {
                            if (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(DateSelected) && !string.IsNullOrEmpty(MonthSelected) && !string.IsNullOrEmpty(YearSelected) && Country != null && Province != null && !string.IsNullOrEmpty(Location) && !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(DateSelected) && !string.IsNullOrWhiteSpace(MonthSelected) && !string.IsNullOrWhiteSpace(YearSelected) && !string.IsNullOrWhiteSpace(Location))
                            {
                                DOB = string.Format("{0}/{1}/{2}", DateSelected, MonthSelected, YearSelected);
                                if (TermConditionCheck.Contains("ic_checkbox"))
                                {
                                    if (DateTime.ParseExact(DOB, "dd/MM/yyyy", null) > DateTime.Now)
                                    {
                                        await MaterialDialog.Instance.SnackbarAsync(AppResource.error_DOBError, 3000);
                                        return;
                                    }
                                    user_age = Common.CalculateAge(DateTime.ParseExact(DOB, "dd/MM/yyyy", null));
                                    if (!string.IsNullOrEmpty(Email) && !string.IsNullOrWhiteSpace(Email))
                                    {
                                        Email = Email.Trim();
                                        if (!Common.CheckValidEmail(Email))
                                        {
                                            await MaterialDialog.Instance.SnackbarAsync(AppResource.error_EmailValidation, 3000);
                                            return;
                                        }
                                    }

                                    if (user_age < 18)
                                    {
                                        await MaterialDialog.Instance.SnackbarAsync(AppResource.error_underAge, 3000);
                                    }
                                    else
                                    {
                                        IsLoaderBusy = true;
                                        string boundary = "---8d0f01e6b3b5dafaaadaad";
                                        MultipartFormDataContent multipartContent = new MultipartFormDataContent(boundary);
                                        multipartContent.Add(new StringContent(Common.FirstCharToUpper(Name)), "name");
                                        if (!string.IsNullOrEmpty(Email))
                                        {
                                            multipartContent.Add(new StringContent(Email), "email");
                                        }
                                        multipartContent.Add(new StringContent(DOB), "dob");
                                        multipartContent.Add(new StringContent(user_id.ToString()), "user_id");

                                        multipartContent.Add(new StringContent(BaseViewModel.countryDataModels.FirstOrDefault().country_id.ToString()), "country");
                                        if (!string.IsNullOrEmpty(CurrentJob))
                                        {
                                            multipartContent.Add(new StringContent(Common.FirstCharToUpper(CurrentJob)), "current_job");
                                        }
                                        multipartContent.Add(new StringContent(MaritalStatus.MaritalStatusEnumValue.ToString()), "martial_status");
                                        multipartContent.Add(new StringContent(Province.province_id.ToString()), "province");
                                        if (Street != AppResource.cyp_StreetPlaceholder)
                                        {
                                            multipartContent.Add(new StringContent(Street), "street");
                                            multipartContent.Add(new StringContent(location.Latitude.ToString()), "latitude");
                                            multipartContent.Add(new StringContent(location.Longitude.ToString()), "longitude");
                                        }
                                        if (!string.IsNullOrEmpty(Location))
                                        {
                                            multipartContent.Add(new StringContent(Location), "description_location");
                                        }


                                        try
                                        {
                                            if (GallData.ImageBytes != null)
                                            {
                                                //StreamImageSource streamImageSource = (StreamImageSource)item.ReferenceImages;
                                                //System.Threading.CancellationToken cancellationToken = System.Threading.CancellationToken.None;
                                                //System.Threading.Tasks.Task<System.IO.Stream> task =
                                                //    streamImageSource.Stream(cancellationToken);
                                                //System.IO.Stream stream = task.Result;

                                                //bitmapData = ReadFully(stream);
                                                var fileContent = new ByteArrayContent(GallData.ImageBytes);

                                                fileContent.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/octet-stream");
                                                fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                                                {
                                                    Name = "PicturePath",
                                                    FileName = "profilepic" + ".png",
                                                };

                                                multipartContent.Add(fileContent);
                                            }
                                        }
                                        catch (Exception)
                                        {
                                        }


                                        SignUpResponseModel response;
                                        try
                                        {
                                            response = await _webApiRestClient.PostAsync<MultipartFormDataContent, SignUpResponseModel>(ApiUrl.SignUp, multipartContent);
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine("SignupApi_Exception:- " + ex.Message);
                                            response = null;
                                            await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_ServerError, msDuration: 3000);
                                            IsLoaderBusy = false;
                                            return;
                                        }
                                        if (response != null)
                                        {
                                            if (response.status)
                                            {
                                                var deviceRequestmodel = new UpdateDeviceInfoModel()
                                                {
                                                    device_id = Device.RuntimePlatform == Device.Android ? 1 : Device.RuntimePlatform == Device.iOS ? 2 : 0,
                                                    user_id = response.userData.user_id,
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
                                                //if (deviceInfoResponse != null)
                                                //{
                                                //    if (deviceInfoResponse.status)
                                                //    {
                                                //        await MaterialDialog.Instance.SnackbarAsync(message: deviceInfoResponse.message, msDuration: 3000);
                                                //    }
                                                //    else
                                                //    {
                                                //        await MaterialDialog.Instance.SnackbarAsync(message: deviceInfoResponse.message, msDuration: 3000);
                                                //    }
                                                //}

                                                user_id = response.userData.user_id;
                                                user_name = response.userData.name;
                                                user_pic = Common.IsImagesValid(response.userData.profile_pic, ApiUrl.BaseUrl);

                                                if (userDataDbService.IsUserDbPresentInDB())
                                                {
                                                    var item = userDataDbService.ReadAllItems().FirstOrDefault();
                                                    BsonValue id = item.ID;
                                                    userDataDbService.UpdateUserDataInDb(id, response.userData);
                                                }
                                                else
                                                {
                                                    userDataDbService.CreateUserDataInDB(response.userData);
                                                }
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
                                                //App.Current.MainPage = new NavigationPage(new MasterPage());
                                                await NavigationService.NavigateAsync(new Uri("/MasterPage/NavigationPage/HomePage", UriKind.Absolute));
                                                #region Resend_Otp_Api_Call
                                                //if (response.userData.email_verified.HasValue && response.userData.email_verified.Value)
                                                //{
                                                //    App.Current.MainPage = new NavigationPage(new MasterPage());
                                                //}
                                                //else
                                                //{
                                                //    VerifyResendOtpModel = new VerifyResendOtpModel()
                                                //    {
                                                //        email = response.userData.email,
                                                //        type = 2,
                                                //        user_id = response.userData.user_id
                                                //    };

                                                //    VerifyResendOtpResponseModel otpresponse;
                                                //    try
                                                //    {
                                                //        otpresponse = await _webApiRestClient.PostAsync<VerifyResendOtpModel, VerifyResendOtpResponseModel>(ApiUrl.ResendOtp, VerifyResendOtpModel);
                                                //    }
                                                //    catch (Exception ex)
                                                //    {
                                                //        otpresponse = null;
                                                //        IsLoaderBusy = false;
                                                //        await MaterialDialog.Instance.SnackbarAsync(AppResource.error_ServerError, 3000);
                                                //        return;
                                                //    }
                                                //    if (otpresponse != null)
                                                //    {
                                                //        Device.BeginInvokeOnMainThread(async () =>
                                                //        {
                                                //            if (otpresponse.status)
                                                //            {
                                                //                await MaterialDialog.Instance.SnackbarAsync(otpresponse.message, 3000);
                                                //                Common.CustomNavigation(_navigation, new EmailVerificationOtpPage(VerifyResendOtpModel));
                                                //            }
                                                //            else
                                                //            {
                                                //                await MaterialDialog.Instance.SnackbarAsync(otpresponse.message, 3000);
                                                //            }
                                                //        });
                                                //    }
                                                //}
                                                #endregion

                                                MessagingCenter.Unsubscribe<string>(this, "LocationAddress");
                                            }
                                            else
                                            {
                                                await MaterialDialog.Instance.SnackbarAsync(message: response.message,
                                                            msDuration: 3000);
                                            }
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
                                await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_BlankRequiredField, msDuration: 3000);
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

        #region UpdateLanguageServer
        private async void UpdateLanguageServer(ChangeLanguagesModel request)
        {
            var result = await _webApiRestClient.PostAsync<ChangeLanguagesModel, CommonResponseModel>(ApiUrl.ChangeLanguage, request);
        }
        #endregion
    }
}
