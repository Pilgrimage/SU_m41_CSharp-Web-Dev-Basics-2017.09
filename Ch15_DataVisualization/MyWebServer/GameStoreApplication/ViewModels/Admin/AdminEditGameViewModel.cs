namespace MyWebServer.GameStoreApplication.ViewModels.Admin
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using MyWebServer.GameStoreApplication.Common;
    using MyWebServer.GameStoreApplication.Utilities;

    public class AdminEditGameViewModel
    {
        [Required]
        [MinLength(ValidationConstants.Game.TitleMinLength,
            ErrorMessage = ValidationConstants.InvalidMinLengthErrorMessage)]
        [MaxLength(ValidationConstants.Game.TitleMaxLength,
            ErrorMessage = ValidationConstants.InvalidMaxLengthErrorMessage)]
        public string Title { get; set; }

        [Display(Name = "YouTube Video URL")]
        [Required]
        [MinLength(ValidationConstants.Game.VideoIdLength,
            ErrorMessage = ValidationConstants.ExactLengthErrorMessage)]
        [MaxLength(ValidationConstants.Game.VideoIdLength,
            ErrorMessage = ValidationConstants.ExactLengthErrorMessage)]
        public string VideoId { get; set; }

        [Required]
        [ImageUrl]
        public string Image { get; set; }

        // In GB
        public double Size { get; set; }

        public decimal Price { get; set; }

        [Required]
        [MinLength(ValidationConstants.Game.DescriptionMinLength,
            ErrorMessage = ValidationConstants.InvalidMinLengthErrorMessage)]
        public string Description { get; set; }

        [Display(Name = "Release Date")]
        [Required]
        public DateTime ReleaseDate { get; set; }

        public int Id { get; set; }
    }
}