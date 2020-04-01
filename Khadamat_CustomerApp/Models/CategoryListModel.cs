using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace Khadamat_CustomerApp.Models
{
    public class SubSubCategory
    {
        public int sub_sub_category_id { get; set; }
        public int sub_category_id { get; set; }
        public int category_id { get; set; }
        public string sub_sub_category_name { get; set; }
        public string sub_sub_category_name_arabic { get; set; }
        public string sub_category_name { get; set; }
        public string category_name { get; set; }
        public string category_name_arabic { get; set; }
        public string icon { get; set; }
        public DateTime created_date { get; set; }
        public bool is_active { get; set; }
    }

    public class SubCategory : INotifyPropertyChanged
    {
        public int sub_category_id { get; set; }
        public int category_id { get; set; }
        public string sub_category_name { get; set; }
        public string sub_category_name_arabic { get; set; }
        public string category_name { get; set; }
        public DateTime created_date { get; set; }
        public bool is_active { get; set; }
        public List<SubSubCategory> SubSubCategories { get; set; }




        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public double _SubServiceListHeight;
        //[JsonIgnore]
        public double SubServiceListHeight
        {
            get { return _SubServiceListHeight; }
            set
            {
                _SubServiceListHeight = value;
                OnPropertyChanged();
            }
        }
    }
    public class SubCategoryResponseModel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public List<SubCategory> SubCategoryData { get; set; }
    }

    public class CategoryListModel
    {
        public long? main_category_id { get; set; }
        public long service_category_id { get; set; }
        public string service_category_name { get; set; }
        public string service_category_name_arabic { get; set; }
        public string icon { get; set; }
        public string picture { get; set; }
        public System.DateTime? created_date { get; set; }
        public bool is_active { get; set; }
        public string terms_conditions { get; set; }
        public string terms_conditions_arabic { get; set; }
        public List<SubCategory> SubCategories { get; set; }
        public string Headername { get; set; }
    }
    public class MainCategory : INotifyPropertyChanged
    {
        public int main_category_id { get; set; }
        public string main_category_name { get; set; }
        public string main_category_name_arabic { get; set; }
        public DateTime created_date { get; set; }
        public bool is_active { get; set; }
        public List<CategoryListModel> Categories { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public double _ServiceListHeight;
        //[JsonIgnore]
        public double ServiceListHeight
        {
            get { return _ServiceListHeight; }
            set
            {
                _ServiceListHeight = value;
                OnPropertyChanged();
            }
        }
        //[JsonIgnore]
        public string LowerBanner { get; set; }
        //[JsonIgnore]
        public bool IsImageFound { get; set; }
    }

    public class CategoryListResponseModel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public CategoryData CategoryData { get; set; }
    }

    public class CategoryData
    {
        public string upper_banner_image { get; set; }
        public string lower_banner_image { get; set; }
        public string upper_banner_title { get; set; }
        public string upper_banner_title_arabic { get; set; }
        public List<MainCategory> AppData { get; set; }
    }
}