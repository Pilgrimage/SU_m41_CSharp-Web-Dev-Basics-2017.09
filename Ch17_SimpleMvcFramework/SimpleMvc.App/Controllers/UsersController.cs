namespace SimpleMvc.App.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using SimpleMvc.App.BindingModels;
    using SimpleMvc.App.ViewModels;
    using SimpleMvc.Data;
    using SimpleMvc.Domain;
    using SimpleMvc.Framework.Attributes.Methods;
    using SimpleMvc.Framework.Contracts;
    using SimpleMvc.Framework.Contracts.Generic;
    using SimpleMvc.Framework.Controllers;
    using WebServer.Exceptions;

    public class UsersController : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Register(RegisterUserBindingModel model)
        {
            using (var db = new NotesDbContext())
            {
                if (db.Users.Any(u => u.Username == model.Username))
                {
                    return this.Register();
                }

                User user = new User
                {
                    Username = model.Username,
                    Password = model.Password
                };

                db.Users.Add(user);
                db.SaveChanges();
            }

            return View();
        }


        [HttpGet]
        public IActionResult<AllUsernamesViewModel> All()
        {
            Dictionary<string, int> usernamesAndIds = new Dictionary<string, int>();

            using (var db = new NotesDbContext())
            {
                usernamesAndIds = db
                    .Users
                    .Select(u => new KeyValuePair<string, int>(u.Username, u.Id))
                    .ToDictionary(d => d.Key, d => d.Value);
            }

            AllUsernamesViewModel viewModel = new AllUsernamesViewModel()
            {
                UsernamesAndIds = usernamesAndIds
            };

            return this.View(viewModel);
        }


        [HttpGet]
        public IActionResult<UserProfileViewModel> Profile(int id)
        {
            User user = null;

            using (var db = new NotesDbContext())
            {
                user = db
                    .Users
                    .Include(u => u.Notes)
                    .FirstOrDefault(u => u.Id == id);
            }

            var viewModel = new UserProfileViewModel()
            {
                OwnerId = user.Id,
                Username = user.Username,
                Notes = user.Notes
                    .Select(n =>
                        new NoteViewModel()
                        {
                            Title = n.Title,
                            Content = n.Content
                        })
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult<UserProfileViewModel> Profile(AddNoteBindingModel model)
        {
            using (var db = new NotesDbContext())
            {
                User user = db
                    .Users
                    .Include(u => u.Notes)
                    .FirstOrDefault(u => u.Id == model.OwnerId);

                Note note = new Note
                {
                    Title = model.Title,
                    Content = model.Content
                };

                user.Notes.Add(note);
                db.SaveChanges();
            };

            return this.Profile(model.OwnerId);
        }

    }
}