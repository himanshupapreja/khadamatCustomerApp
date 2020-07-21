using Khadamat_CustomerApp.DependencyInterface;
using Khadamat_CustomerApp.Helpers;
using Khadamat_CustomerApp.Models;
using Khadamat_CustomerApp.Resources;
using Khadamat_CustomerApp.Services.DBService.LiteDB.ModelDB;
using LiteDB;
using Plugin.FilePicker;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;
using static Khadamat_CustomerApp.Helpers.Enums;

namespace Khadamat_CustomerApp.ViewModels
{
    public class ChatDetailPageViewModel : BaseViewModel, INavigationAware
    {

        public GroupChatMessagesDbService groupChatMessagesDbService;

        public static bool IsBackPress;
        private readonly INavigationService NavigationService;
        public JobChatModel JobData;
        MediaFile file = null;
        int COUNT = 0;
        Timer chatTimer;
        List<string> extenstionList = new List<string>()
        {
            "jpg","jpeg","png","gif","xlsx","xls","docx","doc","pdf","txt"
        };

        IDownloader downloader;

        #region ToolbarIcon
        private string _ToolbarIcon;
        public string ToolbarIcon
        {
            get { return _ToolbarIcon; }
            set { SetProperty(ref _ToolbarIcon, value); }
        }
        #endregion

        #region IsJobCompleted
        private bool _IsJobCompleted;
        public bool IsJobCompleted
        {
            get { return _IsJobCompleted; }
            set { SetProperty(ref _IsJobCompleted, value); }
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

        #region IsToolbarPopup Field
        private bool _IsToolbarPopup;

        public bool IsToolbarPopup
        {
            get { return _IsToolbarPopup; }
            set { SetProperty(ref _IsToolbarPopup, value); }
        }
        #endregion

        #region ChatList
        //private ObservableCollection<DummyChatDetailModel> _ChatDetailList = new ObservableCollection<DummyChatDetailModel>();
        //public ObservableCollection<DummyChatDetailModel> ChatDetailList
        //{
        //    get { return _ChatDetailList; }
        //    set { SetProperty(ref _ChatDetailList, value); }
        //}
        #endregion

        #region ChatDetailTitle Field
        private string _ChatDetailTitle;

        public string ChatDetailTitle
        {
            get { return _ChatDetailTitle; }
            set { SetProperty(ref _ChatDetailTitle, value); }
        }
        #endregion

        #region ChatListView property
        private ObservableCollection<ChatDetailListModel> AllChatDetailList = new ObservableCollection<ChatDetailListModel>();
        private ObservableCollection<ChatDetailListModel> _chatDetailList = new ObservableCollection<ChatDetailListModel>();

        public ObservableCollection<ChatDetailListModel> ChatDetailList
        {
            get { return _chatDetailList; }
            set { SetProperty(ref _chatDetailList, value); }
        }

        private List<ChatDetailListModel> chatlistdetail = new List<ChatDetailListModel>();
        private List<ChatDetailListModel> updatechatlistdetail = new List<ChatDetailListModel>();
        #endregion

        #region MakeGroupChatRead
        private async void MakeGroupChatRead()
        {
            var request = new GroupChatreadRequestModel()
            {
                user_id = user_id,
                job_request_id = JobData.job_id
            };
            Chatdetailresponse response;
            try
            {
                MessageEntry = string.Empty;
                response = await _webApiRestClient.PostAsync<GroupChatreadRequestModel, Chatdetailresponse>(ApiUrl.MakeGroupReadChat, request);
            }
            catch (Exception ex)
            {
                Console.WriteLine("SendMsgApi_Exception:- " + ex.Message);
                response = null;
                return;
            }
        }
        #endregion

        #region Constructor
        public ChatDetailPageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;

            groupChatMessagesDbService = new GroupChatMessagesDbService();
            downloader = DependencyService.Get<IDownloader>();
            downloader.OnFileDownloaded += Downloader_OnFileDownloaded;
            IsPopupVisible = false;
            IsBackPress = true;

            
        }

        private void Downloader_OnFileDownloaded(object sender, DownloadEventArgs e)
        {
            if (e.FileSaved)
            {
                App.Current.MainPage.DisplayAlert("XF Downloader", "File Saved Successfully", "Close");
            }
            else
            {
                App.Current.MainPage.DisplayAlert("XF Downloader", "Error while saving the file", "Close");
            }
        }

