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
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using XF.Material.Forms.UI.Dialogs;
using static Khadamat_CustomerApp.Helpers.Enums;

namespace Khadamat_CustomerApp.ViewModels
{
    public class EditProfilePageViewModel : BaseViewModel, INavigationAware
    {
        UserModel profiledata;

        private readonly INavigationService NavigationService;
        List<string> PhoneStartingValue = new List<string>()
        {
            "77","71","73","70"
        };
        public string AddressfromGPS;
        public static bool IsPermissionSettingOpen;
        public Position AddressPositionfromGPS;
        public Xamarin.Essentials.Location location;
        public ImagesModel GallData;
        public int user_age;
        #region IsLoaderBusy Field
        private bool _IsLoaderBusy;

        public bool IsLoaderBusy
        {
            get { return _IsLoaderBusy; }
            set { SetProperty(ref _IsLoaderBusy, value); }
        }
        #endregion
        #region IsLocationFetch Field
        private bool _IsLocationFetch;

        public bool IsLocationFetch
        {
            get { return _IsLocationFetch; }
            set { SetProperty(ref _IsLocationFetch, value); }
        }
        #endregion

        #region Name
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

        #region UserPic Field
        private ImageSource _UserPic;

        public ImageSource UserPic
        {
            get { return _UserPic; }
            set { SetProperty(ref _UserPic, value); }
        }
        #endregion

        #region UserModel
        private UserModel _profiledata = new UserModel();
        public UserModel Profiledata
        {
            get { return _profiledata; }
            set { SetProperty(ref _profiledata, value); }
        }
        #endregion

        #region DOB
        private string _DOB;

        public string DOB
        {
            get { return _DOB; }
            set { SetProperty(ref _DOB, value); }
        }
        #endregion

        #region PhoneNumber
        private string _PhoneNumber;

        public string PhoneNumber
        {
            get { return _PhoneNumber; }
            set
            {
                SetProperty(ref _PhoneNumber, value);
                if (!string.IsNullOrEmpty(PhoneNumber) && !string.IsNullOrWhiteSpace(PhoneNumber) && PhoneNumber.Length > 9)
                {
                    PhoneNumber = Common.StringBuilderChars(PhoneNumber.Take(9));
                }
            }
        }
        #endregion

        #region CurrentJob
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
        private ObservableCollection<string> _DOBMonthList = new ObservableCollection<string>(Enumerable.Range(1, 12).ToList().ConvertAll(delegate (int i) { return string.Format(i < 10 ? "0{0}" : "{0}", i); }));

