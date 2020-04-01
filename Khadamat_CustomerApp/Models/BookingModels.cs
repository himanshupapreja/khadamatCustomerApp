using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Khadamat_CustomerApp.Models
{
    public class JobRequestData : INotifyPropertyChanged
    {
        public int id { get; set; }
        public int category_id { get; set; }
        public string display_category_name { get; set; }
        public string category_name { get; set; }
        public string category_name_arabic { get; set; }
        public DateTime? job_date_time { get; set; }
        public string venue { get; set; }
        public string description { get; set; }
        public int status { get; set; }
        public DateTime? created_date { get; set; }
        public int customer_id { get; set; }
        public string customer_image { get; set; }
        public string display_sub_category_name { get; set; }
        public string sub_category_name { get; set; }
        public string sub_category_name_arabic { get; set; }
        public string customer_name { get; set; }
        public string coordinator_name { get; set; }
        public int? coordinator_id { get; set; }
        public string coordinator_image { get; set; }
        public string service_provider_name { get; set; }
        public string quote_price { get; set; }
        public string quote_description { get; set; }
        public string job_status_str { get; set; }
        public string cancel_reason { get; set; }
        public string coordinator_name_one { get; set; }
        public int? coordinator_id_one { get; set; }
        public string coordinator_image_one { get; set; }
        public int? service_provider_id { get; set; }
        public string service_provider_image { get; set; }
        public bool? is_start { get; set; }
        public bool? is_completed { get; set; }
        public DateTime? job_completed_date_time { get; set; }
        public DateTime? job_start_date_time { get; set; }
        public bool? is_quote_approved { get; set; }
        public int? user_review { get; set; }
        public object user_review_comment { get; set; }
        public double? user_rating { get; set; }
        public string province_name { get; set; }
        public string customer_address { get; set; }
        public string location { get; set; }
        public double? latitude { get; set; }
        public double? longitude { get; set; }
        public string phone_number { get; set; }
        public string currency { get; set; }




        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //[JsonIgnore]
        private bool _IsJobCancel;
        public bool IsJobCancel
        {
            get { return _IsJobCancel; }
            set 
            { 
                _IsJobCancel = value;
                OnPropertyChanged();
            }
        }
        //[JsonIgnore]
        private bool _IsQuotePrice;
        public bool IsQuotePrice
        {
            get { return _IsQuotePrice; }
            set
            {
                _IsQuotePrice = value;
                OnPropertyChanged();
            }
        }
        //[JsonIgnore]
        private bool _IsQuoteDescription;
        public bool IsQuoteDescription
        {
            get { return _IsQuoteDescription; }
            set
            {
                _IsQuoteDescription = value;
                OnPropertyChanged();
            }
        }
        //[JsonIgnore]
        private string _JobQuote;
        public string JobQuote
        {
            get { return _JobQuote; }
            set
            {
                _JobQuote = value;
                OnPropertyChanged();
            }
        }
        //[JsonIgnore]
        private string _WorkerImage;
        public string WorkerImage {
            get { return _WorkerImage; }
            set
            {
                _WorkerImage = value;
                OnPropertyChanged();
            }
        }
        //[JsonIgnore]
        private string _WorkerName;
        public string WorkerName {
            get { return _WorkerName; }
            set
            {
                _WorkerName = value;
                OnPropertyChanged();
            }
        }
        //[JsonIgnore]
        private string _WorkerReviewText;
        public string WorkerReviewText {
            get { return _WorkerReviewText; }
            set
            {
                _WorkerReviewText = value;
                OnPropertyChanged();
            }
        }
        //[JsonIgnore]
        private string _WorkerServiceName;
        public string WorkerServiceName {
            get { return _WorkerServiceName; }
            set
            {
                _WorkerServiceName = value;
                OnPropertyChanged();
            }
        }
        //[JsonIgnore]
        private string _JobDateTimeValue;
        public string JobDateTimeValue {
            get { return _JobDateTimeValue; }
            set
            {
                _JobDateTimeValue = value;
                OnPropertyChanged();
            }
        }
        //[JsonIgnore]
        private string _job_date;
        public string job_date {
            get { return _job_date; }
            set
            {
                _job_date = value;
                OnPropertyChanged();
            }
        }
        //[JsonIgnore]
        private string _job_time;
        public string job_time {
            get { return _job_time; }
            set
            {
                _job_time = value;
                OnPropertyChanged();
            }
        }
        //[JsonIgnore]
        private bool _IsNoJobStatusPending;
        public bool IsNoJobStatusPending {
            get { return _IsNoJobStatusPending; }
            set
            {
                _IsNoJobStatusPending = value;
                OnPropertyChanged();
            }
        }
        //[JsonIgnore]
        private bool _IsJobCompleted;
        public bool IsJobCompleted {
            get { return _IsJobCompleted; }
            set
            {
                _IsJobCompleted = value;
                OnPropertyChanged();
            }
        }
        //[JsonIgnore]
        private bool _IsJobPending;
        public bool IsJobPending {
            get { return _IsJobPending; }
            set
            {
                _IsJobPending = value;
                OnPropertyChanged();
            }
        }
        //[JsonIgnore]
        private bool _IsJobCancelled;
        public bool IsJobCancelled {
            get { return _IsJobCancelled; }
            set
            {
                _IsJobCancelled = value;
                OnPropertyChanged();
            }
        }
        //[JsonIgnore]
        private bool _IsChatVisible;
        public bool IsChatVisible {
            get { return _IsChatVisible; }
            set
            {
                _IsChatVisible = value;
                OnPropertyChanged();
            }
        }
        //[JsonIgnore]
        private bool _IsRatingVisible;
        public bool IsRatingVisible {
            get { return _IsRatingVisible; }
            set
            {
                _IsRatingVisible = value;
                OnPropertyChanged();
            }
        }
        //[JsonIgnore]
        private bool _IsLocationAvailable;
        public bool IsLocationAvailable {
            get { return _IsLocationAvailable; }
            set
            {
                _IsLocationAvailable = value;
                OnPropertyChanged();
            }
        }
    }

    public class BookingResponseModel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public List<JobRequestData> jobRequestData { get; set; }
    }

    public class JobDetailResponseModel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public JobRequestData jobRequestData { get; set; }
    }

    public class JobRequestBookCancelResponseModel
    {
        public bool status { get; set; }
        public string message { get; set; }
    }

    public class JobRequestCancelData
    {
        public long user_id { get; set; }
        public string cancel_reason { get; set; }
        public int job_request_id { get; set; }
    }
}
