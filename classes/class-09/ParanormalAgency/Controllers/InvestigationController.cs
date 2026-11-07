using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using ParanormalAgency.Models;
using ParanormalAgency.Services;

namespace ParanormalAgency.Controllers
{
    public class InvestigationController : Controller
    {
        private readonly Service service = new Service();

        [HttpGet]
        public IActionResult Index()
        {
            try { return View(service.GetInvestigations()); }
            catch (Exception) { return DatabaseError(); }
        }

        [HttpGet]
        public IActionResult Create() => View(new Investigation());

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

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (id <= 0) return BadRequest();
            try { var item = service.FindInvestigation(id); return item == null ? NotFound() : View(item); }
            catch (Exception) { return DatabaseError(); }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,CaseName,ClientName,PhenomenonType,RiskLevel,BaseBudget,RequiresSpecializedEquipment,InvestigationDate")] Investigation item)
        {
            if (id <= 0 || id != item.Id) return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    if (!service.UpdateInvestigation(item)) return NotFound();
                    return RedirectToAction(nameof(Index));
                }
                catch (ValidationException ex) { ModelState.AddModelError("", ex.Message); }
                catch (Exception) { ModelState.AddModelError("", "No se pudo actualizar. Verifique la base de datos."); }
            }
            return View(item);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (id <= 0) return BadRequest();
            try { var item = service.FindInvestigation(id); return item == null ? NotFound() : View(item); }
            catch (Exception) { return DatabaseError(); }
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (id <= 0) return BadRequest();
            try { return service.DeleteInvestigation(id) ? RedirectToAction(nameof(Index)) : NotFound(); }
            catch (Exception) { return DatabaseError(); }
        }

        [HttpGet]
        public IActionResult Error() => View("Error");

        private IActionResult DatabaseError()
        {
            Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            return View("Error");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) service.Dispose();
            base.Dispose(disposing);
        }
    }
}