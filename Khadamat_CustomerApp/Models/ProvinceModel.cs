using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Khadamat_CustomerApp.Models
{
    public class ProvinceModel
    {
        public long? province_id { get; set; }

        public long? country_id { get; set; }

        public string country_name { get; set; }

        public string province_name { get; set; }

        public DateTime? created_date { get; set; }
    }
}