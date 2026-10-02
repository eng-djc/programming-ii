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
                foreach (var campo in error.MemberNames)
                {
                    // MVC ya valida los atributos; evita duplicar sus mensajes.
                    if (!ModelState.TryGetValue(campo, out var estado) || estado.Errors.Count == 0)
                    {
                        ModelState.AddModelError(campo, error.ErrorMessage ?? "Valor inválido.");
                    }
                }
            }

            if (ModelState.IsValid)
            {
                // Este ejercicio valida datos; todavía no implementa persistencia.
                TempData["MensajeUsuario"] = "Datos validados correctamente. El ejercicio no guarda usuarios.";
                return RedirectToAction(nameof(AgregarUsuario));
            }

            // No devolver la clave introducida al volver a mostrar el formulario.
            modelo.clave = string.Empty;
            ModelState.Remove(nameof(Usuario.clave));
            return View(modelo);
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
