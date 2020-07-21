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
    public class ContactUsPageViewModel : BaseViewModel
    {
        private readonly INavigationService NavigationService;
        #region IsLoaderBusy Field
        private bool _IsLoaderBusy;

        public bool IsLoaderBusy
        {
            get { return _IsLoaderBusy; }
            set { SetProperty(ref _IsLoaderBusy, value); }
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

        #region Email
        private string _Email;

        public string Email
        {
            get { return _Email; }
            set { SetProperty(ref _Email, value); }
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

        #region Constructor
        public ContactUsPageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
            IsLoaderBusy = false;
        }
        #endregion

        #region SendBtnCommand
        public ICommand SendBtnCommand
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    try
                    {
                        if (Common.CheckConnection())
                        {
                            if (!string.IsNullOrEmpty(ServiceDescriptionValue) && !string.IsNullOrWhiteSpace(ServiceDescriptionValue))
                            {
                                IsLoaderBusy = true;
                                var requestData = new ContactUsRequestModel()
                                {
                                    email = Email,
                                    description = ServiceDescriptionValue,
                                    name = Name,
                                    user_id = user_id
                                };
                                ContactUsResponseModel response;
                                try
                                {
                                    response = await _webApiRestClient.PostAsync<ContactUsRequestModel, ContactUsResponseModel>(ApiUrl.ContactUs, requestData);
                                }
                                catch (Exception ex)
                                {
                                    response = null;
                                    //await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_ServerError, msDuration: 3000);
                                    IsLoaderBusy = false;
                                    return;
                                }
                                if (response != null)
                                {
                                    if (response.status)
                                    {
                                        await MaterialDialog.Instance.SnackbarAsync(message: response.message, msDuration: 3000);
                                        await NavigationService.GoBackAsync();
                                    }
                                    else
                                    {
                                        await MaterialDialog.Instance.SnackbarAsync(message: response.message, msDuration: 3000);
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_ContactUsDescription,
                                                    msDuration: 3000);
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
    }
}
