using LearningPlatform.Models.Course;
using LearningPlatform.Models.User;
using Microsoft.AspNetCore.Identity;

namespace LearningPlatform.Data
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            await CreateRoles(roleManager);

            await CreateAdminUser(userManager);

            await CreateCategories(context);
        }

        private static async Task CreateRoles(RoleManager<IdentityRole> roleManager)
        {
            var roleNames = new[] { "Admin", "User" };

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

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
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
    }
}
