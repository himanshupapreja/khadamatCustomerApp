using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Khadamat_CustomerApp.Helpers
{
    public class Enums
    {
        public enum UserTypeEnum
        {
            Admin = 1,
            Customer = 2,
            Worker = 3,
            [Description("Sub Admin")]
            SubAdmin = 4,
            [Description("Finance Officer")]
            FinanceOfficer = 5,
            Coordinator = 6
        }

        public enum MartialStatusEnum
        {
            [Description("Single")]
            Single = 1,
            [Description("In A Relationship")]
            InARelationship = 2,
            [Description("Engaged")]
            Engaged = 3,
            [Description("Married")]
            Married = 4,
            [Description("Its Complicated")]
            ItsComplicated = 5,
            [Description("In An Open Relationship")]
            InAnOpenRelationship = 6,
            [Description("Widowed")]
            Widowed = 7,
            [Description("Divorced")]
            Divorced = 8,
        }

        public enum MartialStatusArabicEnum
        {
            [Description("أعزب")]
            Single = 1,
            [Description("مرتبط")]
            InARelationship = 2,
            [Description("مخطوب")]
            Engaged = 3,
            [Description("متزوج")]
            Married = 4,
            [Description("علاقة معقدة")]
            ItsComplicated = 5,
            [Description("في علاقة مفتوحة")]
            InAnOpenRelationship = 6,
            [Description("أرمل")]
            Widowed = 7,
            [Description("مطلقة")]
            Divorced = 8,
        }

        public enum JobRequestEnum
        {
            Pending = 1,
            Accepted = 2,
            [Description("In-Progress")]
            InProgress = 3,
            Completed = 4,
            Canceled = 5,
            [Description("Quote Canceled")]
            QuoteCanceled = 6,
            Assigned = 7,
            Closed = 8,
        }

        public enum NotificationStatus
        {
            Pending = 1,
            Accepted = 2,
            Rejected = 3,
            SentQuotation = 4,
            AcceptedQuotation = 5,
            RejectedQuotation = 6,
            TimerStarted = 7,
            Completed = 8,
            Assigned = 9,
            Closed = 10,
        }

        public enum AppStatus
        {
            Foreground = 1,
            Background = 2,
            Killed = 3,
        }

        public enum OTPPageEnum
        {
            NewUser = 1,
            ForgotPswd = 2,
            Profile = 3
        }

        public enum CurrencyStatus
        {
            Yer = 1,
            USD = 2,
        }
    }
}