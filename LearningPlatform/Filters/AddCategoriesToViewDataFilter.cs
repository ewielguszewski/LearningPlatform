using LearningPlatform.Data;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LearningPlatform.Filters
{
    public class AddCategoriesToViewDataFilter : IActionFilter
    {
        private readonly ApplicationDbContext _context;

        public AddCategoriesToViewDataFilter(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var categories = _context.Categories.ToList();
            context.HttpContext.Items["Categories"] = categories;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
