using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Khadamat_CustomerApp.Models
{
    public class ContactUsModel
    {
        public string name { get; set; }
        public string email { get; set; }
        public string description { get; set; }
        public long user_id { get; set; }
    }
}