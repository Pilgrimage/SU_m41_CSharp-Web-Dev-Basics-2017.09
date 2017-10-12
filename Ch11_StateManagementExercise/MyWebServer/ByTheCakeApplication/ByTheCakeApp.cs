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

            // State management exercise

            appRouteConfig
                .Get(
                    "/login",
                    request => new AccountController().Login());

            appRouteConfig
                .Post(
                    "/login",
                    request => new AccountController().Login(request));

            appRouteConfig
                .Post(
                    "/logout",
                    request => new AccountController().Logout(request));  

            appRouteConfig
                .Get(
                    "/shopping/add/{(?<id>[0-9]+)}",
                    request => new ShoppingController().AddToCart(request));

            appRouteConfig
                .Get(
                    "/cart",
                    request => new ShoppingController().ShowCart(request));

            appRouteConfig
                .Post(
                    "/shopping/finish-order",
                    request => new ShoppingController().FinishOrder(request));


        }
    }
}