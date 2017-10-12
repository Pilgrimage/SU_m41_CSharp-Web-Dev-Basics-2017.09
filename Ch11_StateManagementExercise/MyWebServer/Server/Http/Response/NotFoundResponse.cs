namespace MyWebServer.Server.Http.Response
{
    using Enums;
    using Server.Common;

    public class NotFoundResponse : ViewResponse
    {
        public NotFoundResponse() : base (HttpStatusCode.NotFound, new NotFoundView())
        {
        }
    }
}