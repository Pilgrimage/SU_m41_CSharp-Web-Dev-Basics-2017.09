namespace Lab05_Shop.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Item
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public decimal Price { get; set; }

        public ICollection<OrderItem> OrdersForItem { get; set; } = new List<OrderItem>();

        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
