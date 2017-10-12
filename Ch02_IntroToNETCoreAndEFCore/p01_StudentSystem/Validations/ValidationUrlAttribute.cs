namespace p01_StudentSystem.Validation
{
    using System.ComponentModel.DataAnnotations;

    public class ValidationUrlAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var valueAsString = value as string;

            if (valueAsString==null)
            {
                // if value is not string - it passed
                return true;
            }

            var result = true;      // some logic

            this.ErrorMessage = "The provided string is not s valid URL.";

            return result;
        }
    }
}
