using Microsoft.AspNetCore.Mvc;
using ESTRO.Data;
using Microsoft.AspNetCore.Authorization;

namespace ESTRO.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // DASHBOARD
        public IActionResult Dashboard()
        {
            var products = _context.Products.ToList();

            return View(products);
        }

        // ORDERS PAGE
        public IActionResult Orders(string searchOrder)
        {
            var orders = _context.Orders.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchOrder))
            {
                orders = orders.Where(o =>
                    o.OrderNumber.Contains(searchOrder));
            }

            return View(
                orders.OrderByDescending(o => o.OrderDate).ToList()
            );
        }

        // NEXT STATUS
        public IActionResult NextStatus(int id)
        {
            var order = _context.Orders
                .FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            switch (order.Status)
            {
                case "Pending":
                    order.Status = "Confirmed";
                    break;

                case "Confirmed":
                    order.Status = "Processing";
                    break;

                case "Processing":
                    order.Status = "Delivering";
                    break;

                case "Delivering":
                    order.Status = "Delivered";
                    break;
            }

            _context.SaveChanges();

            return RedirectToAction("Orders");
        }
    }
}