using ESTRO.Data;
using ESTRO.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ESTRO.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CheckoutController(ApplicationDbContext context)
        {
            _context = context;
        }

        // CHECKOUT PAGE
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
        // PLACE ORDER
        [HttpPost]
        public IActionResult Index(Order order)
        {
            if (ModelState.IsValid)
            {
                var email = User.Identity?.Name;

                var cartItems = _context.CartItems
                    .Include(c => c.Product)
                    .Where(c => c.UserEmail == email)
                    .ToList();

                if (!cartItems.Any())
                {
                    ModelState.AddModelError("", "Your cart is empty.");
                    return View(order);
                }

                // GENERATE ORDER NUMBER
                order.OrderNumber =
                    "EST-" + DateTime.Now.Ticks.ToString().Substring(10);

                order.Status = "Pending";

                order.UserEmail = email;

                // CALCULATE TOTAL
                order.TotalAmount = cartItems.Sum(c =>
                    (c.Product?.Price ?? 0) * c.Quantity);

                // SAVE ORDER
                _context.Orders.Add(order);

                // REDUCE STOCK
                foreach (var item in cartItems)
                {
                    if (item.Product != null)
                    {
                        item.Product.Stock -= item.Quantity;

                        if (item.Product.Stock < 0)
                        {
                            item.Product.Stock = 0;
                        }
                    }
                }

                // CLEAR CART
                _context.CartItems.RemoveRange(cartItems);

                _context.SaveChanges();

                // RESET CART COUNT
                HttpContext.Session.SetInt32("CartCount", 0);

                return RedirectToAction("Success");
            }

            return View(order);
        }

        // CUSTOMER ORDERS
        public IActionResult MyOrders()
        {
            var email = User.Identity?.Name;

            var orders = _context.Orders
                .Where(o => o.UserEmail == email)
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            return View(orders);
        }

        // SUCCESS PAGE
        public IActionResult Success()
        {
            return View();
        }
    }
}