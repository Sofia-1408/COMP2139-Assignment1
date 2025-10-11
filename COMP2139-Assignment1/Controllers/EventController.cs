using COMP2139_Assignment1.Data;
using COMP2139_Assignment1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace COMP2139_Assignment1.Controllers;

public class EventController : Controller
{
     private readonly ApplicationDbContext _context;

    public EventController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index()
    {
        var events = _context.Events.ToList();
        return View(events);
    }

    [HttpGet("Create")]
    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Event @event)
    {
        if (ModelState.IsValid)
        {
            @event.Date = ToUtc(@event.Date);
            _context.Events.Add(@event);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(@event);
    }

    [HttpGet("Edit/{id:int}")]
    public IActionResult Edit(int id)
    {
        var @event = _context.Events.Find(id);
        if (@event == null)
        {
            return NotFound();
        }

        return View(@event);
    }
    
    [HttpPost("Edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [Bind("EventId,Title,Category,Date,TicketPrice,AvailableTickets")] Event @event)
    {
        if (id != @event.EventId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                @event.Date = ToUtc(@event.Date);
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
        return View(@event);
    }

    private bool EventExists(int id)
    {
        return _context.Events.Any(e => e.EventId == id);
    }
    
    [HttpGet("Details/{id:int}")]
    public IActionResult Details(int id)
    {
        var @event = _context.Events.FirstOrDefault(p => p.EventId == id);
        if (@event == null)
        {
            return NotFound();
        }

        return View(@event);
    }

    [HttpGet("Delete/{id:int}")]
    public IActionResult Delete(int id)
    {
        var @event = _context.Events.FirstOrDefault(p => p.EventId == id);
        if (@event == null)
        {
            return NotFound();
        }
        return View(@event);
    }

    [HttpPost("DeleteConfirmed/{id:int}")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
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
    
    private DateTime ToUtc(DateTime input)
    {
        if (input.Kind == DateTimeKind.Utc)
            return input;
        if (input.Kind == DateTimeKind.Unspecified)
            return DateTime.SpecifyKind(input, DateTimeKind.Utc); // assume local is already UTC
        return input.ToUniversalTime();

    }
}