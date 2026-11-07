using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using ParanormalAgency.Models;
using ParanormalAgency.Services;

namespace ParanormalAgency.Controllers
{
    public class InvestigationController : Controller
    {
        private readonly Service service = new Service();

        // Obtiene las investigaciones y las muestra en la lista principal.
        [HttpGet]
        public IActionResult Index()
        {
            try { return View(service.GetInvestigations()); }
            catch (Exception) { return DatabaseError(); }
        }

        // Abre el formulario para registrar una investigación.
        [HttpGet]
        public IActionResult Create() => View(new Investigation());

        // Valida el formulario y guarda una nueva investigación.
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create([Bind("CaseName,ClientName,PhenomenonType,RiskLevel,BaseBudget,RequiresSpecializedEquipment,InvestigationDate")] Investigation item)
        {
            if (ModelState.IsValid)
            {
                try { service.AddInvestigation(item); return RedirectToAction(nameof(Index)); }
                catch (ValidationException ex) { ModelState.AddModelError("", ex.Message); }
                catch (Exception) { ModelState.AddModelError("", "No se pudo guardar. Verifique la base de datos."); }
            }
            return View(item);
        }

        // Busca una investigación y carga sus datos para editarlos.
        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (id <= 0) return BadRequest();
            try { var item = service.FindInvestigation(id); return item == null ? NotFound() : View(item); }
            catch (Exception) { return DatabaseError(); }
        }

        // Valida los cambios y actualiza la investigación seleccionada.
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,CaseName,ClientName,PhenomenonType,RiskLevel,BaseBudget,RequiresSpecializedEquipment,InvestigationDate")] Investigation item)
        {
            // Impide actualizar un registro si el identificador es inválido o no coincide.
            if (id <= 0 || id != item.Id) return BadRequest();
            // Solo guarda los cambios cuando los datos recibidos pasan la validación.
            if (ModelState.IsValid)
            {
                try
                {
                    // Si el registro ya no existe, responde 404 en lugar de crear otro.
                    if (!service.UpdateInvestigation(item)) return NotFound();
                    return RedirectToAction(nameof(Index));
                }
                // Conserva el formulario y muestra el motivo si falla la validación o el guardado.
                catch (ValidationException ex) { ModelState.AddModelError("", ex.Message); }
                catch (Exception) { ModelState.AddModelError("", "No se pudo actualizar. Verifique la base de datos."); }
            }
            return View(item);
        }

        // Muestra los datos de la investigación antes de confirmar su eliminación.
        [HttpGet]
        public IActionResult Delete(int id)
        {
            // Un identificador inválido no permite buscar el registro.
            if (id <= 0) return BadRequest();
            // Presenta la confirmación sin eliminar datos todavía.
            try { var item = service.FindInvestigation(id); return item == null ? NotFound() : View(item); }
            catch (Exception) { return DatabaseError(); }
        }

        // Confirma la eliminación y regresa a la lista de investigaciones.
        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            // La eliminación ocurre únicamente al recibir la confirmación por POST.
            if (id <= 0) return BadRequest();
            // Regresa al listado si se eliminó; responde 404 si el registro no existe.
            try { return service.DeleteInvestigation(id) ? RedirectToAction(nameof(Index)) : NotFound(); }
            catch (Exception) { return DatabaseError(); }
        }

        // Presenta la vista compartida de errores de la aplicación.
        [HttpGet]
        public IActionResult Error() => View("Error");

        // Devuelve la vista de error cuando falla el acceso a los datos.
        private IActionResult DatabaseError()
        {
            // HTTP 503 indica que el servicio no está disponible temporalmente.
            Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            return View("Error");
        }

        // Libera la conexión del servicio al finalizar la solicitud.
        protected override void Dispose(bool disposing)
        {
            if (disposing) service.Dispose();
            base.Dispose(disposing);
        }
    }
}