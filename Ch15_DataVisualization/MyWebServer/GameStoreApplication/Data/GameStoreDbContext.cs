namespace MyWebServer.GameStoreApplication.Data
{
    using Microsoft.EntityFrameworkCore;
    using GameStoreApplication.Data.Models;

    public class GameStoreDbContext  : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Game> Games { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=GameStoreDb;Integrated Security=True;";
            optionsBuilder.UseSqlServer(connectionString);
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<UserGame>()
                .HasKey(ug => new { ug.GameId, ug.UserId });

            // Set property User.Email to be UNIQUE
            modelBuilder
                .Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder
                .Entity<User>()
                .HasMany(u => u.Games)
                .WithOne(ug => ug.User)
                .HasForeignKey(ug => ug.UserId);

            modelBuilder
                .Entity<Game>()
                .HasMany(g => g.Users)
                .WithOne(ug => ug.Game)
                .HasForeignKey(ug => ug.GameId);

        }

    }
}