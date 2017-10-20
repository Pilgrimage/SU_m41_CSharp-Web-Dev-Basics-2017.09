namespace MyWebServer.GameStoreApplication.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GameStoreApplication.Data;
    using GameStoreApplication.Services.Contracts;
    using GameStoreApplication.Common;
    using GameStoreApplication.ViewModels.Admin;
    using MyWebServer.GameStoreApplication.Data.Models;

    public class GameService : IGameService
    {
        public void Create(
            string title,
            string description,
            string image,
            decimal price,
            double size,
            string videoId,
            DateTime releaseDate)
        {
            using (var db = new GameStoreDbContext())
            {
                Game game = new Game
                {
                    Title = title,
                    Description = description,
                    Image = image,
                    Price = price,
                    Size = size,
                    VideoId = videoId,
                    ReleaseDate = releaseDate
                };

                db.Add(game);
                db.SaveChanges();
            }
        }

        public IEnumerable<AdminListGameViewModel> All()
        {
            using (var db = new GameStoreDbContext())
            {
                return db
                    .Games
                    .Select(g => new AdminListGameViewModel
                    {
                        Id = g.Id,
                        Name = g.Title,
                        Price = g.Price,
                        Size = g.Size
                    })
                    .ToList();
            }
        }

        public IEnumerable<GameViewModel> AllGamesFullInfo()
        {
            using (var db = new GameStoreDbContext())
            {
                return db
                    .Games
                    .Select(g => new GameViewModel
                    {
                        Id = g.Id,
                        Price = g.Price,
                        Size = g.Size,
                        Title = g.Title,
                        Description = g.Description,
                        Image = g.Image,
                        VideoId = g.VideoId,
                        ReleaseDate = g.ReleaseDate
                    })
                    .ToList();
            }
        }



        public AdminEditGameViewModel GetGameById(int id)
        {
            using (var db = new GameStoreDbContext())
            {
                if (db.Games.Any(g => g.Id == id))
                {
                    return db
                        .Games
                        .Select(g => new AdminEditGameViewModel
                        {
                            Id = g.Id,
                            Price = g.Price,
                            Size = g.Size,
                            Title = g.Title,
                            Description = g.Description,
                            Image = g.Image,
                            VideoId = g.VideoId,
                            ReleaseDate = g.ReleaseDate
                        })
                        .FirstOrDefault(g => g.Id == id);
                }
                return null;
            }

        }

        public bool Edit(AdminEditGameViewModel editedGame)
        {
            using (var db = new GameStoreDbContext())
            {
                if (db.Games.Any(g => g.Id == editedGame.Id))
                {
                    Game game = db.Games.FirstOrDefault(g => g.Id == editedGame.Id);
                    game.Description = editedGame.Description;
                    game.Image = editedGame.Image;
                    game.Price = editedGame.Price;
                    game.ReleaseDate = editedGame.ReleaseDate;
                    game.Size = editedGame.Size;
                    game.Title = editedGame.Title;
                    game.VideoId = editedGame.VideoId;

                    db.SaveChanges();

                    return true;
                }

                return false;
            }
        }

        public bool Delete(int id)
        {
            using (var db = new GameStoreDbContext())
            {
                Game game = db.Games.FirstOrDefault(g => g.Id == id);

                if (game != null)
                {
                    db.Games.Remove(game);
                    db.SaveChanges();

                    return true;
                }

                return false;
            }
        }
    }
}