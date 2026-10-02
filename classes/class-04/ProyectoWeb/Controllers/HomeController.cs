using Microsoft.AspNetCore.Mvc;
using ProyectoWeb.Models;
using System.Diagnostics;

namespace ProyectoWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var usuarios = Service.mostrar();
            return View(usuarios);
        }

        // Conserva los enlaces anteriores al formulario.
        [HttpGet]
        public IActionResult AgregarUsuario()
        {
            return RedirectToAction("Create", "Usuario");
        }

        [HttpGet]
        public IActionResult MostrarUsuarios()
        {
            return RedirectToAction("Index", "Usuario");
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
}
