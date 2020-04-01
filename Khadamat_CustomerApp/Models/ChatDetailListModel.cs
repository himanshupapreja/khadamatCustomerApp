using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace Khadamat_CustomerApp.Models
{
    public class ChatUserModel
    {
        public long? user_id { get; set; }
        public int? status { get; set; }
        public string name { get; set; }
        public bool? un_read { get; set; }
        public string picture_path { get; set; }
        public string created_date_str { get; set; }
        public string created_date_str_arabic { get; set; }
        public DateTime? created_date { get; set; }
        //[JsonIgnore]
        public string ChatUserMessage { get; set; }
        //[JsonIgnore]
        public string display_created_date { get; set; }
        public int user_type { get; set; }
        public string image_url { get; set; }
        public string file_url { get; set; }
        public string file_name { get; set; }
        public bool is_message { get; set; }
        public bool is_image { get; set; }
        public bool is_file { get; set; }



        public long? customer_id { get; set; }
        public long? worker_id { get; set; }
        public long? job_request_id { get; set; }
        public long? coordinator_id { get; set; }
        public string coordinator_name { get; set; }
        public string worker_name { get; set; }
        public string customer_name { get; set; }
    }
    public class ChatUserResponseModel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public List<ChatUserModel> data { get; set; }
    }

    public class GroupChatreadRequestModel
    {
        public long user_id { get; set; }
        public long job_request_id { get; set; }
    }

    public class SingleChatListModel : INotifyPropertyChanged
    {
        public long? sender_user_id { get; set; }
        public long? receiver_user_id { get; set; }

        public string user_message { get; set; }

        public string user_message_time { get; set; }
        public DateTime msg_datetime { get; set; }
        public string message_date_header { get; set; }

        public string image_url { get; set; }
        //private ImageSource _displayimage_url { get; set; }
        //public ImageSource displayimage_url
        //{
        //    get { return _displayimage_url; }
        //    set
        //    {
        //        _displayimage_url = value;
        //        RaisePropertyChanged(nameof(displayimage_url));
        //    }
        //}
        public string file_url { get; set; }
        public string file_name { get; set; }

        public bool is_image { get; set; }
        public bool is_file { get; set; }
        public bool is_sender { get; set; }
        public bool is_message { get; set; }
        private bool _IsViewBtnVisible { get; set; }
        public bool IsViewBtnVisible
        {
            get { return _IsViewBtnVisible; }
            set
            {
                _IsViewBtnVisible = value;
                RaisePropertyChanged(nameof(IsViewBtnVisible));
            }
        }
        private bool _is_loading { get; set; }
        public bool is_loading 
        {
            get { return _is_loading; }
            set
            {
                _is_loading = value;
                RaisePropertyChanged(nameof(is_loading));
            }
        }
        public bool is_header_visible { get; set; }

        public object time_stamp { get; set; }



        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }

    public class ChatDetailListModel : INotifyPropertyChanged
    {
        public long? sender_user_id { get; set; }
        public string sender_user_Name { get; set; }
        //public long? receiver_user_id { get; set; }
        public string receiver_user_Name { get; set; }

        public long? customer_id { get; set; }
        public string customer_Name { get; set; }

        public long? coordinator_id { get; set; }
        public string coordinator_Name { get; set; }
        
        public long? worker_id { get; set; }
        public string worker_Name { get; set; }
        
        public long job_id { get; set; }
        public string job_Name { get; set; }

        private string _image_url { get; set; }
        public string image_url
        {
            get { return _image_url; }
            set
            {
                _image_url = value;
                RaisePropertyChanged(nameof(image_url));
            }
        }
        public string file_url { get; set; }
        public string file_name { get; set; }


        public bool is_image { get; set; }
        public bool is_file { get; set; }

        public string user_message { get; set; }
        public string user_message_time { get; set; }

        public DateTime msg_datetime { get; set; }

        public bool is_sender { get; set; }
        public bool is_message { get; set; }
        private bool _IsViewBtnVisible { get; set; }
        public bool IsViewBtnVisible
        {
            get { return _IsViewBtnVisible; }
            set
            {
                _IsViewBtnVisible = value;
                RaisePropertyChanged(nameof(IsViewBtnVisible));
            }
        }
        private bool _is_loading { get; set; }
        public bool is_loading
        {
            get { return _is_loading; }
            set
            {
                _is_loading = value;
                RaisePropertyChanged(nameof(is_loading));
            }
        }

        public object time_stamp { get; set; }



        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }

    public class ChatDetailModelApi
    {
        public long from_user_id { get; set; }
        public long to_user_id { get; set; }
    }

    public class Chatdetailresponse
    {
        public bool status { get; set; }
        public string message { get; set; }
    }


    public class JobChatModel
    {
        public long? coordinator_id { get; set; }
        public string coordinator_Name { get; set; }
        public long? worker_id { get; set; }
        public string worker_Name { get; set; }
        public long? customer_id { get; set; }
        public string customer_Name { get; set; }
        public long job_id { get; set; }
        public string job_Name { get; set; }
        public int? job_Status { get; set; }
    }
}
