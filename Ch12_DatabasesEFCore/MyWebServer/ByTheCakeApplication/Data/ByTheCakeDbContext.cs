namespace MyWebServer.ByTheCakeApplication.Data
{
    using Microsoft.EntityFrameworkCore;
    using ByTheCakeApplication.Data.Models;

    public class ByTheCakeDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=ByTheCakeDb;Integrated Security=True;";
            optionsBuilder.UseSqlServer(connectionString);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<OrderProduct>()
                .HasKey(op => new { op.OrderId, op.ProductId });

            modelBuilder
                .Entity<User>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId);

            modelBuilder
                .Entity<Product>()
                .HasMany(pr => pr.Orders)
                .WithOne(op => op.Product)
                .HasForeignKey(op => op.ProductId);

            modelBuilder
                .Entity<Order>()
                .HasMany(o => o.Products)
                .WithOne(op => op.Order)
                .HasForeignKey(op => op.OrderId);
        }

    }
}