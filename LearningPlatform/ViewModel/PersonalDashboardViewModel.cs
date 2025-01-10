using LearningPlatform.Models.Course;
using LearningPlatform.Models.User;

namespace LearningPlatform.ViewModel
{
    public class PersonalDashboardViewModel
    {
        public ApplicationUser User { get; set; }
        public List<Course> RecentlyViewed { get; set; }
        public List<Course> InProgressCourses { get; set; }
        public List<Course> CompletedCourses { get; set; }
    }
}
