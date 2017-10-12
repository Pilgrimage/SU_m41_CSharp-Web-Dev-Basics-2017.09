namespace Lab02_Employees
{
    using Microsoft.EntityFrameworkCore;
    using Lab02_Employees.Models;

    public class EmployeesDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=EmployeesCoreDb;Integrated Security=True;";
            optionsBuilder.UseSqlServer(connectionString);

            base.OnConfiguring(optionsBuilder);    // It's not necessary
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // One-to-many relation
            modelBuilder
                .Entity<Employee>()
                .HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId);

            // Self-reference relation
            modelBuilder
                .Entity<Employee>()
                .HasOne(e => e.Manager)
                .WithMany(m => m.Subordinates)
                .HasForeignKey(e => e.ManagerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
