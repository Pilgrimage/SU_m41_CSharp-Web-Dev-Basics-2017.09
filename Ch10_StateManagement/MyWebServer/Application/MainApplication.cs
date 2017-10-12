namespace MyWebServer.Application
{
    using Controllers;
    using Server.Contracts;
    using Server.Routing.Contracts;

    public class MainApplication : IApplication
    {

        public void Start(IAppRouteConfig appRouteConfig)
        {
            // GET "/"
            appRouteConfig.Get(
                "/", 
                request => new HomeController().Index());

            // GET "/index"
            appRouteConfig.Get(
                "/index", 
                request => new HomeController().Index());

            // GET "/about"
            appRouteConfig.Get(
                "/about", 
                request => new HomeController().About());


            // POST "/register"
            appRouteConfig.Post(
                "/register",
                request => new UserController().RegisterPost(request.FormData["name"]));

            // GET "/register"
            appRouteConfig.Get(
                "/register",
                request => new UserController().RegisterGet());

            // GET details after POST /register
            appRouteConfig.Get(
                "/user/{(?<name>[a-zA-Z]+)}",
                request => new UserController().Details(request.UrlParameters["name"]));


            // GET /testsession
            appRouteConfig.Get(
                "/testsession",
                request => new HomeController().SessionTest(request));

        }
    }
}