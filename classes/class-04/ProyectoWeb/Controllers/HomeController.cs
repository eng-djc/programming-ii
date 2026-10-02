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
            return View();
        }

        [HttpGet]
        public IActionResult AgregarUsuario()
        {
            return View(new Usuario());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AgregarUsuario(Usuario modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Service.agregar(modelo);
                    TempData["MensajeUsuario"] = "Usuario agregado correctamente.";
                    return RedirectToAction(nameof(MostrarUsuarios));
                }
                catch (Exception error)
                {
                    ModelState.AddModelError(string.Empty, error.Message);
                }
            }

            modelo.Clave = string.Empty;
            ModelState.Remove(nameof(Usuario.Clave));
            return View(modelo);
        }

        [HttpGet]
        public IActionResult MostrarUsuarios()
        {
            return View(Service.mostrar());
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
