using Khadamat_CustomerApp.Helpers;
using Khadamat_CustomerApp.Models;
using Khadamat_CustomerApp.Resources;
using Khadamat_CustomerApp.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;
using Rg.Plugins.Popup.Extensions;
using Newtonsoft.Json;

namespace Khadamat_CustomerApp.ViewModels
{
    public class ChatListPageViewModel : BaseViewModel, INavigationAware
    {
        private readonly INavigationService NavigationService;
        private bool IsChatDetailOpen;
        #region IsLoaderBusy Field
        private bool _IsLoaderBusy;

        public bool IsLoaderBusy
        {
            get { return _IsLoaderBusy; }
            set { SetProperty(ref _IsLoaderBusy, value); }
        }

        #region IsNoInternetView Field
        private bool _IsNoInternetView;

        public bool IsNoInternetView
        {
            get { return _IsNoInternetView; }
            set { SetProperty(ref _IsNoInternetView, value); }
        }
        #endregion

        #region IsNoDataFoundView Field
        private bool _IsNoDataFoundView;

        public bool IsNoDataFoundView
        {
            get { return _IsNoDataFoundView; }
            set { SetProperty(ref _IsNoDataFoundView, value); }
        }
        #endregion
        #endregion

        #region IsRefreshing Field
        private bool _IsRefreshing;

        public bool IsRefreshing
        {
            get { return _IsRefreshing; }
            set { SetProperty(ref _IsRefreshing, value); }
        }
        #endregion

        #region ChatList
        private ObservableCollection<ChatUserModel> AllChatList = new ObservableCollection<ChatUserModel>();
        private ObservableCollection<ChatUserModel> _ChatList = new ObservableCollection<ChatUserModel>();
        public ObservableCollection<ChatUserModel> ChatList
        {
            get { return _ChatList; }
            set { SetProperty(ref _ChatList, value); }
        }
        #endregion

        #region ChatSelected
        private ChatUserModel _ChatSelected = new ChatUserModel();
        public ChatUserModel ChatSelected
        {
            get { return _ChatSelected; }
            set
            {
                SetProperty(ref _ChatSelected, value);
                if (ChatSelected != null && !string.IsNullOrEmpty(ChatSelected.name) && !string.IsNullOrWhiteSpace(ChatSelected.name))
                {
                    //Common.CustomNavigation(_navigation, new ChatDetailPage(ChatSelected.name,ChatSelected.user_id.Value));
                    ChatDetailOpen(ChatSelected);
                }
            }
        }

        private async void ChatDetailOpen(ChatUserModel chatSelected)
        {
            if (!IsChatDetailOpen)
            {
                IsChatDetailOpen = true;
                //await App.Current.MainPage.Navigation.PushPopupAsync(new LoaderPopup());
                if (chatSelected.job_request_id.HasValue && chatSelected.job_request_id.Value > 0)
                {
                    JobChatModel jobChatModel = new JobChatModel()
                    {
                        job_id = chatSelected.job_request_id.HasValue ? chatSelected.job_request_id.Value : 0,
                        job_Name = chatSelected.name,
                        coordinator_id = chatSelected.coordinator_id.HasValue ? chatSelected.coordinator_id.Value : 0,
                        coordinator_Name = chatSelected.coordinator_name,
                        customer_id = chatSelected.customer_id.HasValue ? chatSelected.customer_id.Value : 0,
                        customer_Name = chatSelected.customer_name,
                        worker_id = chatSelected.worker_id.HasValue ? chatSelected.worker_id.Value : 0,
                        worker_Name = chatSelected.worker_name,
                        job_Status = chatSelected.status
                    };
                    //Common.CustomNavigation(_navigation, new ChatDetailPage(chatSelected.name, jobChatModel));
                    var param = new NavigationParameters();
                    param.Add("ChatDetailTitle", chatSelected.name);
                    param.Add("ChatDetailData", jobChatModel);
                    await NavigationService.NavigateAsync(nameof(ChatDetailPage), param);
                }
                else
                {
                    //Common.CustomNavigation(_navigation, new SupportPage(chatSelected.name, chatSelected.user_id.Value));

                    var param = new NavigationParameters();
                    param.Add("RecieverName", chatSelected.name);
                    param.Add("RecieverId", Convert.ToInt32(chatSelected.user_id.Value));
                    await NavigationService.NavigateAsync(nameof(SupportPage), param);
                }
                //LoaderPopup.CloseAllPopup();
                IsChatDetailOpen = false;
            }
        }
        #endregion

