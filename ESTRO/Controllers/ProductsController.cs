using Microsoft.AspNetCore.Mvc;
using ESTRO.Data;
using ESTRO.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace ESTRO.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ApplicationDbContext _context;

        public ProductsController(
            ApplicationDbContext context,
            IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // SHOW ALL PRODUCTS
        public IActionResult Index()
        {
            var products = _context.Products.ToList();

            return View(products);
        }

        // CREATE PRODUCT PAGE
        public IActionResult Create()
        {
            return View();
        }

        // SAVE PRODUCT
        [HttpPost]
        public async Task<IActionResult> Create(
            Product product,
            IFormFile imageFile)
        {
            if (imageFile != null)
            {
                string folder = Path.Combine(
                    _environment.WebRootPath,
                    "images");

                string fileName = Guid.NewGuid().ToString()
                    + Path.GetExtension(imageFile.FileName);

                string filePath = Path.Combine(folder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }

                product.ImageUrl = "/images/" + fileName;
            }

            _context.Products.Add(product);

            await _context.SaveChangesAsync();

            return RedirectToAction("Dashboard", "Admin");
        }

        // PRODUCT DETAILS
        public IActionResult Details(int id)
        {
            var product = _context.Products
                .FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            var relatedProducts = _context.Products
                .Where(p =>
                    p.Category == product.Category &&
                    p.Id != product.Id)
                .Take(4)
                .ToList();

            ViewBag.RelatedProducts = relatedProducts;

            ViewBag.Reviews = _context.Reviews
                .Where(r => r.ProductId == id)
                .ToList();

            return View(product);
        }

        // CATEGORY PRODUCTS
        public IActionResult Category(string category)
        {
            var products = _context.Products
                .Where(p => p.Category == category)
                .ToList();

            return View(products);
        }

        // EDIT PRODUCT PAGE
        public IActionResult Edit(int id)
        {
            var product = _context.Products.Find(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // UPDATE PRODUCT
        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Update(product);

                _context.SaveChanges();

                return RedirectToAction("Dashboard", "Admin");
            }

            return View(product);
        }

        // DELETE PRODUCT
        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);

            if (product != null)
            {
                _context.Products.Remove(product);

                _context.SaveChanges();
            }

            return RedirectToAction("Dashboard", "Admin");
        }

        // SEARCH PRODUCTS
        public IActionResult Search(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return RedirectToAction("Index");
            }

            var products = _context.Products
                .Where(p =>
                    p.Name.ToLower().Contains(searchTerm.ToLower()) ||

                    (p.Category != null &&
                     p.Category.ToLower().Contains(searchTerm.ToLower()))
                )
                .ToList();

            return View(products);
        }

        // ADD REVIEW
        [HttpPost]
        public IActionResult AddReview(Review review)
        {
            _context.Reviews.Add(review);

            _context.SaveChanges();

            return RedirectToAction(
                "Details",
                new { id = review.ProductId });
        }
    }
}