        #endregion

        #region GetchatList
        private async void GetChatList()
        {
            try
            {
                chatlistdetail = await FirebaseChatHelper.GetChatForGroup("GroupChat", user_id, JobData.job_id);
            }
            catch (Exception ex)
            {
                chatlistdetail = new List<ChatDetailListModel>();
                //await MaterialDialog.Instance.SnackbarAsync(AppResource.error_ServerError, 3000);
            }

            if (AllChatDetailList != null && AllChatDetailList.Count == chatlistdetail.Count)
            {

            }
            else
            {
                AllChatDetailList = new ObservableCollection<ChatDetailListModel>(chatlistdetail);
                ChatDetailList = AllChatDetailList;
                COUNT = ChatDetailList.Count;
                MessagingCenter.Send("", "ScrollToEnd");
                MessagingCenter.Unsubscribe<string, int>(this, "ChatDetailTitle");


                var msgItem = new GroupChatDBModel()
                {
                    GroupJobId = JobData.job_id,
                    UserMessagesList = ChatDetailList
                };
                if (groupChatMessagesDbService.IsGroupChatPresentInDB())
                {
                    var newmsgItem = groupChatMessagesDbService.ReadAllItems().Where(x => x.GroupJobId == JobData.job_id).ToList();
                    if (newmsgItem != null && newmsgItem.Count > 0)
                    {
                        var msgID = newmsgItem.FirstOrDefault().ID;
                        BsonValue bsonid = msgID;
                        groupChatMessagesDbService.UpdateGroupChatDataInDb(bsonid, msgItem);
                    }
                    else
                    {
                        groupChatMessagesDbService.CreateGroupChatDataInDB(msgItem);
                    }
                }
                else
                {
                    groupChatMessagesDbService.CreateGroupChatDataInDB(msgItem);
                }
            }
            if (ChatDetailList != null && ChatDetailList.Count > 0)
            {
                ToolbarIcon = "resource://Khadamat_CustomerApp.SvgImages.toolbar.svg";
            }
            else
            {
                ToolbarIcon = string.Empty;
            }

            chatTimer = new Timer(_ => UpdateChatFromFirebase(), null, 0, 1000);
        }

