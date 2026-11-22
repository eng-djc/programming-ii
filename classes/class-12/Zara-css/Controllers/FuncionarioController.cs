using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Zara.Models;
using Zara.Services;

namespace Zara.Controllers
{
    public class FuncionarioController : Controller
    {
        private readonly Service service;

        public FuncionarioController()
        {
            service = new Service();
        }

        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                return View(service.mostrarFuncionarios());
            }
            catch (Exception)
            {
                return errorConsulta();
            }
        }

        [HttpGet]
        public ActionResult Details(int id) => mostrarFuncionario(id, "Details");

        [HttpGet]
        public ActionResult Create() => View(new Funcionario());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind("Nombre,Cedula,Telefono,FechaNacimiento,Activo,Perfil,Usuario,Clave")]
            Funcionario funcionario)
        {
            bool claveVacia = string.IsNullOrWhiteSpace(funcionario.Clave);

            if (ModelState.IsValid && !claveVacia)
            {
                try
                {
                    service.agregarFuncionario(funcionario);
                    return RedirectToAction(nameof(Index));
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "No se pudo guardar. Verifique la base de datos.");
                }
            }

            // No conserva la contraseña escrita cuando el formulario vuelve con errores.
            ModelState.Remove(nameof(Funcionario.Clave));
            funcionario.Clave = null;
            if (claveVacia)
                ModelState.AddModelError(nameof(Funcionario.Clave), "Ingrese la clave.");

            return View(funcionario);
        }

        [HttpGet]
        public ActionResult Edit(int id) => mostrarFuncionario(id, "Edit");

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            int id,
            [Bind("Id,Nombre,Cedula,Telefono,FechaNacimiento,Activo,Perfil,Usuario,Clave")]
            Funcionario funcionario)
        {
            if (id <= 0 || id != funcionario.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                try
                {
                    if (!service.actualizarFuncionario(funcionario))
                        return NotFound();

                    return RedirectToAction(nameof(Index));
                }
                catch (ValidationException ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "No se pudo actualizar. Verifique la base de datos.");
                }
            }

            // Conserva la clave almacenada y descarta cualquier clave escrita.
            ModelState.Remove(nameof(Funcionario.Clave));
            funcionario.Clave = null;
            return View(funcionario);
        }

        [HttpGet]
        public ActionResult Delete(int id) => mostrarFuncionario(id, "Delete");

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (id <= 0)
                return BadRequest();

            try
            {
                return service.eliminarFuncionario(id)
                    ? RedirectToAction(nameof(Index))
                    : NotFound();
            }
            catch (Exception)
            {
                return errorConsulta();
            }
        }

        private ActionResult mostrarFuncionario(int id, string vista)
        {
            if (id <= 0)
                return BadRequest();

            try
            {
                var funcionario = service.buscarFuncionario(id);
                return funcionario == null ? NotFound() : View(vista, funcionario);
            }
            catch (Exception)
            {
                return errorConsulta();
            }
        }

        private ActionResult errorConsulta()
        {
            Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            return View("Error", new ErrorViewModel { RequestId = HttpContext.TraceIdentifier });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                service.Dispose();

            base.Dispose(disposing);
        }
    }
}
