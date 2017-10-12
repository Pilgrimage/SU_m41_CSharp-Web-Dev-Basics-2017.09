namespace p02_SocialNetwork.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Album
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string BackgroundColor { get; set; }

        [Required]
        public bool IsPublic { get; set; }

        public ICollection<AlbumPicture> Pictures { get; set; } = new List<AlbumPicture>();

        // Creator
        public int OwnerId { get; set; }
        public User Owner { get; set; }

        public ICollection<AlbumTag> Tags { get; set; } = new List<AlbumTag>();

        public ICollection<AlbumUser> CoOwners { get; set; } = new List<AlbumUser>();
    }
}