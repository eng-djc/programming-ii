using System.Security.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
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

        // GET: abre el formulario con usuario y clave vacíos.
        [HttpGet]
        public ActionResult Login() => View(new LoginFuncionario());

        // POST: procesa el usuario y la clave que se enviaron al presionar Aceptar.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginFuncionario datos)
        {
            // Si faltan datos o algún campo es inválido, la vista muestra los errores del formulario.
            if (!ModelState.IsValid)
                return View(datos);

            // El POST recibe LoginFuncionario. La variable local usuarioLogueado recibe
            // el objeto Funcionario que devuelve el servicio al validar las credenciales.
            Funcionario usuarioLogueado;
            try
            {
                // El servicio devuelve el funcionario activo cuya clave verificó.
                // Si rechaza las credenciales, lanza AuthenticationException y se omite la asignación.
                usuarioLogueado = service.login(datos.Usuario, datos.Clave);
            }
            catch (AuthenticationException ex)
            {
                // La excepción lanzada en Service.login llega a este catch del controlador.
                // La vista puede leer ViewBag.Error al renderizar esta misma solicitud.
                ViewBag.Error = ex.Message;
                return View(datos);
            }
            catch (Exception)
            {
                return errorConsulta();
            }

            // La identidad usa el ID y el nombre de la cuenta que devolvió el servicio.
            // SignInAsync emite la cookie antes de redirigir a Home/Index.
            var identidad = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuarioLogueado.Id.ToString()),
                new Claim(ClaimTypes.Name, usuarioLogueado.Usuario)
            }, "Cookies");
            await HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(identidad));
            // Tras iniciar sesión, el navegador hace una nueva petición a HomeController.Index.
            return Redirect("~/Home/Index");
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
            [FromRoute] int id,
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
