using System;
namespace WiproTask
{
    public class ServiceResponse<T> : ResponseBase
    {
        //Generic Variable to getting Data
        public T Data { get; set; }
    }

}
