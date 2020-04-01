using Khadamat_CustomerApp.ViewModels;
using Xamarin.Forms;

namespace Khadamat_CustomerApp.Views
{
    public partial class CustomMapPage : ContentPage
    {
        public CustomMapPage()
        {
            InitializeComponent();
            CustomMapPageViewModel CustomMapPageViewModel = this.BindingContext as CustomMapPageViewModel;
            CustomMapPageViewModel.customMap = customMap;
        }
    }
}
