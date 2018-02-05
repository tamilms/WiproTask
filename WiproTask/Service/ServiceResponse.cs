using System;
namespace WiproTask
{
    public class ServiceResponse<T> : ResponseBase
    {
        public T Data { get; set; }
    }

}
