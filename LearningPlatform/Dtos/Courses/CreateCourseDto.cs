using System.ComponentModel.DataAnnotations;

namespace LearningPlatform.Dtos.Courses
{
    public class CreateCourseDto
    {
        public string Title { get; set; }
        public string Description { get; set; }

        [Required(ErrorMessage = "The price is required.")]
        [Range(0.01, 1000, ErrorMessage = "The price must be between 0.01 and 1,000.")]
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
    }
}
