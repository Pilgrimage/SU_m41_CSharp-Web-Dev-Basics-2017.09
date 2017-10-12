namespace MyWebServer.Server.Http.Response
{
    using System;
    using Server.Common;
    using Server.Enums;

    public class InternalServerErrorResponse : ViewResponse
    {
        public InternalServerErrorResponse(Exception ex)
            : base(HttpStatusCode.InternalServerError, new InternalServerErrorView(ex))
        {
        }
    }
}