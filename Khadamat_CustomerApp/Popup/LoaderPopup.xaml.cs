using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace Khadamat_CustomerApp.Views
{
    public partial class LoaderPopup : PopupPage
    {
        public LoaderPopup()
        {
            InitializeComponent();
        }

        public static async void CloseAllPopup()
        {
            await Application.Current.MainPage.Navigation.PopAllPopupAsync(false);
        }

        public async void ClosePopup()
        {
            await Application.Current.MainPage.Navigation.RemovePopupPageAsync(this);
        }
    }
}
