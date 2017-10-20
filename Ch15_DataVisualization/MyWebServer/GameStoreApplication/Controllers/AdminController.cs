namespace MyWebServer.GameStoreApplication.Controllers
{
    using System;
    using System.Linq;
    using GameStoreApplication.Services.Contracts;
    using GameStoreApplication.Services;
    using GameStoreApplication.ViewModels.Admin;
    using MyWebServer.Server.Http.Response;
    using Server.Http.Contracts;

    public class AdminController : BaseController
    {
        private const string AddGameView = @"admin\add-game";
        private const string ListGamesView = @"admin\list-games";
        private const string DeleteGameView = @"admin\delete-game";
        private const string EditGameView = @"admin\edit-game";

        private const string AllGamesListPath = "/admin/games/list";


        private readonly IGameService games;

        public AdminController(IHttpRequest request)
            : base(request)
        {
            this.games = new GameService();
        }


        public IHttpResponse Add()
        {
            if (this.Authentication.IsAdmin)
            {
                return this.FileViewResponse(AddGameView);
            }
            else
            {
                return this.RedirectResponse(HomePath);
            }
        }

        public IHttpResponse Add(AdminAddGameViewModel model)
        {
            // is authenticated as admin ?
            if (!this.Authentication.IsAdmin)
            {
                return this.RedirectResponse(HomePath);
            }

            // are validations passed ?
            if (!this.ValidateModel(model))
            {
                return this.Add();
            }

            this.games.Create(
                model.Title,
                model.Description,
                model.Image,
                model.Price,
                model.Size,
                model.VideoId,
                model.ReleaseDate);         //.Value);

            return this.RedirectResponse(AllGamesListPath);
        }


        public IHttpResponse List()
        {
            if (!this.Authentication.IsAdmin)
            {
                return this.RedirectResponse(HomePath);
            }

            var result = this.games
                .All()
                .Select(g => $@"<tr>
                                    <td>{g.Id}</td>
                                    <td>{g.Name}</td>
                                    <td>{g.Size:F2} GB</td>
                                    <td>{g.Price:F2} &euro;</td>
                                    <td>
                                        <a class=""btn btn-warning"" href=""/admin/games/edit/{g.Id}"">Edit</a>
                                        <a class=""btn btn-danger"" href=""/admin/games/delete/{g.Id}"">Delete</a>
                                    </td>
                                </tr>");

            string gamesAsHtml = string.Join(Environment.NewLine, result);

            this.ViewData["games"] = gamesAsHtml;

            return this.FileViewResponse(ListGamesView);
        }



        public IHttpResponse Edit(int id)
        {
            if (!this.Authentication.IsAdmin)
            {
                return this.RedirectResponse(HomePath);
            }

            AdminEditGameViewModel game = this.games.GetGameById(id);

            if (game == null)
            {
                return this.RedirectResponse(HomePath);
            }

            this.ViewData["alert-display"] = "none";
            if (this.ViewData.ContainsKey("alert-message"))
            {
                this.ViewData["alert-display"] = "block";
            }

            this.ViewData["game-title"] = game.Title;
            this.ViewData["game-description"] = game.Description;
            this.ViewData["game-image"] = game.Image;
            this.ViewData["game-price"] = game.Price.ToString();
            this.ViewData["game-release"] = game.ReleaseDate.ToString();
            this.ViewData["game-size"] = game.Size.ToString();
            this.ViewData["game-videoId"] = game.VideoId;
            this.ViewData["game-id"] = game.Id.ToString();

            return this.FileViewResponse(EditGameView);
        }

        public IHttpResponse Edit(AdminEditGameViewModel model)
        {
            if (!this.Authentication.IsAdmin)
            {
                return this.RedirectResponse(HomePath);
            }

            // are validations passed ?
            if (!this.ValidateModel(model))
            {
                return this.Edit(model.Id);
            }

            if (!this.games.Edit(model))
            {
                return new BadRequestResponse();
            }
            
            return this.RedirectResponse(AllGamesListPath);
        }




        public IHttpResponse DeleteGet(int id)
        {
            if (!this.Authentication.IsAdmin)
            {
                return this.RedirectResponse(HomePath);
            }

            AdminEditGameViewModel game = this.games.GetGameById(id);

            if (game == null)
            {
                return this.RedirectResponse(HomePath);
            }

            this.ViewData["game-title"] = game.Title;
            this.ViewData["game-description"] = game.Description;
            this.ViewData["game-image"] = game.Image;
            this.ViewData["game-price"] = game.Price.ToString();
            this.ViewData["game-release"] = game.ReleaseDate.ToString();
            this.ViewData["game-size"] = game.Size.ToString();
            this.ViewData["game-videoId"] = game.VideoId;

            return this.FileViewResponse(DeleteGameView);
        }

        public IHttpResponse Delete(int id)
        {
            if (!this.Authentication.IsAdmin)
            {
                return this.RedirectResponse(HomePath);
            }

            if (!this.games.Delete(id))
            {
                return new BadRequestResponse();
            }

            return new RedirectResponse(AllGamesListPath);
        }
    }
}