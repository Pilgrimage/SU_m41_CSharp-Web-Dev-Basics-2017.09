namespace MyWebServer.Server.Http
{
    using Contracts;
    using Server.Common;

    public class HttpContext : IHttpContext
    {
        private readonly IHttpRequest request;

        public HttpContext(string requestText)
        {
            CommonValidator.ThrowIfNullOrEmpty(requestText, nameof(requestText));

            this.request = new HttpRequest(requestText);
        }

        public HttpContext(IHttpRequest request)
        {
            CommonValidator.ThrowIfNull(request, nameof(request));

            this.request = request;
        }
        
        public IHttpRequest Request => this.request;

    }
}