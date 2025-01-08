using LearningPlatform.Data;
using LearningPlatform.Models.Course;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LearningPlatform.Dtos.Courses;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace LearningPlatform.Controllers
{
    [Authorize(Roles = "Admin,Instructor")]
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CoursesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: CoursesController
        [AllowAnonymous]
        public async Task<ActionResult> Index(string searchQuery, string categoryName)
        {
            IQueryable<Course> coursesQuery = _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Author);

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                coursesQuery = coursesQuery.Where(c => c.Title.Contains(searchQuery));
            }

            if (!string.IsNullOrEmpty(categoryName))
            {
                coursesQuery = coursesQuery.Where(c => c.Category.Name == categoryName);
            }

            var courses = await coursesQuery.ToListAsync();

            ViewData["SearchQuery"] = searchQuery;
            ViewData["SelectedCategoryName"] = categoryName;
            ViewData["Categories"] = await _context.Categories.ToListAsync();

            return View(courses);
        }

        // GET: CoursesController/Details/5
        [AllowAnonymous]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.Category)
                .Include(c => c.Author)
                .Include(c => c.Enrollments)
                .Include(c => c.Lessons)
                .ThenInclude(c => c.LessonContents)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
            {
                return NotFound();
            }

            course.Lessons = course.Lessons ?? new List<Lesson>();

            return View(course);
        }

        // GET: CoursesController/Create
        public ActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: CoursesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateCourseDto courseDto)
        {
            if (ModelState.IsValid)
            {
                var course = new Course
                {
                    Title = courseDto.Title,
                    Description = courseDto.Description,
                    Price = courseDto.Price,
                    StartDate = courseDto.StartDate,
                    CategoryId = courseDto.CategoryId
                };
                _context.Add(course);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", courseDto.CategoryId);
            return View(courseDto);
        }

        // GET: CoursesController/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.Category)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (course == null)
            {
                return NotFound();
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (course.AuthorId != currentUserId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            var courseDto = new EditCourseDto
            {
                Id = course.Id,
                Title = course.Title,
                Description = course.Description,
                Price = course.Price,
                CategoryId = course.CategoryId
            };

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", course.CategoryId);

            return View(courseDto);
        }

        // POST: CoursesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, EditCourseDto courseDto)
        {
            if (id != courseDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var course = await _context.Courses
                    .FindAsync(id);

                if (course == null)
                {
                    return NotFound();
                }

                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (course.AuthorId != currentUserId && !User.IsInRole("Admin"))
                {
                    return Forbid();
                }

                course.Title = courseDto.Title;
                course.Description = courseDto.Description;
                course.Price = courseDto.Price;
                course.CategoryId = courseDto.CategoryId;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_context.Courses.All(e => e.Id != courseDto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", courseDto.CategoryId);
            return View(courseDto);
        }

        // GET: CoursesController/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .Include(c => c.Author)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (course == null)
            {
                return NotFound();
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (course.AuthorId != currentUserId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            return View(course);
        }

        // POST: CoursesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            var course = await _context.Courses
                .FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (course.AuthorId != currentUserId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
