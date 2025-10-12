using COMP2139_Assignment1.Data;
using COMP2139_Assignment1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace COMP2139_Assignment1.Controllers;
//There isn't anything unusual about this controller, it is similar to what we did during labs
public class CategoryController : Controller
{
     private readonly ApplicationDbContext _context; //Connection with the database

    public CategoryController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Index() //Index get
    {
        var categories = _context.Categories.ToList();
        return View(categories);
    }

    [HttpGet]
    public IActionResult Create() //Create get
    {
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Category category) //Create post
    {
        if (ModelState.IsValid)
        {
            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(category);
    }

    [HttpGet]
    public IActionResult Edit(int id) //Edit get
    {
        var category = _context.Categories.Find(id);
        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [Bind("CategoryId, Name, Description")] Category category) //Edit post
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
                _context.SaveChanges();
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
    public IActionResult Details(int id) //Details get
    {
        var category = _context.Categories.FirstOrDefault(p => p.CategoryId == id);
        if (category == null)
        {
            return NotFound();
        }

        return View(category);
    }

    [HttpGet]
    public IActionResult Delete(int id) //Delete get
    {
        var category = _context.Categories.FirstOrDefault(p => p.CategoryId == id);
        if (category == null)
        {
            return NotFound();
        }
        return View(category);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id) //Delete post
    {
        var category = _context.Categories.Find(id);
        if (category != null)
        {
            _context.Categories.Remove(category);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(category);
    }
}