using System;
using System.Collections.Generic;
using System.Text;

namespace Khadamat_CustomerApp.Models
{
    public class UpdateDeviceInfoModel
    {
        public long user_id { get; set; }
        public int device_id { get; set; }
        public string device_token { get; set; }
    }
    public class UpdateDeviceInfoResponse
    {
        public bool status { get; set; }
        public bool approveStatus { get; set; }
        public string message { get; set; }
    }
}
