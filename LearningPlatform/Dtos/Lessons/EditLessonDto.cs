using System.ComponentModel.DataAnnotations;

namespace LearningPlatform.Dtos.Lessons
{
    public class EditLessonDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        public int CourseId { get; set; }

        [Range(1, 300)]
        public int Duration { get; set; }
    }
}
