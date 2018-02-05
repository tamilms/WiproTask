using System;
using AndroidHUD;
using WiproTask.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(LoadProgressbar))]
namespace WiproTask.Droid
{
    public class LoadProgressbar: IProgressbar
    {
        public LoadProgressbar()
        {
        }

        public void Hide()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                AndHUD.Shared.Dismiss(Forms.Context);
            });
        }

        public void Show(string message = "Loading")
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                AndHUD.Shared.Show(Forms.Context, maskType: MaskType.Black);
            });
        }
    }
}
