namespace MyWebServer.ByTheCakeApplication.Controllers
{
    using System.Linq;
    using ByTheCakeApplication.Data;
    using ByTheCakeApplication.Models;
    using ByTheCakeApplication.Infrastructure;
    using Server.Http.Contracts;
    using Server.Http.Response;

    public class ShoppingController : Controller
    {
        private readonly CakesData cakesData;

        public ShoppingController()
        {
            this.cakesData = new CakesData();
        }


        public IHttpResponse AddToCart(IHttpRequest request)
        {
            int id = int.Parse(request.UrlParameters["id"]);

            Cake cake = this.cakesData.Find(id);

            if (cake == null)
            {
                return new NotFoundResponse();
            }

            ShoppingCart shoppingCart = request.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);
            shoppingCart.Orders.Add(cake);

            string redirectUrl = "/search";

            const string searchTermKey = "searchTerm";

            if (request.UrlParameters.ContainsKey(searchTermKey))
            {
                // redirect with query for search
                redirectUrl = $"{redirectUrl}?{searchTermKey}={request.UrlParameters[searchTermKey]}";
            }

            return new RedirectResponse(redirectUrl);
        }


        public IHttpResponse ShowCart(IHttpRequest req)
        {
            ShoppingCart shoppingCart = req.Session.Get<ShoppingCart>(ShoppingCart.SessionKey);

            if (!shoppingCart.Orders.Any())
            {
                this.ViewData["cartItems"] = "No items in your cart";
                this.ViewData["totalCost"] = "0.00";
            }
            else
            {
                var items = shoppingCart
                    .Orders
                    .Select(i => $"<div>{i.Name} - ${i.Price:F2}</div><br />");

                decimal totalPrice = shoppingCart
                    .Orders
                    .Sum(i => i.Price);

                this.ViewData["cartItems"] = string.Join(string.Empty, items);
                this.ViewData["totalCost"] = $"{totalPrice:F2}";
            }

            return this.FileViewResponse(@"shopping\cart");
        }


        public IHttpResponse FinishOrder(IHttpRequest request)
        {
            request.Session.Get<ShoppingCart>(ShoppingCart.SessionKey).Orders.Clear();

            return this.FileViewResponse(@"shopping\finish-order");
        }
    }
}