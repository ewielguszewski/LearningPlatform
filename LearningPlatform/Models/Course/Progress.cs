using LearningPlatform.Models.User;
using System.ComponentModel.DataAnnotations.Schema;

namespace LearningPlatform.Models.Course
{
    public class Progress
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }

        public int RecentLessonId { get; set; }
        [ForeignKey("RecentLessonId")]
        public Lesson Lesson { get; set; }

        public DateTime LastAccessed { get; set; }
        public double CompletionPercentage { get; set; }
    }
}
