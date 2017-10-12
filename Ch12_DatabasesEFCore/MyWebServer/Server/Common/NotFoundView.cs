namespace MyWebServer.Server.Common
{
    using Server.Contracts;

    public class NotFoundView : IView
    {
        public string View() => "<body><h1>404 This page does not exist.</h1></body>";
    }
}