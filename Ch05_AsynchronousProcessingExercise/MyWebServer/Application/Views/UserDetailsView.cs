namespace MyWebServer.Application.Views
{
    using Server;
    using Server.Contracts;

    public class UserDetailsView : IView
    {
        private readonly Model model;

        public UserDetailsView(Model model)
        {
            this.model = model;
        }

        public string View()
        {
            return $"<body><h3>Hello, {model["name"]}!</h3> </br></br><h4><a href=\"/\">Home</a></h4></body>";

        }
    }
}