namespace MyWebServer.Application.Views
{
    using Server.Contracts;

    public class AboutView : IView
    {
        public string View() => "<body><h2>I am the result of anguish!</h2></body>";

    }
}