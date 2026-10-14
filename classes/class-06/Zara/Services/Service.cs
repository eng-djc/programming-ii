using System.Data.Entity;
using System.Data.Entity.Migrations;
using Zara.Models;

namespace Zara.Services
{
    /// <summary>Contexto EF6 y métodos CRUD de Prenda y Fabricante.</summary>
    public class Service : DbContext
    {
        // Inicializador de migraciones automáticas del curso.
        // Se ejecuta al consultar la base de datos, después de crear el contexto.
        // Clase 6 amplía la base Zara existente; conserva Prendas y agrega Fabricantes.
        static Service()
        {
            System.Data.Entity.Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<Service, Zara.Migrations.Configuration>());
        }

        // EF6 no lee directamente appsettings.json en ASP.NET Core.
        // Por eso se indica la conexión SQL Server LocalDB de forma explícita.
        public Service() : base(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Zara;Integrated Security=True;")
        {
        }

        // Cada DbSet representa una tabla en la base de datos.
        public DbSet<Prenda> Prendas { get; set; } = null!;
        public DbSet<Fabricante> Fabricantes { get; set; } = null!;

        #region CRUD de Prendas (Clase 5, conservado)
        public void agregarPrenda(Prenda prenda)
        {
            ArgumentNullException.ThrowIfNull(prenda);
            Prendas.Add(prenda);
            SaveChanges();
        }

        public List<Prenda> mostrarPrendas() => Prendas.ToList();

        public Prenda? buscarPrenda(int id) => Prendas.Find(id);

        public bool actualizarPrenda(Prenda prenda)
        {
            ArgumentNullException.ThrowIfNull(prenda);
            var existente = Prendas.Find(prenda.Id);
            if (existente == null) return false;
            existente.Marca = prenda.Marca;
            existente.Talla = prenda.Talla;
            existente.Precio = prenda.Precio;
            existente.Genero = prenda.Genero;
            SaveChanges();
            return true;
        }

        public bool eliminarPrenda(int id)
        {
            var existente = Prendas.Find(id);
            if (existente == null) return false;
            Prendas.Remove(existente);
            SaveChanges();
            return true;
        }
        #endregion

        #region CRUD de Fabricantes (Clase 6)
        // CREATE: agrega una fila y deja que SQL Server asigne el IDENTITY.
        public void agregarFabricante(Fabricante fabricante)
        {
            ArgumentNullException.ThrowIfNull(fabricante);
            Fabricantes.Add(fabricante);
            SaveChanges();
        }

        // READ: lista todos los fabricantes ordenados por ID.
        public List<Fabricante> mostrarFabricantes() => Fabricantes.OrderBy(f => f.Id).ToList();

        // READ: devuelve la entidad o null si el ID no existe.
        public Fabricante? buscarFabricante(int id) => Fabricantes.Find(id);

        // UPDATE: actualiza únicamente los dos campos editables.
        public bool actualizarFabricante(Fabricante fabricante)
        {
            ArgumentNullException.ThrowIfNull(fabricante);
            var existente = Fabricantes.Find(fabricante.Id);
            if (existente == null) return false;
            existente.Prestigio = fabricante.Prestigio;
            existente.FechaCreacion = fabricante.FechaCreacion;
            SaveChanges();
            return true;
        }

        // DELETE: elimina un fabricante identificado por ID.
        public bool eliminarFabricante(int id)
        {
            var existente = Fabricantes.Find(id);
            if (existente == null) return false;
            Fabricantes.Remove(existente);
            SaveChanges();
            return true;
        }
        #endregion
    }
}
