using System;
using System.Collections.Generic;
using System.Text;

namespace Khadamat_CustomerApp.Models
{
    public class LanguagesModel
    {
        public string LanguageFullName { get; set; }
        public string LanguageCultureName { get; set; }
    }
    public class ChangeLanguagesModel
    {
        public string language { get; set; }
        public long user_id { get; set; }
    }
    public class CommonResponseModel
    {
        public bool status { get; set; }
        public string message { get; set; }
    }
}
