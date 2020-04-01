using Khadamat_CustomerApp.Helpers;
using Khadamat_CustomerApp.Models;
using Khadamat_CustomerApp.Resources;
using Khadamat_CustomerApp.Views;
using LiteDB;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace Khadamat_CustomerApp.ViewModels
{
    public class MenuPageViewModel : BaseViewModel
    {
        bool IsProfile;
        bool IsBooking;
        bool IsChat;
        bool IsSupport;
        bool IsSetting;

        bool IsTerm;
        bool IsPrivacy;
        bool IsContactUs;
        bool IsAboutUs;
        bool IsLogout;
        private readonly INavigationService NavigationService;

        #region IsPresented
        private bool _IsPresented;
        public bool IsPresented
        {
            get { return _IsPresented; }
            set { SetProperty(ref _IsPresented, value); }
        }
        #endregion

        #region newSupportChat
        private bool _newSupportChat;
        public bool newSupportChat
        {
            get { return _newSupportChat; }
            set { SetProperty(ref _newSupportChat, value); }
        }
        #endregion

        #region UserProfilePic Field
        private ImageSource _UserProfilePic;

        public ImageSource UserProfilePic
        {
            get { return _UserProfilePic; }
            set { SetProperty(ref _UserProfilePic, value); }
        }
        #endregion

        #region UserName Field
        private string _UserName;

        public string UserName
        {
            get { return _UserName; }
            set { SetProperty(ref _UserName, value); }
        }
        #endregion

        #region Constructor
        public MenuPageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;

            IsProfile = false;
            IsBooking = false;
            IsChat = false;
            IsSupport = false;
            IsSetting = false;

            IsTerm = false;
            IsPrivacy = false;
            IsContactUs = false;
            IsAboutUs = false;
            IsLogout = false;

            UserProfilePic = user_pic;
            UserName = user_name;
            newSupportChat = false;
            MessagingCenter.Subscribe<string, string>(this, "MenuProfileData", (name, profilepic) =>
            {
                UserName = name;
                UserProfilePic = profilepic;
            });
            MessagingCenter.Subscribe<string>(this, "NewSupportChat", (sender) =>
            {
                newSupportChat = Convert.ToBoolean(sender);
            });
        }
        #endregion

        #region MenuCloseommand
        public Command MenuCloseCommand
        {
            get
            {
                return new Command(() =>
                {
                    //IsPresented = false;
                    MessagingCenter.Send("HomePage", "MenuCloseIconClick");
                });
            }
        }
        #endregion

        #region MenuViewProfileCommand
        public Command MenuViewProfileCommand
        {
            get
            {
                return new Command(async() =>
                {
                    if (!IsProfile)
                    {
                        IsProfile = true;
                        //IsPresented = false;
                        try
                        {
                            await App.Current.MainPage.Navigation.PushPopupAsync(new LoaderPopup());
                            MessagingCenter.Send("ProfilePage", "MenuCloseIconClick");
                            await NavigationService.NavigateAsync(nameof(ProfilePage));
                        }
                        catch (Exception ex)
                        {
                        }
                        finally
                        {
                            IsProfile = false;
                            LoaderPopup.CloseAllPopup();
                        } 
                    }
                });
            }
        }
        #endregion

        #region MenuProfileCommand
        public Command MenuProfileCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (!IsProfile)
                    {
                        IsProfile = true;
                        //IsPresented = false;
                        try
                        {
                            await App.Current.MainPage.Navigation.PushPopupAsync(new LoaderPopup());
                            MessagingCenter.Send("ProfilePage", "MenuCloseIconClick");
                            await NavigationService.NavigateAsync(nameof(ProfilePage));
                        }
                        catch (Exception ex)
                        {
                        }
                        finally
                        {
                            IsProfile = false;
                            LoaderPopup.CloseAllPopup();
                        } 
                    }
                });
            }
        }
        #endregion

        #region MenuHomeCommand
        public Command MenuHomeCommand
        {
            get
            {
                return new Command(() =>
                {
                    //IsPresented = false;
                    MessagingCenter.Send("HomePage", "MenuCloseIconClick");
                    try
                    {
                        //NavigationService.NavigateAsync(new Uri("MasterPage/NavigationPage/HomePage", UriKind.Relative));
                    }
                    catch (Exception ex)
                    {
                    }
                });
            }
        }
        #endregion

        #region MenuMyBookingCommand
        public Command MenuMyBookingCommand
        {
            get
            {
                return new Command(async() =>
                {
                    if (!IsBooking)
                    {
                        IsBooking = true;
                        try
                        {
                            await App.Current.MainPage.Navigation.PushPopupAsync(new LoaderPopup());
                            //IsPresented = false; 
                            MessagingCenter.Send("HomePage", "MenuCloseIconClick");
                            await NavigationService.NavigateAsync(nameof(MyBookingPage));
                        }
                        catch (Exception ex)
                        {
                        }
                        finally
                        {
                            IsBooking = false;
                            LoaderPopup.CloseAllPopup();
                        } 
                    }
                });
            }
        }
        #endregion

        #region MenuChatCommand
        public Command MenuChatCommand
        {
            get
            {
                return new Command(async() =>
                {
                    if (!IsChat)
                    {
                        IsChat = true;
                        try
                        {
                            //IsPresented = false;
                            MessagingCenter.Send("HomePage", "MenuCloseIconClick");
                            await NavigationService.NavigateAsync(nameof(ChatListPage));
                        }
                        catch(Exception ex)
                        {

                        }
                        finally
                        {
                            IsChat = false;
                        }
                    }
                });
            }
        }
        #endregion

        #region MenuSupportCommand
        public Command MenuSupportCommand
        {
            get
            {
                return new Command(async() =>
                {
                    if (!IsSupport)
                    {
                        IsSupport = true;
                        try
                        {
                            MessagingCenter.Send("MenuCloseIconClick", "MenuCloseIconClick");
                            //Common.CustomNavigation(_navigation, supportPage);
                            //NavigationService.NavigateAsync("NavigationPage/SupportPage");
                            await NavigationService.NavigateAsync(nameof(SupportPage));
                        }
                        catch (Exception ex)
                        {

                        }
                        finally
                        {
                            IsSupport = false;
                        } 
                    }
                });
            }
        }
        #endregion

        #region MenuSettingCommand
        public Command MenuSettingCommand
        {
            get
            {
                return new Command(async() =>
                {
                    if (!IsSetting)
                    {
                        IsSetting = true;
                        try
                        {
                            //IsPresented = false;
                            MessagingCenter.Send("MenuCloseIconClick", "MenuCloseIconClick");
                            //Common.CustomNavigation(_navigation, settingPage);
                            //NavigationService.NavigateAsync("NavigationPage/SettingPage");
                            await NavigationService.NavigateAsync(nameof(SettingPage));
                        }
                        catch (Exception ex)
                        {
                        }
                        finally
                        {
                            IsSetting = false;
                        }
                    }
                });
            }
        }
        #endregion

        #region MenuTermConditionCommand
        public Command MenuTermConditionCommand
        {
            get
            {
                return new Command(async() =>
                {
                    if (!IsTerm)
                    {
                        IsTerm = true;
                        try
                        {
                            //IsPresented = false;
                            MessagingCenter.Send("MenuCloseIconClick", "MenuCloseIconClick");
                            //Common.CustomNavigation(_navigation, termConditionPage);
                            //NavigationService.NavigateAsync(new Uri("NavigationPage/TermConditionPage", UriKind.Relative));
                            await NavigationService.NavigateAsync(nameof(TermConditionPage));
                        }
                        catch (Exception ex)
                        {
                        }
                        finally
                        {
                            IsTerm = false;
                        } 
                    }
                });
            }
        }
        #endregion

        #region MenuPrivacyPolicyCommand
        public Command MenuPrivacyPolicyCommand
        {
            get
            {
                return new Command(async() =>
                {
                    if (!IsPrivacy)
                    {
                        IsPrivacy = true;
                        try
                        {
                            //IsPresented = false;
                            MessagingCenter.Send("MenuCloseIconClick", "MenuCloseIconClick");
                            //Common.CustomNavigation(_navigation, privacyPolicyPage);
                            //NavigationService.NavigateAsync(new Uri("/NavigationPage/PrivacyPolicyPage", UriKind.Relative));
                            await NavigationService.NavigateAsync(nameof(PrivacyPolicyPage));
                        }
                        catch (Exception ex)
                        {
                        }
                        finally
                        {
                            IsPrivacy = false;
                        }
                    }
                });
            }
        }
        #endregion

        #region MenuContactUsCommand
        public Command MenuContactUsCommand
        {
            get
            {
                return new Command(async() =>
                {
                    if (!IsContactUs)
                    {
                        IsContactUs = true;
                        try
                        {
                            //IsPresented = false;
                            MessagingCenter.Send("MenuCloseIconClick", "MenuCloseIconClick");
                            //Common.CustomNavigation(_navigation, contactUsPage);
                            //NavigationService.NavigateAsync(new Uri("/ContactUsPage", UriKind.Relative));
                            await NavigationService.NavigateAsync(nameof(ContactUsPage));
                        }
                        catch (Exception ex)
                        {
                        }
                        finally
                        {
                            IsContactUs = false;
                        }
                    }
                });
            }
        }
        #endregion

        #region MenuAboutUsCommand
        public Command MenuAboutUsCommand
        {
            get
            {
                return new Command(async() =>
                {
                    if (!IsAboutUs)
                    {
                        IsAboutUs = true;
                        try
                        {
                            //IsPresented = false;
                            MessagingCenter.Send("MenuCloseIconClick", "MenuCloseIconClick");
                            //Common.CustomNavigation(_navigation, aboutUsPage);
                            //NavigationService.NavigateAsync(new Uri("/NavigationPage/AboutUsPage", UriKind.Relative));
                            await NavigationService.NavigateAsync(nameof(AboutUsPage));
                        }
                        catch (Exception ex)
                        {
                        }
                        finally
                        {
                            IsAboutUs = false;
                        } 
                    }
                });
            }
        }
        #endregion

        #region MenuLogoutCommand
        public Command MenuLogoutCommand
        {
            get
            {
                return new Command(async () =>
                {
                    if (!IsLogout)
                    {
                        IsLogout = true;
                        try
                        {
                            //IsPresented = false;
                            MessagingCenter.Send("MenuCloseIconClick", "MenuCloseIconClick");
                            var answer = await App.Current.MainPage.DisplayAlert(AppResource.Logout, AppResource.LogoutMsg, AppResource.Yes, AppResource.No);
                            if (answer)
                            {
                                if (Common.CheckConnection())
                                {
                                    LogoutResponseModel response;
                                    try
                                    {
                                        response = await _webApiRestClient.GetAsync<LogoutResponseModel>(string.Format(ApiUrl.Logout, user_id));
                                    }
                                    catch (Exception ex)
                                    {
                                        response = null;
                                        await MaterialDialog.Instance.SnackbarAsync(message: ex.Message, msDuration: 3000);
                                        return;
                                    }
                                    if (response != null)
                                    {
                                        if (response.status)
                                        {
                                            if (userDataDbService.IsUserDbPresentInDB())
                                            {
                                                var item = userDataDbService.ReadAllItems().FirstOrDefault();
                                                BsonValue id = item.ID;
                                                userDataDbService.DeleteItemFromDB(id, item);
                                            }
                                            await NavigationService.NavigateAsync(new Uri("/NavigationPage/LoginPage", UriKind.Absolute));
                                        }
                                        else
                                        {
                                            await MaterialDialog.Instance.SnackbarAsync(message: response.message,
                                                        msDuration: 3000);
                                        }
                                    }

                                }
                                else
                                {
                                    await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_InternetError, msDuration: 3000);
                                    //if (userDataDbService.IsUserDbPresentInDB())
                                    //{
                                    //    var item = userDataDbService.ReadAllItems().FirstOrDefault();
                                    //    BsonValue id = item.ID;
                                    //    userDataDbService.DeleteItemFromDB(id, item);
                                    //}
                                    //await NavigationService.NavigateAsync(new Uri("/NavigationPage/LoginPage", UriKind.Absolute));
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                        finally
                        {
                            IsLogout = false;
                        }
                    }
                });
            }
        }
        #endregion
    }
}