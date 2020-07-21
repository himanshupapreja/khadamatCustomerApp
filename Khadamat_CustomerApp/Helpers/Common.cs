using Khadamat_CustomerApp.Resources;
using Plugin.Connectivity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace Khadamat_CustomerApp.Helpers
{
    public class Common
    {
        /// <summary>
        /// Checking the internet connection.
        /// </summary>
        /// <returns></returns>
        public static bool CheckConnection()
        {
            //return (Connectivity.ConnectionProfiles.Contains(ConnectionProfile.WiFi) && Connectivity.NetworkAccess.Equals(NetworkAccess.Internet)) || (Connectivity.ConnectionProfiles.Contains(ConnectionProfile.Cellular) && Connectivity.NetworkAccess.Equals(NetworkAccess.Internet)) ? true : false;
            try
            {
                var status = CrossConnectivity.Current.IsConnected;
                return status;
            }
            catch
            {
                return false;
            }


        }
        public static Boolean InternetAvailable()
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    using (client.OpenRead("http://www.google.com/"))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Checks the valid email.
        /// </summary>
        /// <returns><c>true</c>, if valid email was checked, <c>false</c> otherwise.</returns>
        /// <param name="Email">Email.</param>
        public static bool CheckValidEmail(string Email)
        {
            Email = Email.Trim();
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(Email);
            if (match.Success)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Make first char of input to upper case
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string FirstCharToUpper(string input)
        {
            switch (input)
            {
                case null: return input;
                case "": return input;
                default: return input.First().ToString().ToUpper() + input.Substring(1);
            }
        }

        /// <summary>
        /// converting date to time ago function
        /// </summary>
        /// <param name="theDate"></param>
        /// <returns></returns>
        public static string RelativeDate(DateTime dtEvent)
        {

            const int SECOND = 1;
            const int MINUTE = 60 * SECOND;
            const int HOUR = 60 * MINUTE;
            const int DAY = 24 * HOUR;
            const int MONTH = 30 * DAY;

            var ts = new TimeSpan(DateTime.UtcNow.Ticks - dtEvent.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * MINUTE)
                return ts.Seconds == 1 ? "one second ago" : ts.Seconds + " seconds ago";

            if (delta < 2 * MINUTE)
                return "a minute ago";

            if (delta < 45 * MINUTE)
                return ts.Minutes + " minutes ago";

            if (delta < 90 * MINUTE)
                return "an hour ago";

            if (delta < 24 * HOUR)
                return ts.Hours + " hours ago";

            if (delta < 48 * HOUR)
                return "yesterday";

            if (delta < 30 * DAY)
                return ts.Days + " days ago";

            if (delta < 12 * MONTH)
            {
                int months = Convert.ToInt32(Math.Floor((double)ts.Days / 30));
                return String.Format("{0} {1} " + AppResource.ago, months, (months == 1) ? AppResource.month : AppResource.months);
            }
            else
            {
                int years = Convert.ToInt32(Math.Floor((double)ts.Days / 365));
                return String.Format("{0} {1} " + AppResource.ago, years, (years == 1) ? AppResource.year : AppResource.years);
            }

            //TimeSpan TS = DateTime.Now - dtEvent;
            //Debug.WriteLine(TS);
            //if (TS.Days > 0)
            //{
            //    if (TS.Days >= 365)
            //    {
            //        var intYears = TS.Days / 365;
            //        return String.Format("{0} {1} " + AppResource.ago, intYears, (intYears == 1) ? AppResource.year : AppResource.years);
            //    }
            //    else if(TS.Days < 365 && TS.Days >= 30)
            //    {
            //        var intMonths = TS.Days / 30;
            //        return String.Format("{0} {1} " + AppResource.ago, intMonths, (intMonths == 1) ? AppResource.month : AppResource.months);
            //    }
            //    else
            //    {
            //        return String.Format("{0} {1} " + AppResource.ago, TS.Days, (TS.Days == 1) ? AppResource.day : AppResource.days);
            //    }
            //}
            //else if(TS.Hours > 0)
            //{
            //    return String.Format("{0} {1} " + AppResource.ago, TS.Hours, (TS.Hours == 1) ? AppResource.hour : AppResource.hours);
            //}
            //else if(TS.Minutes > 0)
            //{
            //    return String.Format("{0} {1} " + AppResource.ago, TS.Minutes, (TS.Minutes == 1) ? AppResource.minute : AppResource.minutes);
            //}
            //else
            //{
            //    return String.Format("{0} {1} " + AppResource.ago, TS.Seconds, (TS.Seconds == 1) ? AppResource.second : AppResource.seconds);
            //}
        }

        public static string IsImagesValid(string input, string apiImageBaseUrl = ApiUrl.BaseUrl)
        {
            if (!string.IsNullOrEmpty(input) && !string.IsNullOrWhiteSpace(input))
            {
                if (!input.StartsWith("http"))
                {
                    return apiImageBaseUrl + input;
                    //return "profile.png";
                }
                else
                {
                    return input;
                }
            }
            else
            {
                return "";
            }
        }

        public static async void CustomNavigation(INavigation navigation, Page page)
        {
            try
            {
                await navigation.PushAsync(page);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("static pushasync function exception: " + ex.Message);
            }
        }

        public static async void PopPage(INavigation navigation)
        {
            try
            {
                await navigation.PopAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("static popasync function exception: " + ex.Message);
            }
        }

        public static ObservableCollection<T> ConvertintoObservable<T>(IEnumerable original)
        {
            return new ObservableCollection<T>(original.Cast<T>());
        }

        /// <summary>
        /// Returns the description attribute of an Enum if available, othewise returns
        /// the toString() of the value passed in.
        ///
        /// Useful for Enums with spaces in them.
        /// </summary>
        /// <param name="value">the enum</param>
        /// <returns>the description string of the enum</returns>
        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
            (DescriptionAttribute[])fi.GetCustomAttributes(
            typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static string StringConcat(IEnumerable<char> charSequence)
        {
            return String.Concat(charSequence);
        }

        public static string StringBuilderChars(IEnumerable<char> charSequence)
        {
            var sb = new StringBuilder();
            foreach (var c in charSequence)
            {
                sb.Append(c);
            }
            return sb.ToString();
        }

        public static bool StringStartValue(string phonenumber, List<string> startingstring)
        {
            var IsStartingValueContain = false;
            foreach (var item in startingstring)
            {
                if (phonenumber.StartsWith(item))
                {
                    IsStartingValueContain = true;
                    break;
                }
            }
            return IsStartingValueContain;
        }

        public static bool FileExtensionCheck(string filename, List<string> extensionName)
        {
            var IsStartingValueContain = false;
            foreach (var item in extensionName)
            {
                if (filename.Contains(item))
                {
                    IsStartingValueContain = true;
                    break;
                }
            }
            return IsStartingValueContain;
        }

        public static int CalculateAge(DateTime dateOfBirth)
        {
            int age = 0;
            age = DateTime.Now.Year - dateOfBirth.Year;
            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)
                age = age - 1;

            return age;
        }

        public static string GetLanguage()
        {
            if (Application.Current.Properties.ContainsKey("AppLocale") && !string.IsNullOrEmpty(Application.Current.Properties["AppLocale"].ToString()))
            {
                return Application.Current.Properties["AppLocale"].ToString();
            }
            else
            {
                return "ar-AE";
            }
        }

        public static double ConvertBytesToMegabytes(long bytes)
        {
            return (bytes / 1024f) / 1024f;
        }
    }
}
