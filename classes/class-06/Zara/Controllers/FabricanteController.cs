using Microsoft.AspNetCore.Mvc;
using Zara.Models;
using Zara.Services;

namespace Zara.Controllers
{
    /// <summary>Controlador MVC de Fabricantes: acciones GET muestran; POST procesan.</summary>
    public class FabricanteController : Controller
    {
        private readonly Service service;

        public FabricanteController() { service = new Service(); }

        // GET /Fabricante: muestra la tabla de fabricantes.
        [HttpGet]
        public ActionResult Index() => View(service.mostrarFabricantes());

        // GET /Fabricante/Details/5: consulta un registro sin editarlo.
        [HttpGet]
        public ActionResult Details(int id)
        {
            var fabricante = service.buscarFabricante(id);
            return fabricante == null ? NotFound() : View(fabricante);
        }

        // GET /Fabricante/Create: presenta el formulario vacío.
        [HttpGet]
        public ActionResult Create() => View(new Fabricante());

        // POST /Fabricante/Create: valida, inserta y redirige a Index.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Prestigio,FechaCreacion")] Fabricante fabricante)
        {
            if (!ModelState.IsValid) return View(fabricante);
            try
            {
                service.agregarFabricante(fabricante);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "No se pudo guardar el fabricante. Verifique la base de datos.");
                return View(fabricante);
            }
        }

        // GET /Fabricante/Edit/5: llena el formulario con valores actuales.
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var fabricante = service.buscarFabricante(id);
            return fabricante == null ? NotFound() : View(fabricante);
        }

        // POST /Fabricante/Edit/5: comprueba ID, valida y guarda los cambios.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind("Id,Prestigio,FechaCreacion")] Fabricante fabricante)
        {
            if (id != fabricante.Id) return BadRequest();
            if (!ModelState.IsValid) return View(fabricante);
            try
            {
                if (!service.actualizarFabricante(fabricante)) return NotFound();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "No se pudo actualizar el fabricante.");
                return View(fabricante);
            }
        }

        // GET /Fabricante/Delete/5: pantalla de confirmación (no borra).
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var fabricante = service.buscarFabricante(id);
            return fabricante == null ? NotFound() : View(fabricante);
        }

        // POST /Fabricante/Delete/5: eliminación real después de confirmar.
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!service.eliminarFabricante(id)) return NotFound();
            return RedirectToAction(nameof(Index));
        }
    }
}
