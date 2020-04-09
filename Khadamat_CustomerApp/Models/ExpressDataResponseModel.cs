using System;
using System.Collections.Generic;
using System.Text;

namespace Khadamat_CustomerApp.Models
{
    public class ExpressData
    {
        public int express_id { get; set; }
        public string title { get; set; }
        public DateTime created_date { get; set; }
        public bool? is_active { get; set; }
        public DateTime? modified_date { get; set; }
        public string icon { get; set; }
        public string picture { get; set; }
        public string service_category_name { get; set; }
        public bool IsEnglishView { get; set; }
        public string title_arabic { get; set; }
    }

    public class ExpressDataResponseModel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public List<ExpressData> ExpressData { get; set; }
    }
}
