using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
//using SQLite;
using Xamarin.Forms;

namespace Khadamat_CustomerApp.Models
{
    //public class FaqModel : INotifyPropertyChanged
    //{
    //    public int Id { get; set; }

    //    public string Question { get; set; }

    //    public string Answer { get; set; }

    //    public bool _isViewVisible;

    //    public event PropertyChangedEventHandler PropertyChanged;

    //    public bool IsViewVisible
    //    {
    //        get { return _isViewVisible; }
    //        set
    //        {
    //            _isViewVisible = value;

    //            OnPropertyChanged(); // Notify, that Image has been changed
    //        }
    //    }

    //    public string _icon;

    //    public string Icon
    //    {
    //        get { return _icon; }
    //        set
    //        {
    //            _icon = value;

    //            OnPropertyChanged(); // Notify, that Image has been changed
    //        }
    //    }

    //    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    //    {
    //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    //    }
    //}

    //public class FaqDataResponse
    //{
    //    public FaqData data { get; set; }
    //    public string mesg { get; set; }
    //}

    //public class FaqData
    //{
    //    public string server_time { get; set; }
    //    public List<faq> faq { get; set; }
    //    public int totalpages { get; set; }
    //    public int limit { get; set; }
    //    public int totalRecord { get; set; }
    //}

    //public class Faq
    //{
    //    public string id { get; set; }
    //    public string title { get; set; }
    //    public string content { get; set; }
    //    public string status { get; set; }
    //    public string created { get; set; }
    //    public string modified { get; set; }
    //}

    //public class faq
    //{
    //    public Faq Faq { get; set; }
    //}

    //public class LocalFaqData
    //{
    //    //[PrimaryKey, AutoIncrement]
    //    public int ID { get; set; }

    //    public int FaqID { get; set; }

    //    public string Question { get; set; }

    //    public string Answer { get; set; }

    //    public bool IsViewVisible { get; set; }

    //    public string Icon { get; set; }
    //}






    public class FaqData: INotifyPropertyChanged
    {
        public int faq_id { get; set; }
        public DateTime? modified_date { get; set; }
        public DateTime created_date { get; set; }
        public string question { get; set; }
        public string question_arabic { get; set; }
        public string answer { get; set; }
        public string answer_arabic { get; set; }

        public bool _isViewVisible;
        public bool IsViewVisible
        {
            get { return _isViewVisible; }
            set
            {
                _isViewVisible = value;

                OnPropertyChanged(); // Notify, that Image has been changed
            }
        }

        public string _icon;

        public string Icon
        {
            get { return _icon; }
            set
            {
                _icon = value;

                OnPropertyChanged(); // Notify, that Image has been changed
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public bool is_active { get; set; }
    }

    public class FaqDataResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
        public List<FaqData> faqData { get; set; }
    }
}
