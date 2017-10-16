namespace MyWebServer.ByTheCakeApplication.ViewModels.Shopping
{
    using System;

    public class OrderDetailsViewModel
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public decimal Sum { get; set; }
    }

}