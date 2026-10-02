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

        /// <summary>Muestra la vista y entrega un modelo vacío al formulario.</summary>
        [HttpGet]
        public IActionResult AgregarUsuario()
        {
            return View(new Usuario());
        }

        /// <summary>
        /// Recibe campos con los mismos nombres que las propiedades del modelo.
        /// MVC incorpora también errores de conversión, por ejemplo una edad no entera.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AgregarUsuario(Usuario modelo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Llamada estática con el nombre utilizado por la profesora.
                    Service.agregar(modelo);
                    TempData["MensajeUsuario"] = "Usuario agregado correctamente.";
                    return RedirectToAction(nameof(MostrarUsuarios));
                }
                catch (Exception error)
                {
                    // Mostrar en el formulario el mensaje de usuario repetido.
                    ModelState.AddModelError(string.Empty, error.Message);
                }
            }

            // No devolver la clave introducida al volver a mostrar el formulario.
            modelo.Clave = string.Empty;
            ModelState.Remove(nameof(Usuario.Clave));
            return View(modelo);
        }

        /// <summary>Entrega a la vista los usuarios de la lista estática del servicio.</summary>
        [HttpGet]
        public IActionResult MostrarUsuarios()
        {
            return View(Service.GetAll());
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
