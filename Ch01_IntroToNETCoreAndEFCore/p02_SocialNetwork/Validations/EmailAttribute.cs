namespace p02_SocialNetwork.Validations
{
    using System.ComponentModel.DataAnnotations;
    using System.Text.RegularExpressions;

    public class EmailAttribute : ValidationAttribute
    {
        private const string Pattern = @"^([a-zA-z0-9][a-zA-z0-9.\-_]*[a-zA-z0-9])@(\w+(?>.\w+)+)$";

        public EmailAttribute()
        {
            this.ErrorMessage = "Email is invalid!";
        }

        public override bool IsValid(object value)
        {
            string email = value as string;

            if (email == null)
            {
                return true;
            }

            return Regex.IsMatch(email, Pattern);
        }
    }
}