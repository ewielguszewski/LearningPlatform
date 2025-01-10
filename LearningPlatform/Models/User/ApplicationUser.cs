using LearningPlatform.Models.Relations;
using LearningPlatform.Models.Course;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace LearningPlatform.Models.User
{
    [Index(nameof(Nickname), IsUnique = true)]
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Nickname {  get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public string ProfilePictureUrl { get; set; } = string.Empty;
        public DateTime RegisteredDate { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
        public ICollection<Progress> Progresses { get; set; }

        public ApplicationUser()
        {
            RegisteredDate = DateTime.Now;
        }
    }
}
