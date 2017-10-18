namespace MyWebServer.CalculatorApplication.Controllers
{
    using MyWebServer.Infrastructure;

    public abstract class BaseController : Controller
    {
        protected override string ApplicationDirectory => "CalculatorApplication";
    }
}