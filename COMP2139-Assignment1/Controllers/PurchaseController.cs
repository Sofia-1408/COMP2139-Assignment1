using COMP2139_Assignment1.Data;
using COMP2139_Assignment1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace COMP2139_Assignment1.Controllers;

public class PurchaseController : Controller
{
     private readonly ApplicationDbContext _context; //DB connection

    public PurchaseController(ApplicationDbContext context)
    {
        _context = context;
    }        
    public async Task<IActionResult> Index() //Index get
    {
        var purchases = _context.Purchases.Include(p => p.Event); // includes the events
        return View(await purchases.ToListAsync());
    }
    
    public async Task<IActionResult> Details(int? id) //Details get
    {
        if (id == null)
            return NotFound();
        var purchase = await _context.Purchases
            .Include(p => p.Event)
            .FirstOrDefaultAsync(m => m.PurchaseId == id); //Must include the event

        if (purchase == null)
            return NotFound();

        return View(purchase);
    }
    
    public IActionResult Create() //Create get
    {
        ViewData["EventId"] = new SelectList(_context.Events, "EventId", "Title"); //ViewData is needed to show a list of events when creating a purchase
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("PurchaseId,PurchaseDate,TotalCost,GuestContactInfo,EventId")] Purchase purchase) //Create post
    {
        if (ModelState.IsValid)
        {
            purchase.PurchaseDate = ToUtc(purchase.PurchaseDate);
            _context.Add(purchase);
            await _context.SaveChangesAsync();
            return RedirectToAction("Confirmation", new { purchaseId = purchase.PurchaseId });
        }
        ViewData["EventId"] = new SelectList(_context.Events, "EventId", "Title", purchase.EventId); //ViewData is needed to show a list of events when creating a purchase
        return View(purchase);
    }
    
    public async Task<IActionResult> Edit(int? id) //Edit get
    { 
        if (id == null)
            return NotFound();

        var purchase = await _context.Purchases.FindAsync(id);
        if (purchase == null)
            return NotFound();

        ViewData["EventId"] = new SelectList(_context.Events, "EventId", "Title", purchase.EventId);//ViewData is needed to show a list of events when editing a purchase
        return View(purchase);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("PurchaseId,PurchaseDate,TotalCost,GuestContactInfo,EventId")] Purchase purchase) //Edit post
    {
        if (id != purchase.PurchaseId)
            return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                purchase.PurchaseDate = ToUtc(purchase.PurchaseDate);
                _context.Update(purchase);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Purchases.Any(e => e.PurchaseId == purchase.PurchaseId))
                    return NotFound();
                else
                    throw;
            }
            return RedirectToAction(nameof(Index));
        }
        ViewData["EventId"] = new SelectList(_context.Events, "EventId", "Title", purchase.EventId);//ViewData is needed to show a list of events when editing a purchase
        return View(purchase);
        }
    
    public async Task<IActionResult> Delete(int? id) //Delete get
    {
        if (id == null)
            return NotFound();

        var purchase = await _context.Purchases
            .Include(p => p.Event)
            .FirstOrDefaultAsync(m => m.PurchaseId == id); //includes events
        if (purchase == null)
            return NotFound();

        return View(purchase);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id) //Delete post
    {
        var purchase = await _context.Purchases.FirstOrDefaultAsync(p => p.PurchaseId == id);
        if (purchase == null) 
            return NotFound();
        _context.Purchases.Remove(purchase);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
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
    public async Task<IActionResult> Confirmation(int purchaseId)
    {
        
        var purchase = await _context.Purchases
            .Include(p => p.Event)
            .ThenInclude(e => e.Category)
            .FirstOrDefaultAsync(p => p.PurchaseId == purchaseId);

        if (purchase == null)
        {
            return NotFound();
        }

        return View(purchase);
    }
    
}
    
