using System;
using System.Collections.Generic;
using System.Text;

namespace Khadamat_CustomerApp.Helpers
{
    public class ApiUrl
    {
        //public const string BaseUrl = "http://appmantechnologies.com:8020/Images/Users/";
        //public const string ApiBaseUrl = "http://appmantechnologies.com:8020/api/";
        public const string BaseUrl = "https://ai-khadamat.com/Images/Users/";
        public const string ApiBaseUrl = "https://ai-khadamat.com/api/";
        public const string PdfBaseUrl = "https://ai-khadamat.com/Images/InvoicePDF/";
        public const string ServiceImageBaseUrl = "https://ai-khadamat.com/Images/Category/";
        public const string SubServiceImageBaseUrl = "https://ai-khadamat.com/Images/SubCategory/";

        public const string SendOtp = "Account/SendOtp";
        public const string VerifyOtp = "Account/VerifyOtp";
        public const string ResendOtp = "Account/ResendOtp";
        public const string VerifyEmailOtp = "Account/VerifyEmailOtp";
        public const string CreatePassword = "Account/CreatePassword";
        public const string Login = "Account/Login";
        public const string ForgetPassword = "Account/ForgetPassword?phone_number={0}";
        public const string SignUp = "Account/SignUp";
        public const string UpdateDeviceInfo = "Account/UpdateDeviceInfo";
        public const string GetCountries = "Account/GetCountries";
        public const string GetProvinces = "Account/GetProvinces?country_id={0}";
        public const string Logout = "Account/Logout?user_id={0}";
        public const string GetProfile = "Account/GetProfile?user_id={0}";
        public const string EditProfile = "Account/EditProfile";

        public const string GetAboutus = "Settings/GetAboutus";
        public const string GetTermsConditions = "Settings/GetTermsConditions";
        public const string GetPrivacyPolicy = "Settings/GetPrivacyPolicy";
        public const string ChangePassword = "Settings/ChangePassword";
        public const string ChangeLanguage = "Settings/ChangeLanguage";
        public const string GetFaqData = "Settings/GetFaqData";
        public const string ContactUs = "Settings/ContactUs";
        public const string GetCategories = "Category/GetCategories?province_id={0}";
        public const string GetExpressList = "category/GetExpressList";
        public const string GetSubCategories = "Category/GetSubCategories?category_id={0}";

        public const string AddJobRequest = "Order/AddJobRequest";
        public const string GetCustomerJobRequests = "Order/GetCustomerJobRequests?user_id={0}";
        public const string GetChat = "Chat/GetChat?userId={0}&userType={1}";
        public const string AddGroupChatRequest = "Chat/AddGroupChatRequest?job_request_id={0}&type={1}";
        public const string GetNotifications = "Notification/GetNotifications?userId={0}";
        public const string AddChatRequest = "Chat/AddChatRequest";
        public const string DeleteChatRequest = "Chat/DeleteChatRequest";
        public const string MakeReadChat = "Chat/MakeReadChat";
        public const string MakeGroupReadChat = "Chat/MakeGroupReadChat";
        public const string DeleteGroupChatRequest = "Chat/DeleteGroupChatRequest";

        public const string AcceptRejectQuote = "Order/AcceptRejectQuote";
        public const string AcceptRejectRequestByWorker = "Order/AcceptRejectRequestByWorker";
        public const string GetWorkerJobRequests = "Order/GetWorkerJobRequests?user_id={0}";


        public const string SubmitUserReview = "order/SubmitUserReview";
        public const string CheckUserReview = "order/CheckUserReview?job_request_id={0}";

        public const string CancelJobRequest = "Order/CancelJobRequest";
        public const string GetAllReviews = "Order/GetAllReviews?user_id={0}";
        public const string BookJobRequest = "Order/BookJobRequest?job_request_id={0}&user_id={1}";

        public const string GetJobRequestDetail = "Order/GetJobRequestDetail?job_request_id={0}";


        public const string GetNotificationUnreadCount = "Account/GetNotificationUnreadCount?user_id={0}";
        public const string MakeNotificationRead = "Account/MakeNotificationRead?user_id={0}";
        public const string EditPhoneNumber = "Account/EditPhoneNumber";
    }
}