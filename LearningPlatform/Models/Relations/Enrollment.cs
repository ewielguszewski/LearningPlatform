
namespace LearningPlatform.Models.Relations
{
    using LearningPlatform.Models.User;
    using LearningPlatform.Models.Course;
    public class Enrollment
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public bool IsCompleted { get; set; }
    }
}
