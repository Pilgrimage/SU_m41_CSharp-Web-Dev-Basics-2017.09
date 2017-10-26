namespace SimpleMvc.App.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using SimpleMvc.App.BindingModels;
    using SimpleMvc.Data;
    using SimpleMvc.Domain;
    using SimpleMvc.Framework.Attributes.Methods;
    using SimpleMvc.Framework.Contracts;
    using SimpleMvc.Framework.Controllers;
    using WebServer.Exceptions;

    public class UsersController : Controller
    {
        private const string IndexPage = "/home/index";
        private const string LoginPage = "/users/login";

        [HttpGet]
        public IActionResult Register()
        {
            if (this.User.IsAuthenticated)
            {
                return this.Redirect(IndexPage);
            }

            return this.View();
        }

        [HttpPost]
        public IActionResult Register(RegisterUserBindingModel registerUserBinding)
        {
            if (!this.IsValidModel(registerUserBinding))
            {
                return this.View();
            }

            var user = new User()
            {
                Username = registerUserBinding.Username,
                Password = registerUserBinding.Password
            };

            using (var context = new NotesDbContext())
            {
                context.Users.Add(user);
                context.SaveChanges();
            }

            return this.Redirect(IndexPage);
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (this.User.IsAuthenticated)
            {
                return this.Redirect(IndexPage);
            }

            return this.View();
        }

        [HttpPost]
        public IActionResult Login(LoginUserBindingModel model)
        {
            if (!this.IsValidModel(model))
            {
                return this.View();
            }

            using (var context = new NotesDbContext())
            {
                var foundUser = context.Users.FirstOrDefault(u => u.Username == model.Username);

                if (foundUser == null)
                {
                    return this.Redirect(IndexPage);
                }

                context.SaveChanges();
                this.SignIn(foundUser.Username);
            }

            return this.Redirect(IndexPage);
        }

        [HttpGet]
        public IActionResult All()
        {
            if (!this.User.IsAuthenticated)
            {
                return this.Redirect(LoginPage);
            }

            Dictionary<int, string> users = new Dictionary<int, string>();

            using (var db = new NotesDbContext())
            {
                users = db.Users.ToDictionary(u => u.Id, u => u.Username);
            }

            this.ViewModel["users"] =
                users.Any() ? string.Join(string.Empty, users
                .Select(u => $"<li><a href=\"/users/profile?id={u.Key}\">{u.Value}</a></li>"))
                : string.Empty;

            return this.View();
        }

        [HttpGet]
        public IActionResult Profile(int id)
        {
            using (var db = new NotesDbContext())
            {
                var user = db.Users
                    .Include(u => u.Notes)
                    .FirstOrDefault(u => u.Id == id);

                this.ViewModel["userId"] = user.Id.ToString();
                this.ViewModel["username"] = user.Username;

                this.ViewModel["notes"] =
                    string.Join(string.Empty,
                        user.Notes.Select(n =>
                        $"<li><strong>{n.Title}</strong> - {n.Content}</li>"
                        )
                    );

                return this.View();
            }
        }

        [HttpPost]
        public IActionResult Profile(AddNoteBindingModel model)
        {
            using (var db = new NotesDbContext())
            {
                User user = db.Users.FirstOrDefault(u=>u.Id==model.UserId);

                Note note = new Note()
                {
                    Title = model.Title,
                    Content = model.Content
                };

                user.Notes.Add(note);

                db.SaveChanges();
            }

            return this.Profile(model.UserId);
        }

        [HttpGet]
        public IActionResult Logout()
        {
            this.SignOut();

            return this.Redirect(IndexPage);
        }
    }
}