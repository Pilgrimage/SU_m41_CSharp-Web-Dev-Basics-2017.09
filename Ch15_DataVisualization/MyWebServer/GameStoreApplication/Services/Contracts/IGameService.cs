﻿namespace MyWebServer.GameStoreApplication.Services.Contracts
{
    using System;
    using System.Collections.Generic;
    using GameStoreApplication.ViewModels.Admin;

    public interface IGameService
    {
        void Create(
            string title,
            string description,
            string image,
            decimal price,
            double size,
            string videoId,
            DateTime releaseDate);

        IEnumerable<AdminListGameViewModel> All();

        IEnumerable<GameViewModel> AllGamesFullInfo();

        AdminEditGameViewModel GetGameById(int id);

        bool Edit(AdminEditGameViewModel editedGame);

        bool Delete(int id);
    }
}