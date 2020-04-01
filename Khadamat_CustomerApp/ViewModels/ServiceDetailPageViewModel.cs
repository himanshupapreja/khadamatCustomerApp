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
    public class ServiceDetailPageViewModel : BaseViewModel, INavigationAware
    {
        public static INavigationService NavigationService;
        public static string categoryTermCondition;
        List<SubCategory> subCategories;

        private string CategoryName;

        private string _Title;

        public string Title
        {
            get { return _Title; }
            set { SetProperty(ref _Title, value); }
        }

        #region SubServiceList
        private ObservableCollection<SubCategory> AllSubServiceList = new ObservableCollection<SubCategory>();
        private ObservableCollection<SubCategory> _SubServiceList = new ObservableCollection<SubCategory>();
        public ObservableCollection<SubCategory> SubServiceList
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
        public ServiceDetailPageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("ServiceDetailTitle"))
            {
                CategoryName = (string)parameters["ServiceDetailTitle"];
                Title = CategoryName;
            }
            if (parameters.ContainsKey("ServiceDetailTermData"))
            {
                categoryTermCondition = (string)parameters["ServiceDetailTermData"];
            }
            if (parameters.ContainsKey("ServiceDetailData"))
            {
                subCategories = ((List<SubCategory>)parameters["ServiceDetailData"]).Where(x => x.SubSubCategories != null && x.SubSubCategories.Count > 0).ToList();
            }
            //CategoryName = categoryName;
            if (subCategories != null && subCategories.Count > 0)
            {
                IsNodataFound = false;
                AllSubServiceList = new ObservableCollection<SubCategory>(subCategories);
                foreach (var item in AllSubServiceList)
                {
                    var index = AllSubServiceList.IndexOf(item);
                    AllSubServiceList[index].SubServiceListHeight = item.SubSubCategories.Count % 3 == 0 ? ((item.SubSubCategories.Count / 3) * 150 + (item.SubSubCategories.Count / 3) * 3 * 5) : (((item.SubSubCategories.Count / 3) + 1) * 150 + ((item.SubSubCategories.Count / 3) + 1) * 3 * 5);
                    AllSubServiceList[index].sub_category_name = Common.GetLanguage() != "ar-AE" ? item.sub_category_name : item.sub_category_name_arabic;

                    foreach (var subSubCategoryItem in item.SubSubCategories)
                    {
                        var subSubCategoryItemIndex = AllSubServiceList[index].SubSubCategories.IndexOf(subSubCategoryItem);
                        AllSubServiceList[index].SubSubCategories[subSubCategoryItemIndex].sub_sub_category_name = Common.GetLanguage() != "ar-AE" ? subSubCategoryItem.sub_sub_category_name : subSubCategoryItem.sub_sub_category_name_arabic;
                    }
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
