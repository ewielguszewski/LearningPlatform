namespace LearningPlatform.ViewModel
{
    public class UserProfileViewModel
    {
        public string FullName { get; set; }
        public string Bio { get; set; }
        public string ProfilePictureUrl { get; set; }

        public bool IsInstructor { get; set; }
        public List<CourseViewModel> CreatedCourses { get; set; }

        public List<CourseViewModel> PurchasedCourses { get; set; }

        public int TotalStudents { get; set; }
        public int TotalReviews { get; set; }
        public double AverageRating { get; set; }

        public DateTime MemberSince { get; set; }
    }

}
