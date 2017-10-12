namespace p02_SocialNetwork.Models
{
    public class AlbumUser
    {
        public int AlbumId { get; set; }
        public Album Album { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}