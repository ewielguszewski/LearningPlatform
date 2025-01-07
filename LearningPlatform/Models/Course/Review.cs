using LearningPlatform.Models.User;

namespace LearningPlatform.Models.Course
{
    public class Review
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
