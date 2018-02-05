using Xamarin.Forms;

namespace WiproTask
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage( new FactsPage()){BarBackgroundColor=Color.SlateBlue,Title="Wipro Task",BarTextColor=Color.White,Tint=Color.White};
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
