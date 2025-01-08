using LearningPlatform.Data;
using LearningPlatform.Models.Relations;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using LearningPlatform.Models.Cart;
using Microsoft.EntityFrameworkCore;

namespace LearningPlatform.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /cart
        public async Task <IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(i => i.Course)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                return View(new List<CartItem>());
            }

            return View(cart.CartItems);
        }

        // POST: /cart/add
        [HttpPost]
        public async Task<IActionResult> AddToCart(int courseId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            var course = await _context.Courses.FirstOrDefaultAsync(c => c.Id == courseId);
            if (course == null)
            {
                return NotFound("Course not found");
            }

            if (cart == null)
            {
                cart = new Cart { UserId = userId, CartItems = new List<CartItem>() };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            var existingItem = cart.CartItems.FirstOrDefault(i => i.CourseId == courseId);
            if (existingItem == null)
            {
                cart.CartItems.Add(new CartItem
                {
                    CourseId = courseId,
                    Cart = cart
                });

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Cart");
        }

        // POST: /cart/remove
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int courseId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                return RedirectToAction("Index", "Cart");
            }

            var itemToRemove = cart.CartItems.FirstOrDefault(i => i.CourseId == courseId);

            if (itemToRemove != null)
            {
                cart.CartItems.Remove(itemToRemove);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index", "Cart");
        }

        // GET: /cart/checkout
        public async Task<IActionResult> Checkout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(i => i.Course)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || cart.CartItems.Count == 0)
            {
                return RedirectToAction("Index", "Cart");
            }


            return View(cart);
        }


        // POST: /cart/realizeorder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RealizeOrder()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(i => i.Course)
                .FirstOrDefaultAsync(c => c.UserId == userId);


            var selectedPaymentMethod = Request.Form["paymentMethod"];

            if (selectedPaymentMethod == "free")
            {
                foreach (var item in cart.CartItems)
                {
                    var enrollment = new Enrollment
                    {
                        UserId = userId,
                        CourseId = item.CourseId,
                        EnrollmentDate = DateTime.Now,
                    };
                    _context.Enrollments.Add(enrollment);
                }

                _context.Carts.Remove(cart);

                await _context.SaveChangesAsync();

                return RedirectToAction("PersonalDashboard", "Users");
            }
            else
            {
                return RedirectToAction("Index", "Cart");
            }
        }
    }
}
