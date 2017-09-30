namespace p02_SocialNetwork.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using Data;
    using Models;

    public class StartUp
    {
        private static Random random = new Random();

        public static void Main()
        {
            using (var db = new SocialNetworkDbContext())
            {
                db.Database.Migrate();

                // SeedUsersAndFriendships(db);
                // SeedAlbumsAndPictures(db);
                // SeetTags(db);
                // SeedForSharedAlbums(db);

                // Task21_PrintUsersWithFriends(db);
                // Task22_PrintActiveUsersWithMoreThanFiveFriends(db);

                // Task31_PrintAlbumsWithTotalPictures(db);
                // Task32_PrintPicturesIncludedInMoreThanTwoAlbums(db);
                // Task33_PrintUsersAlbums(db);

                // Task41_PrintAlbumsByTag(db);
                // Task42_PrintUsersWithAlbumsWithMoreThanThreeTags(db);

                Task51_PrintUsersWithSharedAlbums(db);
            }
        }

        
        private static void SeedUsersAndFriendships(SocialNetworkDbContext db)
        {
            // Seed Users
            Console.WriteLine("Seeding users...");

            const int totalUsers = 50;

            int biggestUserId = db.Users
                .OrderByDescending(u => u.Id)
                .Select(u => u.Id)
                .FirstOrDefault() + 1;

            ICollection<User> allUsers = new List<User>();

            for (int i = biggestUserId; i < biggestUserId + totalUsers; i++)
            {
                User user = new User
                {
                    Username = $"Username {i}",
                    Password = $"PaSSw0rd_{i}!",
                    Email = $"email{i}@google.server{i}.com",
                    RegisteredOn = DateTime.Now.AddDays(-100 - i * i),
                    LastTimeLoggedIn = DateTime.Now.AddDays(-i),
                    Age = 18 + i,
                };

                allUsers.Add(user);
                db.Users.Add(user);
            }

            db.SaveChanges();

            // Seed Frientships
            Console.WriteLine("Seeding friendships...");

            var userIds = allUsers.Select(u => u.Id).ToList();

            for (int i = 0; i < userIds.Count; i++)
            {
                var currentUserId = userIds[i];

                var totalFriends = random.Next(4, 11);

                for (int j = 0; j < totalFriends; j++)
                {
                    var friendUserId = userIds[random.Next(0, userIds.Count)];

                    var validFriendship = true;

                    // Cannot be friend to myself
                    if (friendUserId == currentUserId)
                    {
                        validFriendship = false;
                    }

                    //Friendship already exist
                    var friendshipExist = db
                        .Friendships
                        .Any(f => (f.FromUserId == currentUserId && f.ToUserId == friendUserId) ||
                                  (f.FromUserId == friendUserId && f.ToUserId == currentUserId));

                    if (friendshipExist)
                    {
                        validFriendship = false;
                    }

                    if (!validFriendship)
                    {
                        j--;
                        continue;
                    }

                    db.Friendships.Add(new Friendship
                    {
                        FromUserId = currentUserId,
                        ToUserId = friendUserId
                    });

                    db.SaveChanges();
                }
            }
        }

        private static void SeedAlbumsAndPictures(SocialNetworkDbContext db)
        {
            // Albums
            Console.WriteLine("Seeding albums...");

            const int totalAlbums = 100;
            const int totalPictures = 1000;

            var userIds = db.Users
                .Select(u => u.Id)
                .ToList();

            int biggestAlbumId = db.Albums
                .OrderByDescending(a => a.Id)
                .Select(a => a.Id)
                .FirstOrDefault() + 1;

            var albums = new List<Album>();

            for (int i = biggestAlbumId; i < biggestAlbumId + totalAlbums; i++)
            {
                Album album = new Album
                {
                    Name = $"Album_{i}",
                    BackgroundColor = $"Color {i}",
                    IsPublic = random.Next(0, 2) == 0 ? true : false,
                    OwnerId = userIds[random.Next(0, userIds.Count)]
                };

                db.Albums.Add(album);
                albums.Add(album);
            }

            db.SaveChanges();

            // Pictures
            Console.WriteLine("Seeding pictures...");

            int biggestPictureId = db.Pictures
                .OrderByDescending(p => p.Id)
                .Select(p => p.Id)
                .FirstOrDefault() + 1;

            var pictures = new List<Picture>();

            for (int i = biggestPictureId; i < biggestPictureId + totalPictures; i++)
            {
                Picture picture = new Picture
                {
                    Title = $"Picture_{i}",
                    Caption = $"Caption_{i}",
                    Path = $@"C\MyPics\Albums\",
                };

                db.Pictures.Add(picture);
                pictures.Add(picture);
            }

            db.SaveChanges();

            // Pictures in Albums
            Console.Write("Seeding pictures in albums...");

            var albumIds = db.Albums
                .Select(a => a.Id)
                .ToList();

            for (int i = 0; i < pictures.Count; i++)
            {
                Console.Write(".");

                Picture currentPicture = pictures[i];

                int albumsForPicture = random.Next(2, 7);

                for (int j = 0; j < albumsForPicture; j++)
                {
                    int currentAlbumId = albumIds[random.Next(0, albumIds.Count)];

                    bool pictureExistInAlbum = db.Pictures
                        .Any(p => p.Id == currentPicture.Id &&
                                  p.Albums.Any(a => a.AlbumId == currentAlbumId));

                    if (pictureExistInAlbum)
                    {
                        j--;
                        continue;
                    }

                    currentPicture.Albums.Add(new AlbumPicture
                    {
                        AlbumId = currentAlbumId,
                    });

                    db.SaveChanges();
                }
            }

            Console.WriteLine();
        }

        private static void SeetTags(SocialNetworkDbContext db)
        {
            // Tags
            Console.WriteLine("Seeding tags...");

            int totalTags = db.Albums.Count() * 20;

            var tags = new List<Tag>();

            for (int i = 0; i < totalTags; i++)
            {
                Tag tag = new Tag
                {
                    Name = TagTransformer.Transform($"#tag{i}")
                };

                db.Tags.Add(tag);
                tags.Add(tag);
            }

            db.SaveChanges();

            // Tags & Albums
            Console.Write("Connecting tags & Albums...");

            var albumIds = db.Albums.Select(a => a.Id).ToList();

            foreach (Tag itemTag in tags)
            {
                Console.Write(".");

                int totalAlbumsForTag = random.Next(0, 15);

                for (int i = 0; i < totalAlbumsForTag; i++)
                {
                    int albumId = albumIds[random.Next(0, albumIds.Count)];

                    bool tagExistForAlbum = db
                        .Albums
                        .Any(a => a.Id == albumId &&
                                  a.Tags.Any(t => t.TagId == itemTag.Id));

                    if (tagExistForAlbum)
                    {
                        i--;
                        continue;
                    }

                    itemTag.Albums.Add(new AlbumTag
                    {
                        AlbumId = albumId
                    });

                    db.SaveChanges();
                }
            }

            Console.WriteLine();

        }

        private static void SeedForSharedAlbums(SocialNetworkDbContext db)
        {
            var userIds = db.Users
                .Select(u => u.Id)
                .ToList();

            var albums = db.Albums.ToList();


            foreach (Album album in albums)
            {
                int totalOwnersForAlbum = random.Next(0, 5);

                album.CoOwners.Add(new AlbumUser
                {
                    UserId=album.OwnerId
                });

                db.SaveChanges();

                for (int i = 0; i < totalOwnersForAlbum; i++)
                {
                    int userId = userIds[random.Next(0, userIds.Count)];

                    bool ownerExistForAlbum = db
                        .Albums
                        .Any(a => a.Id == album.Id &&
                                  a.CoOwners.Any(t => t.UserId == userId));

                    if (ownerExistForAlbum)
                    {
                        i--;
                        continue;
                    }

                    album.CoOwners.Add(new AlbumUser
                    {
                        UserId = userId
                    });

                    db.SaveChanges();
                }
            }
        }


        private static void Task21_PrintUsersWithFriends(SocialNetworkDbContext db)
        {
            var taskResult = db
                .Users
                .Select(u => new
                {
                    Name = u.Username,
                    Friends = u.ToFriends.Count + u.FromFriends.Count,
                    Status = u.IsDeleted ? "Inactive" : "Active"
                })
                .OrderByDescending(u => u.Friends)
                .ThenBy(u => u.Name)
                .ToList();

            foreach (var user in taskResult)
            {
                Console.WriteLine($"{user.Name} with Status = {user.Status} have {user.Friends} friends.");
            }
        }

        private static void Task22_PrintActiveUsersWithMoreThanFiveFriends(SocialNetworkDbContext db)
        {
            //var taskResult = db
            //    .Users
            //    .Where(u => !u.IsDeleted)
            //    .Where(u => (u.FromFriends.Count + u.ToFriends.Count) > 5)
            //    .OrderBy(u => u.RegisteredOn)
            //    .ThenBy(u => (u.FromFriends.Count + u.ToFriends.Count))
            //    .Select(u => new
            //    {
            //        Name = u.Username,
            //        Friends = u.FromFriends.Count + u.ToFriends.Count,
            //        Period = DateTime.Now.Subtract(u.RegisteredOn.Value).Days
            //    })
            //    .ToList();

            var taskResult = db
                .Users
                .Where(u => !u.IsDeleted)
                .Select(u => new
                {
                    Name = u.Username,
                    u.RegisteredOn,
                    Friends = u.FromFriends.Count + u.ToFriends.Count,
                    Period = DateTime.Now.Subtract(u.RegisteredOn.Value).Days
                })
                .Where(u => u.Friends > 5)
                .OrderBy(u => u.RegisteredOn)
                .ThenBy(u => u.Friends)
                .ToList();

            foreach (var user in taskResult)
            {
                Console.WriteLine($"{user.Name} have {user.Friends} friends after {user.Period} days in network.");
            }
        }

        private static void Task31_PrintAlbumsWithTotalPictures(SocialNetworkDbContext db)
        {
            var taskResult = db
                .Albums
                .Select(a => new
                {
                    Title = a.Name,
                    Owner = a.Owner.Username,
                    Pictures = a.Pictures.Count
                })
                .OrderByDescending(a => a.Pictures)
                .ThenBy(a => a.Owner)
                .ToList();

            foreach (var album in taskResult)
            {
                Console.WriteLine($"{album.Title} with owner {album.Owner} have {album.Pictures} pictures.");
            }
        }

        private static void Task32_PrintPicturesIncludedInMoreThanTwoAlbums(SocialNetworkDbContext db)
        {
            var taskResult = db
                .Pictures
                .Where(p => p.Albums.Count >= 2)
                .Select(p => new
                {
                    p.Title,
                    //Albums = p.Albums.Select(a => a.Album.Name),
                    //Owners = p.Albums.Select(a => a.Album.Owner.Username),
                    Albums = p.Albums.Select(a => new
                    {
                        Album = a.Album.Name,
                        Owner = a.Album.Owner.Username
                    })
                })
                .OrderByDescending(p => p.Albums.Count())
                .ThenBy(p => p.Title)
                .ToList();

            foreach (var picture in taskResult)
            {
                Console.WriteLine($"{picture.Title} is in : ");
                //Console.WriteLine($" - Albums: {string.Join(", ", picture.Albums)}");
                //Console.WriteLine($" - Owners: {string.Join(", ", picture.Owners)}");
                Console.WriteLine($" -  Albums: {string.Join(", ", picture.Albums)}");
                Console.WriteLine(new string('-', 20));
            }
        }

        private static void Task33_PrintUsersAlbums(SocialNetworkDbContext db)
        {
            var userIds = db.Users.Select(u => u.Id).ToList();
            int userId = userIds[random.Next(0, userIds.Count)];

            var taskResult = db
                .Albums
                .Where(a => a.OwnerId == userId)
                .Select(a => new
                {
                    Owner = a.Owner.Username,
                    a.IsPublic,
                    a.Name,
                    Pictures = a.Pictures.Select(p => new
                    {
                        p.Picture.Title,
                        p.Picture.Path
                    })
                })
                .OrderBy(a => a.Name)
                .ToList();

            Console.WriteLine($"Owner: {taskResult.Select(tr => tr.Owner).FirstOrDefault()} (Id={userId})");

            foreach (var album in taskResult)
            {
                Console.Write($"--- {album.Name}:");

                if (!album.IsPublic)
                {
                    Console.WriteLine($"  -  Private content!{Environment.NewLine}");
                }
                else
                {
                    Console.WriteLine();

                    foreach (var picture in album.Pictures)
                    {
                        Console.WriteLine($"    - {picture.Title} is placed in {picture.Path}");
                    }

                    Console.WriteLine();
                }
            }
        }

        private static void Task41_PrintAlbumsByTag(SocialNetworkDbContext db)
        {
            var tagIds = db.Tags.Select(u => u.Id).ToList();
            int tagId = tagIds[random.Next(0, tagIds.Count)];

            var taskResult = db
                .Albums
                .Where(a => a.Tags.Any(t => t.TagId == tagId))
                .Select(a => new
                {
                    a.Name,
                    a.Owner.Username,
                    Tags = a.Tags.Count
                })
                .OrderByDescending(a => a.Tags)
                .ThenBy(a => a.Username)
                .ToList();

            Console.WriteLine($"Tag with Id={tagId}:");

            foreach (var album in taskResult)
            {
                Console.WriteLine($"---{album.Name} with owner {album.Username} have {album.Tags} tags");
            }
        }

        private static void Task42_PrintUsersWithAlbumsWithMoreThanThreeTags(SocialNetworkDbContext db)
        {
            var taskResult = db
                .Users
                .Where(u => u.Albums.Any(a => a.Tags.Count > 3))
                .Select(u => new
                {
                    u.Username,
                    Albums = u.Albums
                        .Where(a => a.Tags.Count > 3)
                        .Select(a => new
                        {
                            a.Name,
                            Tags = a.Tags.Select(t => t.Tag.Name),
                        })
                        .ToList()
                })
                .OrderByDescending(u => u.Albums.Count())
                .ThenByDescending(u => u.Albums.Sum(a => a.Tags.Count()))
                //.ThenByDescending(u => u.Albums.SelectMany(a => a.Tags).Count())
                .ThenBy(u => u.Username)
                .ToList();

            foreach (var user in taskResult)
            {
                Console.WriteLine(user.Username);

                foreach (var album in user.Albums)
                {
                    Console.WriteLine($"---{album.Name} have tags: {string.Join(", ", album.Tags)}");
                }

                Console.WriteLine(new string('-',20));
            }
        }

        private static void Task51_PrintUsersWithSharedAlbums(SocialNetworkDbContext db)
        {
            // TODO:
        }

    }
}
