namespace MyWebServer.Server.Http.Response
{
    using System.Text;
    using Server.Contracts;
    using Enums;
    using MyWebServer.Server.Exceptions;

    public class ViewResponse : HttpResponse
    {
        private readonly IView view;

        public ViewResponse(HttpStatusCode statusCode, IView view)
        {
            this.ValidateStatusCode(statusCode);

            this.view = view;
            this.StatusCode = statusCode;

            this.Headers.Add(HttpHeader.ContentType, "text/html");
        }


        private void ValidateStatusCode(HttpStatusCode statusCode)
        {
            int statusCodeAsNumber = (int)this.StatusCode;

            if (299<statusCodeAsNumber && statusCodeAsNumber<400)
            {
                throw new InvalidResponseException("View responses need a status code below 300 and above 400 (inclysive).");
            }
        }

        public override string ToString()
        {
            //StringBuilder response = new StringBuilder(base.ToString());
            //int statusCodeAsNumber = (int)this.StatusCode;
            //if (statusCodeAsNumber < 300 || statusCodeAsNumber > 400)
            //{
            //    response.AppendLine(this.view.View());
            //}
            //return response.ToString();

            return $"{base.ToString()}{this.view.View()}";
        }

    }
}