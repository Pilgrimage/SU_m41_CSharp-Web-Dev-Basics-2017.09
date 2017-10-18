namespace MyWebServer.Server.Http.Response
{
    using Server.Enums;

    public class BadRequestResponse : HttpResponse
    {
        public BadRequestResponse()
        {
            this.StatusCode = HttpStatusCode.BadRequest;

        }
    }
}