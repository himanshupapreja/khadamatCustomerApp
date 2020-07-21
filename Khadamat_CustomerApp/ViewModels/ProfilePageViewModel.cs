using Khadamat_CustomerApp.Helpers;
using Khadamat_CustomerApp.Models;
using Khadamat_CustomerApp.Resources;
using Khadamat_CustomerApp.Services.ApiService;
using Khadamat_CustomerApp.Views;
using LiteDB;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;
using static Khadamat_CustomerApp.Helpers.Enums;

namespace Khadamat_CustomerApp.ViewModels
{
    public class ProfilePageViewModel : BaseViewModel, INavigationAware
    {
        List<string> PhoneStartingValue = new List<string>()
        {
            "77","71","73","70"
        };

        bool IsEditProfileClick;
        LoaderPopup loaderPopup;

        #region IsPopupVisible Field
        private bool _IsPopupVisible;

        public bool IsPopupVisible
        {
            get { return _IsPopupVisible; }
            set { SetProperty(ref _IsPopupVisible, value); }
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

        #region IsNoInternetFound
        private bool _IsNoInternetFound;
        public bool IsNoInternetFound
        {
            get { return _IsNoInternetFound; }
            set
            {
                _IsNoInternetFound = value;
                OnPropertyChanged(nameof(IsNoInternetFound));
            }
        }
        #endregion

        #region IsCurrentJobVisible
        private bool _IsCurrentJobVisible;

        public bool IsCurrentJobVisible
        {
            get { return _IsCurrentJobVisible; }
            set { SetProperty(ref _IsCurrentJobVisible, value); }
        }
        #endregion

        #region IsEmailVisible
        private bool _IsEmailVisible;

        public bool IsEmailVisible
        {
            get { return _IsEmailVisible; }
            set { SetProperty(ref _IsEmailVisible, value); }
        }
        #endregion

        #region IsMaritalStatusVisible
        private bool _IsMaritalStatusVisible;

        public bool IsMaritalStatusVisible
        {
            get { return _IsMaritalStatusVisible; }
            set { SetProperty(ref _IsMaritalStatusVisible, value); }
        }
        #endregion

        #region IsJobLocationVisible
        private bool _IsJobLocationVisible;

        public bool IsJobLocationVisible
        {
            get { return _IsJobLocationVisible; }
            set { SetProperty(ref _IsJobLocationVisible, value); }
        }
        #endregion

        #region PhoneEmailFieldTitle 
        private string _PhoneEmailFieldTitle;

        public string PhoneEmailFieldTitle
        {
            get { return _PhoneEmailFieldTitle; }
            set { SetProperty(ref _PhoneEmailFieldTitle, value); }
        }
        #endregion

        #region PhoneEmailField 
        private string _PhoneEmailField;

        public string PhoneEmailField
        {
            get { return _PhoneEmailField; }
            set
            {
                SetProperty(ref _PhoneEmailField, value);
                if (!string.IsNullOrEmpty(PhoneEmailField) && !string.IsNullOrWhiteSpace(PhoneEmailField) && PhoneEmailPlaceholder == AppResource.reg_PhoneNumber && PhoneEmailField.Length > 9)
                {
                    PhoneEmailField = Common.StringBuilderChars(PhoneEmailField.Take(9));
                }
            }
        }
        #endregion

        #region PhoneEmailPlaceholder 
        private string _PhoneEmailPlaceholder;

        public string PhoneEmailPlaceholder
        {
            get { return _PhoneEmailPlaceholder; }
            set { SetProperty(ref _PhoneEmailPlaceholder, value); }
        }
        #endregion

        #region UserPic
        private string _UserPic;

        public string UserPic
        {
            get { return _UserPic; }
            set { SetProperty(ref _UserPic, value); }
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
            set { SetProperty(ref _PhoneNumber, value); }
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

        #region MaritalStatus
        private string _MaritalStatus;

        public string MaritalStatus
        {
            get { return _MaritalStatus; }
            set { SetProperty(ref _MaritalStatus, value); }
        }
        #endregion

        #region Country
        private string _Country;

        public string Country
        {
            get { return _Country; }
            set { SetProperty(ref _Country, value); }
        }
        #endregion

        #region Province
        private string _Province;

        public string Province
        {
            get { return _Province; }
            set { SetProperty(ref _Province, value); }
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

        #region Location
        private string _Location;

        public string Location
        {
            get { return _Location; }
            set { SetProperty(ref _Location, value); }
        }
        #endregion

        UserModel userprofileData;

        private readonly INavigationService NavigationService;

        WebApiRestClient _webApiRestClient;

        #region Constructor
        public ProfilePageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
            _webApiRestClient = new WebApiRestClient();
            IsLoaderBusy = false;
            IsPopupVisible = false;
            IsNoInternetFound = false;
            GetProfileData();
        }
        #endregion

        #region GetProfileData Api Call
        private async void GetProfileData()
        {
            try
            {
                if (Common.CheckConnection())
                {
                    UserProfileModel response;
                    try
                    {
                        response = await _webApiRestClient.GetAsync<UserProfileModel>(string.Format(ApiUrl.GetProfile, user_id));
                    }
                    catch (Exception ex)
                    {
                        response = null;
                        //await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_ServerError, msDuration: 3000);
                    }
                    if (response != null)
                    {
                        if (response.status)
                        {
                            userprofileData = response.data;
                            province_id = response.data.province.Value;
                            if (userDataDbService.IsUserDbPresentInDB())
                            {
                                var item = userDataDbService.ReadAllItems().FirstOrDefault();
                                BsonValue id = item.ID;
                                userDataDbService.UpdateUserDataInDb(id, userprofileData);
                            }
                            OfflineData(userprofileData);
                        }
                        else
                        {
                            await MaterialDialog.Instance.SnackbarAsync(message: response.message, msDuration: 3000);
                            if (userDataDbService.IsUserDbPresentInDB())
                            {
                                userprofileData = userDataDbService.ReadAllItems().FirstOrDefault();
                            }
                            OfflineData(userprofileData);
                        }
                    }
                    else
                    {
                        if (userDataDbService.IsUserDbPresentInDB())
                        {
                            userprofileData = userDataDbService.ReadAllItems().FirstOrDefault();
                        }
                        OfflineData(userprofileData);
                    }
                }
                else
                {
                    await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_InternetError, msDuration: 3000);
                    if (userDataDbService.IsUserDbPresentInDB())
                    {
                        userprofileData = userDataDbService.ReadAllItems().FirstOrDefault();
                    }
                    OfflineData(userprofileData);
                    IsNoInternetFound = false;
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

        #region DataWhennoInternet
        private void OfflineData(UserModel profileData)
        {
            user_pic = UserPic = Common.IsImagesValid(profileData.profile_pic, ApiUrl.BaseUrl);
            user_name = Name = profileData.name;
            Email = profileData.email;
            DOB = profileData.dob;
            PhoneNumber = profileData.phone_number;
            CurrentJob = profileData.current_job;
            if (profileData.martial_status.HasValue && profileData.martial_status.Value > 0)
            {
                IsMaritalStatusVisible = true;
                var enumDisplayStatus = Common.GetLanguage() != "ar-AE" ? Common.GetEnumDescription((MartialStatusEnum)profileData.martial_status) : Common.GetEnumDescription((MartialStatusArabicEnum)profileData.martial_status);
                MaritalStatus = enumDisplayStatus.ToString();
            }
            else
            {
                IsMaritalStatusVisible = false;
            }
            Street = profileData.street;
            Location = profileData.description_location;
            if (!string.IsNullOrEmpty(Email) && !string.IsNullOrWhiteSpace(Email))
            {
                IsEmailVisible = true;
            }
            else
            {
                IsEmailVisible = false;
            }
            if (!string.IsNullOrEmpty(CurrentJob) && !string.IsNullOrWhiteSpace(CurrentJob))
            {
                IsCurrentJobVisible = true;
            }
            else
            {
                IsCurrentJobVisible = false;
            }
            if (!string.IsNullOrEmpty(Street) && !string.IsNullOrWhiteSpace(Street) && Street != AppResource.cyp_StreetPlaceholder)
            {
                IsJobLocationVisible = true;
            }
            else
            {
                IsJobLocationVisible = false;
            }


            MessagingCenter.Send(Name, "MenuProfileData", UserPic);
            //MaritalStatus = profileData.martial_status;
            Country = Common.GetLanguage() != "ar-AE" ? BaseViewModel.countryDataModels.Where(x => x.country_id == Convert.ToInt32(profileData.country.Value)).FirstOrDefault().country_name : BaseViewModel.countryDataModels.Where(x => x.country_id == Convert.ToInt32(profileData.country.Value)).FirstOrDefault().arabic_country_name;
            Province = Common.GetLanguage() != "ar-AE" ? BaseViewModel.provienceDataModels.Where(x => x.province_id == Convert.ToInt32(profileData.province.Value)).FirstOrDefault().province_name : BaseViewModel.provienceDataModels.Where(x => x.province_id == Convert.ToInt32(profileData.province.Value)).FirstOrDefault().arabic_province_name;
            //Province = profileData.province_name;
        }
        #endregion

        #region PopupCloseCommand
        public Command PopupCloseCommand
        {
            get
            {
                return new Command(() =>
                {
                    IsPopupVisible = false;
                    PhoneEmailField = string.Empty;
                });
            }
        }
        #endregion

        #region PhoneEmailEditCommand
        public Command PhoneEmailEditCommand
        {
            get
            {
                return new Command((e) =>
                {
                    if (userprofileData != null)
                    {
                        IsPopupVisible = true;
                        switch (e.ToString())
                        {
                            case "email":
                                PhoneEmailFieldTitle = AppResource.profile_PopupEmailTitle;
                                PhoneEmailPlaceholder = AppResource.cyp_EmailPlaceholder;
                                PhoneEmailField = userprofileData.email;
                                break;
                            case "phone":
                                PhoneEmailFieldTitle = AppResource.profile_PopupPhoneTitle;
                                PhoneEmailPlaceholder = AppResource.reg_PhoneNumber;
                                PhoneEmailField = userprofileData.phone_number;
                                break;
                        } 
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
                            if (!string.IsNullOrEmpty(PhoneEmailField) && !string.IsNullOrWhiteSpace(PhoneEmailField))
                            {
                                int phonenumber;
                                var data = int.TryParse(PhoneEmailField, out phonenumber);
                                if (PhoneEmailField.Length < 9 || !Common.StringStartValue(PhoneEmailField, PhoneStartingValue) || !data)
                                {
                                    await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_ValidPhone, msDuration: 3000);
                                    return;
                                }
                                else
                                {
                                    VerifyResendOtpModel requestModel = new VerifyResendOtpModel()
                                    {
                                        phone_number = PhoneEmailField,
                                        user_id = user_id
                                    };

                                    EditPhoneNumberApiCall(requestModel, data ? phonenumber.ToString() : PhoneEmailField);
                                }
                            }
                            else
                            {
                                await MaterialDialog.Instance.SnackbarAsync(AppResource.error_BlankPhone, 3000);
                            }
                        }
                        else
                        {
                            await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_InternetError, msDuration: 3000);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                });
            }
        }
        #endregion

        #region ResendOtpApiCall
        private async void EditPhoneNumberApiCall(VerifyResendOtpModel requestModel, string phoneNumber)
        {
            try
            {
                IsLoaderBusy = true;
                EditPhoneResponseModel response;
                try
                {
                    response = await _webApiRestClient.PostAsync<VerifyResendOtpModel, EditPhoneResponseModel>(ApiUrl.EditPhoneNumber, requestModel);
                }
                catch (Exception ex)
                {
                    response = null;
                    await MaterialDialog.Instance.SnackbarAsync(AppResource.error_ServerError, 3000);
                    IsLoaderBusy = false;
                    return;
                }
                if (response != null)
                {
                    if (response.status)
                    {
                        await MaterialDialog.Instance.SnackbarAsync(response.message, 3000);
                        if (response.otpCode != null)
                        {
                            if (response.otpCode.otp > 0)
                            {
                                //await App.Current.MainPage.DisplayAlert("", response.otpCode.otp.ToString(), AppResource.Ok);
                            }
                            var param = new NavigationParameters();
                            param.Add("PhoneNumber", phoneNumber);
                            param.Add("IsForgotPassword", false);
                            param.Add("IsProfilePage", true);
                            param.Add("ProfileData", response);
                            await NavigationService.NavigateAsync(nameof(OtpPage), param);
                            IsPopupVisible = false;
                            PhoneEmailField = string.Empty; 
                        }
                    }
                    else
                    {
                        if (response.otpCode != null)
                        {
                            var param = new NavigationParameters();
                            param.Add("PhoneNumber", phoneNumber);
                            param.Add("IsForgotPassword", false);
                            param.Add("IsProfilePage", true);
                            param.Add("ProfileData", response);
                            await NavigationService.NavigateAsync(nameof(OtpPage), param);
                            IsPopupVisible = false;
                            PhoneEmailField = string.Empty;
                        }
                        else
                        {
                            await MaterialDialog.Instance.SnackbarAsync(response.message, 3000);
                        }
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

        #region NoInternetCommand
        public Command NoInternetCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (Common.CheckConnection())
                    {
                        IsNoInternetFound = false;
                        GetProfileData();
                    }
                    else
                    {
                        //await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_InternetError, msDuration: 3000);
                    }
                });
            }
        }
        #endregion

        #region RightIconCommand
        public Command RightIconCommand
        {
            get
            {
                return new Command(async() =>
                {
                    if (!IsEditProfileClick)
                    {
                        IsEditProfileClick = true;
                        try
                        {
                            loaderPopup = new LoaderPopup();
                            await App.Current.MainPage.Navigation.PushPopupAsync(loaderPopup);
                            //Common.CustomNavigation(_navigation, new EditProfilePage(profileData));
                            var param = new NavigationParameters();
                            param.Add("ProfileData", userprofileData);
                            await NavigationService.NavigateAsync(nameof(EditProfilePage), param);
                        }
                        finally
                        {
                            IsEditProfileClick = false;
                            loaderPopup.ClosePopup();
                        } 
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

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("UpdateProfileData"))
            {
                GetProfileData();
            }

            IsEditProfileClick = false;
        }
    }
}
