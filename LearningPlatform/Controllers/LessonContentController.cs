using LearningPlatform.Data;
using LearningPlatform.Models.Course;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LearningPlatform.Controllers
{
    public class LessonContentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LessonContentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LessonContent/Create
        public IActionResult Create(int lessonId)
        {
            ViewData["LessonId"] = lessonId;
            return View();
        }

        // POST: LessonContent/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LessonContent lessonContent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lessonContent);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Lessons", new { id = lessonContent.LessonId });
            }
            return View(lessonContent);
        }

        // GET: LessonContent/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lessonContent = await _context.LessonContents
                .FirstOrDefaultAsync(m => m.Id == id);

            if (lessonContent == null)
            {
                return NotFound();
            }

            return View(lessonContent);
        }

        // POST: LessonContent/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, LessonContent lessonContent)
        {
            if (id != lessonContent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lessonContent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.LessonContents.Any(lc => lc.Id == lessonContent.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Lessons", new { id = lessonContent.LessonId });
            }
            return View(lessonContent);
        }

        // GET: LessonContent/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lessonContent = await _context.LessonContents
                .FirstOrDefaultAsync(m => m.Id == id);

            if (lessonContent == null)
            {
                return NotFound();
            }

            return View(lessonContent);
        }

        // POST: LessonContent/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lessonContent = await _context.LessonContents.FindAsync(id);
            _context.LessonContents.Remove(lessonContent);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Lessons", new { id = lessonContent.LessonId });
        }
    }

}
