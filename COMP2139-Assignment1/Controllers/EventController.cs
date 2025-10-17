using COMP2139_Assignment1.Data;
using COMP2139_Assignment1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace COMP2139_Assignment1.Controllers;
//The only "unusual" thing about this controller is the required methods to include categories and list them, other than that nothing unusual
public class EventController : Controller
{
     private readonly ApplicationDbContext _context; //Connection with the database

    public EventController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index(int? categoryId, string userSearch, string sortOrder, DateTime? startDate, DateTime? endDate, string availability) //Index get
    {
        ViewBag.TitleSort = sortOrder == "title_asc" ? "title_desc" : "title_asc";
        ViewBag.DateSort = sortOrder == "date_asc" ? "date_desc" : "date_asc";
        ViewBag.PriceSort = sortOrder == "price_asc" ? "price_desc" : "price_asc";
        
        var events = _context.Events.Include(e => e.Category).AsQueryable(); //Switched from Tolist to AsQueryable -A

        if (categoryId.HasValue) //filter by category
        {
            events = events.Where(e => e.CategoryId == categoryId.Value);
        }

        if (!string.IsNullOrEmpty(userSearch)) // Searching by title
        {
            events = events.Where(e => e.Title.ToLower().Contains(userSearch.ToLower()));
        }
        
        if(startDate.HasValue)
            events = events.Where(e => e.Date >= startDate.Value);
        
        if(endDate.HasValue)
            events = events.Where(e => e.Date <= endDate.Value);

        if (!string.IsNullOrEmpty(availability))
        {
            if (availability == "available")
                events = events.Where(e => e.AvailableTickets > 0);
            else if (availability == "unavailable")
                events = events.Where(e => e.AvailableTickets == 0);
        }

        switch (sortOrder)
        {
            case "title_desc":
                events = events.OrderByDescending(e => e.Title);
                break;
            case "date_asc":
                events = events.OrderBy(e => e.Date);
                break;
            case "date_desc":
                events = events.OrderByDescending(e => e.Date);
                break;
            case "price_asc":
                events = events.OrderBy(e => e.TicketPrice);
                break;
            case "price_desc":
                events = events.OrderByDescending(e => e.TicketPrice);
                break;
            default:
                events = events.OrderBy(e => e.Title);
                break;
        }

        
        
        ViewBag.Categories = new SelectList(_context.Categories, "CategoryId", "Name");
        return View(events.ToList());
    }

    [HttpGet]
    public IActionResult Create() //Create get 
    {
        ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name"); // adding this so the user can select the category when creating the event
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Event @event) //Create post
    { 
        if (ModelState.IsValid)
        {
            @event.Date = ToUtc(@event.Date); //Postgre doesn't like non UTC times
            _context.Events.Add(@event);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", @event.CategoryId); //In case we fail to create, this is required
        return View(@event);
    }

    [HttpGet]
    public IActionResult Edit(int id) //Edit get
    {
        var @event = _context.Events.Find(id);
        if (@event == null)
        {
            return NotFound();
        }
        ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", @event.CategoryId); // Adding this so the user can select the category when editing the event
        return View(@event);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [Bind("EventId,Title,Date,TicketPrice,AvailableTickets,CategoryId")] Event @event) //Edit post
    {
        if (id != @event.EventId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                @event.Date = ToUtc(@event.Date); //Database doesn't like non UTC
                _context.Events.Update(@event);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventExists(@event.EventId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("Index");
        }
        ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", @event.CategoryId);
        return View(@event);
    }

    private bool EventExists(int id) //Just checks if the event exists
    {
        return _context.Events.Any(e => e.EventId == id);
    }
    
    [HttpGet]
    public IActionResult Details(int id) //Details get
    {
        var @event = _context.Events.Include(e => e.Category).FirstOrDefault(p => p.EventId == id); // includes the categories 
        if (@event == null)
        {
            return NotFound();
        }

        return View(@event);
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var @event = _context.Events.Include(e => e.Category).FirstOrDefault(p => p.EventId == id); //includes the categories
        if (@event == null)
        {
            return NotFound();
        }
        return View(@event);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id) //Delete post
    {
        var @event = _context.Events.Find(id);
        if (@event != null)
        {
            _context.Events.Remove(@event);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(@event);
    }
    
    private DateTime ToUtc(DateTime input) //This is the same method from labs, converts the datetime
    {
        if (input.Kind == DateTimeKind.Utc)
            return input;
        if (input.Kind == DateTimeKind.Unspecified)
            return DateTime.SpecifyKind(input, DateTimeKind.Utc); // assume local is already UTC
        return input.ToUniversalTime();

    }

    [HttpGet]
    public IActionResult Summary()
    {
        ViewBag.TotalEvents = _context.Events.Count();
        ViewBag.TotalCategories = _context.Categories.Count();
        ViewBag.LowTicketEvents = _context.Events.Where(e => e.AvailableTickets < 5).Include(e => e.Category).ToList();
        
        return View();
    }
}