using LearningPlatform.Models.Course;
using LearningPlatform.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LearningPlatform.Data
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            await CreateRoles(roleManager);

            await CreateAdminUser(userManager);

            await CreateCategories(context);

            await CreateCourses(context);
        }

        private static async Task CreateRoles(RoleManager<IdentityRole> roleManager)
        {
            var roleNames = new[] { "Admin", "Instructor", "User" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    var role = new IdentityRole(roleName);
                    await roleManager.CreateAsync(role);
                }
            }
        }

        private static async Task CreateAdminUser(UserManager<ApplicationUser> userManager)
        {
            var adminEmail = "admin@admin.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            var instructorEmail = "instructor@instructor.com";
            var instructorUser = await userManager.FindByEmailAsync(instructorEmail);

            var userEmail = "user@user.com";
            var user = await userManager.FindByEmailAsync(userEmail);
            
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "Adminov",
                    Nickname = "Admin123"
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
                else
                {
                    throw new InvalidOperationException("Failed to create admin user.");
                }
            }

            if (instructorUser == null)
            {
                instructorUser = new ApplicationUser
                {
                    UserName = instructorEmail,
                    Email = instructorEmail,
                    FirstName = "Instructor",
                    LastName = "Instructorov",
                    Nickname = "Instructor123"
                };

                var result = await userManager.CreateAsync(instructorUser, "Instructor123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(instructorUser, "Instructor");
                }
                else
                {
                    throw new InvalidOperationException("Failed to create instructor user.");
                }
            }

            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = userEmail,
                    Email = userEmail,
                    FirstName = "User",
                    LastName = "Userov",
                    Nickname = "User123"
                };

                var result = await userManager.CreateAsync(user, "User123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "User");
                }
                else
                {
                    throw new InvalidOperationException("Failed to create user.");
                }
            }

        }

        private static async Task CreateCategories(ApplicationDbContext context)
        {
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
            {
                new Category { Name = "Programming languages" },
                new Category { Name = "Databases" },
                new Category { Name = "Machine learning" }
            };

                context.Categories.AddRange(categories);
                await context.SaveChangesAsync();
            }
        }

        private static async Task CreateCourses(ApplicationDbContext context)
        {
            if (!context.Courses.Any())
            {
                var category1 = context.Categories.FirstOrDefault(c => c.Name == "Programming languages");
                var category2 = context.Categories.FirstOrDefault(c => c.Name == "Databases");

                var instructor = await context.Users.FirstOrDefaultAsync(u => u.Email == "admin@admin.com");

                var course1 = new Course
                {
                    Title = "C# Basics",
                    Description = "Learn the basics of C# programming.",
                    Price = 500.00m,
                    Category = category1,
                    Author = instructor
                };

                var course2 = new Course
                {
                    Title = "Sql Basics",
                    Description = "Learn the basics of Sql.",
                    Price = 300.00m,
                    Category = category2,
                    Author = instructor
                };

                context.Courses.AddRange(course1, course2);
                await context.SaveChangesAsync();

                await CreateLessons(course1, context);
            }
        }

        private static async Task CreateLessons(Course course, ApplicationDbContext context)
        {
            if (course.Lessons == null)
            {
                course.Lessons = new List<Lesson>();
            }

            if (!course.Lessons.Any())
            {
                var lesson1 = new Lesson
                {
                    Title = "Introduction to C#",
                    CourseId = course.Id
                };

                var lesson2 = new Lesson
                {
                    Title = "Data Types in C#",
                    CourseId = course.Id
                };

                context.Lessons.AddRange(lesson1, lesson2);
                await context.SaveChangesAsync();
            }
        }
    }
}
