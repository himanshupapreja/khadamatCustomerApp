using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;

namespace Khadamat_CustomerApp.ViewModels
{
    public class CustomMapPageViewModel : BaseViewModel, INavigationAware
    {
        private readonly INavigationService NavigationService;
        public CustomControls.CustomMap customMap;
        Location location;
        public CustomMapPageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("CurrentLocationPoints"))
            {
                location = (Location)parameters["CurrentLocationPoints"];

                if(customMap != null && location != null)
                {
                    try
                    {
                        customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(location.Latitude, location.Longitude), Distance.FromMiles(0.5)));
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }

        #region BackIconCommand
        public ICommand BackIconCommand
        {
            get
            {
                return new DelegateCommand(() =>
                {
                    NavigationService.GoBackAsync();
                });
            }
        }
        #endregion
    }
}
