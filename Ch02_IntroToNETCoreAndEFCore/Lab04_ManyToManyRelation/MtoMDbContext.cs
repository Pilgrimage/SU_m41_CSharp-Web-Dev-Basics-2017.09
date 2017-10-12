namespace Lab04_ManyToManyRelation
{
    using Microsoft.EntityFrameworkCore;
    using Lab04_ManyToManyRelation.Models;

    public class MtoMDbContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=MtoMRelationDb;Integrated Security=True;";
            optionsBuilder.UseSqlServer(connectionString);

            base.OnConfiguring(optionsBuilder);    // It's not necessary
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Create composite PK for Join table
            modelBuilder
                .Entity<StudentCourse>()
                .HasKey(sc => new { sc.StudentId, sc.CourseId });

            // Many-to-many Relation
            modelBuilder
                .Entity<Student>()
                .HasMany(s => s.Courses)
                .WithOne(sc => sc.Student)
                .HasForeignKey(sc => sc.StudentId);

            // Equivalent to:
            //modelBuilder
            //    .Entity<StudentCourse>()
            //    .HasOne(sc => sc.Student)
            //    .WithMany(s => s.Courses)
            //    .HasForeignKey(sc => sc.StudentId);

            modelBuilder
                .Entity<Course>()
                .HasMany(c => c.Students)
                .WithOne(sc => sc.Course)
                .HasForeignKey(sc => sc.CourseId);

            // Equivalent to:
            //modelBuilder
            //    .Entity<StudentCourse>()
            //    .HasOne(sc => sc.Course)
            //    .WithMany(s => s.Students)
            //    .HasForeignKey(sc => sc.CourseId);

        }
    }
}
