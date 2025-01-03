using LearningPlatform.Models.Relations;
using Microsoft.AspNetCore.Identity;

namespace LearningPlatform.Models.User
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
