namespace MyWebServer.LoginFormApplication.Controllers
{
    using MyWebServer.Infrastructure;

    public abstract class BaseController : Controller
    {
        protected override string ApplicationDirectory => "LoginFormApplication";
    }
}