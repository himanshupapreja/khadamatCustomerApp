using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Khadamat_CustomerApp.CustomControls.Converters
{
    public class BoolToNotBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(bool))
                throw new InvalidOperationException("The target must be a boolean");

            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            if (targetType != typeof(bool))
                throw new InvalidOperationException("The target must be a boolean");

            return (bool)value;
        }
    }
}
