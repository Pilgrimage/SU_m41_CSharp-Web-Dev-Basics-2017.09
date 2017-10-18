namespace MyWebServer.GameStoreApplication.ViewModels.Account
{
    using System.ComponentModel.DataAnnotations;
    using GameStoreApplication.Utilities;

    public class LoginViewModel
    {
        [Required]
        [Email]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

    }
}