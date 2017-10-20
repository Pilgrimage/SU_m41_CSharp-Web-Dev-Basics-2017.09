namespace MyWebServer.GameStoreApplication
{
    using GameStoreApplication.Data;
    using GameStoreApplication.Controllers;
    using GameStoreApplication.ViewModels.Account;
    using GameStoreApplication.ViewModels.Admin;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Globalization;
    using Server.Contracts;
    using Server.Routing.Contracts;

    public class GameStoreApp : IApplication
    {
        public void InitializeDatabase()
        {
            using (GameStoreDbContext db = new GameStoreDbContext())
            {
                db.Database.Migrate();
            }
        }

        public void Start(IAppRouteConfig appRouteConfig)
        {
            appRouteConfig.AnonymousPaths.Add("/");
            appRouteConfig.AnonymousPaths.Add("/account/register");
            appRouteConfig.AnonymousPaths.Add("/account/login");

            // HOME Controller

            appRouteConfig
                .Get("/", request => new HomeController(request).Index());

            appRouteConfig
                .Get("/home/index", request => new HomeController(request).Index());

            appRouteConfig
                .Get("/games/info/{(?<id>[0-9]+)}", request => new HomeController(request).Details());


            // ACCOUNT Controller

            appRouteConfig
                .Get("/account/register",
                    request => new AccountController(request).Register());

            appRouteConfig
                .Post("/account/register",
                    request => new AccountController(request).Register(
                        new RegisterViewModel
                        {
                            Email = request.FormData["email"],
                            FullName = request.FormData["full-name"],
                            Password = request.FormData["password"],
                            ConfirmPassword = request.FormData["confirm-password"]
                        }));


            appRouteConfig
                .Get("/account/login",
                    request => new AccountController(request).Login());

            appRouteConfig
                .Post("/account/login",
                    request => new AccountController(request).Login(
                        new LoginViewModel
                        {
                            Email = request.FormData["email"],
                            Password = request.FormData["password"]
                        }));


            appRouteConfig
                .Get("/account/logout",
                    request => new AccountController(request).Logout());

            // ADMIN Controller

            appRouteConfig
                .Get("/admin/games/add",
                    request => new AdminController(request).Add());

            appRouteConfig
                .Post("/admin/games/add",
                    request => new AdminController(request).Add(
                        new AdminAddGameViewModel
                        {
                            Title = request.FormData["title"],
                            Description = request.FormData["description"],
                            Image = request.FormData["thumbnail"],
                            Price = decimal.Parse(request.FormData["price"]),
                            Size = double.Parse(request.FormData["size"]),
                            VideoId = request.FormData["video-id"],
                            ReleaseDate = DateTime.ParseExact(request.FormData["release-date"], "yyyy-MM-dd", CultureInfo.InvariantCulture)
                        }));


            appRouteConfig
                .Get("/admin/games/edit/{(?<id>[0-9]+)}",
                    request => new AdminController(request).Edit(int.Parse(request.UrlParameters["id"])));

            appRouteConfig
                .Post("/admin/games/edit/{(?<id>[0-9]+)}",
                    request => new AdminController(request).Edit(
                        new AdminEditGameViewModel
                        {
                            Id = int.Parse(request.UrlParameters["id"]),
                            Title = request.FormData["title"],
                            Description = request.FormData["description"],
                            Image = request.FormData["thumbnail"],
                            Price = decimal.Parse(request.FormData["price"]),
                            Size = double.Parse(request.FormData["size"]),
                            VideoId = request.FormData["video-id"],
                            ReleaseDate = DateTime.ParseExact(request.FormData["release-date"], "yyyy-MM-dd", CultureInfo.InvariantCulture)
                        }));


            appRouteConfig
                .Get("/admin/games/delete/{(?<id>[0-9]+)}",
                    request => new AdminController(request).DeleteGet(int.Parse(request.UrlParameters["id"])));

            appRouteConfig
                .Post("/admin/games/delete/{(?<id>[0-9]+)}",
                    request => new AdminController(request).Delete(int.Parse(request.UrlParameters["id"])));


            appRouteConfig
                .Get("/admin/games/list",
                    request => new AdminController(request).List());


        }
    }
}