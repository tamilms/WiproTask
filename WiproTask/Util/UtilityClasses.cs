using System;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace WiproTask
{
    public static class UtilityClasses
    {
        /// <summary>
        /// Is the net work is available?
        /// </summary>
        /// <returns><c>true</c>, if net work available was ised, <c>false</c> otherwise.</returns>
        public static bool isNetWorkAvailable()
        {
            return CrossConnectivity.Current.IsConnected;

        }

        /// <summary>
        /// Calls the display alert.
        /// </summary>
        /// <param name="Title">Title.</param>
        /// <param name="Message">Message.</param>
        /// <param name="Page">Page.</param>
        /// <param name="isActionRequired">If set to <c>true</c> is action required for open Network setting.</param>
        /// <param name="typeOfAction">Type of action.</param>
        /// <param name="button1">.</param>
        /// <param name="button2">Button2.</param>
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
