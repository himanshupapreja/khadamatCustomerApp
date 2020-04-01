using System;
using System.Collections.Generic;
using System.Text;

namespace Khadamat_CustomerApp.Models
{
    public class UserProfileModel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public UserModel data { get; set; }
    }
    public class EditUserProfileModel
    {
        public bool status { get; set; }
        public string message { get; set; }
    }
}
