using System;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace WiproTask
{
    public static class UtilityClasses
    {
        /// <summary>
        /// Check if net work is available before calling to web service?
        /// </summary>
        public static bool isNetWorkAvailable()
        {
            return CrossConnectivity.Current.IsConnected;

        }

      /// <summary>
      /// Whenever the network is not avaible then ask user to enbale netwrok with options  
      /// </summary>
        public static async void CallDisplayAlert(string Title, String Message, ContentPage Page, bool isActionRequired, string typeOfAction = "", string button1 = "", string button2 = "")
        {
            if (isActionRequired == false)
            {
                (Page as ContentPage).DisplayAlert(Title, Message, button1);
            }
            else
            {
                switch (typeOfAction)
                {
                    case "Network":
                        {
                            Device.BeginInvokeOnMainThread(async () => {
                                bool ans = await (Page as ContentPage).DisplayAlert(Title, Message, button2, button1);
                                if (ans == true)
                                {

                                }
                                else
                                {
                                    DependencyService.Get<ISettingsService>().OpenSettings();
                                }
                            });
                           
                        }
                        break;

                }
            }
        }
    }
}
