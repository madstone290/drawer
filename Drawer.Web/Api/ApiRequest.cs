namespace Drawer.Web.Api
{
    public class ApiRequest<TResponseData> 
    {
        public HttpRequestMessage Message { get; }

        public ApiRequest(HttpRequestMessage request)
        {
            Message = request;
        }

        public ApiRequest(HttpMethod method, string uri)
        {
            Message = new HttpRequestMessage(method, uri);
        }

        public ApiRequest(HttpMethod method, string uri, object content)
        {
            Message = new HttpRequestMessage(method, uri)
            {
                Content = JsonContent.Create(content)
            };
        }
    }

    public class ApiRequest : ApiRequest<Unit>
    {
        public ApiRequest(HttpRequestMessage request) : base(request)
        {
        }

        public ApiRequest(HttpMethod method, string uri) : base(method, uri)
        {
        }

        public ApiRequest(HttpMethod method, string uri, object content) : base(method, uri, content)
        {
        }
    }

}
