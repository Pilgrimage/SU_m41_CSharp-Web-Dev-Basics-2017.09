namespace MyWebServer.GameStoreApplication.Services
{
    using System.Linq;
    using GameStoreApplication.Services.Contracts;
    using GameStoreApplication.Data;
    using GameStoreApplication.Data.Models;

    public class UserService : IUserService
    {
        public bool Create(string email, string name, string password)
        {
            using (GameStoreDbContext db = new GameStoreDbContext())
            {
                if (db.Users.Any(u => u.Email == email))
                {
                    // email must be unique
                    return false;
                }

                // the first registered becomes admin
                bool isAdmin = !db.Users.Any();

                User user = new User
                {
                    Email = email,
                    Name = name,
                    Password = password,
                    IsAdmin = isAdmin
                };

                db.Add(user);
                db.SaveChanges();
            }

            return true;
        }

        public bool Find(string email, string password)
        {
            using (var db = new GameStoreDbContext())
            {
                return db.Users.Any(u => u.Email == email && u.Password == password);
            }
        }

        public bool IsAdmin(string email)
        {
            using (var db = new GameStoreDbContext())
            {
                return db.Users.Any(u => u.Email == email && u.IsAdmin);
            }
        }
    }
}