using System.Data.Entity;
using Zara.Models;

namespace Zara.Services
{
    public class Service : DbContext
    {
        // Constructor para inicializar el contexto de la base de datos
        public Service() : base("Zara")
        {
        }

        // Representa la tabla Prendas en la base de datos
        public DbSet<Prenda> Prendas { get; set; } = null!;

        #region Metodos CRUD para Prendas

        // Agregar una nueva prenda
        public void agregarPrenda(Prenda prenda)
        {
            ArgumentNullException.ThrowIfNull(prenda);

            Prendas.Add(prenda);
            SaveChanges();
        }

        // Mostrar todas las prendas
        public List<Prenda> mostrarPrendas()
        {
            return Prendas.ToList();
        }

        // Buscar una prenda por ID
        public Prenda? buscarPrenda(int id)
        {
            return Prendas.Find(id);
        }

        // Actualizar una prenda existente
        public bool actualizarPrenda(Prenda prenda)
        {
            ArgumentNullException.ThrowIfNull(prenda);

            var existente = Prendas.Find(prenda.Id);

            if (existente == null)
                return false;

            existente.Marca = prenda.Marca;
            existente.Talla = prenda.Talla;
            existente.Precio = prenda.Precio;
            existente.Genero = prenda.Genero;

            SaveChanges();
            return true;
        }

        // Eliminar una prenda por ID
        public bool eliminarPrenda(int id)
        {
            var prenda = Prendas.Find(id);

            if (prenda == null)
                return false;

            Prendas.Remove(prenda);
            SaveChanges();
            return true;
        }

        #endregion
    }
}
