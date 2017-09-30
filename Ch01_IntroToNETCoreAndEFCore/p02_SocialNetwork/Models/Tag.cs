namespace p02_SocialNetwork.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Validations;

    public class Tag
    {
        public int Id { get; set; }

        [Required]
        [Tag]
        [MaxLength(20)]
        public string Name { get; set; }

        public ICollection<AlbumTag> Albums { get; set; } = new List<AlbumTag>();
    }
}
