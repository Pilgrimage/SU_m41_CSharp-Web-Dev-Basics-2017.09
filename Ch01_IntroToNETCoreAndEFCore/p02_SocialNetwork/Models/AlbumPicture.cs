namespace p02_SocialNetwork.Models
{
    public class AlbumPicture
    {
        public int AlbumId { get; set; }
        public Album Album { get; set; }

        public int PictureId { get; set; }
        public Picture Picture { get; set; }
    }
}