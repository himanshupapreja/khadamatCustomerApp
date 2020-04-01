using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace Khadamat_CustomerApp.Models
{
    public class ImagesModel
    {
        public ImageSource Image { get; set; }

        public byte[] ImageBytes { get; set; }

        public Stream ImageStream { get; set; }

        public string ImagePath { get; set; }
    }
}
