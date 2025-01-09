namespace LearningPlatform.Models.Course
{
    public class Lesson
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public ICollection<LessonContent> LessonContents { get; set; } = new List<LessonContent>();
        public int Duration { get; set; }
        public bool IsCompleted { get; set; } = false;
    }
}
