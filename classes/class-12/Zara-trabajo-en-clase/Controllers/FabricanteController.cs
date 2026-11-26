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
            // Un IDENTITY válido es positivo; los errores de conversión tampoco se consultan.
            if (!ModelState.IsValid || id <= 0) return BadRequest();
            var fabricante = service.buscarFabricante(id);
            return fabricante == null ? NotFound() : View(fabricante);
        }

        // GET /Fabricante/Create: presenta el formulario vacío.
        [HttpGet]
        public ActionResult Create() => View(new Fabricante());

        // POST /Fabricante/Create: valida, inserta y redirige a Index.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Nombre,Prestigio,FechaCreacion")] Fabricante fabricante)
        {
            // MVC incorpora a ModelState los errores de conversión (por ejemplo, una
            // fecha imposible) y las reglas del modelo. Si hay errores, no se llama al
            // servicio: se conserva el formulario y se muestran los mensajes por campo.
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
            // Un IDENTITY válido es positivo; los errores de conversión tampoco se consultan.
            if (!ModelState.IsValid || id <= 0) return BadRequest();
            var fabricante = service.buscarFabricante(id);
            return fabricante == null ? NotFound() : View(fabricante);
        }

        // POST /Fabricante/Edit/5: comprueba ID, valida y guarda los cambios.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([FromRoute] int id, [Bind("Id,Nombre,Prestigio,FechaCreacion")] Fabricante fabricante)
        {
            // FromRoute impide que el ID oculto sustituya al ID de la URL.
            // Bind limita los campos recibidos, pero no valida sus valores.
            if (id <= 0 || fabricante.Id <= 0 || id != fabricante.Id) return BadRequest();
            // MVC incorpora a ModelState los errores de conversión (por ejemplo, una
            // fecha imposible) y las reglas del modelo. Si hay errores, no se llama al
            // servicio: se conserva el formulario y se muestran los mensajes por campo.
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

        // POST /Fabricante/Delete/5: elimina desde Index, sin confirmación.
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!ModelState.IsValid || id <= 0) return BadRequest();
            if (!service.eliminarFabricante(id)) return NotFound();
            return RedirectToAction(nameof(Index));
        }
    }
}
