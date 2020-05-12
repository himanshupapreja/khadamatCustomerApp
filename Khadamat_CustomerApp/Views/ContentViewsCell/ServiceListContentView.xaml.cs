using Khadamat_CustomerApp.Helpers;
using Khadamat_CustomerApp.Models;
using Khadamat_CustomerApp.Resources;
using Khadamat_CustomerApp.Services.ApiService;
using System;
using Rg.Plugins.Popup.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.Material.Forms.UI.Dialogs;
using Prism.Navigation;
using Khadamat_CustomerApp.ViewModels;

namespace Khadamat_CustomerApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ServiceListContentView : ContentView
    {
        bool IsServiceClick;
        private WebApiRestClient _webApiRestClient;
        public ServiceListContentView()
        {
            InitializeComponent();
            IsServiceClick = false;
            _webApiRestClient = new WebApiRestClient();
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
            MessagingCenter.Subscribe<string>(this, "ServiceListHeightUpdate", (sender) =>
            {
                var serviceListCount = Convert.ToInt32(sender);

                categorieslistview.HeightRequest = serviceListCount % 2 == 0 ? (200 * (serviceListCount / 2)) + (20 * (serviceListCount / 2)) : (198 * (serviceListCount / 2 + 1)) + (20 * (serviceListCount / 2 + 1));
            });
        }

        private async void Homemaintenancelistview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //categorieslistview.SelectedItem = null;
            
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (!IsServiceClick)
            {
                IsServiceClick = true;
                try
                {
                    var a = (CategoryListModel)(((TappedEventArgs)e).Parameter);
                    if (a != null)
                    {
                        //await App.Current.MainPage.Navigation.PushPopupAsync(new LoaderPopup());
                        //GetSubServiceData(a.service_category_name, a.SubCategories, a.terms_conditions);
                        var param = new NavigationParameters();
                        param.Add("ServiceDetailTitle", a.service_category_name);
                        param.Add("CategoryId", a.service_category_id);
                        param.Add("ServiceDetailData", a.SubCategories);
                        param.Add("ServiceDetailTermData", a.terms_conditions);
                        await HomePageViewModel.NavigationService.NavigateAsync(nameof(ServiceDetailPage), param);

                    }
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    IsServiceClick = false;
                    //await Task.Delay(1500);
                    //LoaderPopup.CloseAllPopup();
                }
            }
        }

        #region GetSubServiceData
        //private async void GetSubServiceData(string service_category_name, List<SubCategory> subCategoryData, string terms_conditions)
        //{

        //}
        #endregion

        //private void SelectedItemTap(object sender, ItemTappedEventArgs e)
        //{
        //    var a = (CategoryListModel)e.Item;
        //    GetSubServiceData(a.service_category_name, a.service_category_id);
        //}
    }
}