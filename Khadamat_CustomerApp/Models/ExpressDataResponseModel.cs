using System;
using System.Collections.Generic;
using System.Text;

namespace Khadamat_CustomerApp.Models
{
    public class ExpressDataResponseModel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public ExpressData1 ExpressData { get; set; }
    }



    public class ExpressSubCategory
    {
        public int sub_express_id { get; set; }
        public string sub_express_category_name { get; set; }
        public string sub_express_name { get; set; }
        public string sub_express_name_arabic { get; set; }
        public string title { get; set; }
        public string title_arabic { get; set; }
        public int express_id { get; set; }
        public string icon { get; set; }
        public DateTime created_date { get; set; }
        public bool is_active { get; set; }
    }

    public class ExpressData2
    {
        public int express_id { get; set; }
        public int sub_express_id { get; set; }
        public string title { get; set; }
        public string title_arabic { get; set; }
        public string icon { get; set; }
        public string picture { get; set; }
        public DateTime created_date { get; set; }
        public bool is_active { get; set; }
        public List<ExpressSubCategory> SubCategories { get; set; }
        public DateTime? modified_date { get; set; }
        public string service_category_name { get; set; }
        public bool IsEnglishView { get; set; }
    }

    public class ExpressData1
    {
        public string upper_banner_image { get; set; }
        public string lower_banner_image { get; set; }
        public string upper_banner_title { get; set; }
        public string upper_banner_title_arabic { get; set; }
        public List<ExpressData2> ExpressData { get; set; }
    }
}
