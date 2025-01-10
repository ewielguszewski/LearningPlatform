using LearningPlatform.Models.Course;
using LearningPlatform.Models.User;

namespace LearningPlatform.Models.Relations
{
    public class UserLessonProgress
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }
        public DateTime VisitedAt { get; set; }
    }
}
