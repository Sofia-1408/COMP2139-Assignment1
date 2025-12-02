using COMP2139_Assignment1.Data;
using COMP2139_Assignment1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace COMP2139_Assignment1.Controllers;

public class CategoryController : Controller
{
     private readonly ApplicationDbContext _context; //Connection with the database

    public CategoryController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string searchString) //Index get
    {
        var categories = _context.Categories.AsQueryable();
        if (!string.IsNullOrEmpty(searchString))
        {
            categories = categories.Where(c => c.Name.ToLower().Contains(searchString.ToLower()));
        }
        return View(await categories.ToListAsync()); //Was throwing an unhandled exception so we will execute before sending it to view
    }

    [HttpGet]
    public IActionResult Create() //Create get
    {
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Category category) //Create post
    {
        if (ModelState.IsValid)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        return View(category);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id) //Edit get
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("CategoryId, Name, Description")] Category category) //Edit post
    {
        if (id != category.CategoryId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Categories.Update(category);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(category.CategoryId))
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
        return View(category);
    }

    private bool CategoryExists(int id) //Checking if the category exists
    {
        return _context.Categories.Any(e => e.CategoryId == id);
    }
    
    [HttpGet]
    public async Task<IActionResult> Details(int id) //Details get
    {
        var category = await _context.Categories.FirstOrDefaultAsync(p => p.CategoryId == id);
        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id) //Delete get
    {
        var category = await _context.Categories.FirstOrDefaultAsync(p => p.CategoryId == id);
        if (category == null)
        {
            return NotFound();
        }
        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id) //Delete post
    {
        var category = await _context.Categories.FindAsync(id);
        if (category != null)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        return View(category);
    }
    [HttpGet]
    public async Task<IActionResult> Search(string searchString)
    {
        var categories = _context.Categories.AsQueryable();

        if (!string.IsNullOrEmpty(searchString))
        {
            categories = categories.Where(c => c.Name.ToLower().Contains(searchString.ToLower()));
        }

        return PartialView("_CategoryTable", await categories.ToListAsync());
    }

}