using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ESTRO.Data;
using ESTRO.Models;
using Microsoft.AspNetCore.Authorization;

namespace ESTRO.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var email = User.Identity?.Name;

            var cartItems = _context.CartItems
                .Include(c => c.Product)
                .Where(c => c.UserEmail == email)
                .ToList();

            return View(cartItems);
        }

        public IActionResult AddToCart(int id)
        {
            var email = User.Identity?.Name;

            var product = _context.Products
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            var existingItem = _context.CartItems
                .FirstOrDefault(c =>
                    c.ProductId == id &&
                    c.UserEmail == email);

            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                _context.CartItems.Add(new CartItem
                {
                    ProductId = product.Id,
                    UserEmail = email,
                    Quantity = 1
                });
            }

            _context.SaveChanges();

            var cartCount = _context.CartItems
                .Where(c => c.UserEmail == email)
                .Sum(c => c.Quantity);

            HttpContext.Session.SetInt32(
                "CartCount",
                cartCount);

            return RedirectToAction("Index");
        }

        public IActionResult Remove(int id)
        {
            var email = User.Identity?.Name;

            var item = _context.CartItems
                .FirstOrDefault(c =>
                    c.Id == id &&
                    c.UserEmail == email);

            if (item != null)
            {
                _context.CartItems.Remove(item);

                _context.SaveChanges();
            }

            var cartCount = _context.CartItems
                .Where(c => c.UserEmail == email)
                .Sum(c => c.Quantity);

            HttpContext.Session.SetInt32(
                "CartCount",
                cartCount);

            return RedirectToAction("Index");
        }
    }
}