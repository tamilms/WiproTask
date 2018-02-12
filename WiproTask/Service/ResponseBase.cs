using System;
namespace WiproTask
{
    /// <summary>
    /// Generic Model for getting status of web api results
    /// </summary>
    public class ResponseBase
    {
        //Check status if web api result is success or failure
        public bool IsSuccess { get; set; }
        //if web api result is false then get the corresponding error message to display for user
        public string Message { get; set; }
    }
}
