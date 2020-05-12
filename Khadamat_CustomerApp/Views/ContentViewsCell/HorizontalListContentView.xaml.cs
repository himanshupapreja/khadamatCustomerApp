using Khadamat_CustomerApp.Helpers;
using Khadamat_CustomerApp.Models;
using Khadamat_CustomerApp.ViewModels;
using Prism.Navigation;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Khadamat_CustomerApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HorizontalListContentView : ContentView
    {
        bool IsServiceClick;
        public HorizontalListContentView()
        {
            InitializeComponent();
            IsServiceClick = false;
            //if (Application.Current.Properties.ContainsKey("AppLocale") && !string.IsNullOrEmpty(Application.Current.Properties["AppLocale"].ToString()))
            //{
            //    var languageculture = Application.Current.Properties["AppLocale"].ToString();
            //    if (languageculture == "ar-AE")
            //    {
            //        this.FlowDirection = FlowDirection.RightToLeft;
            //    }
            //    else
            //    {
            //        this.FlowDirection = FlowDirection.LeftToRight;
            //    }
            //}
            //else
            //{
            //    this.FlowDirection = FlowDirection.LeftToRight;
            //}
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (!IsServiceClick)
            {
                IsServiceClick = true;
                try
                {
                    //await App.Current.MainPage.Navigation.PushPopupAsync(new LoaderPopup());
                    var a = (SubSubCategory)(((TappedEventArgs)e).Parameter);
                    var param = new NavigationParameters();
                    param.Add("SelectedCategories", a);
                    param.Add("CategoryId", ServiceDetailPageViewModel.service_category_id);
                    param.Add("CategoryTermCondition", ServiceDetailPageViewModel.categoryTermCondition);
                    await ServiceDetailPageViewModel.NavigationService.NavigateAsync(nameof(JobRequestPage), param, false, true);
                    //LoaderPopup.CloseAllPopup(); 
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    IsServiceClick = false;
                }
            }
        }
    }
}