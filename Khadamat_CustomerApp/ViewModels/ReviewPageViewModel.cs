using Khadamat_CustomerApp.Helpers;
using Khadamat_CustomerApp.Models;
using Khadamat_CustomerApp.Resources;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace Khadamat_CustomerApp.ViewModels
{
    public class ReviewPageViewModel : BaseViewModel, INavigationAware
    {
        private readonly INavigationService NavigationService;
        JobRequestData UserReviewJob;
        int userrating;
        #region IsNewReview Field
        private bool _IsNewReview;

        public bool IsNewReview
        {
            get { return _IsNewReview; }
            set { SetProperty(ref _IsNewReview, value); }
        }
        #endregion

        #region IsNoInternetView Field
        private bool _IsNoInternetView;

        public bool IsNoInternetView
        {
            get { return _IsNoInternetView; }
            set { SetProperty(ref _IsNoInternetView, value); }
        }
        #endregion

        #region IsLoaderBusy Field
        private bool _IsLoaderBusy;

        public bool IsLoaderBusy
        {
            get { return _IsLoaderBusy; }
            set { SetProperty(ref _IsLoaderBusy, value); }
        }
        #endregion

        #region ReviewTitle field
        private string _ReviewTitle;

        public string ReviewTitle
        {
            get { return _ReviewTitle; }
            set { SetProperty(ref _ReviewTitle, value); }
        }
        #endregion

        #region RatingTitle field
        private string _RatingTitle;

        public string RatingTitle
        {
            get { return _RatingTitle; }
            set { SetProperty(ref _RatingTitle, value); }
        }
        #endregion

        #region CommentTitle field
        private string _CommentTitle;

        public string CommentTitle
        {
            get { return _CommentTitle; }
            set { SetProperty(ref _CommentTitle, value); }
        }
        #endregion

        #region Star1 field
        private string _Star1;

        public string Star1
        {
            get { return _Star1; }
            set { SetProperty(ref _Star1, value); }
        }
        #endregion

        #region Star2 field
        private string _Star2;

        public string Star2
        {
            get { return _Star2; }
            set { SetProperty(ref _Star2, value); }
        }
        #endregion

        #region Star3 field
        private string _Star3;

        public string Star3
        {
            get { return _Star3; }
            set { SetProperty(ref _Star3, value); }
        }
        #endregion

        #region Star4 field
        private string _Star4;

        public string Star4
        {
            get { return _Star4; }
            set { SetProperty(ref _Star4, value); }
        }
        #endregion

        #region Star5 field
        private string _Star5;

        public string Star5
        {
            get { return _Star5; }
            set { SetProperty(ref _Star5, value); }
        }
        #endregion

        #region WorkerReviewValue field
        private string _WorkerReviewValue;

        public string WorkerReviewValue
        {
            get { return _WorkerReviewValue; }
            set { SetProperty(ref _WorkerReviewValue, value); }
        }
        #endregion

        #region Constructor
        public ReviewPageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
            IsNoInternetView = false;
        }
        #endregion

        #region CheckReview
        private async void CheckReview()
        {
            try
            {
                if (UserReviewJob.user_rating.HasValue && UserReviewJob.user_rating.Value > 0.0)
                {
                    ReviewTitle = AppResource.review_ReviewTitle1;
                    RatingTitle = AppResource.review_RatingTitle1;
                    CommentTitle = AppResource.review_CommentTitle1;
                    IsLoaderBusy = true;
                    IsNewReview = false;

                    if (Common.CheckConnection())
                    {
                        IsLoaderBusy = true;
                        WorkerReviewResponseModel response;
                        try
                        {
                            response = await _webApiRestClient.GetAsync<WorkerReviewResponseModel>(string.Format(ApiUrl.CheckUserReview, UserReviewJob.id));
                        }
                        catch (Exception ex)
                        {
                            response = null;
                            IsLoaderBusy = false;
                            //await MaterialDialog.Instance.SnackbarAsync(message: ex.Message, msDuration: 3000);
                        }
                        if (response != null)
                        {
                            if (response.status)
                            {
                                var reviewData = JsonConvert.SerializeObject(response.jobReview);
                                await SecureStorage.SetAsync("ReviewData", reviewData);
                                rateStarMethod(response.jobReview.rating);
                                WorkerReviewValue = response.jobReview.user_review;
                            }
                            else
                            {
                                await MaterialDialog.Instance.SnackbarAsync(message: response.message,
                                            msDuration: 3000);
                            }
                        }
                        else
                        {
                            if(SecureStorage.GetAsync("ReviewData") != null)
                            {
                                var jobReviewData = JsonConvert.DeserializeObject<ReviewModel>(SecureStorage.GetAsync("ReviewData").Result.ToString());
                                rateStarMethod(jobReviewData.rating);
                                WorkerReviewValue = jobReviewData.user_review;
                            }
                            else
                            {
                                IsNoInternetView = true;
                            }
                        }
                    }
                    else
                    {
                        //await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_InternetError,
                        //                        msDuration: 3000);
                        if (SecureStorage.GetAsync("ReviewData") != null)
                        {
                            var jobReviewData = JsonConvert.DeserializeObject<ReviewModel>(SecureStorage.GetAsync("ReviewData").Result.ToString());
                            rateStarMethod(jobReviewData.rating);
                            WorkerReviewValue = jobReviewData.user_review;
                        }
                        else
                        {
                            IsNoInternetView = true;
                        }
                    }
                }
                else
                {
                    ReviewTitle = AppResource.review_ReviewTitle;
                    RatingTitle = AppResource.review_RatingTitle;
                    CommentTitle = AppResource.review_CommentTitle;
                    IsNewReview = true;
                    Star1 = "resource://Khadamat_CustomerApp.SvgImages.ic_star_blank.svg";
                    Star2 = "resource://Khadamat_CustomerApp.SvgImages.ic_star_blank.svg";
                    Star3 = "resource://Khadamat_CustomerApp.SvgImages.ic_star_blank.svg";
                    Star4 = "resource://Khadamat_CustomerApp.SvgImages.ic_star_blank.svg";
                    Star5 = "resource://Khadamat_CustomerApp.SvgImages.ic_star_blank.svg";
                    userrating = 0;
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsLoaderBusy = false;
            }
        }
        #endregion

        #region RatingCommand
        public Command RatingCommand
        {
            get
            {
                return new Command((e) =>
                {
                    if (UserReviewJob.user_rating.HasValue && UserReviewJob.user_rating.Value <= 0.0)
                    {
                        rateStarMethod(Convert.ToInt32(e.ToString()));
                    }
                });
            }
        }
        #endregion

        #region rateStarMethod
        private void rateStarMethod(int e)
        {
            switch (e)
            {
                case 1:
                    Star1 = "resource://Khadamat_CustomerApp.SvgImages.ic_star_active.svg";
                    Star2 = "resource://Khadamat_CustomerApp.SvgImages.ic_star_blank.svg";
                    Star3 = "resource://Khadamat_CustomerApp.SvgImages.ic_star_blank.svg";
                    Star4 = "resource://Khadamat_CustomerApp.SvgImages.ic_star_blank.svg";
                    Star5 = "resource://Khadamat_CustomerApp.SvgImages.ic_star_blank.svg";
                    userrating = 1;
                    break;
                case 2:
                    Star1 = "resource://Khadamat_CustomerApp.SvgImages.ic_star_active.svg";
                    Star2 = "resource://Khadamat_CustomerApp.SvgImages.ic_star_active.svg";
                    Star3 = "resource://Khadamat_CustomerApp.SvgImages.ic_star_blank.svg";
                    Star4 = "resource://Khadamat_CustomerApp.SvgImages.ic_star_blank.svg";
                    Star5 = "resource://Khadamat_CustomerApp.SvgImages.ic_star_blank.svg";
                    userrating = 2;
                    break;
                case 3:
                    Star1 = "resource://Khadamat_CustomerApp.SvgImages.ic_star_active.svg";
                    Star2 = "resource://Khadamat_CustomerApp.SvgImages.ic_star_active.svg";
                    Star3 = "resource://Khadamat_CustomerApp.SvgImages.ic_star_active.svg";
                    Star4 = "resource://Khadamat_CustomerApp.SvgImages.ic_star_blank.svg";
                    Star5 = "resource://Khadamat_CustomerApp.SvgImages.ic_star_blank.svg";
                    userrating = 3;
                    break;
                case 4:
                    Star1 = "resource://Khadamat_CustomerApp.SvgImages.ic_star_active.svg";
                    Star2 = "resource://Khadamat_CustomerApp.SvgImages.ic_star_active.svg";
                    Star3 = "resource://Khadamat_CustomerApp.SvgImages.ic_star_active.svg";
                    Star4 = "resource://Khadamat_CustomerApp.SvgImages.ic_star_active.svg";
                    Star5 = "resource://Khadamat_CustomerApp.SvgImages.ic_star_blank.svg";
                    userrating = 4;
                    break;
                case 5:
                    Star1 = "resource://Khadamat_CustomerApp.SvgImages.ic_star_active.svg";
                    Star2 = "resource://Khadamat_CustomerApp.SvgImages.ic_star_active.svg";
                    Star3 = "resource://Khadamat_CustomerApp.SvgImages.ic_star_active.svg";
                    Star4 = "resource://Khadamat_CustomerApp.SvgImages.ic_star_active.svg";
                    Star5 = "resource://Khadamat_CustomerApp.SvgImages.ic_star_active.svg";
                    userrating = 5;
                    break;
            }
        }
        #endregion

        #region DoneBtnCommand
        public Command DoneBtnCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        if (userrating > 0)
                        {
                            if (Common.CheckConnection())
                            {
                                IsLoaderBusy = true;
                                ReviewModel request = new ReviewModel
                                {
                                    customer_id = UserReviewJob.customer_id,
                                    job_request_id = UserReviewJob.id,
                                    rating = userrating,
                                    user_review = WorkerReviewValue,
                                    worker_id = UserReviewJob.service_provider_id.Value
                                };
                                WorkerReviewResponseModel response;
                                try
                                {
                                    response = await _webApiRestClient.PostAsync<ReviewModel, WorkerReviewResponseModel>(ApiUrl.SubmitUserReview, request);
                                }
                                catch (Exception ex)
                                {
                                    response = null;
                                    IsLoaderBusy = false;
                                    await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_ServerError, msDuration: 3000);
                                    return;
                                }
                                if (response != null)
                                {
                                    if (response.status)
                                    {
                                        await NavigationService.NavigateAsync(new Uri("/MasterPage/NavigationPage/HomeTabbedPage", UriKind.Absolute));
                                    }
                                    else
                                    {
                                        await MaterialDialog.Instance.SnackbarAsync(message: response.message,
                                                    msDuration: 3000);
                                    }
                                }
                            }
                            else
                            {
                                await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_InternetError,
                                                        msDuration: 3000);
                            }
                        }
                        else
                        {
                            await MaterialDialog.Instance.SnackbarAsync(AppResource.error_ReviewError, 3000);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    finally
                    {
                        IsLoaderBusy = false;
                    }
                });
            }
        }
        #endregion

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("UserReview"))
            {
                UserReviewJob = (JobRequestData)parameters["UserReview"];
            }

            CheckReview();
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
