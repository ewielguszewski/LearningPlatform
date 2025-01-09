using LearningPlatform.Models.Relations;
using LearningPlatform.Models.User;
using System.ComponentModel.DataAnnotations;

namespace LearningPlatform.Models.Course
{
    public class Course
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The title is required.")]
        [StringLength(100, ErrorMessage = "The title must be at most 100 characters long.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "The description is required.")]
        [StringLength(100, ErrorMessage = "The description must be at most 100 characters long.")]
        public string Description { get; set; }
        public string ThumbnailUrl { get; set; }

        [Required(ErrorMessage = "The price is required.")]
        [Range(0.01, 1000, ErrorMessage = "The price must be between 0.01 and 1,000.")]
        public decimal Price { get; set; }

        public DateTime StartDate { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; } = new List<Enrollment>();
        public ICollection<Lesson> Lessons { get; set; } = new List<Lesson>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();

        public Course()
        {
            ThumbnailUrl = "/images/default-thumbnail.jpg";
            StartDate = DateTime.UtcNow;
        }
    }

}