        #region Constructor
        public ChatListPageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
            IsChatDetailOpen = false;
            //GetChat();
        }
        #endregion

        #region DeleteChat
        private async void DeleteChat(ChatUserModel data)
        {
            if (data.job_request_id != null && data.job_request_id.Value > 0)
            {
                var deleteChatResult = await App.Current.MainPage.DisplayAlert(AppResource.DeleteGroup, AppResource.DeleteGroupConfirm, AppResource.Yes, AppResource.No);
                if (deleteChatResult)
                {
                    try
                    {
                        if (Common.CheckConnection())
                        {
                            await App.Current.MainPage.Navigation.PushPopupAsync(new LoaderPopup());
                            Chatdetailresponse response;
                            var request = new GroupChatreadRequestModel()
                            {
                                user_id = user_id,
                                job_request_id = data.job_request_id.Value
                            };
                            try
                            {
                                response = await _webApiRestClient.PostAsync<GroupChatreadRequestModel, Chatdetailresponse>(ApiUrl.DeleteGroupChatRequest, request);
                            }
                            catch (Exception ex)
                            {
                                response = null;
                                LoaderPopup.CloseAllPopup();
                                IsRefreshing = false;
                                await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_ServerError, msDuration: 3000);
                                return;
                            }
                            if (response != null)
                            {
                                if (response.status)
                                {
                                    await FirebaseChatHelper.DeleteGroupChat("GroupChat", user_id, data.job_request_id.Value);
                                    GetChat();
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
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                        //await Task.Delay(1500);
                        LoaderPopup.CloseAllPopup();
                        IsRefreshing = false;
                    }
                }
            }
            else
            {
                var deleteChatResult = await App.Current.MainPage.DisplayAlert(AppResource.DeleteChat, AppResource.DeleteChatConfirm, AppResource.Yes, AppResource.No);
                if (deleteChatResult)
                {
                    try
                    {
                        if (Common.CheckConnection())
                        {
                            await App.Current.MainPage.Navigation.PushPopupAsync(new LoaderPopup());
                            Chatdetailresponse response;
                            var request = new ChatDetailModelApi()
                            {
                                from_user_id = user_id,
                                to_user_id = data.user_id.Value
                            };
                            try
                            {
                                response = await _webApiRestClient.PostAsync<ChatDetailModelApi, Chatdetailresponse>(ApiUrl.DeleteChatRequest, request);
                            }
                            catch (Exception ex)
                            {
                                response = null;
                                LoaderPopup.CloseAllPopup();
                                IsRefreshing = false;
                                await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_ServerError, msDuration: 3000);
                                return;
                            }
                            if (response != null)
                            {
                                if (response.status)
                                {
                                    await FirebaseChatHelper.DeleteSingleChat("Chat", user_id, data.user_id.Value);
                                    GetChat();
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
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                        await Task.Delay(1500);
                        LoaderPopup.CloseAllPopup();
                        IsRefreshing = false;
                    }
                }
            }
        }
        #endregion

        #region GetChat
        private async void GetChat()
        {
            try
            {
                if (Common.CheckConnection())
                {
                    IsLoaderBusy = true;
                    ChatUserResponseModel response;
                    try
                    {
                        response = await _webApiRestClient.GetAsync<ChatUserResponseModel>(string.Format(ApiUrl.GetChat, user_id, Enums.UserTypeEnum.Customer.GetHashCode()));
                    }
                    catch (Exception ex)
                    {
                        response = null;
                        IsLoaderBusy = false;
                        IsRefreshing = false;
                        await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_ServerError, msDuration: 3000);
                    }
                    if (response != null)
                    {
                        if (response.status)
                        {
                            Application.Current.Properties["ChatListData"] = JsonConvert.SerializeObject(response.data);
                            Application.Current.SavePropertiesAsync();
                            AllChatList.Clear();
                            var chatData = response.data.Where(x => x.user_type != Convert.ToInt32(Enums.UserTypeEnum.Admin)).OrderByDescending(x=>x.created_date).ToList();
                            foreach (var item in chatData)
                            {
                                item.display_created_date = Common.GetLanguage() != "ar-AE" ? item.created_date_str : item.created_date_str_arabic;
                                item.name = item.user_type == Convert.ToInt32(Enums.UserTypeEnum.Coordinator) ? AppResource.coordinator_Name : item.user_type == Convert.ToInt32(Enums.UserTypeEnum.FinanceOfficer) ? AppResource.finance_officername : item.name;
                                item.picture_path = Common.IsImagesValid(item.picture_path);
                                AllChatList.Add(item);
                            }
                            ChatList = AllChatList;

                            if (ChatList.Count > 0)
                            {
                                IsNoDataFoundView = false;
                                IsNoInternetView = false;
                            }
                            else
                            {
                                IsNoDataFoundView = true;
                                IsNoInternetView = false;
                            }
                        }
                        else
                        {
                            //await MaterialDialog.Instance.SnackbarAsync(message: response.message,
                            //            msDuration: 3000);
                            if (Application.Current.Properties.ContainsKey("ChatListData"))
                            {
                                OfflineData();
                            }
                            else
                            {
                                IsNoDataFoundView = true;
                                IsNoInternetView = false;
                            }
                        }
                    }
                    else
                    {
                        if (Application.Current.Properties.ContainsKey("ChatListData"))
                        {
                            OfflineData();
                        }
                        else
                        {
                            IsNoDataFoundView = false;
                            IsNoInternetView = true;
                        }
                    }
                }
                else
                {
                    if (Application.Current.Properties.ContainsKey("ChatListData"))
                    {
                        OfflineData();
                    }
                    else
                    {
                        IsNoDataFoundView = false;
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
                IsRefreshing = false;
            }
        }
        #endregion

        private void OfflineData()
        {
            var chatlistdata = JsonConvert.DeserializeObject<List<ChatUserModel>>(Application.Current.Properties["ChatListData"].ToString());
            AllChatList.Clear();
            var chatData = chatlistdata.Where(x => x.user_type != Convert.ToInt32(Enums.UserTypeEnum.Admin)).ToList();
            foreach (var item in chatData)
            {
                item.display_created_date = Common.GetLanguage() != "ar-AE" ? item.created_date_str : item.created_date_str_arabic;
                item.name = item.user_type == Convert.ToInt32(Enums.UserTypeEnum.Coordinator) ? AppResource.coordinator_Name : item.user_type == Convert.ToInt32(Enums.UserTypeEnum.FinanceOfficer) ? AppResource.finance_officername : item.name;
                item.picture_path = Common.IsImagesValid(item.picture_path);
                AllChatList.Add(item);
            }
            ChatList = AllChatList;

            if (ChatList.Count > 0)
            {
                IsNoDataFoundView = false;
                IsNoInternetView = false;
            }
            else
            {
                IsNoDataFoundView = true;
                IsNoInternetView = false;
            }
        }

        #region DeleteChatCommand
        public Command DeleteChatCommand
        {
            get
            {
                return new Command((e) =>
                {
                    var data = (ChatUserModel)e;
                    DeleteChat(data);
                });
            }
        }
        #endregion

        #region RefreshCommand
        public Command RefreshCommand
        {
            get
            {
                return new Command(() =>
                {
                    GetChat();
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

        #region Navigation Aware
        public void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            GetChat();
        } 
        #endregion
    }
}
