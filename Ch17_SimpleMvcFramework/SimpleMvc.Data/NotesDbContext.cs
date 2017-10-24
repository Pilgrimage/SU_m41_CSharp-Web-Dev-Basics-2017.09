namespace SimpleMvc.Data
{
    using Microsoft.EntityFrameworkCore;
    using SimpleMvc.Domain;

    public class NotesDbContext : DbContext
    {
        public DbSet<User>  Users { get; set; }
        public DbSet<Note>  Notes { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=NotesDb;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Notes)
                .WithOne(n => n.Owner)
                .HasForeignKey(n => n.OwnerId);
        }

    }
}