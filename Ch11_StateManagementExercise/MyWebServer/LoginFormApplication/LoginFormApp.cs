namespace MyWebServer.LoginFormApplication
{
    using LoginFormApplication.Controllers;
    using Server.Contracts;
    using Server.Routing.Contracts;

    public class LoginFormApp : IApplication
    {
        public void Start(IAppRouteConfig appRouteConfig)
        {
            appRouteConfig
                .Get("/", request => new LoginFormController().Login());

            appRouteConfig
                .Get("/login", request => new LoginFormController().Login());

            appRouteConfig
                .Post("/", request => new LoginFormController().Login(request));

            appRouteConfig
                .Post("/login", request => new LoginFormController().Login(request));

        }
    }
}