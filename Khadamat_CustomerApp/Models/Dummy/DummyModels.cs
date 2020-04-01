using System;
using System.Collections.Generic;
using System.Text;

namespace Khadamat_CustomerApp.Models
{
    public class DummyServiceModel
    {
        public string Image { get; set; }
        public string Icon { get; set; }
        public string Name { get; set; }
    }

    public class DummyNotificationModel
    {
        public string UserPic { get; set; }
        public string NotificationText { get; set; }
    }

    public class DummyServiceDetailList
    {
        public string Icon { get; set; }
        public string Name { get; set; }
    }

    public class DummyChatListModel
    {
        public string UserPic { get; set; }
        public string ChatUserName { get; set; }
        public string ChatMsgTime { get; set; }
        public string ChatUserMessage { get; set; }
    }

    public class DummyChatDetailModel
    {
        public string UserMessage { get; set; }
        public string UserMessageTime { get; set; }
        public bool IsSender { get; set; }
    }

    public class DummyMyBookingModel
    {
        public string ServiceName { get; set; }
        public bool JobCancelled { get; set; }
        public bool JobCompleted { get; set; }
    }
}
