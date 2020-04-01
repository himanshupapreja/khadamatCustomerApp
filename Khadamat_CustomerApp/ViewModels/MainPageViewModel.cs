using Khadamat_CustomerApp.Views;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Khadamat_CustomerApp.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Main Page";
        }

        public ICommand LoginCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    try
                    {
                        NavigationService.NavigateAsync(nameof(LoginPage));
                    }
                    catch (Exception ex)
                    {

                    }
                });
            }
        }
    }
}
