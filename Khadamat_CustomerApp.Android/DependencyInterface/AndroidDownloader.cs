using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Khadamat_CustomerApp.DependencyInterface;
using Khadamat_CustomerApp.Droid.DependencyInterface;
using Xamarin.Forms;

[assembly: Dependency(typeof(AndroidDownloader))]
namespace Khadamat_CustomerApp.Droid.DependencyInterface
{
    public class AndroidDownloader : IDownloader
    {
        public event EventHandler<DownloadEventArgs> OnFileDownloaded;

        public void DownloadFile(string url, string folder)
        {
            string pathToNewFolder = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, folder);
            Directory.CreateDirectory(pathToNewFolder);

            try
            {
                WebClient webClient = new WebClient();
                webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                string pathToNewFile = Path.Combine(pathToNewFolder, Path.GetFileName(url));
                webClient.DownloadFileAsync(new Uri(url), pathToNewFile);
            }
            catch (Exception ex)
            {
                if (OnFileDownloaded != null)
                    OnFileDownloaded.Invoke(this, new DownloadEventArgs(false));
            }
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                if (OnFileDownloaded != null)
                    OnFileDownloaded.Invoke(this, new DownloadEventArgs(false));
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(e.Error);
                if (OnFileDownloaded != null)
                    OnFileDownloaded.Invoke(this, new DownloadEventArgs(true));
            }
        }
    }
}