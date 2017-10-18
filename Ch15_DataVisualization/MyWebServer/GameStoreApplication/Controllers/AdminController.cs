namespace MyWebServer.GameStoreApplication.Controllers
{
    using System;
    using System.Linq;
    using GameStoreApplication.Services.Contracts;
    using GameStoreApplication.Services;
    using GameStoreApplication.ViewModels.Admin;
    using Server.Http.Contracts;

    public class AdminController : BaseController
    {
        private const string AddGameView = @"admin\add-game";
        private const string ListGamesView = @"admin\list-games";

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
                model.ReleaseDate.Value);

            return this.RedirectResponse("/admin/games/list");
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
    }
}