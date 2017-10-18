namespace MyWebServer.GameStoreApplication.Controllers
{
    using GameStoreApplication.Services.Contracts;
    using GameStoreApplication.Services;
    using GameStoreApplication.ViewModels.Account;
    using Server.Http;
    using Server.Http.Contracts;

    public class AccountController : BaseController
    {
        private const string RegisterView = @"account\register";
        private const string LoginView = @"account\login";

        private readonly IUserService users;

        public AccountController(IHttpRequest request)
            : base(request)
        {
            this.users = new UserService();
        }

        // register Get
        public IHttpResponse Register() => this.FileViewResponse(RegisterView);

        // register Post
        public IHttpResponse Register(RegisterViewModel model)
        {
            if (!this.ValidateModel(model))
            {
                //return this.FileViewResponse(RegisterView);   // this is the same as next line
                return this.Register();
            }

            bool success = this.users.Create(model.Email, model.FullName, model.Password);

            if (!success)
            {
                this.ShowError("E-mail is taken.");
                return this.Register();
            }
            else
            {
                this.LoginUser(model.Email);
                return this.RedirectResponse(HomePath);
            }
        }


        // login Get
        public IHttpResponse Login() => this.FileViewResponse(LoginView);

        // login Post
        public IHttpResponse Login(LoginViewModel model)
        {
            if (!this.ValidateModel(model))
            {
                return this.Login();
            }

            bool success = this.users.Find(model.Email, model.Password);

            if (!success)
            {
                this.ShowError("Invalid user details.");

                return this.Login();
            }
            else
            {
                this.LoginUser(model.Email);

                return this.RedirectResponse(HomePath);
            }
        }

        // open a session for the user (by email, because it's unique)
        private void LoginUser(string email)
        {
            this.Request.Session.Add(SessionStore.CurrentUserKey, email);
        }


        // logout
        public IHttpResponse Logout()
        {
            this.Request.Session.Clear();

            return this.RedirectResponse(HomePath);
        }

    }
}