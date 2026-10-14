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
            var prenda = service.buscarPrenda(id);

            if (prenda == null)
                return NotFound();

            return View(prenda);
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
            var prenda = service.buscarPrenda(id);

            if (prenda == null)
                return NotFound();

            return View(prenda);
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

            if (!service.actualizarPrenda(prenda))
                return NotFound();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Prenda/Delete/5
        // Muestra los datos y solicita confirmación antes de eliminar.
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var prenda = service.buscarPrenda(id);

            if (prenda == null)
                return NotFound();

            return View(prenda);
        }

        // POST: /Prenda/Delete/5
        // Confirma la eliminación del registro y vuelve al listado.
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!service.eliminarPrenda(id))
                return NotFound();

            return RedirectToAction(nameof(Index));
        }
    }
}
