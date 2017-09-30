namespace p02_SocialNetwork.Data
{
    using System;
    using System.Collections.Generic;
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Models;

    public class SocialNetworkDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Tag> Tags { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            const string connectionString =
                @"Server=(localdb)\MSSQLLocalDB;Database=SocialNetworkDb;Integrated Security=True;";
            optionsBuilder.UseSqlServer(connectionString);
            base.OnConfiguring(optionsBuilder);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // many-to-many self-relationship
            modelBuilder
                .Entity<Friendship>()
                .HasKey(f => new {f.FromUserId, f.ToUserId});

            modelBuilder
                .Entity<User>()
                .HasMany(u => u.FromFriends)
                .WithOne(f => f.FromUser)
                .HasForeignKey(f => f.FromUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<User>()
                .HasMany(u => u.ToFriends)
                .WithOne(f => f.ToUser)
                .HasForeignKey(f => f.ToUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // many-to-many Album-Puctures
            modelBuilder
                .Entity<AlbumPicture>()
                .HasKey(ap => new {ap.AlbumId, ap.PictureId});

            modelBuilder
                .Entity<Album>()
                .HasMany(a => a.Pictures)
                .WithOne(p => p.Album)
                .HasForeignKey(p => p.AlbumId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<Picture>()
                .HasMany(p => p.Albums)
                .WithOne(a => a.Picture)
                .HasForeignKey(a => a.PictureId)
                .OnDelete(DeleteBehavior.Restrict);

            //
            modelBuilder
                .Entity<User>()
                .HasMany(u => u.Albums)
                .WithOne(a => a.Owner)
                .HasForeignKey(a => a.OwnerId);

            // many-to-many Album-Tag
            modelBuilder
                .Entity<AlbumTag>()
                .HasKey(at => new {at.AlbumId, at.TagId});

            modelBuilder
                .Entity<Album>()
                .HasMany(a=>a.Tags)
                .WithOne(t=>t.Album)
                .HasForeignKey(t=>t.AlbumId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<Tag>()
                .HasMany(t => t.Albums)
                .WithOne(a => a.Tag)
                .HasForeignKey(a => a.TagId)
                .OnDelete(DeleteBehavior.Restrict);

            // many-to-many Album-User
            modelBuilder
                .Entity<AlbumUser>()
                .HasKey(au => new {au.AlbumId, au.UserId});

            modelBuilder
                .Entity<Album>()
                .HasMany(a=>a.CoOwners)
                .WithOne(u=>u.Album)
                .HasForeignKey(u=>u.AlbumId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<User>()
                .HasMany(u=>u.OwnedAlbums)
                .WithOne(a=>a.User)
                .HasForeignKey(a=>a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

        }


        // This code is to make working the custom attributes in .Net Core
        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            var serviceProvider = this.GetService<IServiceProvider>();
            var items = new Dictionary<object, object>();

            foreach (var entry in this.ChangeTracker.Entries()
                                                    .Where(e => (e.State == EntityState.Added) ||
                                                                (e.State == EntityState.Modified)))
            {
                var entity = entry.Entity;
                var context = new ValidationContext(entity, serviceProvider, items);
                var results = new List<ValidationResult>();         // here are results from validations

                if (Validator.TryValidateObject(entity, context, results, true) == false)
                {
                    foreach (var result in results)
                    {
                        if (result != ValidationResult.Success)
                        {
                            throw new ValidationException(result.ErrorMessage);
                        }
                    }
                }

            }

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }
    }
}