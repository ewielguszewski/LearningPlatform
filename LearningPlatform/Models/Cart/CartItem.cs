
namespace LearningPlatform.Models.Cart
{
    using LearningPlatform.Models.Course;
    public class CartItem
    {
        public int Id { get; set; }
        public int CartId { get; set; }
        public Cart Cart { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public decimal Price { get; set; }
    }
}
