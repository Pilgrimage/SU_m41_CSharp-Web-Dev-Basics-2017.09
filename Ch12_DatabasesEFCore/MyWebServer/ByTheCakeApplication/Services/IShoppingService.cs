namespace MyWebServer.ByTheCakeApplication.Services
{
    using System.Collections.Generic;
    using ByTheCakeApplication.ViewModels.Shopping;

    public interface IShoppingService
    {
        void CreateOrder(int userId, IEnumerable<int> productIds);

        IEnumerable<OrderDetailsViewModel> GetOrders(int userId);

        OrderDetailsByIdViewModel Find(int id);

    }
}