        public ObservableCollection<string> DOBMonthList
        {
            get { return _DOBMonthList; }
            set { SetProperty(ref _DOBMonthList, value); }
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

        #region Country Selected Field
        private string _Country;

        public string Country
        {
            get { return _Country; }
            set
            {
                SetProperty(ref _Country, value);
            }
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

        #region Street
        private string _Street;

        public string Street
        {
            get { return _Street; }
            set { SetProperty(ref _Street, value); }
        }
        #endregion

        #region MaritalStatus Picker Selectedindex
        private int _MaritalStatusPickerSelectedindex;

        public int MaritalStatusPickerSelectedindex
        {
            get { return _MaritalStatusPickerSelectedindex; }
            set { SetProperty(ref _MaritalStatusPickerSelectedindex, value); }
        }
        #endregion

        #region CountryPicker SelectedIndexChanged
        private int _CountryPickerSelectedIndexChanged;

        public int CountryPickerSelectedIndexChanged
        {
            get { return _CountryPickerSelectedIndexChanged; }
            set { SetProperty(ref _CountryPickerSelectedIndexChanged, value); }
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

        #region DOBDatePickerSelectedindex
        private int _DOBDatePickerSelectedindex;

        public int DOBDatePickerSelectedindex
        {
            get { return _DOBDatePickerSelectedindex; }
            set { SetProperty(ref _DOBDatePickerSelectedindex, value); }
        }
        #endregion

        #region DOBMonthPickerSelectedindex
        private int _DOBMonthPickerSelectedindex;

        public int DOBMonthPickerSelectedindex
        {
            get { return _DOBMonthPickerSelectedindex; }
            set { SetProperty(ref _DOBMonthPickerSelectedindex, value); }
        }
        #endregion

        #region DOBYearPickerSelectedindex
        private int _DOBYearPickerSelectedindex;

        public int DOBYearPickerSelectedindex
        {
            get { return _DOBYearPickerSelectedindex; }
            set { SetProperty(ref _DOBYearPickerSelectedindex, value); }
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

        #region Constructor
        public EditProfilePageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;

            IsPermissionSettingOpen = false;
            MessagingCenter.Subscribe<string, Position>(this, "LocationAddress", (sender, pickedposition) =>
            {
                Street = AddressfromGPS = sender.ToString();
                AddressPositionfromGPS = pickedposition;
            });

            IsLoaderBusy = false;
            IsLocationFetch = false;
            GetMaritalStatusList();
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


                //ProviencePickerSelectedindex = ProvinceList.IndexOf(ProvinceList.Where(x => x.province_name.ToLower().Contains("sana") || x.arabic_province_name.Contains("sana")).ToList().FirstOrDefault());
            }

            MessagingCenter.Subscribe<ImagesModel>(this, "ProfilePicture", (sender) =>
            {
                GallData = sender;
                UserPic = GallData.Image;
            });
        }
        #endregion

        #region GetMaritalStatusList
        private void GetMaritalStatusList()
        {
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
            //MaritalStatusList.Add(new MaritalStatusPickerModel
            //{
            //    MaritalStatusDisplay = Common.GetLanguage() != "ar-AE" ? Common.GetEnumDescription(MartialStatusEnum.Widowed) : Common.GetEnumDescription(MartialStatusArabicEnum.Widowed),
            //    MaritalStatusEnumValue = Convert.ToInt32(MartialStatusEnum.Widowed)
            //});

            MaritalStatusList.Add(new MaritalStatusPickerModel
            {
                MaritalStatusDisplay = Common.GetLanguage() != "ar-AE" ? Common.GetEnumDescription(MartialStatusEnum.Divorced) : Common.GetEnumDescription(MartialStatusArabicEnum.Divorced),
                MaritalStatusEnumValue = Convert.ToInt32(MartialStatusEnum.Divorced)
            });
        }
        #endregion

        #region SaveBtnCommand
        public Command SaveBtnCommand
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
                                    //multipartContent.Add(new StringContent(PhoneNumber), "phone_number");
                                    multipartContent.Add(new StringContent(user_id.ToString()), "user_id");

                                    multipartContent.Add(new StringContent(BaseViewModel.countryDataModels.FirstOrDefault().country_id.ToString()), "country");
                                    if (!string.IsNullOrEmpty(CurrentJob))
                                    {
                                        multipartContent.Add(new StringContent(CurrentJob), "current_job");
                                    }
                                    if (MaritalStatus != null)
                                    {

                                        multipartContent.Add(new StringContent(MaritalStatus.MaritalStatusEnumValue.ToString()), "martial_status");
                                    }
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
                                            var fileContent = new ByteArrayContent(GallData.ImageBytes);

                                            fileContent.Headers.ContentType = System.Net.Http.Headers.MediaTypeHeaderValue.Parse("application/octet-stream");
                                            fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                                            {
                                                Name = "profile_pic",
                                                FileName = "profilepic" + ".png",
                                            };

                                            multipartContent.Add(fileContent);
                                        }
                                    }
                                    catch (Exception)
                                    {
                                    }


