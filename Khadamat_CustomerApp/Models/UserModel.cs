using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Khadamat_CustomerApp.Models
{
    public class UserModel
    {
        [JsonIgnore]
        public int ID { get; set; }
        public string phone_number { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public string dob { get; set; }
        public string current_job { get; set; }
        public int? martial_status { get; set; }
        public long? country { get; set; }
        public string country_name { get; set; }
        public string province_name { get; set; }
        public long? province { get; set; }
        public string street { get; set; }
        public string description_location { get; set; }
        public int? user_type { get; set; }
        public long user_id { get; set; }
        public string id { get; set; }
        public string id_number { get; set; }
        public bool? is_active { get; set; }
        public bool? is_approved { get; set; }
        public DateTime? created_date { get; set; }
        public DateTime? modified_date { get; set; }
        public string profile_pic { get; set; }
        public int? device_id { get; set; }
        public string device_token { get; set; }
        public string email { get; set; }
        public bool? email_verified { get; set; }
        public double? latitude { get; set; }
        public double? longitude { get; set; }
    }
}