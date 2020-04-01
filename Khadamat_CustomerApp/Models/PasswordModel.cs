using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Khadamat_CustomerApp.Models
{
    public class ChangePasswordModel
    {
        public string old_password { get; set; }
        public string password { get; set; }
        public long user_id { get; set; }
    }
    public class ChangePasswordResponseModel
    {
        public bool status { get; set; }
        public string message { get; set; }
    }
    public class ForgotPasswordResponseModel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public ForgotPasswordOtpModel userData { get; set; }
    }
    public class ForgotPasswordOtpModel
    {
        public int? user_id { get; set; }
        public string phone_number { get; set; }
        public int? otp { get; set; }
        public int? user_type { get; set; }
        public int? otp_attempts { get; set; }
    }


    public class CreatePasswordRequestModel
    {
        public long user_id { get; set; }
        public string password { get; set; }
    }
    public class CreatePasswordResponseModel
    {
        public bool status { get; set; }
        public string message { get; set; }
    }
}