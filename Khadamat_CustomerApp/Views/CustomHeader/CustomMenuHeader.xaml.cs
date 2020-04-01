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
    public partial class CustomMenuHeader : ContentView
    {
        public CustomMenuHeader()
        {
            InitializeComponent();
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

        public static readonly BindableProperty TitleTextProperty = BindableProperty.Create("TitleText", typeof(string), typeof(Label), null);
        public string TitleText
        {
            get { return (string)GetValue(TitleTextProperty); }
            set { SetValue(TitleTextProperty, value); }
        }

        public static readonly BindableProperty MenuIconImageProperty = BindableProperty.Create("MenuIconImage", typeof(string), typeof(Image), null);
        public string MenuIconImage
        {
            get { return (string)GetValue(MenuIconImageProperty); }
            set { SetValue(MenuIconImageProperty, value); }
        }

        public static readonly BindableProperty RightIconImageProperty = BindableProperty.Create("RightIconImage", typeof(string), typeof(Image), null);
        public string RightIconImage
        {
            get { return (string)GetValue(RightIconImageProperty); }
            set { SetValue(RightIconImageProperty, value); }
        }

        public static readonly BindableProperty Right1IconImageProperty = BindableProperty.Create("Right1IconImage", typeof(string), typeof(Image), null);
        public string Right1IconImage
        {
            get { return (string)GetValue(Right1IconImageProperty); }
            set { SetValue(Right1IconImageProperty, value); }
        }
    }
}