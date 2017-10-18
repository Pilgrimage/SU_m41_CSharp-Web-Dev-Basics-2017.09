namespace MyWebServer.GameStoreApplication.Utilities
{
    using System.ComponentModel.DataAnnotations;

    public class ImageUrlAttribute : ValidationAttribute
    {
        private const string protocol = "http://";
        private const string protocolSecure = "https://";

        public ImageUrlAttribute()
        {
            this.ErrorMessage = "Thumbnail URL should be a plain text starting with 'http://', 'https://' or being null";
        }

        public override bool IsValid(object value)
        {
            string imageUrl = value as string;
            if (imageUrl == null)
            {
                return true;
            }
            
            return imageUrl.StartsWith(protocol) || imageUrl.StartsWith(protocolSecure);
        }

    }
}