namespace MyWebServer.GameStoreApplication.ViewModels.Admin
{
    using System;

    public class GameViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string VideoId { get; set; }

        public string Image { get; set; }

        public double Size { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public DateTime ReleaseDate { get; set; }
    }
}