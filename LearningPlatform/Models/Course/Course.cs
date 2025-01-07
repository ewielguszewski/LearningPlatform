using LearningPlatform.Models.Relations;
using LearningPlatform.Models.User;

namespace LearningPlatform.Models.Course
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ThumbnailUrl { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public string AuthorId { get; set; }
        public ApplicationUser Author { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }
        public ICollection<Lesson> Lessons { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}
