using System;
using WiproTask.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(SettingsServiceAndroid))]
namespace WiproTask.Droid
{
    public class SettingsServiceAndroid: ISettingsService
    {
        public void OpenSettings()
        {
            Xamarin.Forms.Forms.Context.StartActivity(new Android.Content.Intent(Android.Provider.Settings.ActionLocat‌​ionSourceSettings));
        }
    }
}