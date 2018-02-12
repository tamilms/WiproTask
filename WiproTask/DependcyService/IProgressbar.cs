using System;
namespace WiproTask
{
    public interface IProgressbar
    {
        //For Showing progressbar
        void Show(string message = "Loading");

        //For Hiding progressbar
        void Hide();
    }
}
