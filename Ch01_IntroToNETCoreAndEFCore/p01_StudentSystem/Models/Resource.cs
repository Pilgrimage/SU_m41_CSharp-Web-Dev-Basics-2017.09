namespace p01_StudentSystem.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Enums;
    using Validation;

    public class Resource
    {
        // id, name, type of resource (video / presentation / document / other), URL

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ResourceType Type { get; set; }

        [Required]
        [ValidationUrl]
        public string Url { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public ICollection<License> Licenses { get; set; } = new List<License>();
    }
}
