using Microsoft.AspNetCore.Mvc;

namespace COMP2139_Assignment1.Controllers;

public class UsersController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}