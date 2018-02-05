using System;
using Foundation;
using UIKit;
using WiproTask.iOS;
using Xamarin.Forms;

[assembly: Dependency(typeof(SettingsServiceIOS))]
namespace WiproTask.iOS
{
    public class SettingsServiceIOS: ISettingsService
    {
        public void OpenSettings()
        {
            //UIKit.UIApplication.SharedApplication.OpenUrl(new Foundation.NSUrl(UIKit.UIApplication.OpenSettingsUrlString));

            var WiFiURL = new NSUrl("prefs:root=WIFI");
            if (UIApplication.SharedApplication.CanOpenUrl(WiFiURL))
            {   //Pre iOS 10
                UIApplication.SharedApplication.OpenUrl(WiFiURL);
            }
            else
            {   //iOS 10
                UIApplication.SharedApplication.OpenUrl(new NSUrl("App-Prefs:root=WIFI"));
            }
        }
    }
}