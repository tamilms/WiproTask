using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace WiproTask
{
    public class FactsPageViewModel:INotifyPropertyChanged
    {
        FactsPage mPage;
       
        //Set Default value of List collection is Ascending order
        bool isAlreadySortAsc = false;
        public FactsPageViewModel(FactsPage page)
        {
            mPage = page;

            //Load data from server webapi and assign to listview
            InitializePage();

            mPage.listviewFacts.ItemTapped+=delegate {
                
                mPage.listviewFacts.SelectedItem = null;
            };
        }



        /// SortCommand is Sort button click event for Sorting result based on Alphabetic Ascending/Descending order
        public ICommand SortCommand{
            get{
                return new Command((obj) => {
                    //by default first click we can sort the listview in Ascending order
                    if (isAlreadySortAsc == false)
                    {
                        if (ListBindingSourceData != null)
                        {
                            isAlreadySortAsc = true;

                            ListBindingSourceData = new ObservableCollection<Row>(ListBindingSourceData.OrderBy(x => x.title).ToList());
                        }
                    }
                    else //if listview already ascending then changed into descending
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

      
        //RefreshCommand is Refersh button click event for reload all items back into the observable collection
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

        //Extra Credit point load data from  local file.
        //Here I am saving data inot .txt file and read the DataFile.txt file and loaded into listview
        public ICommand LoadDataFromLocalFileCommand
        {
            get
            {
                return new Command((obj) => {
                    LoadDataFromLocalFile();
                });
            }
        }

       
        /// Loads the data from local .txt(DataFile.txt) file from PCL projects.
        public async void LoadDataFromLocalFile()
        {
            try
            {
                FactsSourceData = null;
                ListBindingSourceData = null;
                isAlreadySortAsc = false;

                //Load Application aaembly of the current page 
                var assembly = typeof(FactsPage).GetTypeInfo().Assembly;

                //once we can acess application assembly then we can able to access local file
                //Here our local application file is DataFile.txt. No we can get data from local file and load it into listview
                Stream stream = assembly.GetManifestResourceStream("WiproTask.DataFile.txt");

                string text = "";

                //In DataFile.txt file the data is like JSON format so we can read JSON data using Newtonsoft library from file
                using (var reader = new System.IO.StreamReader(stream))
                {
                    text = reader.ReadToEnd();
                }

                //The deserialize the JSON string into FactsModel collection
                var saveData = JsonConvert.DeserializeObject<FactsModel>(text); 
                if (saveData != null)
                {

                    FactsSourceData = saveData;

                    //Assign Page title from local file title 
                    if (FactsSourceData.rows.Count > 0)
                    {
                        ListTitle = FactsSourceData.title; 

                        //Convert FactsModel into listview datasource collection 
                        ListBindingSourceData =new ObservableCollection<Row>( FactsSourceData.rows.OrderByDescending(x=>x.title).ToList());

                    }

                }
            }
            catch (Exception ex)
            {

            }
        }


        //Initializes the page for getting Data 
        public async void InitializePage()
        {
            try
            {
                //Check if network is available or not
                if (!UtilityClasses.isNetWorkAvailable())
                {
                    //if network is not available the ask to open setting page for enable network
                    UtilityClasses.CallDisplayAlert("Mobile Data is Turned Off", "Turn on mobile data or use Wi-Fi to access data.", mPage, true, "Network", "Setting", "OK");
                }
                else //if network is available then call web api for getting data
                {
                    ServiceResponse<string> getnewsdata = new ServiceResponse<String>();

                    //Call webapi for getting data using Generic RestCall Using HttpClient
                    await Task.Factory.StartNew(() =>
                    {
                        //Show progressbar when calling webapi for avoiding ANR message
                        DependencyService.Get<IProgressbar>().Show("");
                        getnewsdata = ServiceManager.GenericRestCallUsingHttpClient<string, string>("https://dl.dropboxusercontent.com/s/2iodh4vg0eortkl/facts.json", HttpMethod.Get, "");
                    });
                    //Once received datat from server hide progressbar
                    DependencyService.Get<IProgressbar>().Hide();
                    //get response string from server and check whether it is null or not
                    if (getnewsdata != null)
                    {
                        //if getting error from web APii then give the alert regarding the Isue
                        if (getnewsdata.Message == "")
                        {
                            var alertResult = await App.Current.MainPage.DisplayAlert("Alert", getnewsdata.Message, null, "OK");
                            if (!alertResult)
                            {


                            }
                        }
                        else
                        {
                            //once sucessfully received the data from server then convert the response JSON string to collection by using Newtonsoft library
                            var saveData = JsonConvert.DeserializeObject<FactsModel>(getnewsdata.Data);
                            if (saveData != null) //if converted json string is not null then assign to ObservableCollection for updating listview with data
                            {
                                //
                                FactsSourceData = saveData;

                                if (FactsSourceData.rows.Count > 0)
                                {
                                    //Assign the resposne title to the page title
                                    ListTitle = FactsSourceData.title;
                                    //assign listview viewcell  template for showing data into according to our design
                                    mPage.listviewFacts.ItemTemplate = new DataTemplate(typeof(FactsListViewCell));
                                    // once received the data like collection and then assign into listview datasource which is MVVM binding we already assigned variable name as ListBindingSourceData
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

        //Set Page Title from web api
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
       
        //Set Listview Datasource which is received from web api/ load from local file in PCL project
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

        //MVVM pattern for calling property changed event whenever property value changed in the viewmodel
        public virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
