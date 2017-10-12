namespace p01_StudentSystem.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Course
    {
        // id, name, description (optional), start date, end date, price

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal Price { get; set; }

        public ICollection<StudentCourse> Students { get; set; } = new List<StudentCourse>();

        public ICollection<Resource> Resources { get; set; } = new List<Resource>();

        public ICollection<Homework> Homeworks { get; set; } = new List<Homework>();
    }
}
