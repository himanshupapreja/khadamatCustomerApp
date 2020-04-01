using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms.Maps;

namespace Khadamat_CustomerApp.CustomControls
{
    public class CustomMap : Map
    {
        public List<CustomPin> CustomPins { get; set; }
    }
    public class CustomPin : Pin
    {
        public string Url { get; set; }
    }
}
