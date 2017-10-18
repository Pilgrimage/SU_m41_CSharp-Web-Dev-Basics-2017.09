namespace MyWebServer.ByTheCakeApplication.Controllers
{
    using System;
    using ByTheCakeApplication.ViewModels;
    using ByTheCakeApplication.Services;
    using ByTheCakeApplication.ViewModels.Account;
    using Server.Http;
    using Server.Http.Response;
    using Server.Http.Contracts;

    public class AccountController : BaseController
    {

        private const string RegisterView = @"account\register";
        private const string LoginView = @"account\login";

        private readonly IUserService users;

        public AccountController()
        {
            this.users = new UserService();
        }

        public IHttpResponse Register()
        {
            this.SetDefaultViewData();
            return this.FileViewResponse(RegisterView);
        }

        public IHttpResponse Register( IHttpRequest request, RegisterUserViewModel model)
        {
            this.SetDefaultViewData();

            if (model.Username.Length < 3
                || model.Password.Length < 3
                || model.ConfirmPassword != model.Password)
            {
                this.ShowError("Invalid user details");

                return this.FileViewResponse(RegisterView);
            }

            bool success = this.users.Create(model.Username, model.Password);

            if (success)
            {
                this.LoginUser(request, model.Username);

                return new RedirectResponse("/");
            }
            else
            {
                this.ShowError("This username is taken");

                return this.FileViewResponse(RegisterView);
            }
        }

        public IHttpResponse Login()
        {
            this.SetDefaultViewData();
            return this.FileViewResponse(LoginView);
        }

        public IHttpResponse Login(IHttpRequest request, LoginViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Username)
                || string.IsNullOrWhiteSpace(model.Password))
            {
                this.ShowError("You have empty fields");

                return this.FileViewResponse(LoginView);
            }

            bool success = this.users.Find(model.Username, model.Password);

            if (success)
            {
                this.LoginUser(request, model.Username);

                return new RedirectResponse("/");
            }
            else
            {
                this.ShowError("Invalid user details");

                return this.FileViewResponse(LoginView);
            }
        }


        public IHttpResponse Profile(IHttpRequest request)
        {
            if (!request.Session.Contains(SessionStore.CurrentUserKey))
            {
                throw new InvalidOperationException("There is no logged in user.");
            }

            string username = request.Session.Get<string>(SessionStore.CurrentUserKey);

            ProfileViewModel profile = this.users.Profile(username);

            if (profile == null)
            {
                throw new InvalidOperationException($"The user {username} could not be found in the database.");
            }

            this.ViewData["username"] = profile.Username;
            this.ViewData["registrationDate"] = profile.RegistrationDate.ToShortDateString();
            this.ViewData["totalOrders"] = profile.TotalOrders.ToString();

            return this.FileViewResponse(@"account\profile");
        }

        public IHttpResponse Logout(IHttpRequest request)
        {
            request.Session.Clear();

            return new RedirectResponse("/login");
        }

        private void SetDefaultViewData() => this.ViewData["authDisplay"] = "none";


        private void LoginUser(IHttpRequest request, string username)
        {
            request.Session.Add(SessionStore.CurrentUserKey, username);
            request.Session.Add(ShoppingCart.SessionKey, new ShoppingCart());
        }




    }
}