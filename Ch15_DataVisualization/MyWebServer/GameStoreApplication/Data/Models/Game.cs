namespace MyWebServer.GameStoreApplication.Data.Models
{
    using System;
    using System.Collections.Generic;
    using GameStoreApplication.Common;
    using System.ComponentModel.DataAnnotations;
    using MyWebServer.GameStoreApplication.Utilities;

    public class Game
    {
        public int Id { get; set; }

        [Required]
        [MinLength(ValidationConstants.Game.TitleMinLength)]
        [MaxLength(ValidationConstants.Game.TitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [MinLength(ValidationConstants.Game.VideoIdLength)]
        [MaxLength(ValidationConstants.Game.VideoIdLength)]
        public string VideoId { get; set; }

        [Required]
        [ImageUrl]
        public string Image { get; set; }

        // In GB
        public double Size { get; set; }

        public decimal Price { get; set; }

        [Required]
        [MinLength(ValidationConstants.Game.DescriptionMinLength)]
        public string Description { get; set; }

        public DateTime ReleaseDate { get; set; }

        public List<UserGame> Users { get; set; } = new List<UserGame>();
    }
}