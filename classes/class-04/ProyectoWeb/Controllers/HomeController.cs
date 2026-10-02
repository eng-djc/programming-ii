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
            var servicio = new Service(modelo);
            foreach (var error in servicio.Validar())
            {
                foreach (var propiedad in error.MemberNames)
                {
                    // NombreUsuario se vincula con el nombre original del formulario.
                    var campo = propiedad == nameof(Usuario.NombreUsuario) ? "usuario" : propiedad;
                    // MVC ya valida los atributos; evita duplicar sus mensajes.
                    if (!ModelState.TryGetValue(campo, out var estado) || estado.Errors.Count == 0)
                    {
                        ModelState.AddModelError(campo, error.ErrorMessage ?? "Valor inválido.");
                    }
                }
            }

            if (ModelState.IsValid)
            {
                servicio.AgregarUsuario(modelo);
                TempData["MensajeUsuario"] = "Usuario agregado correctamente.";
                // Redirigir evita repetir el POST al actualizar la página de resultados.
                return RedirectToAction(nameof(MostrarUsuarios));
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
            var servicio = new Service();
            return View(servicio.MostrarUsuarios());
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