                                    EditUserProfileModel response;
                                    try
                                    {
                                        response = await _webApiRestClient.PostAsync<MultipartFormDataContent, EditUserProfileModel>(ApiUrl.EditProfile, multipartContent);
                                    }
                                    catch (Exception ex)
                                    {
                                        response = null;
                                        await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_ServerError, msDuration: 3000);
                                        IsLoaderBusy = false;
                                        return;
                                    }
                                    if (response != null)
                                    {
                                        if (response.status)
                                        {
                                            await MaterialDialog.Instance.SnackbarAsync(message: response.message,
                                                        msDuration: 3000);
                                            //Common.PopPage(_navigation);
                                            var param = new NavigationParameters();
                                            param.Add("UpdateProfileData", true);
                                            await NavigationService.GoBackAsync(param);
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

        #region MapLocationCommand
        public Command MapLocationCommand
        {
            get
            {
                return new Command(() =>
                {
                    try
                    {
                        //Common.CustomNavigation(_navigation, new CustomMapPage(location));
                        var param = new NavigationParameters();
                        param.Add("CurrentLocationPoints", location);
                        NavigationService.NavigateAsync(nameof(CustomMapPage), param);
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
        public ICommand LocationCommand
        {
            get
            {
                return new DelegateCommand(async () =>
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
                                            IsPermissionSettingOpen = CrossPermissions.Current.OpenAppSettings();
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
                    }
                });
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

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("ProfileData"))
            {
                profiledata = (UserModel)parameters["ProfileData"];

                try
                {
                    if (profiledata != null)
                    {
                        Profiledata = profiledata;
                        Name = Profiledata.name;
                        Email = Profiledata.email;
                        DOB = profiledata.dob;
                        UserPic = Common.IsImagesValid(profiledata.profile_pic);
                        PhoneNumber = profiledata.phone_number;
                        CurrentJob = profiledata.current_job;
                        Street = !string.IsNullOrEmpty(profiledata.street) && !string.IsNullOrWhiteSpace(profiledata.street) ? profiledata.street : AppResource.cyp_StreetPlaceholder;
                        if (!string.IsNullOrEmpty(DOB) && !string.IsNullOrWhiteSpace(DOB))
                        {
                            var dobdata = DOB.Split('/');

                            DOBDatePickerSelectedindex = DOBDateList.IndexOf(dobdata[0]);
                            DOBMonthPickerSelectedindex = DOBMonthList.IndexOf(dobdata[1]);
                            DOBYearPickerSelectedindex = DOBYearList.IndexOf(dobdata[2]);
                        }



                        if (profiledata.longitude.HasValue && profiledata.latitude.HasValue)
                        {
                            AddressPositionfromGPS = new Position(profiledata.latitude.Value, profiledata.longitude.Value);
                            location = new Location(profiledata.latitude.Value, profiledata.longitude.Value);
                        }
                        Location = profiledata.description_location;

                        if (profiledata.martial_status.HasValue)
                        {
                            switch (profiledata.martial_status.Value)
                            {
                                case 1:
                                    MaritalStatusPickerSelectedindex = 0;
                                    break;
                                case 2:
                                    MaritalStatusPickerSelectedindex = 1;
                                    break;
                                case 3:
                                    MaritalStatusPickerSelectedindex = 2;
                                    break;
                                case 4:
                                    MaritalStatusPickerSelectedindex = 3;
                                    break;
                                case 5:
                                    MaritalStatusPickerSelectedindex = 4;
                                    break;
                                case 6:
                                    MaritalStatusPickerSelectedindex = 5;
                                    break;
                                case 7:
                                    MaritalStatusPickerSelectedindex = 6;
                                    break;
                                default:
                                    MaritalStatusPickerSelectedindex = -1;
                                    break;
                            }
                        }
                        if (profiledata.province.HasValue)
                        {
                            ProviencePickerSelectedindex = ProvinceList.IndexOf(ProvinceList.Where(x => x.province_id == (int)profiledata.province.Value).ToList().FirstOrDefault());
                        }
                    }
                }
                catch (Exception ex)
                {
                }
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