using System;
using System.Collections.Generic;
using System.Text;

namespace Khadamat_CustomerApp.Models
{
    public class CountryDataModel
    {
        public int country_id { get; set; }
        public string display_country_name { get; set; }
        public string country_name { get; set; }
        public string arabic_country_name { get; set; }
        public string country_code { get; set; }
        public DateTime created_date { get; set; }
    }

    public class CountriesModel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public List<CountryDataModel> data { get; set; }
    }
    public class ProvienceDataModel
    {
        public int province_id { get; set; }
        public int country_id { get; set; }
        public string country_name { get; set; }
        public string display_province_name { get; set; }
        public string province_name { get; set; }
        public string arabic_province_name { get; set; }
        public DateTime created_date { get; set; }
    }

    public class ProvienceModel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public List<ProvienceDataModel> data { get; set; }
    }
}
