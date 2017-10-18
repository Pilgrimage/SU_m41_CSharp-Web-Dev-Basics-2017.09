namespace MyWebServer.GameStoreApplication.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using GameStoreApplication.Common;
    using GameStoreApplication.Utilities;

    public class User
    {
        public int Id { get; set; }

        [MinLength(ValidationConstants.Account.NameMinLength)]
        [MaxLength(ValidationConstants.Account.NameMaxLength)]

        public string Name { get; set; }

        [Required]
        [MaxLength(ValidationConstants.Account.EmailMaxLength)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(ValidationConstants.Account.PasswordMinLength)]
        [MaxLength(ValidationConstants.Account.PasswordMaxLength)]
        [Password]
        public string Password { get; set; }

        public bool IsAdmin { get; set; }

        public List<UserGame> Games { get; set; } = new List<UserGame>();
    }
}