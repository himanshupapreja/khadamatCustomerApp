using Khadamat_CustomerApp.Models;
using Khadamat_CustomerApp.Services.ApiService;
using Khadamat_CustomerApp.ViewModels;
using Prism.Navigation;
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
    public partial class ExpressServiceListContentView : ContentView
    {
        bool IsServiceClick;
        private WebApiRestClient _webApiRestClient;
        public ExpressServiceListContentView()
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

                expresscategorieslistview.HeightRequest = serviceListCount % 2 == 0 ? (200 * (serviceListCount / 2)) + (20 * (serviceListCount / 2)) : (198 * (serviceListCount / 2 + 1)) + (20 * (serviceListCount / 2 + 1));
            });
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
                        param.Add("ServiceDetailData", a.SubCategories);
                        param.Add("ServiceDetailTermData", a.terms_conditions);
                        await ExpressServicePageViewModel.NavigationService.NavigateAsync(nameof(ServiceDetailPage), param);

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
    }
}