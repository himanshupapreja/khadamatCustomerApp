using System;
using System.Collections.Generic;
using System.Text;

namespace Khadamat_CustomerApp.Models
{
    public class SignUpResponseModel
    {
        public bool status { get; set; }
        public string message { get; set; }
        public UserModel userData { get; set; }
    }
}