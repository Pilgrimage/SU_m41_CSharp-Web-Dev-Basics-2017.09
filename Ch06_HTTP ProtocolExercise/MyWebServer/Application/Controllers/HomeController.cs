namespace MyWebServer.Application.Controllers
{
    using Server.Enums;
    using Server.Http.Contracts;
    using Server.Http.Response;
    using Application.Views;
    using Server.Http;

    public class HomeController
    {
        // GET /
        public IHttpResponse Index()
        {
            ViewResponse response = new ViewResponse(HttpStatusCode.OK, new HomeIndexView());

            response.Cookies.Add(new HttpCookie("lang", "en"));

            return response;
        }

        // GET /about
        public IHttpResponse About()
        {
            ViewResponse response = new ViewResponse(HttpStatusCode.OK, new AboutView());

            response.Cookies.Add(new HttpCookie("lang", "en"));

            return response;
        }

    }
}