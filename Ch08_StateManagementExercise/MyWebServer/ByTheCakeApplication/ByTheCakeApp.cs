namespace MyWebServer.ByTheCakeApplication
{
    using ByTheCakeApplication.Controllers;
    using Server.Contracts;
    using Server.Routing.Contracts;

    public class ByTheCakeApp : IApplication
    {
        public void Start(IAppRouteConfig appRouteConfig)
        {
            appRouteConfig
                .Get("/", request => new HomeController().Index());

            appRouteConfig
                .Get("/about", request => new HomeController().About());

            
            appRouteConfig
                .Get("/add", request => new CakesController().Add());

            appRouteConfig
                .Post("/add", request => new CakesController().Add(request.FormData["name"], request.FormData["price"]));


            appRouteConfig
                .Get("/search", request => new CakesController().Search(request));

        }
    }
}