using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using COMP2139_Assignment1.Data;
using Microsoft.EntityFrameworkCore;

namespace COMP2139_Assignment1.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> MyTickets()
        {
            var purchases = await _context.Purchases
                .Include(p => p.Event)
                .ToListAsync();

            return View(purchases);
        }

        public async Task<IActionResult> PurchaseHistory()
        {
            var purchases = await _context.Purchases
                .Include(p => p.Event)
                .ToListAsync();

            return View(purchases);
        }

        [Authorize(Roles = "Admin,Organizer")]
        public async Task<IActionResult> MyEvents()
        {
            var events = await _context.Events
                .Include(e => e.Purchases)
                .ToListAsync();

            return View(events);
        }

        public IActionResult Profile()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Profile(string fullName, string phoneNumber)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            return RedirectToAction("Index");
        }
    }
}