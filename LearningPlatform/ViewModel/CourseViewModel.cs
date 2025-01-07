namespace LearningPlatform.ViewModel
{
    public class CourseViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ThumbnailUrl { get; set; }
        public decimal Price { get; set; }
        public int TotalStudents { get; set; }
        public double AverageRating { get; set; }
    }
}
