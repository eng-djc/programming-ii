using Microsoft.AspNetCore.Mvc;
namespace EGamer.Controllers;
public class HomeController : Controller
{
    public IActionResult Error() => View();
}