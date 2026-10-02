using Microsoft.AspNetCore.Mvc;
using ProyectoWeb.Models;

namespace ProyectoWeb.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario
        public ActionResult Index()
        {
            var usuarios = Service.mostrar();
            return View(usuarios);
        }

        // GET: Usuario/Create
        public ActionResult Create()
        {
            return View(new Usuario());
        }

        // POST: Usuario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Usuario usuarito)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Service.agregar(usuarito);
                    TempData["MensajeUsuario"] = "Usuario agregado correctamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception error)
                {
                    ModelState.AddModelError(string.Empty, error.Message);
                }
            }

            // No devolver la clave al mostrar los errores.
            usuarito.Clave = string.Empty;
            ModelState.Remove(nameof(Usuario.Clave));
            return View(usuarito);
        }
    }
}
