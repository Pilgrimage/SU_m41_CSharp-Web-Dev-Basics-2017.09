namespace MyWebServer.GameStoreApplication.Utilities
{
    using System.ComponentModel.DataAnnotations;
    using System.Text.RegularExpressions;

    public class EmailAttribute : ValidationAttribute
    {
        public EmailAttribute()
        {
            this.ErrorMessage = "Email must contain @ sign and a period.";
        }

        public override bool IsValid(object value)
        {
            string email = value as string;
            if (email == null)
            {
                return true;
            }

            const string emailPattern = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,4})+)$";

            return Regex.IsMatch(email, emailPattern);
        }

    }
}