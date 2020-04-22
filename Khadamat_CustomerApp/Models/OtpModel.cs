    using System;
using System.Collections.Generic;
using System.Text;

namespace Khadamat_CustomerApp.Models
{
    public class VerifyOtpRequestModel
    {
        public long user_id { get; set; }
        public int otp { get; set; }
        public string phone_number { get; set; }
    }
    public class VerifyOtpResponseModel
    {
        public bool status { get; set; }
        public string message { get; set; }
    }
    public class SendOtpRequestModel
    {
        public string phone_number { get; set; }
        public int user_type { get; set; }
    }
    public class SendOtpModel
    {
        public int? user_id { get; set; }
        public string phone_number { get; set; }
        public int? otp { get; set; }
        public int? user_type { get; set; }
        public int? otp_attempts { get; set; }

    }
    public class SendOtpResponseModel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public SendOtpModel otpData { get; set; }
        public DateTime? lastUpdatedDate { get; set; }
        public string phone_number_one { get; set; }
        public string phone_number_two { get; set; }
        public string phone_number_three { get; set; }
    }



    //public class VerifyEmailOtpModel
    //{
    //    public long user_id { get; set; }
    //    public string email { get; set; }
    //    public string phone_number { get; set; }
    //    public int? type { get; set; }
    //}
    public class VerifyResendOtpModel
    {
        public int? otp { get; set; }
        public long user_id { get; set; }
        public string phone_number { get; set; }
        public int page_type { get; set; }
    }
    public class VerifyResendOtpResponseModel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public int? otpCode { get; set; }
        public DateTime? lastUpdatedDate { get; set; }
        public string phone_number_one { get; set; }
        public string phone_number_two { get; set; }
        public string phone_number_three { get; set; }
    }




    public class OtpCode
    {
        public int user_id { get; set; }
        public string phone_number { get; set; }
        public int otp { get; set; }
        public int user_type { get; set; }
        public int otp_attempts { get; set; }
        public object page_type { get; set; }
        public object calling_numbers { get; set; }
    }

    public class EditPhoneResponseModel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public OtpCode otpCode { get; set; }
        public DateTime lastUpdatedDate { get; set; }
        public string phone_number_one { get; set; }
        public string phone_number_two { get; set; }
        public string phone_number_three { get; set; }
    }
}
