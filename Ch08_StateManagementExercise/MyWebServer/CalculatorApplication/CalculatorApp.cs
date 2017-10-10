namespace MyWebServer.CalculatorApplication
{
    using CalculatorApplication.Controllers;
    using Server.Contracts;
    using Server.Routing.Contracts;

    public class CalculatorApp : IApplication
    {
        public void Start(IAppRouteConfig appRouteConfig)
        {
            appRouteConfig
                .Get("/", request => new CalculatorController().Calculate());

            appRouteConfig
                .Post(
                    "/",
                    request => new CalculatorController().Calculate(request));
        }
    }
}