using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Cherkessia.Models;
using Cherkessia.Services;

namespace Cherkessia.Controllers
{
    public class InstrumentoController : Controller
    {
        // Crea el contexto de EF6 como en los controladores de Zara y EGamer.
        private readonly Service service;

        public InstrumentoController()
        {
            service = new Service();
        }

        [HttpGet]
        public IActionResult Index()
        {
            try { return View(service.mostrarInstrumentos()); }
            catch (Exception ex) { return errorConsulta(ex); }
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            if (!ModelState.IsValid || id <= 0) return BadRequest();
            try
            {
                var instrumento = service.buscarInstrumento(id);
                return instrumento == null ? NotFound() : View(instrumento);
            }
            catch (Exception ex) { return errorConsulta(ex); }
        }

        [HttpGet]
        public IActionResult Create() => View(new Instrumento());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Codigo,Nombre,FechaRegistro,Precio,Garantia")] Instrumento instrumento)
        {
            if (!ModelState.IsValid) return View(instrumento);
            try
            {
                service.agregarInstrumento(instrumento);
                return RedirectToAction(nameof(Index));
            }
            catch (ValidationException ex) { ModelState.AddModelError("", ex.Message); }
            catch (Exception)
            {
                ModelState.AddModelError("", "No se pudo guardar. Verifique que el código no esté repetido y que la base de datos esté disponible.");
            }
            return View(instrumento);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (!ModelState.IsValid || id <= 0) return BadRequest();
            try
            {
                var instrumento = service.buscarInstrumento(id);
                return instrumento == null ? NotFound() : View(instrumento);
            }
            catch (Exception ex) { return errorConsulta(ex); }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int id, [Bind("Codigo,Nombre,FechaRegistro,Precio,Garantia")] Instrumento instrumento)
        {
            if (id <= 0 || id != instrumento.Codigo) return BadRequest();
            if (!ModelState.IsValid) return View(instrumento);
            try
            {
                if (!service.actualizarInstrumento(instrumento)) return NotFound();
                return RedirectToAction(nameof(Index));
            }
            catch (ValidationException ex) { ModelState.AddModelError("", ex.Message); }
            catch (Exception)
            {
                ModelState.AddModelError("", "No se pudo actualizar el instrumento. Inténtelo nuevamente.");
            }
            return View(instrumento);
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
