using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Khadamat_CustomerApp.Models;
using Khadamat_CustomerApp.Resources;
using System.IO;
using Firebase.Storage;

namespace Khadamat_CustomerApp.Helpers
{
    public static class FirebaseChatHelper
    {
        static FirebaseClient firebase = new FirebaseClient("https://khadamat-d4692.firebaseio.com/");

        #region Group Chat
        public static async Task<List<ChatDetailListModel>> GetChatForGroup(string firebasechatname, long senderUserId, long jobid)
        {
            return (await firebase
              .Child(firebasechatname).Child(jobid.ToString()).Child(senderUserId.ToString())
              .OnceAsync<ChatDetailListModel>()).Select(item => new ChatDetailListModel
              {
                  is_sender = item.Object.is_sender,
                  job_id = item.Object.job_id,
                  sender_user_id = item.Object.sender_user_id,
                  user_message = item.Object.user_message,
                  user_message_time = item.Object.user_message_time,
                  customer_id = item.Object.customer_id,
                  coordinator_id = item.Object.coordinator_id,
                  coordinator_Name = item.Object.coordinator_Name,
                  customer_Name = item.Object.customer_Name,
                  file_url = item.Object.file_url,
                  image_url = item.Object.image_url,
                  is_message = item.Object.is_message,
                  job_Name = item.Object.job_Name,
                  receiver_user_Name = item.Object.receiver_user_Name == item.Object.coordinator_Name && !string.IsNullOrEmpty(item.Object.coordinator_Name) ? AppResource.coordinator_Name : item.Object.receiver_user_Name,
                  sender_user_Name = item.Object.sender_user_Name,
                  worker_id = item.Object.worker_id,
                  worker_Name = item.Object.worker_Name,
                  is_loading = item.Object.is_loading,
                  IsViewBtnVisible = item.Object.IsViewBtnVisible,
                  msg_datetime = item.Object.msg_datetime,
                  file_name = item.Object.file_name,
                  is_file = item.Object.is_file,
                  is_image = item.Object.is_image,
                  time_stamp = item.Object.time_stamp
              }).OrderBy(x => x.time_stamp).ToList();
        }

        public static async Task<bool> AddChatMessageForGroup(ChatDetailListModel chatModel, string firebasechatname)
        {
            try
            {
                //For Customer
                await firebase
                      .Child(firebasechatname).Child(chatModel.job_id.ToString()).Child(chatModel.sender_user_id.ToString()).PostAsync(chatModel);
                
                //For Coordinator
                var chatModel_Coordinator = new ChatDetailListModel()
                {
                    is_sender = !chatModel.is_sender,
                    job_id = chatModel.job_id,
                    sender_user_id = chatModel.coordinator_id,
                    coordinator_id = null,
                    customer_id = chatModel.sender_user_id,
                    worker_id = chatModel.worker_id,
                    job_Name = chatModel.job_Name,
                    sender_user_Name = chatModel.coordinator_Name,
                    coordinator_Name = null,
                    file_url = chatModel.file_url,
                    image_url = chatModel.image_url,
                    is_loading = false,
                    is_message = chatModel.is_message,
                    msg_datetime = chatModel.msg_datetime,
                    receiver_user_Name = chatModel.sender_user_Name,
                    customer_Name = chatModel.sender_user_Name,
                    worker_Name = chatModel.worker_Name,
                    user_message = chatModel.user_message,
                    user_message_time = chatModel.user_message_time,
                    file_name = chatModel.file_name,
                    is_image = chatModel.is_image,
                    is_file = chatModel.is_file,
                    time_stamp = chatModel.time_stamp
                };

                await firebase
                      .Child(firebasechatname).Child(chatModel_Coordinator.job_id.ToString()).Child(chatModel_Coordinator.sender_user_id.ToString()).PostAsync(chatModel_Coordinator);

                // For worker
                if (chatModel.worker_id.HasValue && chatModel.worker_id.Value > 0)
                {
                    var chatModel_Worker = new ChatDetailListModel()
                    {
                        is_sender = !chatModel.is_sender,
                        job_id = chatModel.job_id,
                        sender_user_id = chatModel.worker_id,
                        coordinator_id = chatModel.coordinator_id,
                        customer_id = chatModel.sender_user_id,
                        worker_id = null,
                        sender_user_Name = chatModel.worker_Name,
                        coordinator_Name = chatModel.coordinator_Name,
                        customer_Name = chatModel.sender_user_Name,
                        job_Name = chatModel.job_Name,
                        worker_Name = null,
                        file_url = chatModel.file_url,
                        image_url = chatModel.image_url,
                        is_loading = false,
                        is_message = chatModel.is_message,
                        msg_datetime = chatModel.msg_datetime,
                        receiver_user_Name = chatModel.sender_user_Name,
                        user_message = chatModel.user_message,
                        user_message_time = chatModel.user_message_time,
                        file_name = chatModel.file_name,
                        is_image = chatModel.is_image,
                        is_file = chatModel.is_file,
                        time_stamp = chatModel.time_stamp
                    };
                    await firebase
                          .Child(firebasechatname).Child(chatModel_Worker.job_id.ToString()).Child(chatModel_Worker.sender_user_id.ToString()).PostAsync(chatModel_Worker);
                }


                // Common(admin/finance officer viewing)
                var chatModel_Common = new ChatDetailListModel()
                {
                    is_sender = !chatModel.is_sender,
                    job_id = chatModel.job_id,
                    sender_user_id = 0,
                    coordinator_id = chatModel.coordinator_id,
                    customer_id = chatModel.sender_user_id,
                    worker_id = chatModel.worker_id,
                    job_Name = chatModel.job_Name,
                    sender_user_Name = chatModel.sender_user_Name,
                    coordinator_Name = chatModel.coordinator_Name,
                    file_url = chatModel.file_url,
                    image_url = chatModel.image_url,
                    is_loading = false,
                    is_message = chatModel.is_message,
                    msg_datetime = chatModel.msg_datetime,
                    receiver_user_Name = null,
                    customer_Name = chatModel.sender_user_Name,
                    worker_Name = chatModel.worker_Name,
                    user_message = chatModel.user_message,
                    user_message_time = chatModel.user_message_time,
                    file_name = chatModel.file_name,
                    is_image = chatModel.is_image,
                    is_file = chatModel.is_file,
                    time_stamp = chatModel.time_stamp
                };

                await firebase
                      .Child(firebasechatname).Child(chatModel_Common.job_id.ToString()).Child(chatModel_Common.sender_user_id.ToString()).PostAsync(chatModel_Common);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("AddingChatToFirebase_Exception:- " + ex.Message);
                return false;
            }
        }
        #endregion

