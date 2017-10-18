namespace MyWebServer.ByTheCakeApplication
{
    using ByTheCakeApplication.Controllers;
    using Microsoft.EntityFrameworkCore;
    using ByTheCakeApplication.Data;
    using ByTheCakeApplication.ViewModels.Account;
    using ByTheCakeApplication.ViewModels.Products;
    using Server.Contracts;
    using Server.Routing.Contracts;

    public class ByTheCakeApp : IApplication
    {
        public void InitializeDatabase()
        {
            using (ByTheCakeDbContext db = new ByTheCakeDbContext())
            {
                db.Database.Migrate();
            }
        }

        public void Start(IAppRouteConfig appRouteConfig)
        {
            appRouteConfig.AnonymousPaths.Add("/login");
            appRouteConfig.AnonymousPaths.Add("/register");

            // HOME Controller

            appRouteConfig
                .Get("/", request => new HomeController().Index());

            appRouteConfig
                .Get("/about", request => new HomeController().About());


            // ACCOUNT Controller

            appRouteConfig
                .Get("/register",
                    request => new AccountController().Register());

            appRouteConfig
                .Post("/register",
                    request => new AccountController().Register(
                        request,
                        new RegisterUserViewModel
                        {
                            Username = request.FormData["username"],
                            Password = request.FormData["password"],
                            ConfirmPassword = request.FormData["confirm-password"]
                        }));

            appRouteConfig
                .Get("/login",
                    request => new AccountController().Login());

            appRouteConfig
                .Post("/login",
                    request => new AccountController().Login(
                        request,
                        new LoginViewModel
                        {
                            Username = request.FormData["username"],
                            Password = request.FormData["password"]
                        }));
            
            appRouteConfig
                .Post("/logout",
                    request => new AccountController().Logout(request));

            appRouteConfig
                .Get("/profile",
                    request => new AccountController().Profile(request));


            // PRODUCT Controller

            appRouteConfig
                .Get("/add", request => new ProductsController().Add());

            appRouteConfig
                .Post("/add",
                    request => new ProductsController().Add(
                        new AddProductViewModel
                        {
                            Name = request.FormData["name"],
                            Price = decimal.Parse(request.FormData["price"]),
                            ImageUrl = request.FormData["imageUrl"]
                        }));

            appRouteConfig
                .Get("/search",
                    request => new ProductsController().Search(request));


            appRouteConfig
                .Get("/cakes/{(?<id>[0-9]+)}",
                    request => new ProductsController().Details(int.Parse(request.UrlParameters["id"])));


            // SHOPPING Controller

            appRouteConfig
                .Get("/shopping/add/{(?<id>[0-9]+)}",
                    request => new ShoppingController().AddToCart(request));

            appRouteConfig
                .Get("/cart",
                    request => new ShoppingController().ShowCart(request));

            appRouteConfig
                .Post("/shopping/finish-order",
                    request => new ShoppingController().FinishOrder(request));


            appRouteConfig
                .Get("/orders", 
                    request => new ShoppingController().GetOrdersDetails(request));

            appRouteConfig
                .Get("/order/{(?<id>[0-9]+)}",
                    request => new ShoppingController().GetOrderDetailsById(int.Parse(request.UrlParameters["id"])));

        }
    }
}