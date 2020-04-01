using Khadamat_CustomerApp.DependencyInterface;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Text;
using Xamarin.Forms;

namespace Khadamat_CustomerApp
{
    public class L10n
    {
        public static void SetLocale()
        {
            DependencyService.Get<ILocale>().SetLocale();
        }
        public static string SetLocale(string culturevalue)
        {
            return DependencyService.Get<ILocale>().SetLocale(culturevalue);
        }

        /// <remarks>
        /// Maybe we can cache this info rather than querying every time
        /// </remarks>
        public static string Locale()
        {
            return DependencyService.Get<ILocale>().GetCurrent();
        }

        public static string Localize(string key, string comment)
        {

            var netLanguage = Locale();
            // Platform-specific
            ResourceManager temp = new ResourceManager("Khadamat_CustomerApp.Resources.AppResource", typeof(L10n).GetTypeInfo().Assembly);

            string result = temp.GetString(key, new CultureInfo(netLanguage));

            return result;
        }
    }
}
