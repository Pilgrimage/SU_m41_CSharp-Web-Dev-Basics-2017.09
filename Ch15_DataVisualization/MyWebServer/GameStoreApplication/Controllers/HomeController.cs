namespace MyWebServer.GameStoreApplication.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using GameStoreApplication.Services.Contracts;
    using GameStoreApplication.Services;
    using MyWebServer.GameStoreApplication.ViewModels.Admin;
    using MyWebServer.Server.Http;
    using Server.Http.Contracts;

    public class HomeController : BaseController
    {
        private const string HomeView = @"home\index";
        private const string DetailsView = @"home\details";

        private readonly IGameService games;


        public HomeController(IHttpRequest request)
            : base(request)
        {
            this.games = new GameService();
        }


        public IHttpResponse Index()
        {
            // Implemented only for all games.
            // Filter is not omplemented

            StringBuilder sb= new StringBuilder();

            var allGames = this.games.AllGamesFullInfo();

            string groupStart = @"<div class=""card-group"">";
            string groupEnd = @"</div>";

            sb.AppendLine(groupStart);

            int num = 0;
            
            foreach (GameViewModel game in allGames)
            {
                string description = game.Description;
                if (game.Description.Length>300)
                {
                    description = game.Description.Substring(0, 300);
                }

                if (num==0 || num % 3 == 0)
                {
                    sb.AppendLine(groupStart);
                }
                
                sb.AppendLine(@"<div class=""card col-4 thumbnail"">");
                sb.AppendLine($@"<img class=""card-image-top img-fluid img-thumbnail"" onerror=""this.src = '{game.Image}';"" src=""{game.Image}"">");
                sb.AppendLine($@"<div class=""card-body"" >");
                sb.AppendLine($@"<h4 class=""card-title"" >{game.Title}</h4>");
                sb.AppendLine($@"<p class=""card-text"" > <strong>Price</strong> - {game.Price}&euro; </p>");
                sb.AppendLine($@"<p class=""card-text"" > <strong>Size</strong> - {game.Size} GB </p>");
                sb.AppendLine($@"<p class=""card-text"" >{description} </p>");
                sb.AppendLine($@"</div>");
                sb.AppendLine($@"<div class=""card-footer"" >");
                sb.AppendLine(
                    $@"<a hidden class=""card-button btn btn-warning"" name=""edit"" href=""/admin/games/edit/{game.Id}"">Edit</a>");
                sb.AppendLine(
                    $@"<a hidden class=""card-button btn btn-danger"" name=""delete"" href=""/admin/games/delete/{game.Id}"">Delete</a>");
                sb.AppendLine(
                    $@"<a class=""card-button btn btn-outline-primary"" name=""info"" href=""/games/info/{game.Id}"">Info</a>");
                sb.AppendLine($@"<a class=""card-button btn btn-primary"" name=""buy"" href=""#"" >Buy</a>");
                sb.AppendLine(@"</div>");
                sb.AppendLine(@"</div>");

                if (num+1 % 3 == 0)
                {
                    sb.AppendLine(groupEnd);
                }

                num++;
            }

            if (this.Authentication.IsAdmin)
            {
                sb = sb.Replace("hidden", "");
            }

            this.ViewData["set-of-games"] = sb.ToString();

            return this.FileViewResponse(@"home\index");
        }




        public IHttpResponse Details()
        {
            int id = int.Parse(this.Request.UrlParameters["id"]);
            AdminEditGameViewModel game = this.games.GetGameById(id);

            this.ViewData["buy-button"] = "inline";
            if (!this.Authentication.IsAuthenticated)
            {
                return this.RedirectResponse(HomePath);
                this.ViewData["buy-button"] = "none";
            }


            //this.ViewData["bought-info"] = "none";
            //if (this.Request.UrlParameters.ContainsKey("bought") && this.Request.UrlParameters["bought"] == "success")
            //{
            //    this.ViewData["bought-info"] = "block";
            //}

            this.ViewData["game-title"] = game.Title;
            this.ViewData["game-videoUrl"] = game.VideoId;
            this.ViewData["game-description"] = game.Description;
            this.ViewData["game-price"] = game.Price.ToString();
            this.ViewData["game-size"] = game.Size.ToString();
            this.ViewData["game-release"] = game.ReleaseDate.ToShortDateString();
            this.ViewData["game-id"] = game.Id.ToString();

            if (this.Authentication.IsAdmin)
            {
                this.ViewData["admin-set"] = "inline";
            }
            else
            {
                this.ViewData["admin-set"] = "none";
            }

            return this.FileViewResponse(DetailsView);
        }

    }
}