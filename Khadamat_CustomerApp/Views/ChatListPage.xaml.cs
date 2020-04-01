using Xamarin.Forms;

namespace Khadamat_CustomerApp.Views
{
    public partial class ChatListPage : ContentPage
    {
        public ChatListPage()
        {
            InitializeComponent();
            if (Application.Current.Properties.ContainsKey("AppLocale") && !string.IsNullOrEmpty(Application.Current.Properties["AppLocale"].ToString()))
            {
                var languageculture = Application.Current.Properties["AppLocale"].ToString();
                if (languageculture == "ar-AE")
                {
                    this.FlowDirection = FlowDirection.RightToLeft;
                }
                else
                {
                    this.FlowDirection = FlowDirection.LeftToRight;
                }
            }
            else
            {
                this.FlowDirection = FlowDirection.LeftToRight;
            }
        }

        private void chatList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            chatList.SelectedItem = null;
        }
    }
}
