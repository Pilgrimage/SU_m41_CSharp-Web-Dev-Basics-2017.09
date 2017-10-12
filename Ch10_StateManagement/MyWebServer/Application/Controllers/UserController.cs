namespace MyWebServer.Application.Controllers
{
    using MyWebServer.Application.Views;
    using MyWebServer.Server;
    using MyWebServer.Server.Enums;
    using MyWebServer.Server.Http.Contracts;
    using MyWebServer.Server.Http.Response;

    public class UserController
    {


        public IHttpResponse RegisterGet()
        {
            return new ViewResponse(HttpStatusCode.OK, new RegisterView());
        }


        public IHttpResponse RegisterPost(string name)
        {
            return new RedirectResponse($"/user/{name}");
        }


        public IHttpResponse Details(string name)
        {
            Model model = new Model { ["name"] = name };
            return new ViewResponse(HttpStatusCode.OK, new UserDetailsView(model));
        }

    }
}