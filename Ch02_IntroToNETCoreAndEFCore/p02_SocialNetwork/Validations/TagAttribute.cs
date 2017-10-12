namespace p02_SocialNetwork.Validations
{
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class TagAttribute : ValidationAttribute
    {
        public TagAttribute()
        {
            this.ErrorMessage = "Tag is invalid!";
        }
        
        public override bool IsValid(object value)
        {
            string tag = value as string;
            if (tag == null)
            {
                return true;
            }

            return tag.StartsWith("#") && 
                   tag.All(ch => !char.IsWhiteSpace(ch)) && 
                   tag.Length<=20;
        }
    }
}