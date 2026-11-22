using System.ComponentModel.DataAnnotations;
using Cherkessia.Models;
using Cherkessia.Services;
using Microsoft.AspNetCore.Mvc;

namespace Cherkessia.Controllers
{
    public class FuncionarioController : Controller
    {
        private readonly Service service;

        public FuncionarioController()
        {
            service = new Service();
        }

        [HttpGet]
        public IActionResult Index()
        {
            try { return View(service.mostrarFuncionarios()); }
            catch (Exception) { return errorConsulta(); }
        }

        [HttpGet]
        public IActionResult Details(int id) => mostrarFuncionario(id, "Details");

        [HttpGet]
        public IActionResult Create() => View(new Funcionario());

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Nombre,Cedula,Telefono,FechaNacimiento,Activo,Perfil,Usuario,Clave")] Funcionario funcionario)
        {
            if (string.IsNullOrWhiteSpace(funcionario.Clave))
                ModelState.AddModelError(nameof(Funcionario.Clave), "Ingrese la clave.");
            if (ModelState.IsValid)
            {
                try
                {
                    service.agregarFuncionario(funcionario);
                    return RedirectToAction(nameof(Index));
                }
                catch (ValidationException ex) { ModelState.AddModelError("", ex.Message); }
                catch (Exception) { ModelState.AddModelError("", "No se pudo guardar. Verifique la base de datos."); }
            }
            return View(funcionario);
        }

        [HttpGet]
        public IActionResult Edit(int id) => mostrarFuncionario(id, "Edit");

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Nombre,Cedula,Telefono,FechaNacimiento,Activo,Perfil,Usuario,Clave")] Funcionario funcionario)
        {
            if (id <= 0 || id != funcionario.Id) return BadRequest();
            if (ModelState.IsValid)
            {
                try
                {
                    if (!service.actualizarFuncionario(funcionario)) return NotFound();
                    return RedirectToAction(nameof(Index));
                }
                catch (ValidationException ex) { ModelState.AddModelError("", ex.Message); }
                catch (Exception) { ModelState.AddModelError("", "No se pudo actualizar. Verifique la base de datos."); }
            }
            return View(funcionario);
        }

        [HttpGet]
        public IActionResult Delete(int id) => mostrarFuncionario(id, "Delete");

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (id <= 0) return BadRequest();
            try { return service.eliminarFuncionario(id) ? RedirectToAction(nameof(Index)) : NotFound(); }
            catch (Exception) { return errorConsulta(); }
        }

        private IActionResult mostrarFuncionario(int id, string vista)
        {
            if (id <= 0) return BadRequest();
            try
            {
                var funcionario = service.buscarFuncionario(id);
                return funcionario == null ? NotFound() : View(vista, funcionario);
            }
            catch (Exception) { return errorConsulta(); }
        }

        private IActionResult errorConsulta()
        {
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
