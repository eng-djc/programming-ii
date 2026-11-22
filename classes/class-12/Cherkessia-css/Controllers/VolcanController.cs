using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Cherkessia.Models;
using Cherkessia.Services;

namespace Cherkessia.Controllers
{
    public class VolcanController : Controller
    {
        // Crea el contexto de EF6 dentro del controlador, como clase 6.
        private readonly Service service;

        public VolcanController()
        {
            service = new Service();
        }

        [HttpGet]
        public IActionResult Index()
        {
            try { return View(service.mostrarVolcanes()); }
            catch (Exception ex) { return errorConsulta(ex); }
        }

        // El nombre viaja como query string para admitir espacios y barras sin cambiar la ruta MVC.
        [HttpGet]
        public IActionResult Details([FromQuery] string nombre) => mostrarVolcan(nombre, "Details");

        [HttpGet]
        public IActionResult Create()
        {
            cargarInstrumentos();
            return View(new Volcan());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Nombre,Provincia,Activo,Gases,InstrumentoCodigo")] Volcan volcan)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    service.agregarVolcan(volcan);
                    return RedirectToAction(nameof(Index));
                }
                catch (ValidationException ex) { ModelState.AddModelError("", ex.Message); }
                catch (Exception) { errorGuardado(); }
            }
            cargarInstrumentos();
            return View(volcan);
        }

        [HttpGet]
        public IActionResult Edit([FromQuery] string nombre) => mostrarVolcan(nombre, "Edit");

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromQuery] string nombre, [Bind("Nombre,Provincia,Activo,Gases,InstrumentoCodigo")] Volcan volcan)
        {
            if (string.IsNullOrWhiteSpace(nombre) || !string.Equals(nombre, volcan.Nombre, StringComparison.Ordinal))
                return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    if (!service.actualizarVolcan(volcan)) return NotFound();
                    return RedirectToAction(nameof(Index));
                }
                catch (ValidationException ex) { ModelState.AddModelError("", ex.Message); }
                catch (Exception) { errorGuardado(); }
            }
            cargarInstrumentos();
            return View(volcan);
        }

        [HttpGet]
        public IActionResult Delete([FromQuery] string nombre) => mostrarVolcan(nombre, "Delete");

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed([FromQuery] string nombre)
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(nombre)) return BadRequest();
            try
            {
                if (!service.eliminarVolcan(nombre)) return NotFound();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) { return errorConsulta(ex); }
        }

        private IActionResult mostrarVolcan(string nombre, string vista)
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(nombre)) return BadRequest();
            try
            {
                var volcan = service.buscarVolcan(nombre);
                if (volcan == null) return NotFound();
                if (vista == "Edit") cargarInstrumentos();
                return View(vista, volcan);
            }
            catch (Exception ex) { return errorConsulta(ex); }
        }

        private void cargarInstrumentos()
        {
            try
            {
                var instrumentos = service.mostrarInstrumentos();
                ViewBag.Instrumentos = new SelectList(instrumentos, "Codigo", "Nombre");
                if (instrumentos.Count == 0)
                    ModelState.AddModelError("", "Primero registre un instrumento para poder guardar un volcán.");
            }
            catch (Exception)
            {
                ViewBag.Instrumentos = new SelectList(Array.Empty<Instrumento>(), "Codigo", "Nombre");
                ModelState.AddModelError("", "No se pudieron cargar los instrumentos. Verifique la conexión a la base de datos.");
            }
        }

        private void errorGuardado()
        {
            ModelState.AddModelError("", "No se pudo guardar. Verifique que el nombre no esté repetido y que la base de datos esté disponible.");
        }

        private IActionResult errorConsulta(Exception ex)
        {
            _ = ex;
            Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            return View("Error", new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) service.Dispose();
            base.Dispose(disposing);
        }
    }
}