        private async void UpdateChatFromFirebase()
        {
            try
            {
                try
                {
                    chatlistdetail = await FirebaseChatHelper.GetChatForGroup("GroupChat", user_id, JobData.job_id);
                }
                catch (Exception ex)
                {
                    chatlistdetail = new List<ChatDetailListModel>();
                }
                if (chatlistdetail.Count > ChatDetailList.Count)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        MakeGroupChatRead();
                    });
                    updatechatlistdetail = chatlistdetail.Skip(ChatDetailList.Count).ToList();
                    try
                    {
                        foreach (var item in updatechatlistdetail)
                        {
                            AllChatDetailList.Add(item);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    ChatDetailList = AllChatDetailList;
                    COUNT = ChatDetailList.Count;
                    MessagingCenter.Send("", "ScrollToEnd");


                    var msgItem = new GroupChatDBModel()
                    {
                        GroupJobId = JobData.job_id,
                        UserMessagesList = ChatDetailList
                    };
                    if (groupChatMessagesDbService.IsGroupChatPresentInDB())
                    {
                        var newmsgItem = groupChatMessagesDbService.ReadAllItems().Where(x => x.GroupJobId == JobData.job_id).ToList();
                        if (newmsgItem != newmsgItem)
                        {
                            var msgID = newmsgItem.FirstOrDefault().ID;
                            BsonValue bsonid = msgID;
                            groupChatMessagesDbService.UpdateGroupChatDataInDb(bsonid, msgItem);
                        }
                        else
                        {
                            groupChatMessagesDbService.CreateGroupChatDataInDB(msgItem);
                        }
                    }
                    else
                    {
                        groupChatMessagesDbService.CreateGroupChatDataInDB(msgItem);
                    }
                }

                if (ChatDetailList != null && ChatDetailList.Count > 0)
                {
                    ToolbarIcon = "resource://Khadamat_CustomerApp.SvgImages.toolbar.svg";
                }
                else
                {
                    ToolbarIcon = string.Empty;
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region Message Entry Property
        private string _messageEntry;

        public string MessageEntry
        {
            get { return _messageEntry; }
            set { SetProperty(ref _messageEntry, value); }
        }
        #endregion

        #region SendMsg Command
        public Command SendMsgCommand
        {
            get
            {
                return new Command(() =>
                {
                    if (!string.IsNullOrEmpty(MessageEntry) && !string.IsNullOrWhiteSpace(MessageEntry))
                    {
                        AddMessagetoFirebase();
                    }
                });
            }
        }
        #endregion

        #region AddMessageFirebase
        public async void AddMessageFirebase(ChatDetailListModel item)
        {
            try
            {
                if (Common.CheckConnection())
                {
                    Chatdetailresponse response;
                    try
                    {
                        response = await _webApiRestClient.GetAsync<Chatdetailresponse>(string.Format(ApiUrl.AddGroupChatRequest, JobData.job_id, Convert.ToInt32(Enums.UserTypeEnum.Customer)));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("SendMsgApi_Exception:- " + ex.Message);
                        response = null;
                        IsBackPress = true;
                        //await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_ServerError, msDuration: 3000);
                        return;
                    }
                    if (response != null)
                    {
                        if (response.status)
                        {
                            MessagingCenter.Send("", "ScrollToEnd");
                            MessageEntry = string.Empty;
                            var data = await FirebaseChatHelper.AddChatMessageForGroup(item, "GroupChat");
                        }
                        else if (response.message == null)
                        {
                            MessagingCenter.Send("", "ScrollToEnd");
                            MessageEntry = string.Empty;
                            var data = await FirebaseChatHelper.AddChatMessageForGroup(item, "GroupChat");
                        }
                        else
                        {
                            await MaterialDialog.Instance.SnackbarAsync(message: response.message,
                                        msDuration: 3000);
                        }


                        var msgItem = new GroupChatDBModel()
                        {
                            GroupJobId = JobData.job_id,
                            UserMessagesList = ChatDetailList
                        };
                        if (groupChatMessagesDbService.IsGroupChatPresentInDB())
                        {
                            var newmsgItem = groupChatMessagesDbService.ReadAllItems().Where(x => x.GroupJobId == JobData.job_id).ToList();
                            if (newmsgItem != null && newmsgItem.Count > 0)
                            {
                                var msgID = newmsgItem.FirstOrDefault().ID;
                                msgItem.ID = msgID;
                                BsonValue bsonid = msgID;
                                groupChatMessagesDbService.UpdateGroupChatDataInDb(bsonid, msgItem);
                            }
                            else
                            {
                                groupChatMessagesDbService.CreateGroupChatDataInDB(msgItem);
                            }
                        }
                        else
                        {
                            groupChatMessagesDbService.CreateGroupChatDataInDB(msgItem);
                        }
                    }

                    IsBackPress = true;
                }
                else
                {
                    await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_InternetError,
                                            msDuration: 3000);
                }

            }
            catch (Exception ex)
            {
                //await MaterialDialog.Instance.SnackbarAsync(message: ex.Message,
                //                            msDuration: 3000);
                IsBackPress = true;
                Console.WriteLine("SendMsgCommand_Exception:- " + ex.Message);
            }
        }
        #endregion

        #region AddMessagetoFirebase
        public async void AddMessagetoFirebase()
        {
            try
            {
                if (Common.CheckConnection())
                {
                    var item = new ChatDetailListModel()
                    {
                        sender_user_id = user_id,
                        job_id = JobData.job_id,
                        coordinator_id = JobData.coordinator_id,
                        worker_id = JobData.worker_id,
                        customer_id = null,
                        sender_user_Name = user_name,
                        coordinator_Name = !string.IsNullOrEmpty(JobData.coordinator_Name) ? AppResource.coordinator_Name : JobData.coordinator_Name,
                        job_Name = ChatDetailTitle,
                        receiver_user_Name = null,
                        worker_Name = JobData.worker_Name,
                        customer_Name = null,
                        image_url = null,
                        file_url = null,
                        is_file = false,
                        is_image = false,
                        is_loading = false,
                        IsViewBtnVisible = false,
                        is_message = true,
                        msg_datetime = DateTime.Now,
                        user_message = MessageEntry,
                        user_message_time = DateTime.Now.ToString("dd/MM/yyyy, hh:mm:ss tt"),
                        is_sender = true,
                        time_stamp = DependencyService.Get<IGetTimeStamp>().TimeStamp()
                    };
                    //var request = new ChatDetailModelApi()
                    //{
                    //    from_user_id = user_id,
                    //    to_user_id = JobData.job_id
                    //};
                    Chatdetailresponse response;
                    try
                    {
                        MessageEntry = string.Empty;
                        response = await _webApiRestClient.GetAsync<Chatdetailresponse>(string.Format(ApiUrl.AddGroupChatRequest, JobData.job_id, Convert.ToInt32(Enums.UserTypeEnum.Customer)));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("SendMsgApi_Exception:- " + ex.Message);
                        response = null;
                        //await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_ServerError, msDuration: 3000);
                        return;
                    }
                    if (response != null)
                    {
                        if (response.status)
                        {
                            try
                            {
                                AllChatDetailList.Add(item);
                            }
                            catch (Exception ex)
                            {
                            }
                            ChatDetailList = AllChatDetailList;
                            MessagingCenter.Send("", "ScrollToEnd");
                            MessageEntry = string.Empty;
                            var data = await FirebaseChatHelper.AddChatMessageForGroup(item, "GroupChat");


                        }
                        else if (response.message == null)
                        {
                            try
                            {
                                AllChatDetailList.Add(item);
                            }
                            catch (Exception ex)
                            {
                            }

                            ChatDetailList = AllChatDetailList;
                            MessagingCenter.Send("", "ScrollToEnd");
                            MessageEntry = string.Empty;
                            var data = await FirebaseChatHelper.AddChatMessageForGroup(item, "GroupChat");
                        }
                        else
                        {
                            await MaterialDialog.Instance.SnackbarAsync(message: response.message,
                                        msDuration: 3000);
                        }

                        var msgItem = new GroupChatDBModel()
                        {
                            GroupJobId = JobData.job_id,
                            UserMessagesList = ChatDetailList
                        };
                        if (groupChatMessagesDbService.IsGroupChatPresentInDB())
                        {
                            var newmsgItem = groupChatMessagesDbService.ReadAllItems().Where(x => x.GroupJobId == JobData.job_id).ToList();
                            if (newmsgItem != null && newmsgItem.Count > 0)
                            {
                                var msgID = newmsgItem.FirstOrDefault().ID;
                                msgItem.ID = msgID;
                                BsonValue bsonid = msgID;
                                groupChatMessagesDbService.UpdateGroupChatDataInDb(bsonid, msgItem);
                            }
                            else
                            {
                                groupChatMessagesDbService.CreateGroupChatDataInDB(msgItem);
                            }
                        }
                        else
                        {
                            groupChatMessagesDbService.CreateGroupChatDataInDB(msgItem);
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
                //await MaterialDialog.Instance.SnackbarAsync(message: ex.Message,
                //                            msDuration: 3000);
                Console.WriteLine("SendMsgCommand_Exception:- " + ex.Message);
            }
        }
        #endregion

        #region AttachmentCommand
        public Command AttachmentCommand
        {
            get
            {
                return new Command(() =>
                {
                    IsPopupVisible = true;
                    IsToolbarPopup = false;

                    //TabBarPopup popup = new TabBarPopup(JobData.job_id, this,false);
                    //App.Current.MainPage.Navigation.PushPopupAsync(popup);
                });
            }
        }
        #endregion

        #region RightIconCommand
        public Command RightIconCommand
        {
            get
            {
                return new Command(() =>
                {
                    if (ChatDetailList != null && ChatDetailList.Count > 0)
                    {
                        IsPopupVisible = true;
                        IsToolbarPopup = true;
                    }

                    //TabBarPopup popup = new TabBarPopup(JobData.job_id,this);
                    //App.Current.MainPage.Navigation.PushPopupAsync(popup);
                });
            }
        }
        #endregion

        #region DeleteChatCommand
        public Command DeleteChatCommand
        {
            get
            {
                return new Command(async (e) =>
                {
                    try
                    {
                        var answer = await App.Current.MainPage.DisplayAlert(AppResource.DeleteChat, AppResource.DeleteChatConfirm, AppResource.Yes, AppResource.No);
                        if (answer)
                        {
                            await FirebaseChatHelper.DeleteGroupChat("GroupChat", user_id, JobData.job_id);
                            chatlistdetail = new List<ChatDetailListModel>();
                            AllChatDetailList = new ObservableCollection<ChatDetailListModel>();
                            ChatDetailList = AllChatDetailList;
                            IsPopupVisible = false;
                            IsToolbarPopup = false;
                            ToolbarIcon = string.Empty;
                        }
                    }
                    catch
                    {
                    }
                });
            }
        }
        #endregion

        #region ImageUploadCommand

        public Command ImageUploadCommand
        {
            get
            {
                return new Command(async (e) =>
                {
                    try
                    {
                        //if (Device.RuntimePlatform == Device.Android)
                        //{
                        //    DependencyService.Get<IStoragePermissions>().GetGalleryPermissions();
                        //}
                        var resultrs = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
                        if (resultrs != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                        {
                            await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera, Permission.Storage);
                        }


                        var action = await App.Current.MainPage.DisplayActionSheet(AppResource.AddPhoto, AppResource.Cancel, null, AppResource.Camera, AppResource.Gallery);

                        MediaFile file;

                        if (action == AppResource.Gallery)
                        {
                            IsPopupVisible = false;
                            await CrossMedia.Current.Initialize();
                            if (!CrossMedia.Current.IsPickPhotoSupported)
                            {
                                return;
                            }
                            byte[] myfile;
                            file = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                            {
                                //PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,

                            });
                            if (file != null)
                            {
                                using (var memoryStream = new MemoryStream())
                                {
                                    file.GetStream().CopyTo(memoryStream);
                                    myfile = memoryStream.ToArray();
                                }

                            }
                        }
                        else if (action == AppResource.Camera)
                        {
                            IsPopupVisible = false;
                            await CrossMedia.Current.Initialize();
                            if (!CrossMedia.Current.IsTakePhotoSupported)
                            {
                                return;
                            }
                            byte[] myfile;
                            file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                            {
                                Directory = "Profile Photo",
                                SaveToAlbum = true,
                                PhotoSize = PhotoSize.Medium,
                                MaxWidthHeight = 2000,
                                DefaultCamera = CameraDevice.Rear
                            });
                            if (file != null)
                            {
                                using (var memoryStream = new MemoryStream())
                                {
                                    file.GetStream().CopyTo(memoryStream);
                                    myfile = memoryStream.ToArray();
                                }

                            }
                        }
                        else
                        {
                            return;
                        }
                        if (file != null)
                        {
                            var fileinfo = new FileInfo(file.Path);
                            var fileLength = Common.ConvertBytesToMegabytes(fileinfo.Length);
                            if (fileLength < 1.2)
                            {
                                var item = new ChatDetailListModel()
                                {
                                    sender_user_id = user_id,
                                    job_id = JobData.job_id,
                                    coordinator_id = JobData.coordinator_id,
                                    worker_id = JobData.worker_id,
                                    customer_id = null,
                                    sender_user_Name = user_name,
                                    coordinator_Name = !string.IsNullOrEmpty(JobData.coordinator_Name) ? AppResource.coordinator_Name : JobData.coordinator_Name,
                                    job_Name = ChatDetailTitle,
                                    receiver_user_Name = null,
                                    worker_Name = JobData.worker_Name,
                                    customer_Name = null,
                                    image_url = null,
                                    file_url = null,
                                    is_loading = true,
                                    IsViewBtnVisible = false,
                                    is_file = false,
                                    is_image = true,
                                    is_message = false,
                                    user_message = string.Empty,
                                    user_message_time = DateTime.Now.ToString("dd/MM/yyyy, hh:mm:ss tt"),
                                    is_sender = true,
                                    msg_datetime = DateTime.Now,
                                    time_stamp = DependencyService.Get<IGetTimeStamp>().TimeStamp()
                                };
                                IsBackPress = false;
                                try
                                {
                                    AllChatDetailList.Add(item);
                                }
                                catch (Exception ex)
                                {
                                }
                                ChatDetailList = AllChatDetailList;
                                var returnUrl = await FirebaseChatHelper.StoreImages(file.GetStream(), Path.GetFileName(file.Path));
                                var index = ChatDetailList.IndexOf(ChatDetailList.LastOrDefault());
                                ChatDetailList[index].is_loading = false;
                                ChatDetailList[index].IsViewBtnVisible = true;
                                ChatDetailList[index].image_url = returnUrl;
                                if (ChatDetailList.Count > COUNT)
                                {
                                    COUNT = ChatDetailList.Count;
                                    AddMessageFirebase(ChatDetailList[index]);
                                }
                            }
                            else
                            {
                                await MaterialDialog.Instance.SnackbarAsync(AppResource.msg_attachmentError, 3000);
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                });
            }
        }

        //public Command ImageUploadCommand
        //{
        //    get
        //    {
        //        return new Command(async () =>
        //        {
        //            var action = await App.Current.MainPage.DisplayActionSheet(AppResource.AddPhoto, AppResource.Cancel, null, AppResource.Camera, AppResource.Gallery);
        //            if (action == AppResource.Camera)
        //            {
        //                IsPopupVisible = false;
        //                IsToolbarPopup = false;
        //                await CameraCommand();
        //            }
        //            else if (action == AppResource.Gallery)
        //            {
        //                IsPopupVisible = false;
        //                IsToolbarPopup = false;
        //                await GalleryCommand();
        //            }

        //            //if (action == AppResource.Camera)
        //            //{
        //            //    IsPopupVisible = false;
        //            //    IsToolbarPopup = false;
        //            //    await CrossMedia.Current.Initialize();
        //            //    if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsPickPhotoSupported)
        //            //    {
        //            //        return;
        //            //    }
        //            //    if (Device.RuntimePlatform == "iOS")
        //            //    {
        //            //        await Task.Delay(1000);
        //            //    }
        //            //    file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
        //            //    {
        //            //        Directory = "Chat Photo",
        //            //        SaveToAlbum = true,
        //            //        PhotoSize = PhotoSize.Medium,
        //            //        DefaultCamera = CameraDevice.Rear
        //            //    });
        //            //    if (file == null)
        //            //        return;
        //            //    using (var memoryStream = new MemoryStream())
        //            //    {
        //            //        file.GetStream().CopyTo(memoryStream);
        //            //    }
        //            //}
        //            //else if (action == AppResource.Gallery)
        //            //{
        //            //    IsPopupVisible = false;
        //            //    IsToolbarPopup = false;
        //            //    await CrossMedia.Current.Initialize();
        //            //    if (!CrossMedia.Current.IsPickPhotoSupported)
        //            //    {
        //            //        return;
        //            //    }
        //            //    file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
        //            //    {
        //            //        PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,
        //            //    });
        //            //    if (file != null)
        //            //    {
        //            //        using (var memoryStream = new MemoryStream())
        //            //        {
        //            //            file.GetStream().CopyTo(memoryStream);
        //            //        }
        //            //    }
        //            //}
        //            //else
        //            //{
        //            //    return;
        //            //}

        //            //MessagingCenter.Send(file, "ImageChat");
        //            //file.Dispose();
        //        });
        //    }
        //}
        #endregion

        #region FileUploadCommand
        public Command FileUploadCommand
        {
            get
            {
                return new Command(async (e) =>
                {
                    IsPopupVisible = false;
                    FileuploadModel FileuploadModel = new FileuploadModel();
                    var data = await CrossFilePicker.Current.PickFile();
                    if (data != null)
                    {
                        var fileinfo = new FileInfo(data.FilePath);
                        double fileLength;
                        try
                        {
                            fileLength = Common.ConvertBytesToMegabytes(fileinfo.Length);
                        }
                        catch (Exception ex)
                        {
                            fileLength = 0;
                        }
                        if (fileLength < 1.2)
                        {
                            FileuploadModel.DataArray = data.DataArray;
                            FileuploadModel.FileName = data.FileName;


                            if (Common.FileExtensionCheck(FileuploadModel.FileName, extenstionList))
                            {
                                var item = new ChatDetailListModel()
                                {
                                    sender_user_id = user_id,
                                    job_id = JobData.job_id,
                                    coordinator_id = JobData.coordinator_id,
                                    worker_id = JobData.worker_id,
                                    customer_id = null,
                                    sender_user_Name = user_name,
                                    coordinator_Name = !string.IsNullOrEmpty(JobData.coordinator_Name) ? AppResource.coordinator_Name : JobData.coordinator_Name,
                                    job_Name = ChatDetailTitle,
                                    receiver_user_Name = null,
                                    worker_Name = JobData.worker_Name,
                                    customer_Name = null,
                                    image_url = null,
                                    file_url = null,
                                    is_loading = true,
                                    is_file = true,
                                    is_image = false,
                                    IsViewBtnVisible = false,
                                    is_message = false,
                                    file_name = FileuploadModel.FileName,
                                    user_message = string.Empty,
                                    user_message_time = DateTime.Now.ToString("dd/MM/yyyy, hh:mm:ss tt"),
                                    is_sender = true,
                                    msg_datetime = DateTime.Now,
                                    time_stamp = DependencyService.Get<IGetTimeStamp>().TimeStamp()
                                };
                                IsBackPress = false;
                                try
                                {
                                    AllChatDetailList.Add(item);
                                }
                                catch (Exception ex)
                                {
                                }
                                ChatDetailList = AllChatDetailList;
                                var returnUrl = await FirebaseChatHelper.StoreImages(new MemoryStream(FileuploadModel.DataArray), Path.GetFileName(FileuploadModel.FileName));
                                var index = ChatDetailList.IndexOf(ChatDetailList.LastOrDefault());
                                ChatDetailList[index].is_loading = false;
                                ChatDetailList[index].IsViewBtnVisible = true;
                                ChatDetailList[index].file_url = returnUrl;
                                if (ChatDetailList.Count > COUNT)
                                {
                                    if (string.IsNullOrEmpty(returnUrl) || string.IsNullOrWhiteSpace(returnUrl))
                                    {
                                        await App.Current.MainPage.DisplayAlert("", AppResource.error_FileUploading, AppResource.Ok);
                                    }
                                    else
                                    {
                                        COUNT = ChatDetailList.Count;
                                        AddMessageFirebase(ChatDetailList[index]);
                                    }
                                }
                            }
                            else
                            {
                                await App.Current.MainPage.DisplayAlert("", AppResource.error_invalidFileExtension, AppResource.Ok);
                            }
                        }
                        else
                        {
                            await MaterialDialog.Instance.SnackbarAsync(AppResource.msg_attachmentError, 3000);
                        }
                    }
                });
            }
        }
        #endregion

        #region DownloadFileCommand
        public Command DownloadFileCommand
        {
            get
            {
                return new Command((e) =>
                {
                    var data = (ChatDetailListModel)e;
                    downloader.DownloadFile(data.file_url, "khadamat_Downloads");
                });
            }
        }
        #endregion

        #region CloseCommand
        public Command CloseCommand
        {
            get
            {
                return new Command((e) =>
                {
                    IsPopupVisible = false;
                });
            }
        }
        #endregion

        #region OnDisappearing
        public void OnDisappearing()
        {
            if (chatTimer != null)
            {
                chatTimer.Dispose();
            }
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("ChatDetailTitle"))
            {
                ChatDetailTitle = (string)parameters["ChatDetailTitle"];
            }
            if (parameters.ContainsKey("ChatDetailData"))
            {
                JobData = (JobChatModel)parameters["ChatDetailData"];
            }
            if(JobData !=null && JobData.job_Status.HasValue && (JobData.job_Status.Value == Convert.ToInt32(JobRequestEnum.Completed) || JobData.job_Status.Value == Convert.ToInt32(JobRequestEnum.Canceled) || JobData.job_Status.Value == Convert.ToInt32(JobRequestEnum.Closed) || JobData.job_Status.Value == Convert.ToInt32(JobRequestEnum.QuoteCanceled)))
            {
                IsJobCompleted = true;
            }
            else
            {
                IsJobCompleted = false;
            }

            if (groupChatMessagesDbService.IsGroupChatPresentInDB())
            {
                var dataitem = groupChatMessagesDbService.ReadAllItems().Where(x => x.GroupJobId == JobData.job_id).ToList();
                if (dataitem != null && dataitem.Count > 0)
                {
                    var data = dataitem.FirstOrDefault();
                    AllChatDetailList = data.UserMessagesList;
                    ChatDetailList = AllChatDetailList;
                    MessagingCenter.Send("", "ScrollToEnd"); 
                }
            }
            if (Common.CheckConnection())
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    GetChatList();
                });
                Device.BeginInvokeOnMainThread(() =>
                {
                    MakeGroupChatRead();
                });
            }
        }
        #endregion

        #region BackIconCommand
        public ICommand BackIconCommand
        {
            get
            {
                return new DelegateCommand(async() =>
                {
                    await NavigationService.GoBackAsync();
                });
            }
        }
        #endregion
    }
}
