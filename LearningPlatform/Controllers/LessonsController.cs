using LearningPlatform.Data;
using LearningPlatform.Dtos.Lessons;
using LearningPlatform.Models.Course;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System.Security.Claims;

namespace LearningPlatform.Controllers
{
    public class LessonsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LessonsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Lessons/Create
        public IActionResult Create(int courseId)
        {
            var model = new CreateLessonDto
            {
                CourseId = courseId
            };

            return View(model);
        }

        // POST: Lessons/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateLessonDto model)
        {
            if (ModelState.IsValid)
            {
                var lesson = new Lesson
                {
                    Title = model.Title,
                    CourseId = model.CourseId,
                    Duration = model.Duration
                };

                _context.Lessons.Add(lesson);
                await _context.SaveChangesAsync();

                return RedirectToAction("Details", "Courses", new { id = model.CourseId });
            }

            return View(model);
        }

        // GET: Lessons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lesson = await _context.Lessons
                .FirstOrDefaultAsync(l => l.Id == id);


            if (lesson == null)
            {
                return NotFound();
            }

            var model = new EditLessonDto
            {
                Id = lesson.Id,
                Title = lesson.Title,
                Duration = lesson.Duration,
                CourseId = lesson.CourseId
            };

            return View(model);
        }

        // POST: Lessons/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditLessonDto model)
        {
            if (ModelState.IsValid)
            {
                var lesson = await _context.Lessons.FindAsync(model.Id);

                if (lesson == null)
                {
                    return NotFound();
                }

                lesson.Title = model.Title;
                lesson.Duration = model.Duration;

                await _context.SaveChangesAsync();

                return RedirectToAction("Details", "Courses", new { id = model.CourseId });
            }
            return View(model);
        }

        // GET: Lessons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lesson = await _context.Lessons
                .Include(l => l.LessonContents)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (lesson == null)
            {
                return NotFound();
            }

            return View(lesson);
        }

        // POST: Lessons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lesson = await _context.Lessons.FindAsync(id);
            _context.Lessons.Remove(lesson);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Courses", new { id = lesson.CourseId });
        }

        public async Task<IActionResult> Learn(int id)
        {
            var lesson = await _context.Lessons
                .Include(l => l.LessonContents)
                .Include(l => l.Course)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (lesson == null)
            {
                return NotFound();
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isEnrolled = await _context.Enrollments
                .AnyAsync(e => e.UserId == currentUserId && e.CourseId == lesson.CourseId);

            var isAdmin = User.IsInRole("Admin");
            var course = lesson.Course;
            var isAuthor = course != null && course.AuthorId == currentUserId;

            if (!isEnrolled && !isAdmin && !isAuthor)
            {
                return Forbid();
            }

            var previousLesson = await _context.Lessons
           .Where(l => l.CourseId == lesson.CourseId && l.Id < lesson.Id)
           .OrderByDescending(l => l.Id)
           .FirstOrDefaultAsync();

            var nextLesson = await _context.Lessons
                .Where(l => l.CourseId == lesson.CourseId && l.Id > lesson.Id)
                .OrderBy(l => l.Id)
                .FirstOrDefaultAsync();

            ViewBag.PreviousLessonId = previousLesson?.Id;
            ViewBag.NextLessonId = nextLesson?.Id;

            return View(lesson);
        }
    }
}
