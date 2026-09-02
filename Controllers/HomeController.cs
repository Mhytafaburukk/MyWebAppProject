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
        return RedirectToAction("Index");
    }
    public IActionResult SavePreferences(string theme, string language)
    {
    HttpContext.Session.SetString("UserTheme", theme);
    HttpContext.Session.SetString("UserLanguage", language);

    TempData["SuccessMessage"] = "Preferences saved successfully!";
    return RedirectToAction("Preferences");
    }

    public IActionResult Preferences()
    {
    ViewBag.CurrentTheme = HttpContext.Session.GetString("UserTheme") ?? "light";
    ViewBag.CurrentLanguage = HttpContext.Session.GetString("UserLanguage") ?? "en";

    return View();
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
