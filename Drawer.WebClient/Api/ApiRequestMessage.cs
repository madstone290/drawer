namespace Drawer.WebClient.Api
{
    public class ApiRequestMessage<TResponseData> 
    {
        public HttpRequestMessage Request { get; }

        public ApiRequestMessage(HttpRequestMessage request)
        {
            Request = request;
        }
    }

    public class ApiRequestMessage : ApiRequestMessage<Unit>
    {
        public ApiRequestMessage(HttpRequestMessage request) : base(request)
        {
        }
    }

}
