using System;
using BigTed;
using WiproTask.iOS;
using Xamarin.Forms;

[assembly: Dependency(typeof(LoadProgressbar))]
namespace WiproTask.iOS
{
    public class LoadProgressbar: IProgressbar
    {
        public LoadProgressbar()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                BTProgressHUD.ForceiOS6LookAndFeel = true;
            });
        }

        public void Hide()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                BTProgressHUD.Dismiss();
            });
        }

        public void Show(string message = "Loading")
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                BTProgressHUD.Show(maskType: ProgressHUD.MaskType.Gradient);
            });
        }
    }
}
