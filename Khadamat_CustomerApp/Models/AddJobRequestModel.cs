using System;
using System.Collections.Generic;
using System.Text;

namespace Khadamat_CustomerApp.Models
{
    public class AddJobRequestModel
    {
        public int user_id { get; set; }
        public long category_id { get; set; }
        public int sub_sub_category_id { get; set; }
        public DateTime? job_date_time { get; set; }
        public string job_date_time_str { get; set; }
        public string venue { get; set; }
        public string description { get; set; }
        public string location { get; set; }
        public double? latitude { get; set; }
        public double? longitude { get; set; }
    }
    public class AddJobRequestResponseModel
    {
        public bool status { get; set; }
        public string message { get; set; }
    }
}
