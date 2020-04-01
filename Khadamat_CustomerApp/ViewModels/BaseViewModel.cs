using Khadamat_CustomerApp.Models;
using Khadamat_CustomerApp.Services.ApiService;
using Khadamat_CustomerApp.Services.DBService.LiteDB.ModelDB;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Khadamat_CustomerApp.ViewModels
{
    public class BaseViewModel : BindableBase
    {
        #region Existing Data

        public static long user_id;
        public static string user_name;
        public static string user_pic;
        public static bool email_verified;
        private byte[] myfile;

        public static List<ProvienceDataModel> provienceDataModels;
        public static List<CountryDataModel> countryDataModels;

        protected WebApiRestClient _webApiRestClient;
        public UserDataDbService userDataDbService;



        //header menu icon command
        #region MenuIconCommand
        public Command MenuIconCommand
        {
            get
            {
                return new Command(() =>
                {
                    MessagingCenter.Send("MenuIconClick", "MenuIconClick");
                });
            }
        }
        #endregion

        //header back icon command
        

        #region Gallery/Camera command
        protected async Task<ImagesModel> GalleryCommand()
        {
            ImagesModel model = new ImagesModel();
            try
            {
                MediaFile _mediaFile;
                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await App.Current.MainPage.DisplayAlert("Photos Not Supported", ":( Permission not granted to photos.", "OK");
                    return null;
                }
                _mediaFile = await Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium,

                });


                if (_mediaFile == null)
                    return null;


                using (var memoryStream = new MemoryStream())
                {
                    _mediaFile.GetStream().CopyTo(memoryStream);
                    myfile = memoryStream.ToArray();
                    //myfile = await CrossImageResizer.Current.ResizeImageWithAspectRatioAsync(mysfile, 200, 200);


                }
                model.ImageBytes = myfile;
                model.Image = ImageSource.FromStream(() => new MemoryStream(myfile));
                model.ImageStream = _mediaFile.GetStream();
                model.ImagePath = _mediaFile.Path;
                _mediaFile.Dispose();

                MessagingCenter.Send(model, "ProfilePicture");

                MessagingCenter.Unsubscribe<ImagesModel>(model, "ProfilePicture");
                return model;//
                //ImagePicker = ImageSource.FromStream(() =>
                //{
                //    var stream = _mediaFile.GetStream();
                //    _mediaFile.Dispose();
                //    return stream;
                //});
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        protected async Task<ImagesModel> CameraCommand()
        {
            ImagesModel model = new ImagesModel();
            try
            {
                MediaFile _mediaFile;
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await App.Current.MainPage.DisplayAlert("No Camera", ":( No camera available.", "OK");
                    return null;
                }

                _mediaFile = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "Profile Photo",
                    SaveToAlbum = true,
                    PhotoSize = PhotoSize.Medium,
                    MaxWidthHeight = 2000,
                    DefaultCamera = CameraDevice.Rear
                });


                if (_mediaFile == null)
                    return null;

                using (var memoryStream = new MemoryStream())
                {
                    _mediaFile.GetStream().CopyTo(memoryStream);
                    myfile = memoryStream.ToArray();
                    _mediaFile.Dispose();
                }
                model.ImageBytes = myfile;
                model.Image = ImageSource.FromStream(() => new MemoryStream(myfile));
                MessagingCenter.Send(model, "ProfilePicture");
                return model;
                //CameraPicker = ImageSource.FromStream(() =>
                // {
                //     var stream = _mediaFile.GetStream();
                //     _mediaFile.Dispose();
                //     return stream;
                // });
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Converting Stream into byte array
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
        #endregion
        #endregion

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public BaseViewModel()
        {
            _webApiRestClient = new WebApiRestClient();
            userDataDbService = new UserDataDbService();
        }
    }
}
