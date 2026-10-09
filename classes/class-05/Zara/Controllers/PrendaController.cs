using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Zara.Services;

namespace Zara.Controllers
{
    public class PrendaController : Controller
    {
        private readonly Service service;
        public PrendaController()
        {
            service = new Service();
        }
        
        // GET: PrendaController  Para mostrar la vista principal de prendas
        public ActionResult Index()
        {
            var prendas = service.mostrarPrendas();
            return View(prendas);
        }

        // GET: PrendaController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PrendaController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PrendaController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PrendaController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PrendaController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PrendaController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PrendaController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
