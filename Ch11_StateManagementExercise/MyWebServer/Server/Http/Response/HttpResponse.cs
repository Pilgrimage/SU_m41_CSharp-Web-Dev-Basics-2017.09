namespace MyWebServer.Server.Http.Response
{
    using System.Text;
    using Enums;
    using Contracts;

    public abstract class HttpResponse : IHttpResponse
    {
        private string statusCodeMessage => this.StatusCode.ToString();
        
        protected HttpResponse()
        {
            this.Headers = new HttpHeaderCollection();
            this.Cookies = new HttpCookieCollection();
        }

        public IHttpHeaderCollection Headers { get; }
        public IHttpCookieCollection Cookies { get; }

        public HttpStatusCode StatusCode { get; protected set; }


        public override string ToString()
        {
            StringBuilder response = new StringBuilder();

            int statusCodeAsNumber = (int)this.StatusCode;

            response.AppendLine($"HTTP/1.1 {statusCodeAsNumber} {this.statusCodeMessage}");
            response.AppendLine(this.Headers.ToString());
            //response.AppendLine();

            return response.ToString();
        }

        public void AddHeader(string key, string value)
        {
            this.Headers.Add(new HttpHeader(key, value));
        }
    }
}