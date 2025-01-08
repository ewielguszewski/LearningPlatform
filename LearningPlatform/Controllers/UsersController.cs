using LearningPlatform.Data;
using LearningPlatform.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LearningPlatform.ViewModel;

namespace LearningPlatform.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Route("user/{nickname}")]
        public async Task<IActionResult> UserProfile(string nickname)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Nickname == nickname);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var createdCourses = await _context.Courses
                .Where(c => c.AuthorId == user.Id)
                .Include(c => c.Enrollments)
                .Include(c => c.Reviews)
                .ToListAsync();

            var purchasedCourses = await _context.Courses
                .Where(c => c.Enrollments.Any(e => e.UserId == user.Id))
                .Include(c => c.Enrollments)
                .Include(c => c.Reviews)
                .ToListAsync();



            var model = new UserProfileViewModel
            {
                FullName = $"{user.FirstName} {user.LastName}",
                Bio = user.Bio,
                ProfilePictureUrl = user.ProfilePictureUrl,
                IsInstructor = createdCourses.Any(),
                CreatedCourses = createdCourses.Select(c => new CourseViewModel
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    ThumbnailUrl = c.ThumbnailUrl,
                    Price = c.Price,
                    TotalStudents = c.Enrollments.Count,
                    AverageRating = c.Reviews.Any() ? c.Reviews.Average(r => r.Rating) : 0
                }).ToList(),
                PurchasedCourses = purchasedCourses.Select(c => new CourseViewModel
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    ThumbnailUrl = c.ThumbnailUrl,
                    Price = c.Price,
                    TotalStudents = c.Enrollments.Count,
                    AverageRating = c.Reviews.Any() ? c.Reviews.Average(r => r.Rating) : 0
                }).ToList(),
                TotalStudents = createdCourses.Sum(c => c.Enrollments.Count),
                TotalReviews = createdCourses.Sum(c => c.Reviews.Count),
                MemberSince = user.RegisteredDate,
                AverageRating = createdCourses
                    .Where(c => c.Reviews.Any())
                    .Select(c => c.Reviews.Average(r => r.Rating))
                    .DefaultIfEmpty(0)
                    .Average()

            };
            return View(model);
        }


        [Authorize]
        [Route("personal")]
        public async Task<IActionResult> PersonalDashboard()
        {
            var user = await _userManager.GetUserAsync(User);

            var recentlyViewed = _context.UserActivities
                .Where(ua => ua.UserId == user.Id)
                .OrderByDescending(ua => ua.LastAccessed)
                .Take(5)
                .Select(ua => ua.Course)
                .ToList();

            var inProgressCourses = await _context.Enrollments
                .Where(e => e.UserId == user.Id && !e.IsCompleted)
                .Include(e => e.Course)
                .ThenInclude(c => c.Reviews)
                .Select(e => e.Course)
                .ToListAsync();

            var model = new PersonalDashboardViewModel
            {
                User = user,
                RecentlyViewed = recentlyViewed,
                InProgressCourses = inProgressCourses
            };

            return View(model);
        }
    }
}
