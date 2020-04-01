using Khadamat_CustomerApp.DependencyInterface;
using Khadamat_CustomerApp.Droid.DependencyInterface;
using System;
using System.Threading;
using Xamarin.Forms;

[assembly: Dependency(typeof(Locale_Droid))]
namespace Khadamat_CustomerApp.Droid.DependencyInterface
{
    public class Locale_Droid : ILocale
    {
        public void SetLocale()
        {
            try
            {
                var androidLocale = Java.Util.Locale.Default; // user's preferred locale
                var netLocale = androidLocale.ToString().Replace("_", "-");
                var ci = new System.Globalization.CultureInfo(netLocale);
                Thread.CurrentThread.CurrentCulture = ci;
                Thread.CurrentThread.CurrentUICulture = ci;
            }
            catch (Exception)
            {


            }
        }

        public string SetLocale(string culturevalue)
        {
            try
            {
                var netLocale = culturevalue.ToString().Replace("_", "-");
                var ci = new System.Globalization.CultureInfo(netLocale);
                Thread.CurrentThread.CurrentCulture = ci;
                Thread.CurrentThread.CurrentUICulture = ci;
                return netLocale;
            }
            catch (Exception)
            {
                return null;

            }
        }

        /// <remarks>
        /// Not sure if we can cache this info rather than querying every time
        /// </remarks>
        public string GetCurrent()
        {
            var androidLocale = Java.Util.Locale.Default; // user's preferred locale

            // en, es, ja
            var netLanguage = androidLocale.Language.Replace("_", "-");
            // en-US, es-ES, ja-JP
            var netLocale = androidLocale.ToString().Replace("_", "-");
            try
            {
                if (Application.Current.Properties.ContainsKey("AppLocale") && !string.IsNullOrEmpty(Application.Current.Properties["AppLocale"].ToString()))
                {
                    var languageculture = Application.Current.Properties["AppLocale"].ToString();
                    netLanguage = languageculture;
                    netLocale = languageculture;
                }
                else
                {
                    netLanguage = "en-US";
                    netLocale = "en-US";
                }
            }
            catch (Exception)
            {
            }

            return netLocale;
        }
    }
}