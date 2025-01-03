
namespace LearningPlatform.Models.Course
{
    public class LessonContent
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }
        public ContentType ContentType { get; set; }
    }
}
public enum ContentType
{
    Text,
    Video,
    Image
}