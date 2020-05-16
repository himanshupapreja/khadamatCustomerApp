using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Khadamat_CustomerApp.Models
{
    public class NotificationsModel : INotifyPropertyChanged
    {
        public int id { get; set; }
        public int? job_request_id { get; set; }
        public string text { get; set; }
        public string arabic_text { get; set; }
        public int from_user_id { get; set; }
        public string from_user_name { get; set; }
        public int to_user_id { get; set; }
        public string to_user_name { get; set; }
        public int notification_status { get; set; }
        public DateTime created_date { get; set; }
        public string from_user_image { get; set; }
        public string to_user_image { get; set; }
        public string pdf_file { get; set; }
        public string created_date_str { get; set; }
        public string created_date_str_arabic { get; set; }
        public string display_created_date { get; set; }
        public object to_user_id_array { get; set; }
        public object provinces_id_array { get; set; }
        public int user_type { get; set; }
        public object JobDetails { get; set; }







        //[JsonIgnore]
        public string UserPic { get; set; }
        //[JsonIgnore]
        public string display_text { get; set; }
        //[JsonIgnore]
        private string _ViewNotificationBtn;
        //[JsonIgnore]
        public string ViewNotificationBtn 
        {
            get { return _ViewNotificationBtn; }
            set
            {
                _ViewNotificationBtn = value;
                OnPropertyChanged();
            }
        }
        //[JsonIgnore]
        public bool IsQuoteSent { get; set; }
        //[JsonIgnore]
        public bool IsViewDetail { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class NotificationsResponseModel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public List<NotificationsModel> data { get; set; }
        public int total_count { get; set; }
    }

    public class AcceptRejectQuoteModel
    {
        public long user_id { get; set; }
        public long job_request_id { get; set; }
        public long notification_id { get; set; }
        public bool is_accept { get; set; }
    }
    public class AcceptRejectQuoteResponseModel
    {
        public bool status { get; set; }
        public string message { get; set; }
    }

    public class NotificationCountResponseModel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public int? notificationUnreadCount { get; set; }
    }
}
