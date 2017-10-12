namespace MyWebServer.ByTheCakeApplication.Controllers
{
    using ByTheCakeApplication.Infrastructure;
    using ByTheCakeApplication.Models;
    using Server.Http;
    using Server.Http.Response;
    using Server.Http.Contracts;

    public class AccountController : Controller
    {
        public IHttpResponse Login()
        {
            this.ViewData["showError"] = "none";
            this.ViewData["authDisplay"] = "none";

            return this.FileViewResponse(@"Account\login");
        }

        public IHttpResponse Login(IHttpRequest request)
        {
            const string formNameKey = "name";
            const string formPasswordKey = "password";

            if (!request.FormData.ContainsKey(formNameKey) ||
                !request.FormData.ContainsKey(formPasswordKey))
            {
                this.ViewData["error"] = "You have missing or empty fields";
                this.ViewData["showError"] = "block";

                return this.FileViewResponse(@"account\login");
                //return new BadRequestResponse();
            }

            string name = request.FormData[formNameKey];
            string password = request.FormData[formPasswordKey];

            if (string.IsNullOrWhiteSpace(name) || 
                string.IsNullOrWhiteSpace(password))
            {
                this.ViewData["error"] = "You have empty fields";
                this.ViewData["showError"] = "block";

                return this.FileViewResponse(@"account\login");
            }

            // we save only username of the user
            request.Session.Add(SessionStore.CurrentUserKey, name);
            request.Session.Add(ShoppingCart.SessionKey, new ShoppingCart());

            return new RedirectResponse("/");
        }

        public IHttpResponse Logout(IHttpRequest request)
        {
            request.Session.Clear();

            return new RedirectResponse("/login");
        }

    }
}