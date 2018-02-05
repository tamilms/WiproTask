using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using Xamarin.Forms;
using System.Linq;

namespace WiproTask
{
    public class FactsPageViewModel:INotifyPropertyChanged
    {
        FactsPage Page;
        /// <summary>
        /// Flag for set Sort Asc/Desc
        /// </summary>
        bool isAlreadySortAsc = false;
        public FactsPageViewModel(FactsPage page)
        {
            Page = page;
            InitializePage();

            Page.listviewFacts.ItemTapped+=delegate {
                Page.listviewFacts.SelectedItem = null;
            };
        }

        /// <summary>
        /// Sort the Collection in alphabetical order by title
        /// If pressed again, the sort order will be reversed
        /// </summary>
        /// <value>The sort command.</value>
        public ICommand SortCommand{
            get{
                return new Command((obj) => {
                    
                    if (isAlreadySortAsc == false)
                    {
                        if (ListBindingSourceData != null)
                        {
                            isAlreadySortAsc = true;

                            ListBindingSourceData = new ObservableCollection<Row>(ListBindingSourceData.OrderBy(x => x.title).ToList());
                        }
                    }
                    else
                    {
                        isAlreadySortAsc = false;
                        if (ListBindingSourceData != null)
                        {
                            ListBindingSourceData = new ObservableCollection<Row>(ListBindingSourceData.OrderByDescending(x => x.title).ToList());
                        }
                    }
                });
            }
        }

        /// <summary>
        /// The refresh function will reload all items back into the observable collection
        /// </summary>
        /// <value>The refresh command.</value>
        public ICommand RefreshCommand
        {
            get
            {
                return new Command((obj) => {
                    if (FactsSourceData != null)
                    {
                        isAlreadySortAsc = false;
                        ListBindingSourceData = new ObservableCollection<Row>(FactsSourceData.rows.ToList());
                    }
                });
            }
        }

        /// <summary>
        /// Initializes the page.
        /// </summary>
        public async void InitializePage()
        {
            try
            {
                if (!UtilityClasses.isNetWorkAvailable())
                {
                    UtilityClasses.CallDisplayAlert("Mobile Data is Turned Off", "Turn on mobile data or use Wi-Fi to access data.", Page, true, "Network", "Setting", "OK");
                }
                else
                {
                    ServiceResponse<string> getnewsdata = new ServiceResponse<String>();
                  
                    await Task.Factory.StartNew(() =>
                    {
                        DependencyService.Get<IProgressbar>().Show("");
                        getnewsdata = ServiceManager.GenericRestCallUsingHttpClient<string, string>("https://dl.dropboxusercontent.com/s/2iodh4vg0eortkl/facts.json", HttpMethod.Get, "");
                    });

                    DependencyService.Get<IProgressbar>().Hide();
                    if (getnewsdata != null)
                    {
                        if (getnewsdata.Message == "")
                        {
                            var alertResult = await App.Current.MainPage.DisplayAlert("Alert", getnewsdata.Message, null, "OK");
                            if (!alertResult)
                            {


                            }
                        }
                        else
                        {
                            var saveData = JsonConvert.DeserializeObject<FactsModel>(getnewsdata.Data);
                            if (saveData != null)
                            {

                                FactsSourceData = saveData;

                                if (FactsSourceData.rows.Count > 0)
                                {
                                    ListTitle = FactsSourceData.title;
                                    Page.listviewFacts.ItemTemplate = new DataTemplate(typeof(FactsListViewCell));
                                    ListBindingSourceData = FactsSourceData.rows;

                                }

                            }

                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }
        }
        /// <summary>
        /// GET and SET The Title Of the Web API.
        /// </summary>
        private String _ListTitle ="";
        public String ListTitle
        {
            get
            {
                return _ListTitle;
            }
            set
            {
                _ListTitle = value;
                OnPropertyChanged("ListTitle");
            }
        }
       
        /// <summary>
        /// Get and Set the Data from Web API to listview
        /// </summary>
        private ObservableCollection<Row> _listBindingSourceData = new ObservableCollection<Row>();
        public ObservableCollection<Row> ListBindingSourceData
        {
            get
            {
                return _listBindingSourceData;
            }
            set
            {
                _listBindingSourceData = value;
                OnPropertyChanged("ListBindingSourceData");
            }
        }
        private FactsModel _FactsSourceData = new FactsModel();
        public FactsModel FactsSourceData
        {
            get
            {
                return _FactsSourceData;
            }
            set
            {
                _FactsSourceData = value;
                OnPropertyChanged("FactsSourceData");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
