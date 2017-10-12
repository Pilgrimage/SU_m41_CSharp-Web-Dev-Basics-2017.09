namespace MyWebServer.Application.Views
{
    using Server.Contracts;

    public class HomeIndexView : IView
    {
        public string View() => "<body><h1>Welcome!!!</h1></body>";
    }
}