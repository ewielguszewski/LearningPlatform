using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LearningPlatform.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using LearningPlatform.Models.User;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options => 
                            options.UseSqlServer(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
                            .AddEntityFrameworkStores<ApplicationDbContext>()
                            .AddDefaultTokenProviders();



builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();


//builder.Services.AddRazorPages(options =>
//{
//    options.Conventions.AddAreaPageRoute("Identity", "/Account/Login", "/Account/Login");
//    options.Conventions.AddAreaPageRoute("Identity", "/Account/Register", "/Account/Register");
//});


builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";

    options.SlidingExpiration = true;

    options.Events.OnValidatePrincipal = context =>
    {
        var userPrincipal = context.Principal;
        if (userPrincipal != null)
        {
            if (userPrincipal.IsInRole("Admin"))
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
            }
            else
            {
                options.ExpireTimeSpan = TimeSpan.FromDays(14);
            }
        }

        return Task.CompletedTask;
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
var context = services.GetRequiredService<ApplicationDbContext>();

await SeedData.Initialize(services, userManager, roleManager, context);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Courses}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();

