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
    public IActionResult Index() //Index get
    {
        var events = _context.Events.Include(e => e.Category).ToList(); //Gotta include the category
        return View(events);
    }

    [HttpGet]
    public IActionResult Create() //Create get
    {
        ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name"); //Gotta add this so the user can select the category when creating the event
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Event @event) //Create post
    { //I have to name the variable @event because if I just name it event the environment screams at me :(, and I felt bad naming it anything else
        if (ModelState.IsValid)
        {
            @event.Date = ToUtc(@event.Date); //Postgre doesn't like non UTC times..
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
        ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", @event.CategoryId); //Gotta add this so the user can select the category when editing the event
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
        var @event = _context.Events.Include(e => e.Category).FirstOrDefault(p => p.EventId == id); //Gotta include the categories 
        if (@event == null)
        {
            return NotFound();
        }

        return View(@event);
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {
        var @event = _context.Events.Include(e => e.Category).FirstOrDefault(p => p.EventId == id); //Gotta include the categories
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
}