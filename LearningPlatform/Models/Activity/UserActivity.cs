using LearningPlatform.Models.Course;
using LearningPlatform.Models.User;

namespace LearningPlatform.Models.Activity
{
    public class UserActivity
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int CourseId { get; set; }
        public LearningPlatform.Models.Course.Course Course { get; set; }
        public int? LessonId { get; set; }
        public Lesson Lesson { get; set; }
        public DateTime LastAccessed { get; set; }
    }

}
