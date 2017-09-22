namespace Lab05_Shop
{
    using Microsoft.EntityFrameworkCore;
    using Lab05_Shop.Models;

    public class ShopDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Salesman> Salesmen { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Item> Items { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=ShopDb;Integrated Security=True;";
            optionsBuilder.UseSqlServer(connectionString);

            base.OnConfiguring(optionsBuilder);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<OrderItem>()
                .HasKey(oi => new { oi.OrderId, oi.ItemId });

            modelBuilder
                .Entity<Salesman>()
                .HasMany(s => s.Customers)
                .WithOne(c => c.Salesman)
                .HasForeignKey(c => c.SalesmanId);

            modelBuilder
                .Entity<Customer>()
                .HasMany(c => c.Orders)
                .WithOne(o => o.Customer)
                .HasForeignKey(o => o.CustomerId);

            modelBuilder
                .Entity<Customer>()
                .HasMany(c => c.Reviews)
                .WithOne(r => r.Customer)
                .HasForeignKey(r => r.CustomerId);

            modelBuilder
                .Entity<Order>()
                .HasMany(o => o.ItemsInOrder)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId);

            modelBuilder
                .Entity<Item>()
                .HasMany(i => i.OrdersForItem)
                .WithOne(oi => oi.Item)
                .HasForeignKey(oi => oi.ItemId);

            modelBuilder
                .Entity<Item>()
                .HasMany(i => i.Reviews)
                .WithOne(r => r.Item)
                .HasForeignKey(r => r.ItemId);
        }

    }
}
