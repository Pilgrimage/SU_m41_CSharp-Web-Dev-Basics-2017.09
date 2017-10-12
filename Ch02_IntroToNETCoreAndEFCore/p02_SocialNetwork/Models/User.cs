namespace p02_SocialNetwork.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Validations;

    public class User
    {
        public int Id { get; set; }

        [Required]
        [MinLength(4)]
        [MaxLength(30)]
        public string Username { get; set; }

        [Required]
        [Password]
        [MinLength(6)]
        [MaxLength(50)]
        public string Password { get; set; }

        [Required]
        [Email]
        public string Email { get; set; }

        // Max Length in elements - in MS SQL Server up to 4000 bytes or max (2GB)
        [MaxLength(1024)]
        public byte[] ProfilePicture { get; set; }

        public DateTime? RegisteredOn { get; set; }

        public DateTime? LastTimeLoggedIn { get; set; }

        [Range(1,120)]
        public int? Age { get; set; }

        public bool IsDeleted { get; set; }


        public ICollection<Friendship> FromFriends { get; set; } = new List<Friendship>();

        public ICollection<Friendship> ToFriends { get; set; } = new List<Friendship>();

        // Created Albums
        public ICollection<Album> Albums { get; set; } = new List<Album>();

        // Owned Albums
        public ICollection<AlbumUser> OwnedAlbums { get; set; } = new List<AlbumUser>();

    }
}