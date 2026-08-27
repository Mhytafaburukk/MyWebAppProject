using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MyWebAppProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace MyWebAppProject.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
    public IActionResult ProcessForm(string UserInput)
    {
        if (!string.IsNullOrEmpty(UserInput)){
            TempData["SuccessMessage"] = $"You entered: {UserInput}";
            TempData["MessageType"] = "success";
        }
        else
        {
            TempData["ErrorMessage"] = "Please enter a value.";
            TempData["MessageType"] = "error";
        }
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
