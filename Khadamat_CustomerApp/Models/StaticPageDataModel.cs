using System;
using System.Collections.Generic;
using System.Text;

namespace Khadamat_CustomerApp.Models
{
    public class AboutUsResponseModel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public AboutUsData AboutUsData { get; set; }
    }

    public class AboutUsData
    {
        public int about_us_id { get; set; }
        public string text { get; set; }
        public string text_arabic { get; set; }
        public DateTime? created_date { get; set; }
    }

    public class TermConditionResponseModel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public TermsConditionsData TermsConditionsData { get; set; }
    }

    public class TermsConditionsData
    {
        public int terms_id { get; set; }
        public string text { get; set; }
        public string text_arabic { get; set; }
        public DateTime? created_date { get; set; }
    }

    public class PrivacyPolicyResponseModel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public PrivacyPolicyData PrivacyPolicyData { get; set; }
    }

    public class PrivacyPolicyData
    {
        public int? privacy_policy_id { get; set; }
        public string text { get; set; }
        public string text_arabic { get; set; }
        public DateTime? created_date { get; set; }
    }

    public class ContactUsRequestModel
    {
        public string name { get; set; }
        public string email { get; set; }
        public string description { get; set; }
        public long user_id { get; set; }
    }

    public class ContactUsResponseModel
    {
        public bool status { get; set; }
        public string message { get; set; }
    }
}
