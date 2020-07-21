using Xamarin.Forms;

namespace Khadamat_CustomerApp.Views
{
    public partial class StartupPage : ContentPage
    {
        public StartupPage()
        {
			try
			{
				InitializeComponent();
				gifLoading.Play();
			}
			catch (System.Exception ex)
			{
			}
        }
    }
}
