using LearningPlatform.Models.Cart;
using LearningPlatform.Models.Course;
using LearningPlatform.Models.Order;
using LearningPlatform.Models.Relations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LearningPlatform.Models.User;
using System.Reflection.Emit;
using LearningPlatform.Models.User.Configuration;

namespace LearningPlatform.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Course> Courses { get; set; }
    public DbSet<Lesson> Lessons { get; set; }
    public DbSet<LessonContent> LessonContents { get; set; }
    public DbSet<Progress> Progresses { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Course>()
        .Property(c => c.Price)
        .HasColumnType("decimal(18,2)");

        builder.Entity<Course>()
        .HasOne(c => c.Author)
        .WithMany()
        .HasForeignKey(c => c.AuthorId)
        .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Order>()
            .Property(o => o.TotalAmount)
            .HasColumnType("decimal(18,2)");

        builder.Entity<OrderItem>()
            .Property(o => o.Price)
            .HasColumnType("decimal(18,2)");

        builder.Entity<CartItem>()
            .Property(o => o.Price)
            .HasColumnType("decimal(18,2)");

        builder.ApplyConfiguration(new ApplicationUserConfiguration());
    }
}
