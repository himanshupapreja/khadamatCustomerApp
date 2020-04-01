using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Khadamat_CustomerApp.Models
{
    public class LoginRequestModel
    {
        public string phone_number { get; set; }
        public string password { get; set; }
        public int user_type { get; set; }
    }
    public class LoginResponseModel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public UserModel userData { get; set; }
    }
}