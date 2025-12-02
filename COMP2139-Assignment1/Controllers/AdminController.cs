using COMP2139_Assignment1.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace COMP2139_Assignment1.Controllers
{
    //Admins only have access
    //[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        
        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        //Admin Dashboard page
        public async Task<IActionResult> Index()
        {
            var totalEvents = await _context.Events.CountAsync();
            var totalUsers = await _context.Users.CountAsync();
            var totalPurchases = await _context.Purchases.CountAsync();

            ViewBag.TotalEvents = totalEvents;
            ViewBag.TotalUsers = totalUsers;
            ViewBag.TotalPurchases = totalPurchases;

            return View();
        }
       //Listing events for admin to stay up to date
        public async Task<IActionResult> ManageEvents()
        {
            var events = await _context.Events
                .Include(e => e.Category) // if Category is null it will still work
                .ToListAsync();

            return View(events);
        }
        
        // Read only page that shows the admin all the current users
        public async Task<IActionResult> ManageUsers()
        {
            // _context.Users comes from IdentityDbContext<ApplicationUser>
            var users = await _context.Users
                .OrderBy(u => u.Email)
                .ToListAsync();

            return View(users);
        }

        // Allows admin to look at the purchases
        public async Task<IActionResult> ManagePurchases()
        {
            var purchases = await _context.Purchases
                .Include(p => p.Event)
                .ToListAsync();

            return View(purchases);
        }
    }
}