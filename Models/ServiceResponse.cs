namespace banka_net_core.Models
{
    public class ServiceResponse<T>
    {
        public ServiceResponseCodes Status { get; set; }
        public string Message { get; set; }
        public T Data {get; set; }
    }
}