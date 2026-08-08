using ESTRO.Data;
using ESTRO.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ESTRO.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(
            ILogger<HomeController> logger,
            ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            // ADMIN GOES TO DASHBOARD
            if (User.Identity != null &&
                User.Identity.IsAuthenticated &&
                User.IsInRole("Admin"))
            {
                return RedirectToAction(
                    "Dashboard",
                    "Admin");
            }

            // CUSTOMERS SEE HOMEPAGE
            var products = _context.Products
                .Take(8)
                .ToList();

            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]
        public IActionResult Error()
        {
            return View(
                new ErrorViewModel
                {
                    RequestId =
                        Activity.Current?.Id
                        ?? HttpContext.TraceIdentifier
                });
        }
    }
}