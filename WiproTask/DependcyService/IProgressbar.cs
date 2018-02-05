using System;
namespace WiproTask
{
    public interface IProgressbar
    {
        void Show(string message = "Loading");

        void Hide();
    }
}