        #region Single Chat
        public static async Task<List<SingleChatListModel>> GetChatForUserID(long senderUserId, long recieverUserId)
        {
            return (await firebase
              .Child("Chat").Child(senderUserId.ToString()).Child(recieverUserId.ToString())
              .OnceAsync<SingleChatListModel>()).Select(item => new SingleChatListModel
              {
                  is_sender = item.Object.is_sender,
                  receiver_user_id = item.Object.receiver_user_id,
                  sender_user_id = item.Object.sender_user_id,
                  user_message = item.Object.user_message,
                  user_message_time = item.Object.user_message_time,
                  time_stamp = item.Object.time_stamp,
                  file_url = item.Object.file_url,
                  image_url = item.Object.image_url,
                  is_header_visible= item.Object.is_header_visible,
                  is_loading = item.Object.is_loading,
                  is_message = item.Object.is_message,
                  message_date_header = item.Object.message_date_header,
                  msg_datetime = item.Object.msg_datetime,
                  is_file = item.Object.is_file,
                  IsViewBtnVisible = item.Object.IsViewBtnVisible,
                  file_name = item.Object.file_name,
                  is_image = item.Object.is_image,
                  
                  //message_date_header = item.Object.msg_datetime.Date == DateTime.Now.Date ? AppResource.msg_Today : item.Object.msg_datetime.Date == DateTime.Now.Date.AddDays(-1) ? AppResource.msg_Yesterday : item.Object.msg_datetime.ToString("dd/MMM/yyyy"),
                  //is_header_visible = true
              }).OrderBy(x => x.time_stamp).ToList();
        }

        public static async Task<bool> AddChatMessage(SingleChatListModel chatModel)
        {
            try
            {
                await firebase
                      .Child("Chat").Child(chatModel.sender_user_id.ToString()).Child(chatModel.receiver_user_id.ToString()).PostAsync(chatModel);
                var chatModel1 = new SingleChatListModel()
                {
                    is_sender = !chatModel.is_sender,
                    receiver_user_id = chatModel.sender_user_id,
                    sender_user_id = chatModel.receiver_user_id,
                    user_message = chatModel.user_message,
                    user_message_time = chatModel.user_message_time,
                    msg_datetime = chatModel.msg_datetime,
                    file_url = chatModel.file_url,
                    image_url= chatModel.image_url,
                    is_loading = chatModel.is_loading,
                    is_message= chatModel.is_message,
                    message_date_header= chatModel.message_date_header,
                    is_header_visible = chatModel.is_header_visible,
                    file_name = chatModel.file_name,
                    is_image = chatModel.is_image,
                    is_file = chatModel.is_file,
                    time_stamp = chatModel.time_stamp
                };
                await firebase
                      .Child("Chat").Child(chatModel1.sender_user_id.ToString()).Child(chatModel1.receiver_user_id.ToString()).PostAsync(chatModel1);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("AddingChatToFirebase_Exception:- " + ex.Message);
                return false;
            }
        }
        #endregion

        #region DeleteChat
        public static async Task DeleteGroupChat(string firebasechatname, long senderUserId, long jobid)
        {
            await firebase.Child(firebasechatname).Child(jobid.ToString()).Child(senderUserId.ToString()).DeleteAsync();
        }
        public static async Task DeleteSingleChat(string firebasechatname, long senderUserId, long recieverUserId)
        {
            await firebase.Child(firebasechatname).Child(senderUserId.ToString()).Child(recieverUserId.ToString()).DeleteAsync();
        }
        #endregion

        #region StoreImages
        public static async Task<string> StoreImages(Stream imageStream, string imageName)
        {
            string imgurl = string.Empty;
            try
            {
                var stroageImage = new FirebaseStorage("khadamat-d4692.appspot.com");
                imgurl = await stroageImage.Child("ChatImages").Child(imageName).PutAsync(imageStream); ;
            }
            catch (Exception ex)
            {

            }
            return imgurl;
        }
        #endregion
    }
}
