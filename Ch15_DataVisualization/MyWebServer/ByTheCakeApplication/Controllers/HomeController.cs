namespace MyWebServer.ByTheCakeApplication.Controllers
{
    using Server.Http.Contracts;

    public class HomeController : BaseController
    {

        public IHttpResponse Index()
        {
            return this.FileViewResponse(@"Home\index");
        }

        public IHttpResponse About()
        {
            return this.FileViewResponse(@"Home\about");
        }

    }
}