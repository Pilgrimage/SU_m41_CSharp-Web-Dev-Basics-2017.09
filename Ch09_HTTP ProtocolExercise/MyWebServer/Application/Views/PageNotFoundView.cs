namespace MyWebServer.Application.Views
{
    using Server.Contracts;

    public class PageNotFoundView : IView
    {
        public string View() => "<body><h1>Page Not Found.</h1></body>";

    }
}