using Khadamat_CustomerApp.Helpers;
using Khadamat_CustomerApp.Models;
using Khadamat_CustomerApp.Resources;
using Khadamat_CustomerApp.Views;
using LiteDB;
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
    public class MasterPageViewModel : BaseViewModel
    {
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
        public MasterPageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;

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
                    IsPresented = false;
                });
            }
        }
        #endregion

        #region MenuViewProfileCommand
        public Command MenuViewProfileCommand
        {
            get
            {
                return new Command(() =>
                {
                    MessagingCenter.Send("ProfilePage", "MenuCloseIconClick");
                });
            }
        }
        #endregion

        #region MenuProfileCommand
        public Command MenuProfileCommand
        {
            get
            {
                return new Command(() =>
                {
                    //MessagingCenter.Send("ProfilePage", "MenuCloseIconClick");

                    //Common.CustomNavigation(_navigation, profilePage);
                    NavigationService.NavigateAsync("NavigationPage/ProfilePage");
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
                    //MessagingCenter.Send("HomePage", "MenuCloseIconClick");
                    NavigationService.NavigateAsync(new Uri("/NavigationPage/HomePage", UriKind.Relative));
                });
            }
        }
        #endregion

        #region MenuMyBookingCommand
        public Command MenuMyBookingCommand
        {
            get
            {
                return new Command(() =>
                {
                    // MessagingCenter.Send("MenuCloseIconClick", "MenuCloseIconClick");
                    //Common.CustomNavigation(_navigation, myBookingPage);
                    NavigationService.NavigateAsync("NavigationPage/MyBookingPage");
                });
            }
        }
        #endregion

        #region MenuChatCommand
        public Command MenuChatCommand
        {
            get
            {
                return new Command(() =>
                {
                    //  MessagingCenter.Send("MenuCloseIconClick", "MenuCloseIconClick");
                    //Common.CustomNavigation(_navigation, chatListPage);
                    NavigationService.NavigateAsync("NavigationPage/ChatListPage");
                });
            }
        }
        #endregion

        #region MenuSupportCommand
        public Command MenuSupportCommand
        {
            get
            {
                return new Command(() =>
                {
                    // MessagingCenter.Send("MenuCloseIconClick", "MenuCloseIconClick");
                    //Common.CustomNavigation(_navigation, supportPage);
                    NavigationService.NavigateAsync("NavigationPage/SupportPage");
                });
            }
        }
        #endregion

        #region MenuSettingCommand
        public Command MenuSettingCommand
        {
            get
            {
                return new Command(() =>
                {
                    //MessagingCenter.Send("MenuCloseIconClick", "MenuCloseIconClick");
                    //Common.CustomNavigation(_navigation, settingPage);
                    NavigationService.NavigateAsync("NavigationPage/SettingPage");
                });
            }
        }
        #endregion

        #region MenuTermConditionCommand
        public Command MenuTermConditionCommand
        {
            get
            {
                return new Command(() =>
                {
                    // MessagingCenter.Send("MenuCloseIconClick", "MenuCloseIconClick");
                    //Common.CustomNavigation(_navigation, termConditionPage);
                    NavigationService.NavigateAsync(new Uri("NavigationPage/TermConditionPage", UriKind.Relative));
                });
            }
        }
        #endregion

        #region MenuPrivacyPolicyCommand
        public Command MenuPrivacyPolicyCommand
        {
            get
            {
                return new Command(() =>
                {
                    // MessagingCenter.Send("MenuCloseIconClick", "MenuCloseIconClick");
                    //Common.CustomNavigation(_navigation, privacyPolicyPage);
                    NavigationService.NavigateAsync(new Uri("/NavigationPage/PrivacyPolicyPage", UriKind.Relative));
                });
            }
        }
        #endregion

        #region MenuContactUsCommand
        public Command MenuContactUsCommand
        {
            get
            {
                return new Command(() =>
                {
                    // MessagingCenter.Send("MenuCloseIconClick", "MenuCloseIconClick");
                    //Common.CustomNavigation(_navigation, contactUsPage);
                    NavigationService.NavigateAsync(new Uri("/ContactUsPage", UriKind.Relative));
                });
            }
        }
        #endregion

        #region MenuAboutUsCommand
        public Command MenuAboutUsCommand
        {
            get
            {
                return new Command(() =>
                {
                    //MessagingCenter.Send("MenuCloseIconClick", "MenuCloseIconClick");
                    //Common.CustomNavigation(_navigation, aboutUsPage);
                    //NavigationService.NavigateAsync(new Uri("/NavigationPage/AboutUsPage", UriKind.Relative));
                    NavigationService.NavigateAsync(nameof(AboutUsPage));
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
                    try
                    {
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
                                        //App.Current.MainPage = new NavigationPage(new LoginPage());
                                        await NavigationService.NavigateAsync("NavigationPage/LoginPage");
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
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                });
            }
        }
        #endregion
    }
}
