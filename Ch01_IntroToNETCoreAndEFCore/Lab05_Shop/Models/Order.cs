namespace Lab05_Shop.Models
{
    using System.Collections.Generic;

    public class Order
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }
        public Customer Customer { get; set; }

        public ICollection<OrderItem> ItemsInOrder { get; set; } = new List<OrderItem>();
    }
}
