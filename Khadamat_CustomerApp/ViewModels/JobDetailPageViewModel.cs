using Khadamat_CustomerApp.Models;
using Khadamat_CustomerApp.Resources;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using static Khadamat_CustomerApp.Helpers.Enums;

namespace Khadamat_CustomerApp.ViewModels
{
    public class JobDetailPageViewModel : BaseViewModel, INavigationAware
    {
        private readonly INavigationService NavigationService;
        JobRequestData jobRequestData;
        #region JobDetailData
        private JobRequestData _JobDetailData = new JobRequestData();
        public JobRequestData JobDetailData
        {
            get { return _JobDetailData; }
            set { SetProperty(ref _JobDetailData, value); }
        }
        #endregion

        #region Constructor
        public JobDetailPageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
        }
        #endregion

        #region Navigation Aware Meyhods
        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("JobData"))
            {
                jobRequestData = (JobRequestData)parameters["JobData"];

                if (jobRequestData != null)
                {
                    JobDetailData = jobRequestData;
                    JobDetailData.IsJobCancel = (!string.IsNullOrEmpty(JobDetailData.cancel_reason) && !string.IsNullOrEmpty(JobDetailData.cancel_reason)) ? true : false;
                    JobDetailData.IsQuotePrice = (!string.IsNullOrEmpty(JobDetailData.quote_price) && !string.IsNullOrEmpty(JobDetailData.quote_price)) ? true : false;
                    JobDetailData.IsQuoteDescription = (!string.IsNullOrEmpty(JobDetailData.quote_description) && !string.IsNullOrEmpty(JobDetailData.quote_description)) ? true : false;
                    if (!string.IsNullOrEmpty(JobDetailData.currency) && !string.IsNullOrWhiteSpace(JobDetailData.currency))
                    {
                        JobDetailData.JobQuote = Convert.ToInt32(JobDetailData.currency) == Convert.ToInt32(CurrencyStatus.Yer) ? jobRequestData.quote_price + " " + CurrencyStatus.Yer.ToString() : jobRequestData.quote_price + " " + CurrencyStatus.USD.ToString();
                    }
                    JobDetailData.IsLocationAvailable = !string.IsNullOrEmpty(JobDetailData.location) && !string.IsNullOrWhiteSpace(JobDetailData.location) && JobDetailData.location != AppResource.cyp_StreetPlaceholder ? true : false;
                    JobDetailData.JobDateTimeValue = JobDetailData.job_date_time.HasValue ? JobDetailData.job_date_time.Value.ToString("dd-MMM-yyyy, hh:mm tt") : null;
                }
            }
        }
        #endregion

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
