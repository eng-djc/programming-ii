using Microsoft.AspNetCore.Mvc;
using Zara.Models;
using Zara.Services;

namespace Zara.Controllers
{
    // Controlador MVC: muestra las vistas y procesa las operaciones CRUD de prendas.
    public class PrendaController : Controller
    {
        // Contexto de EF6 para consultar y guardar prendas.
        private readonly Service service;

        public PrendaController()
        {
            service = new Service();
        }

        // GET: /Prenda
        // Muestra el listado de todas las prendas.
        [HttpGet]
        public ActionResult Index()
        {
            return View(service.mostrarPrendas());
        }

        // GET: /Prenda/Details/5
        // Muestra una prenda sin modificarla. No requiere POST.
        [HttpGet]
        public ActionResult Details(int id)
        {
            try
            {
                return View(service.buscarPrenda(id));
            }
            catch (Exception ex) when (ex.Message == "Esa prenda no se encuentra registrada")
            {
                return NotFound();
            }
        }

        // GET: /Prenda/Create
        // Abre el formulario vacío para registrar una prenda.
        [HttpGet]
        public ActionResult Create()
        {
            return View(new Prenda());
        }

        // POST: /Prenda/Create
        // Recibe los campos del formulario y valida antes de insertar la prenda.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Marca,Talla,Precio,Genero")] Prenda prendita)
        {
            try
            {
                // Solo guardar si los datos cumplen las validaciones del modelo.
                if (ModelState.IsValid)
                {
                    service.agregarPrenda(prendita);
                    return RedirectToAction(nameof(Index));
                }

                // Si hay datos inválidos, conservarlos y mostrar sus errores.
                return View(prendita);
            }
            catch (Exception)
            {
                // Si falla la base de datos, informar sin perder lo ingresado.
                ModelState.AddModelError("", "No se pudo guardar la prenda. Inténtelo nuevamente.");
                return View(prendita);
            }
        }

        // GET: /Prenda/Edit/5
        // Carga los valores de la prenda en el formulario de edición.
        [HttpGet]
        public ActionResult Edit(int id)
        {
            try
            {
                return View(service.buscarPrenda(id));
            }
            catch (Exception ex) when (ex.Message == "Esa prenda no se encuentra registrada")
            {
                return NotFound();
            }
        }

        // POST: /Prenda/Edit/5
        // Recibe el formulario y actualiza el registro existente.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind("Id,Marca,Talla,Precio,Genero")] Prenda prenda)
        {
            // El ID de la URL debe coincidir con el identificador enviado.
            if (id != prenda.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(prenda);

            try
            {
                // La profesora definió actualizarPrenda como void.
                service.actualizarPrenda(prenda);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) when (ex.Message == "No se encontraron registros de esa prenda")
            {
                return NotFound();
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "No se pudo actualizar la prenda. Inténtelo nuevamente.");
                return View(prenda);
            }
        }

        // GET: /Prenda/Delete/5
        // Muestra los datos y solicita confirmación; no elimina registros.
        [HttpGet]
        public ActionResult Delete(int id)
        {
            try
            {
                return View(service.buscarPrenda(id));
            }
            catch (Exception ex) when (ex.Message == "Esa prenda no se encuentra registrada")
            {
                return NotFound();
            }
        }

        // POST: /Prenda/Delete/5
        // Elimina la prenda únicamente después de confirmar el formulario.
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                var prenda = service.buscarPrenda(id);
                service.eliminarPrenda(prenda);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex) when (ex.Message == "Esa prenda no se encuentra registrada")
            {
                return NotFound();
            }
        }
    }
}
