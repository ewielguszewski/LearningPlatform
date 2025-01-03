using LearningPlatform.Models.User;

namespace LearningPlatform.Models.Course
{
    public class Progress
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int RecentLessonId { get; set; }
        public Lesson Lesson { get; set; }
    }
}
