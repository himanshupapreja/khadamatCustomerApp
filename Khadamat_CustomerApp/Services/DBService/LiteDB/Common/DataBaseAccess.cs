using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace LiteDB.Common
{
    public static class DataBaseAccess
    {
        #region ISQLite implementation
        public static string DatabasePath()
        {
            var fileName = "DBKhadamatCustomer.db3";
            if (Device.RuntimePlatform == Device.Android)
            {
                var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), fileName);

                if (!File.Exists(path))
                {
                    File.Create(path).Dispose();
                }

                return path;
            }
            else
            {
                string docFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                string libFolder = Path.Combine(docFolder, "..", "Library", "Databases");

                if (!Directory.Exists(libFolder))
                {
                    Directory.CreateDirectory(libFolder);
                }

                return Path.Combine(libFolder, fileName);
            }
        }

        #endregion
    }
}
