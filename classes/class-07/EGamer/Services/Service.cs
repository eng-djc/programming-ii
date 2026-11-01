using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using EGamer.Models;

namespace EGamer.Services;

/// <summary>DbContext y CRUD como en Clase 6. Sin operaciones monetarias.</summary>
public class Service : DbContext
{
    static Service() => System.Data.Entity.Database.SetInitializer(
        new MigrateDatabaseToLatestVersion<Service, EGamer.Migrations.Configuration>());

    public Service() : base(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EGamer;Integrated Security=True;") { }

    public DbSet<Producto> Productos { get; set; } = null!;

    protected override void OnModelCreating(DbModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Producto>().Property(p => p.Precio).HasPrecision(18, 2);
    }

    private static void Validar(Producto producto)
    {
        ArgumentNullException.ThrowIfNull(producto);
        Validator.ValidateObject(producto, new ValidationContext(producto), true);
        var errores = producto.Validate(new ValidationContext(producto)).ToList();
        if (errores.Count > 0) throw new ValidationException(errores[0].ErrorMessage);
    }

    public void agregarProducto(Producto producto)
    {
        Validar(producto);
        if (producto.Id != 0) throw new ArgumentException("SQL Server debe generar el Id.");
        Productos.Add(producto);
        SaveChanges();
    }

    public List<Producto> mostrarProductos() => Productos.OrderBy(p => p.Id).ToList();

    public Producto? buscarProducto(int id) => id > 0 ? Productos.Find(id) : null;

    public bool actualizarProducto(Producto producto)
    {
        Validar(producto);
        if (producto.Id <= 0) throw new ArgumentException("Id inválido.");
        var actual = Productos.Find(producto.Id);
        if (actual == null) return false;
        actual.FechaFactura = producto.FechaFactura;
        actual.Nombre = producto.Nombre;
        actual.Proveedor = producto.Proveedor;
        actual.Precio = producto.Precio;
        actual.Calidad = producto.Calidad;
        actual.Descripcion = producto.Descripcion;
        actual.EntregaDomicilio = producto.EntregaDomicilio;
        SaveChanges();
        return true;
    }

    public bool eliminarProducto(int id)
    {
        var actual = buscarProducto(id);
        if (actual == null) return false;
        Productos.Remove(actual);
        SaveChanges();
        return true;
    }
}