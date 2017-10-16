namespace MyWebServer.ByTheCakeApplication.ViewModels.Shopping
{
    using System;
    using System.Collections.Generic;

    public class OrderDetailsByIdViewModel
    {
        public int Id { get; set; }

        public IEnumerable<OrderProductsDetailsViewModel> Products { get; set; }

        public DateTime CreatedOn { get; set; }

    }
}