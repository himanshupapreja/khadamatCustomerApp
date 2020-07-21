using Khadamat_CustomerApp.Helpers;
using Khadamat_CustomerApp.Models;
using Khadamat_CustomerApp.Resources;
using Khadamat_CustomerApp.Services.ApiService;
using Khadamat_CustomerApp.Views;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;
using XF.Material.Forms.UI.Dialogs;

namespace Khadamat_CustomerApp.ViewModels
{
    public class FaqPageViewModel : ObservableCollection<FaqPage>, INotifyPropertyChanged
    {
        private readonly INavigationService NavigationService;

        WebApiRestClient _webApiRestClient;
        #region IsLoaderBusy Field
        private bool _IsLoaderBusy;

        public bool IsLoaderBusy
        {
            get { return _IsLoaderBusy; }
            set
            {
                _IsLoaderBusy = value;
                OnPropertyChanged(nameof(IsLoaderBusy));
            }
            //set { SetProperty(ref _IsLoaderBusy, value); }
        }
        #endregion

        #region IsNodataFound
        private bool _IsNodataFound;
        public bool IsNodataFound
        {
            get { return _IsNodataFound; }
            set
            {
                _IsNodataFound = value;
                OnPropertyChanged(nameof(IsNodataFound));
            }
            //set { SetProperty(ref _IsNodataFound, value); }
        }
        #endregion

        #region IsNoInternetView Field
        private bool _IsNoInternetView;

        public bool IsNoInternetView
        {
            get { return _IsNoInternetView; }
            set
            {
                _IsNoInternetView = value;
                OnPropertyChanged(nameof(IsNoInternetView));
            }
        }
        #endregion

        public static int rowCount = 0;
        public static int pageSize = 0;
        public int pageindex = 1;

        public List<FaqData> AllFaqlist = new List<FaqData>();
        private List<FaqData> _list = new List<FaqData>();
        public List<FaqData> List
        {
            get { return _list; }
            //set { SetProperty(ref _list, value); }
            set
            {
                _list = value;
                OnPropertyChanged(nameof(List));
            }
        }
        public FaqPageViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;

            try
            {
                _webApiRestClient = new WebApiRestClient();
                IsLoaderBusy = false;
                IsNodataFound = true;
                DownloadDataAsync();
            }
            catch (Exception)
            {

            }
        }
        #region BackIconCommand
        public Command BackIconCommand
        {
            get
            {
                return new Command(() =>
                {
                    NavigationService.GoBackAsync();
                });
            }
        }
        #endregion
        private async void DownloadDataAsync()
        {
            try
            {
                if (Common.CheckConnection())
                {
                    IsLoaderBusy = true;
                    FaqDataResponse response;
                    try
                    {
                        response = await _webApiRestClient.GetAsync<FaqDataResponse>(ApiUrl.GetFaqData);
                    }
                    catch (Exception ex)
                    {
                        response = null;
                        IsLoaderBusy = false;
                        //await MaterialDialog.Instance.SnackbarAsync(message: AppResource.error_ServerError, msDuration: 3000);
                    }
                    if (response != null)
                    {
                        if (response.status)
                        {
                            Application.Current.Properties["FAQData"] = JsonConvert.SerializeObject(response.faqData);
                            Application.Current.SavePropertiesAsync();
                            foreach (var item in response.faqData)
                            {
                                var faqdata = new FaqData()
                                {
                                    answer = Common.GetLanguage() == "ar-AE" ? item.answer_arabic : item.answer,
                                    answer_arabic = item.answer_arabic,
                                    question = Common.GetLanguage() == "ar-AE" ? item.question_arabic : item.question,
                                    question_arabic = item.question_arabic,
                                    created_date = item.created_date,
                                    faq_id = item.faq_id,
                                    Icon = "faq_arrow_left.png",
                                    IsViewVisible = false,
                                    is_active = item.is_active,
                                    modified_date = item.modified_date
                                };
                                AllFaqlist.Add(faqdata);
                            }
                            List = AllFaqlist;
                            if (List != null && List.Count > 0)
                            {
                                IsNodataFound = false;
                            }
                            
                        }
                    }
                    else
                    {
                        if (Application.Current.Properties.ContainsKey("FAQData") && !string.IsNullOrEmpty(Application.Current.Properties["FAQData"].ToString()) && !string.IsNullOrWhiteSpace(Application.Current.Properties["FAQData"].ToString()))
                        {
                            OfflineData(); 
                            IsNodataFound = false;
                        }
                    }
                }
                else
                {
                    if (Application.Current.Properties.ContainsKey("FAQData") && !string.IsNullOrEmpty(Application.Current.Properties["FAQData"].ToString()) && !string.IsNullOrWhiteSpace(Application.Current.Properties["FAQData"].ToString()))
                    {
                        OfflineData(); 
                        IsNodataFound = false;
                    }
                    else
                    {
                        IsNodataFound = false;
                        IsNoInternetView = true;
                    }
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

        #region DataWhennoInternet
        private void OfflineData()
        {
            var faqData = JsonConvert.DeserializeObject<List<FaqData>>(Application.Current.Properties["FAQData"].ToString());
            foreach (var item in faqData)
            {
                var faqdata = new FaqData()
                {
                    answer = Common.GetLanguage() == "ar-AE" ? item.answer_arabic : item.answer,
                    answer_arabic = item.answer_arabic,
                    question = Common.GetLanguage() == "ar-AE" ? item.question_arabic : item.question,
                    question_arabic = item.question_arabic,
                    created_date = item.created_date,
                    faq_id = item.faq_id,
                    Icon = "faq_arrow_left.png",
                    IsViewVisible = false,
                    is_active = item.is_active,
                    modified_date = item.modified_date
                };
                AllFaqlist.Add(faqdata);
            }
            List = AllFaqlist;
        }
        #endregion

        public ICommand Drop_Image_Command => new Command(getData =>
        {
            var getCurrentData = getData as FaqData;

            if (getCurrentData != null)
            {
                var index = List.ToList().IndexOf(getCurrentData);

                //for (int i = 0; i < List.Count; i++)
                //{
                //    if (i != index)
                //    {
                //        List[i].IsViewVisible = false;
                //        List[i].Icon = "faq_arrow_left.png";

                //        RaisePropertyChanged(nameof(List));
                //    }
                //}

                if (!List[index].IsViewVisible)
                {
                    List[index].IsViewVisible = true;


                    List[index].Icon = "faq_arrow_down.png";
                }
                else
                {
                    List[index].IsViewVisible = false;

                    List[index].Icon = "faq_arrow_left.png";
                }
            }

            RaisePropertyChanged(nameof(List));
            //if (getCurrentData != null)
            //{
            //    var index = AllFaqlist.IndexOf(getCurrentData);

            //    if (AllFaqlist[index].Icon == "faq_arrow_down.png")
            //    {
            //        AllFaqlist[index].IsViewVisible = false;
            //        AllFaqlist[index].Icon = "faq_arrow_left.png";
            //    }
            //    else if (AllFaqlist[index].Icon == "faq_arrow_left.png")
            //    {
            //        AllFaqlist[index].IsViewVisible = true;
            //        AllFaqlist[index].Icon = "faq_arrow_down.png";
            //    }
            //    List = AllFaqlist;
            //}
        });

        void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
