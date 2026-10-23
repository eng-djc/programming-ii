using Microsoft.AspNetCore.Mvc;
namespace EGamer.Controllers;

// Mantiene los endpoints Home/Index, Home/Privacy y Home/Error de la plantilla MVC.
public class HomeController : Controller
{
    [HttpGet]
    public IActionResult Index() => View();

    [HttpGet]
    public IActionResult Privacy() => View();

    [HttpGet]
    public IActionResult Error() => View();
}