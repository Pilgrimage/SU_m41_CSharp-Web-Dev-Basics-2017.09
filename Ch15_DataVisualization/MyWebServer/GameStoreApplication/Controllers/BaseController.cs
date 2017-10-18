namespace MyWebServer.GameStoreApplication.Controllers
{
    using GameStoreApplication.Services.Contracts;
    using GameStoreApplication.Common;
    using GameStoreApplication.Services;
    using MyWebServer.Infrastructure;
    using Server.Http;
    using Server.Http.Contracts;

    public abstract class BaseController : Controller
    {
        protected const string HomePath = "/";

        private readonly IUserService users;

        protected BaseController(IHttpRequest request)
        {
            this.Request = request;
            this.Authentication = new Authentication(false, false);

            this.users = new UserService();

            this.ApplyAuthentication();
        }

        protected IHttpRequest Request { get; private set; }

        protected Authentication Authentication { get; private set; }

        // set main directory of the application
        protected override string ApplicationDirectory => "GameStoreApplication";


        private void ApplyAuthentication()
        {
            // determine navbar for visualization
            string anonymousDisplay = "flex";
            string authDisplay = "none";
            string adminDisplay = "none";
            string adminButtons = "hidden";

            // bool isAuthenticated = this.Request.Session.Contains(SessionStore.CurrentUserKey);

            string authenticatedUserEmail = this.Request
                .Session
                .Get<string>(SessionStore.CurrentUserKey);

            if (authenticatedUserEmail != null)
            {
                // is Logged!
                anonymousDisplay = "none";
                authDisplay = "flex";

                bool isAdmin = this.users.IsAdmin(authenticatedUserEmail);

                if (isAdmin)
                {
                    // is Admin!
                    adminDisplay = "flex";
                    adminButtons = " ";
                }

                this.Authentication = new Authentication(true, isAdmin);
            }

            this.ViewData["anonymousDisplay"] = anonymousDisplay;
            this.ViewData["authDisplay"] = authDisplay;
            this.ViewData["adminDisplay"] = adminDisplay;
            this.ViewData["adminButtons"] = adminButtons;

        }

    }
}