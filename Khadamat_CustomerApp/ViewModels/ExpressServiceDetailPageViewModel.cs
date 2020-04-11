using Khadamat_CustomerApp.Helpers;
using Khadamat_CustomerApp.Models;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Khadamat_CustomerApp.ViewModels
{
    public class ExpressServiceDetailPageViewModel : BaseViewModel, INavigationAware
    {
        public static INavigationService NavigationService;
        public static string categoryTermCondition;
        List<ExpressSubCategory> subCategories;

        private string CategoryName;

        private string _Title;

        public string Title
        {
            get { return _Title; }
            set { SetProperty(ref _Title, value); }
        }

        #region SubServiceList
        private ObservableCollection<ExpressSubCategory> AllSubServiceList = new ObservableCollection<ExpressSubCategory>();
        private ObservableCollection<ExpressSubCategory> _SubServiceList = new ObservableCollection<ExpressSubCategory>();
        public ObservableCollection<ExpressSubCategory> SubServiceList
        {
            get { return _SubServiceList; }
            set { SetProperty(ref _SubServiceList, value); }
        }
        #endregion

        #region IsNodataFound
        private bool _IsNodataFound;
        public bool IsNodataFound
        {
            get { return _IsNodataFound; }
            set { SetProperty(ref _IsNodataFound, value); }
        }
        #endregion

        public ExpressServiceDetailPageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("ExpressServiceDetailTitle"))
            {
                CategoryName = (string)parameters["ExpressServiceDetailTitle"];
                Title = CategoryName;
            }
            if (parameters.ContainsKey("ExpressServiceDetailData"))
            {
                subCategories = ((List<ExpressSubCategory>)parameters["ExpressServiceDetailData"]).ToList();
            }
            //CategoryName = categoryName;
            if (subCategories != null && subCategories.Count > 0)
            {
                IsNodataFound = false;
                AllSubServiceList = new ObservableCollection<ExpressSubCategory>(subCategories);
                foreach (var item in AllSubServiceList)
                {
                    var index = AllSubServiceList.IndexOf(item);
                    AllSubServiceList[index].sub_express_category_name = Common.GetLanguage() != "ar-AE" ? item.sub_express_name : item.sub_express_name_arabic;
                    AllSubServiceList[index].icon = Common.IsImagesValid(item.icon,ApiUrl.ServiceImageBaseUrl);

                }
                SubServiceList = AllSubServiceList;
            }
            else
            {
                IsNodataFound = true;
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
