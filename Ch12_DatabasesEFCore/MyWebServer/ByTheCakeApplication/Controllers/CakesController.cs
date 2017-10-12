namespace MyWebServer.ByTheCakeApplication.Controllers
{
    using System;
    using System.Linq;
    using Infrastructure;
    using ByTheCakeApplication.Models;
    using ByTheCakeApplication.Data;
    using Server.Http.Contracts;

    public class CakesController : Controller
    {
        private readonly CakesData cakesData;

        public CakesController()
        {
            this.cakesData = new CakesData();
        }

        public IHttpResponse Add()
        {
            this.ViewData["showResult"] = "none";

            return this.FileViewResponse(@"cakes\add");
        }

        public IHttpResponse Add(string name, string price)
        {
            Cake cake = new Cake
            {
                Name = name,
                Price = decimal.Parse(price)
            };

            this.cakesData.Add(name, price);

            this.ViewData["name"] = name;
            this.ViewData["price"] = price;
            this.ViewData["showResult"] = "block";

            return this.FileViewResponse(@"cakes\add");
        }


        public IHttpResponse Search(IHttpRequest request)
        {
            const string searchTermKey = "searchTerm";

            var urlParameters = request.UrlParameters;

            this.ViewData["results"] = string.Empty;
            this.ViewData["searchTerm"] = string.Empty;

            if (urlParameters.ContainsKey(searchTermKey))
            {
                string searchTerm = urlParameters[searchTermKey];

                this.ViewData["searchTerm"] = searchTerm;


                var savedCakesDivs = this.cakesData
                    .All()
                    .Where(c => c.Name.ToLower().Contains(searchTerm.ToLower()))
                    .Select(c =>
                        //$@"<div>{c.Name} - ${c.Price:F2} <a href=""/shopping/add/{c.Id}?searchTerm={searchTerm}"">Order</a></div>");
                        $@"<div style=""height: 30px;"" >{c.Name} - ${c.Price:F2} <a href=""/shopping/add/{c.Id}?searchTerm={searchTerm}""><button>Order</a></div>");
                        //$@"<form style=""height: 30px;"" action=""/shopping/add/{c.Id}?searchTerm={searchTerm}"" method=""get"">{c.Name} - ${c.Price:F2}  <button>Order</button></form>");

                string results = "No cakes found";

                if (savedCakesDivs.Any())
                {
                    results = string.Join(Environment.NewLine, savedCakesDivs);
                }

                this.ViewData["results"] = results;
            }
            else
            {
                this.ViewData["results"] = "Please, enter search term";
            }

            this.ViewData["showCart"] = "none";

            ShoppingCart shoppingCart = request.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);

            if (shoppingCart.Orders.Any())
            {
                int totalProducts = shoppingCart.Orders.Count;
                string totalProductsText = totalProducts != 1 ? "products" : "product";

                this.ViewData["showCart"] = "block";
                this.ViewData["products"] = $"{totalProducts} {totalProductsText}";
            }

            return this.FileViewResponse(@"cakes\search");
        }
    }
}