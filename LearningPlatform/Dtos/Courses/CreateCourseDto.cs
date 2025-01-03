namespace LearningPlatform.Dtos.Courses
{
    public class CreateCourseDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public int CategoryId { get; set; }
    }
}
