using Microsoft.AspNetCore.Mvc;
using ESTRO.Data;
using ESTRO.Models;

namespace ESTRO.Controllers
{
    public class WishlistController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WishlistController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var items = _context.WishlistItems.ToList();

            return View(items);
        }

        public IActionResult Add(int id)
        {
            var product = _context.Products
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            var item = new WishlistItem
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Price = product.Price,
                ImageUrl = product.ImageUrl
            };

            _context.WishlistItems.Add(item);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Remove(int id)
        {
            var item = _context.WishlistItems.Find(id);

            if (item != null)
            {
                _context.WishlistItems.Remove(item);

                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
