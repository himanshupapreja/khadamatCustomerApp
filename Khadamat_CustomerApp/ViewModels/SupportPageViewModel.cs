using Khadamat_CustomerApp.DependencyInterface;
using Khadamat_CustomerApp.Helpers;
using Khadamat_CustomerApp.Models;
using Khadamat_CustomerApp.Resources;
using Plugin.FilePicker;
using Plugin.Media;
using Plugin.Media.Abstractions;
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
using Xamarin.Essentials;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace Khadamat_CustomerApp.ViewModels
{
    public class SupportPageViewModel : BaseViewModel, INavigationAware
    {
        public static bool IsBackPress;
        private readonly INavigationService NavigationService;

        int RecieverId;
        int COUNT = 0;

        List<string> extenstionList = new List<string>()
        {
            "jpg","jpeg","png","gif","xlsx","xls","docx","doc","pdf","txt"
        };

        Timer chatTimer;

        #region DeleteChatIcon
        private string _DeleteChatIcon;
        public string DeleteChatIcon
        {
            get { return _DeleteChatIcon; }
            set { SetProperty(ref _DeleteChatIcon, value); }
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

        #region ChatDetailTitle Field
        private string _ChatDetailTitle;

        public string ChatDetailTitle
        {
            get { return _ChatDetailTitle; }
            set { SetProperty(ref _ChatDetailTitle, value); }
        }
        #endregion

        #region SupportChatList
        private ObservableCollection<SingleChatListModel> AllSupportChatList = new ObservableCollection<SingleChatListModel>();
        private ObservableCollection<SingleChatListModel> _SupportChatList = new ObservableCollection<SingleChatListModel>();
        public ObservableCollection<SingleChatListModel> SupportChatList
        {
            get { return _SupportChatList; }
            set { SetProperty(ref _SupportChatList, value); }
        }

        private List<SingleChatListModel> supportchatlistdetail = new List<SingleChatListModel>();
        private List<SingleChatListModel> updatesupportchatlistdetail = new List<SingleChatListModel>();
        #endregion

        #region Constructor
        public SupportPageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
            IsBackPress = true;
        }
        #endregion

        #region RightIconCommand
        public ICommand RightIconCommand
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    if (SupportChatList != null && SupportChatList.Count > 0)
                    {
                        var deleteChatResult = await App.Current.MainPage.DisplayAlert(AppResource.DeleteChat, AppResource.DeleteChatConfirm, AppResource.Yes, AppResource.No);
                        if (deleteChatResult)
                        {
                            await FirebaseChatHelper.DeleteSingleChat("Chat", user_id, RecieverId);
                            AllSupportChatList = new ObservableCollection<SingleChatListModel>();
                            SupportChatList = AllSupportChatList;
                            DeleteChatIcon = string.Empty;
                        }
                    }
                });
            }
        }
        #endregion

        #region AddMessageFirebase
        public async void AddMessageFirebase(SingleChatListModel item)
        {
            try
            {
                if (Common.CheckConnection())
                {
                    var request = new ChatDetailModelApi()
                    {
                        from_user_id = user_id,
                        to_user_id = RecieverId
                    };
                    Chatdetailresponse response;
                    try
                    {
                        response = await _webApiRestClient.PostAsync<ChatDetailModelApi, Chatdetailresponse>(ApiUrl.AddChatRequest, request);
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
                            var data = await FirebaseChatHelper.AddChatMessage(item);
                        }
                        else if (response.message == null)
                        {
                            MessagingCenter.Send("", "ScrollToEnd");
                            MessageEntry = string.Empty;
                            var data = await FirebaseChatHelper.AddChatMessage(item);
                        }
                        else
                        {
                            await MaterialDialog.Instance.SnackbarAsync(message: response.message,
                                        msDuration: 3000);
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
                await MaterialDialog.Instance.SnackbarAsync(message: ex.Message,
                                            msDuration: 3000);
                IsBackPress = true;
                Console.WriteLine("SendMsgCommand_Exception:- " + ex.Message);
            }
        }
        #endregion

        #region MakeChatRead
        private async void MakeChatRead()
        {
            var request = new ChatDetailModelApi()
            {
                from_user_id = user_id,
                to_user_id = RecieverId
            };
            Chatdetailresponse response;
            try
            {
                MessageEntry = string.Empty;
                response = await _webApiRestClient.PostAsync<ChatDetailModelApi, Chatdetailresponse>(ApiUrl.MakeReadChat, request);
            }
            catch (Exception ex)
            {
                Console.WriteLine("SendMsgApi_Exception:- " + ex.Message);
                response = null;
                return;
            }
        }
        #endregion

        #region GetchatList
        private async void GetFirebaseChatList()
        {
            try
            {
                supportchatlistdetail = await FirebaseChatHelper.GetChatForUserID(user_id, recieverUserId: RecieverId);
            }
            catch (Exception ex)
            {
                supportchatlistdetail = new List<SingleChatListModel>();
            }
            //var _sortedlist = supportchatlistdetail.OrderBy(x => x.TimeStamp).ToList();
            AllSupportChatList = new ObservableCollection<SingleChatListModel>();
            try
            {
                foreach (var item in supportchatlistdetail)
                {
                    //if (!string.IsNullOrEmpty(item.image_url) && !string.IsNullOrWhiteSpace(item.image_url))
                    //{
                    //    item.displayimage_url = new UriImageSource()
                    //    {
                    //        Uri = new Uri(item.image_url),
                    //        CachingEnabled = true,
                    //        CacheValidity = new TimeSpan(5, 0, 0, 0)
                    //    };
                    //}
                    AllSupportChatList.Add(item);
                }
            }
            catch (Exception ex)
            {

            }

            SupportChatList = AllSupportChatList;
            COUNT = SupportChatList.Count;
            if (SupportChatList != null && SupportChatList.Count > 0)
            {
                DeleteChatIcon = "resource://Khadamat_CustomerApp.SvgImages.delete.svg";
            }
            else
            {
                DeleteChatIcon = string.Empty;
            }
            MessagingCenter.Send("", "ScrollToEnd");
            MessagingCenter.Unsubscribe<string, int>(this, "ChatDetailTitle");


            chatTimer = new Timer(_ => UpdateChatFromFirebase(), null, 0, 1000);
        }

        private async void UpdateChatFromFirebase()
        {
            try
            {
                try
                {
                    supportchatlistdetail = await FirebaseChatHelper.GetChatForUserID(user_id, recieverUserId: RecieverId);
                }
                catch (Exception ex)
                {
                    supportchatlistdetail = new List<SingleChatListModel>();
                }
                //var _sortedlist = supportchatlistdetail.OrderBy(x => x.TimeStamp).ToList();
                //supportchatlistdetail = _sortedlist;
                if (supportchatlistdetail.Count > SupportChatList.Count)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        MakeChatRead();
                    });
                    updatesupportchatlistdetail = supportchatlistdetail.Skip(SupportChatList.Count).ToList();
                    try
                    {
                        foreach (var item in updatesupportchatlistdetail)
                        {
                            //if (!string.IsNullOrEmpty(item.image_url) && !string.IsNullOrWhiteSpace(item.image_url))
                            //{
                            //    item.displayimage_url = new UriImageSource()
                            //    {
                            //        Uri = new Uri(item.image_url),
                            //        CachingEnabled = true,
                            //        CacheValidity = new TimeSpan(5, 0, 0, 0)
                            //    };
                            //}
                            AllSupportChatList.Add(item);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    SupportChatList = AllSupportChatList;
                    COUNT = SupportChatList.Count;
                    MessagingCenter.Send("", "ScrollToEnd");
                }


                if (SupportChatList != null && SupportChatList.Count > 0)
                {
                    DeleteChatIcon = "resource://Khadamat_CustomerApp.SvgImages.delete.svg";
                }
                else
                {
                    DeleteChatIcon = string.Empty;
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
                return new Command(async () =>
                {
                    try
                    {
                        if (!string.IsNullOrEmpty(MessageEntry) && !string.IsNullOrWhiteSpace(MessageEntry))
                        {
                            if (Common.CheckConnection())
                            {
                                var item = new SingleChatListModel()
                                {
                                    sender_user_id = user_id,
                                    receiver_user_id = RecieverId,
                                    user_message = MessageEntry,
                                    user_message_time = DateTime.Now.ToString("dd/MM/yyyy, hh:mm tt"),
                                    msg_datetime = DateTime.Now,
                                    is_sender = true,
                                    is_header_visible = false,
                                    is_message = true,
                                    IsViewBtnVisible = false,
                                    is_file = false,
                                    is_image = false,
                                    is_loading = false,
                                    time_stamp = DependencyService.Get<IGetTimeStamp>().TimeStamp(),
                                };
                                var request = new ChatDetailModelApi()
                                {
                                    from_user_id = user_id,
                                    to_user_id = RecieverId
                                };
                                Chatdetailresponse response;
                                try
                                {
                                    MessageEntry = string.Empty;
                                    response = await _webApiRestClient.PostAsync<ChatDetailModelApi, Chatdetailresponse>(ApiUrl.AddChatRequest, request);
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
                                            AllSupportChatList.Add(item);
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                        SupportChatList = AllSupportChatList;
                                        COUNT = SupportChatList.Count;
                                        MessagingCenter.Send("", "ScrollToEnd");
                                        MessageEntry = string.Empty;
                                        var data = await FirebaseChatHelper.AddChatMessage(item);
                                    }
                                    else if (response.message == null)
                                    {
                                        try
                                        {
                                            AllSupportChatList.Add(item);
                                        }
                                        catch (Exception ex)
                                        {
                                        }
                                        SupportChatList = AllSupportChatList;
                                        COUNT = SupportChatList.Count;
                                        MessagingCenter.Send("", "ScrollToEnd");
                                        MessageEntry = string.Empty;
                                        var data = await FirebaseChatHelper.AddChatMessage(item);
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
                                await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_InternetError,
                                                        msDuration: 3000);
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        await MaterialDialog.Instance.SnackbarAsync(message: ex.Message,
                                                    msDuration: 3000);
                        Console.WriteLine("SendMsgCommand_Exception:- " + ex.Message);
                    }
                });
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
                });
            }
        }
        #endregion

        //#region AddMessageFirebase
        //public async void AddMessageFirebase(ChatDetailListModel item)
        //{
        //    try
        //    {
        //        if (Common.CheckConnection())
        //        {
        //            var request = new ChatDetailModelApi()
        //            {
        //                from_user_id = user_id,
        //                to_user_id = RecieverId
        //            };
        //            Chatdetailresponse response;
        //            try
        //            {
        //                response = await _webApiRestClient.PostAsync<ChatDetailModelApi, Chatdetailresponse>(ApiUrl.AddChatRequest, request);
        //            }
        //            catch (Exception ex)
        //            {
        //                Console.WriteLine("SendMsgApi_Exception:- " + ex.Message);
        //                response = null;
        //                await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_ServerError, msDuration: 3000);
        //                return;
        //            }
        //            if (response != null)
        //            {
        //                if (response.status)
        //                {
        //                    MessagingCenter.Send("", "ScrollToEnd");
        //                    MessageEntry = string.Empty;
        //                    var data = await FirebaseChatHelper.AddChatMessageForGroup(item, "GroupChat");
        //                }
        //                else if (response.message == null)
        //                {
        //                    MessagingCenter.Send("", "ScrollToEnd");
        //                    MessageEntry = string.Empty;
        //                    var data = await FirebaseChatHelper.AddChatMessageForGroup(item, "GroupChat");
        //                }
        //                else
        //                {
        //                    await MaterialDialog.Instance.SnackbarAsync(message: response.message,
        //                                msDuration: 3000);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_InternetError,
        //                                    msDuration: 3000);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        await MaterialDialog.Instance.SnackbarAsync(message: ex.Message,
        //                                    msDuration: 3000);
        //        Console.WriteLine("SendMsgCommand_Exception:- " + ex.Message);
        //    }
        //}
        //#endregion

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
                                PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,

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
                            var item = new SingleChatListModel()
                            {
                                sender_user_id = user_id,
                                receiver_user_id = RecieverId,
                                user_message = string.Empty,
                                user_message_time = DateTime.Now.ToString("dd/MM/yyyy, hh:mm tt"),
                                msg_datetime = DateTime.Now,
                                is_sender = true,
                                is_header_visible = false,
                                is_message = false,
                                IsViewBtnVisible = false,
                                is_loading = true,
                                is_file = false,
                                is_image = true,
                                image_url = null,
                                //displayimage_url = null,
                                file_url = null,
                                time_stamp = DependencyService.Get<IGetTimeStamp>().TimeStamp(),
                            };
                            IsBackPress = false;
                            //var request = new ChatDetailModelApi()
                            //{
                            //    from_user_id = user_id,
                            //    to_user_id = RecieverId
                            //};
                            //Chatdetailresponse response;
                            try
                            {
                                AllSupportChatList.Add(item);
                            }
                            catch (Exception ex)
                            {
                            }

                            SupportChatList = AllSupportChatList;
                            var returnUrl = await FirebaseChatHelper.StoreImages(file.GetStream(), Path.GetFileName(file.Path));
                            var index = SupportChatList.IndexOf(SupportChatList.LastOrDefault());
                            if (!string.IsNullOrEmpty(returnUrl) && !string.IsNullOrWhiteSpace(returnUrl))
                            {
                                SupportChatList[index].is_loading = false;
                                SupportChatList[index].IsViewBtnVisible = true;
                                SupportChatList[index].image_url = returnUrl;
                                //SupportChatList[index].displayimage_url = new UriImageSource()
                                //{
                                //    Uri = new Uri(returnUrl),
                                //    CachingEnabled = true,
                                //    CacheValidity = new TimeSpan(5, 0, 0, 0)
                                //};
                            }
                            else
                            {
                                AllSupportChatList.Remove(item);
                                SupportChatList = AllSupportChatList;
                            }

                            if (string.IsNullOrEmpty(returnUrl) || string.IsNullOrWhiteSpace(returnUrl))
                            {
                                await App.Current.MainPage.DisplayAlert("", AppResource.error_ImageUploading, AppResource.Ok);
                                IsBackPress = true;
                            }
                            else
                            {
                                if (SupportChatList.Count > COUNT)
                                {
                                    COUNT = SupportChatList.Count;
                                    AddMessageFirebase(SupportChatList[index]);
                                }
                            } 
                        }

                        //try
                        //{
                        //    MessageEntry = string.Empty;
                        //    response = await _webApiRestClient.PostAsync<ChatDetailModelApi, Chatdetailresponse>(ApiUrl.AddChatRequest, request);
                        //}
                        //catch (Exception ex)
                        //{
                        //    Console.WriteLine("SendMsgApi_Exception:- " + ex.Message);
                        //    response = null;
                        //    await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_ServerError, msDuration: 3000);
                        //    return;
                        //}
                        //if (response != null)
                        //{
                        //    if (response.status)
                        //    {

                        //    }
                        //    else if (string.IsNullOrWhiteSpace(response.message) || string.IsNullOrEmpty(response.message))
                        //    {
                        //        try
                        //        {
                        //            AllSupportChatList.Add(item);
                        //        }
                        //        catch (Exception ex)
                        //        {
                        //        }

                        //        SupportChatList = AllSupportChatList;
                        //        var returnUrl = await FirebaseChatHelper.StoreImages(file.GetStream(), Path.GetFileName(file.Path));
                        //        var index = SupportChatList.IndexOf(SupportChatList.LastOrDefault());
                        //        if (!string.IsNullOrEmpty(returnUrl) && !string.IsNullOrWhiteSpace(returnUrl))
                        //        {
                        //            SupportChatList[index].is_loading = false;
                        //            SupportChatList[index].image_url = returnUrl;
                        //            SupportChatList[index].displayimage_url = new UriImageSource()
                        //            {
                        //                Uri = new Uri(returnUrl),
                        //                CachingEnabled = true,
                        //                CacheValidity = new TimeSpan(5, 0, 0, 0)
                        //            };
                        //        }
                        //        else
                        //        {
                        //            AllSupportChatList.Remove(item);
                        //            SupportChatList = AllSupportChatList;
                        //        }

                        //        if (string.IsNullOrEmpty(returnUrl) || string.IsNullOrWhiteSpace(returnUrl))
                        //        {
                        //            await App.Current.MainPage.DisplayAlert("", AppResource.error_ImageUploading, AppResource.Ok);
                        //        }
                        //        else
                        //        {
                        //            await App.Current.MainPage.DisplayAlert("", AppResource.success_ImageUploading, AppResource.Ok);
                        //            if (SupportChatList.Count > COUNT)
                        //            {
                        //                COUNT = SupportChatList.Count;
                        //                AddMessageFirebase(SupportChatList[index]);
                        //            }
                        //        }
                        //    }
                        //    else
                        //    {
                        //        await MaterialDialog.Instance.SnackbarAsync(message: response.message,
                        //                            msDuration: 3000);
                        //    }
                        //}
                    }
                    catch (Exception)
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
        //                await CameraCommand();
        //            }
        //            else if (action == AppResource.Gallery)
        //            {
        //                IsPopupVisible = false;
        //                await GalleryCommand();
        //            }
        //        });
        //    }
        //}
        #endregion

        #region FileUploadCommand
        public ICommand FileUploadCommand
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    IsPopupVisible = false;
                    FileuploadModel FileuploadModel = new FileuploadModel();
                    var data = await CrossFilePicker.Current.PickFile();
                    if (data != null)
                    {
                        FileuploadModel.DataArray = data.DataArray;
                        FileuploadModel.FileName = data.FileName;


                        if (Common.FileExtensionCheck(FileuploadModel.FileName, extenstionList))
                        {
                            var item = new SingleChatListModel()
                            {
                                sender_user_id = user_id,
                                receiver_user_id = RecieverId,
                                user_message = string.Empty,
                                user_message_time = DateTime.Now.ToString("dd/MM/yyyy, hh:mm tt"),
                                msg_datetime = DateTime.Now,
                                is_sender = true,
                                is_header_visible = false,
                                is_message = false,
                                IsViewBtnVisible = false,
                                is_loading = true,
                                is_file = true,
                                file_name = FileuploadModel.FileName,
                                is_image = false,
                                image_url = null,
                                file_url = null,
                                time_stamp = DependencyService.Get<IGetTimeStamp>().TimeStamp(),
                            };
                            IsBackPress = false;
                            try
                            {
                                AllSupportChatList.Add(item);
                            }
                            catch (Exception ex)
                            {
                            }
                            SupportChatList = AllSupportChatList;
                            var returnUrl = await FirebaseChatHelper.StoreImages(new MemoryStream(FileuploadModel.DataArray), Path.GetFileName(FileuploadModel.FileName));
                            var index = SupportChatList.IndexOf(SupportChatList.LastOrDefault());
                            if (!string.IsNullOrEmpty(returnUrl) && !string.IsNullOrWhiteSpace(returnUrl))
                            {
                                SupportChatList[index].is_loading = false;
                                SupportChatList[index].IsViewBtnVisible = true;
                                SupportChatList[index].file_url = returnUrl;
                            }
                            else
                            {
                                AllSupportChatList.Remove(item);
                                SupportChatList = AllSupportChatList;
                            }
                            if (SupportChatList.Count > COUNT)
                            {
                                if (string.IsNullOrEmpty(returnUrl) || string.IsNullOrWhiteSpace(returnUrl))
                                {
                                    await App.Current.MainPage.DisplayAlert("", AppResource.error_FileUploading, AppResource.Ok);
                                    IsBackPress = true;
                                }
                                else
                                {
                                    COUNT = SupportChatList.Count;
                                    AddMessageFirebase(SupportChatList[index]);
                                }
                            }
                        }
                        else
                        {
                            await App.Current.MainPage.DisplayAlert("", AppResource.error_invalidFileExtension, AppResource.Ok);
                        }
                    }
                });
            }
        }
        #endregion

        #region DownloadFileCommand
        //public Command DownloadFileCommand
        //{
        //    get
        //    {
        //        return new Command((e) =>
        //        {

        //        });
        //    }
        //}
        #endregion

        #region CloseCommand
        public ICommand CloseCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    IsPopupVisible = false;
                });
            }
        }
        #endregion

        #region OnAppearing
        public void OnAppearing()
        {
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        private void Connectivity_ConnectivityChanged(object sender, Xamarin.Essentials.ConnectivityChangedEventArgs e)
        {
            if((e.ConnectionProfiles.Contains(ConnectionProfile.WiFi) || e.ConnectionProfiles.Contains(ConnectionProfile.Cellular)) && e.NetworkAccess == NetworkAccess.Internet)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    MakeChatRead();
                });
                GetFirebaseChatList();
            }
        }
        #endregion

        #region OnDisappearing
        public void OnDisappearing()
        {
            Connectivity.ConnectivityChanged -= Connectivity_ConnectivityChanged;
            if (chatTimer != null)
            {
                chatTimer.Dispose();
            }
        }
        #endregion

        #region Navigation Aware Methods
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("RecieverId"))
            {
                try
                {
                    RecieverId = (int)parameters["RecieverId"];
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                RecieverId = 5;
            }
            if (parameters.ContainsKey("RecieverName"))
            {
                ChatDetailTitle = (string)parameters["RecieverName"];
            }
            else
            {
                ChatDetailTitle = AppResource.support_HeaderTitle;
            }

            //if (RecieverId != 5)
            //{
            //    ChatDetailTitle = name;
            //}
            //else
            //{
            //    ChatDetailTitle = AppResource.support_HeaderTitle;
            //}
            if (Common.CheckConnection())
            {
                Device.BeginInvokeOnMainThread(() =>
                    {
                        MakeChatRead();
                    });
                GetFirebaseChatList(); 
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
