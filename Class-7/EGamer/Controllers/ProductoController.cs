using EGamer.Models;
using EGamer.Services;
using Microsoft.AspNetCore.Mvc;

namespace EGamer.Controllers;

/// <summary>Controlador MVC: solicitudes HTTP y validación, sin cálculo matemático.</summary>
public class ProductoController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        using var service = new Service();
        return View(service.mostrarProductos());
    }

    [HttpGet]
    public IActionResult Create() => View(new Producto());

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Create([Bind("FechaFactura,Nombre,Proveedor,Precio,Calidad,Descripcion,EntregaDomicilio")] Producto producto)
    {
        if (!ModelState.IsValid) return View(producto);
        try
        {
            using var service = new Service();
            service.agregarProducto(producto);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception)
        {
            ModelState.AddModelError("", "No se pudo guardar. Revise la base de datos.");
            return View(producto);
        }
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        if (!ModelState.IsValid || id <= 0) return BadRequest();
        using var service = new Service();
        var producto = service.buscarProducto(id);
        return producto == null ? NotFound() : View(producto);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Edit([FromRoute] int id, [Bind("Id,FechaFactura,Nombre,Proveedor,Precio,Calidad,Descripcion,EntregaDomicilio")] Producto producto)
    {
        if (id <= 0 || producto.Id != id) return BadRequest();
        if (!ModelState.IsValid) return View(producto);
        try
        {
            using var service = new Service();
            if (!service.actualizarProducto(producto)) return NotFound();
            return RedirectToAction(nameof(Index));
        }
        catch (Exception)
        {
            ModelState.AddModelError("", "No se pudo actualizar.");
            return View(producto);
        }
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        if (!ModelState.IsValid || id <= 0) return BadRequest();
        using var service = new Service();
        return service.eliminarProducto(id) ? RedirectToAction(nameof(Index)) : NotFound();
    }
}