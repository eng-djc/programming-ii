
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using Zara.Models;

namespace Zara.Services
{
    /// <summary>
    /// Servicio encargado de las operaciones CRUD
    /// de las entidades Prenda y Fabricante.
    /// Hereda de DbContext para utilizar Entity Framework 6.
    /// </summary>
    public class Service : DbContext
    {
        #region Configuración de Entity Framework

        /// <summary>
        /// Configura las migraciones de la base de datos.
        /// Se ejecuta una sola vez por tipo.
        /// </summary>
        static Service()
        {
            System.Data.Entity.Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<Service,
                    Zara.Migrations.Configuration>());
        }

        /// <summary>
        /// Constructor que establece la conexión
        /// con la base de datos Zara en SQL Server LocalDB.
        /// </summary>
        public Service()
            : base(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Zara;Integrated Security=True;")
        {
        }

        /// <summary>
        /// Conjunto de registros de prendas.
        /// </summary>
        public DbSet<Prenda> Prendas { get; set; } = null!;

        /// <summary>
        /// Conjunto de registros de fabricantes.
        /// </summary>
        public DbSet<Fabricante> Fabricantes { get; set; } = null!;

        #endregion

        #region CRUD de Prendas (Código de la profesora)

        /// <summary>
        /// Agrega una nueva prenda a la base de datos.
        /// </summary>
        /// <param name="prendita">
        /// Objeto Prenda que se desea registrar.
        /// </param>
        public void agregarPrenda(Prenda prendita)
        {
            // Agrega la prenda al contexto.
            Prendas.Add(prendita);

            // Guarda el registro en la base de datos.
            SaveChanges();
        }

        /// <summary>
        /// Obtiene todas las prendas registradas.
        /// </summary>
        /// <returns>
        /// Lista de objetos Prenda.
        /// </returns>
        public List<Prenda> mostrarPrendas()
        {
            // Consulta las prendas y devuelve una lista.
            return Prendas.ToList();
        }

        /// <summary>
        /// Busca una prenda utilizando su identificador.
        /// </summary>
        /// <param name="id">
        /// Identificador de la prenda que se desea buscar.
        /// </param>
        /// <returns>
        /// Objeto Prenda encontrado.
        /// </returns>
        /// <exception cref="Exception">
        /// Se genera cuando la prenda no existe.
        /// </exception>
        public Prenda buscarPrenda(int id)
        {
            // Busca la primera prenda cuyo Id coincida.
            // Si no existe, FirstOrDefault devuelve null.
            var prendaBuscada = Prendas.FirstOrDefault(p => p.Id == id);

            // Verifica si se encontró la prenda.
            if (prendaBuscada != null)
                return prendaBuscada;

            // Genera una excepción si no existe.
            else throw new Exception("Esa prenda no se encuentra registrada");
        }

        /// <summary>
        /// Elimina una prenda de la base de datos.
        /// </summary>
        /// <param name="prendita">
        /// Objeto Prenda que se desea eliminar.
        /// </param>
        public void eliminarPrenda(Prenda prendita)
        {
            // Marca la prenda para eliminarla.
            Prendas.Remove(prendita);

            // Guarda la eliminación en la base de datos.
            SaveChanges();
        }

        /// <summary>
        /// Actualiza los datos de una prenda registrada.
        /// </summary>
        /// <param name="prendita">
        /// Objeto Prenda que contiene los nuevos valores.
        /// </param>
        /// <exception cref="Exception">
        /// Se genera cuando la prenda no existe.
        /// </exception>
        public void actualizarPrenda(Prenda prendita)
        {
            // Busca la prenda por su identificador.
            var prendaBuscada = Prendas.FirstOrDefault(p => p.Id == prendita.Id);

            // Verifica que el registro exista.
            if (prendaBuscada != null)
            {
                // Actualiza la marca de la prenda.
                prendaBuscada.Marca = prendita.Marca;

                // Actualiza la talla de la prenda.
                prendaBuscada.Talla = prendita.Talla;

                // Actualiza el precio de la prenda.
                prendaBuscada.Precio = prendita.Precio;

                // Guarda los cambios en la base de datos.
                SaveChanges();
            }

            // Genera una excepción si no encuentra el registro.
            else throw new Exception("No se encontraron registros de esa prenda");
        }

        #endregion

        #region CRUD de Fabricantes (Clase 6)

        /// <summary>
        /// Agrega un nuevo fabricante a la base de datos.
        /// </summary>
        /// <param name="fabricante">
        /// Objeto Fabricante que se desea registrar.
        /// </param>
        public void agregarFabricante(Fabricante fabricante)
        {
            // Verifica que el objeto no sea nulo.
            ArgumentNullException.ThrowIfNull(fabricante);

            // Valida las propiedades definidas en el modelo.
            Validator.ValidateObject(
                fabricante,
                new ValidationContext(fabricante),
                validateAllProperties: true);

            // El Id debe ser cero porque SQL Server
            // asigna automáticamente el identificador.
            if (fabricante.Id != 0)
                throw new ArgumentException(
                    "El ID de un fabricante nuevo debe ser cero.",
                    nameof(fabricante));

            // Agrega el fabricante al contexto.
            Fabricantes.Add(fabricante);

            // Guarda el registro en la base de datos.
            SaveChanges();
        }

        /// <summary>
        /// Obtiene todos los fabricantes registrados,
        /// ordenados por identificador.
        /// </summary>
        /// <returns>
        /// Lista de objetos Fabricante.
        /// </returns>
        public List<Fabricante> mostrarFabricantes()
        {
            // Ordena los fabricantes por Id y devuelve la lista.
            return Fabricantes.OrderBy(f => f.Id).ToList();
        }

        /// <summary>
        /// Busca un fabricante utilizando su identificador.
        /// </summary>
        /// <param name="id">
        /// Identificador del fabricante que se desea buscar.
        /// </param>
        /// <returns>
        /// Fabricante encontrado o null si no existe.
        /// </returns>
        public Fabricante? buscarFabricante(int id)
        {
            // Find busca el registro mediante su clave primaria.
            return Fabricantes.Find(id);
        }

        /// <summary>
        /// Actualiza los datos de un fabricante existente.
        /// </summary>
        /// <param name="fabricante">
        /// Objeto que contiene los nuevos valores.
        /// </param>
        /// <returns>
        /// true si el fabricante existe y se actualiza;
        /// false si no se encuentra.
        /// </returns>
        public bool actualizarFabricante(Fabricante fabricante)
        {
            // Verifica que el objeto no sea nulo.
            ArgumentNullException.ThrowIfNull(fabricante);

            // Valida las propiedades del fabricante.
            Validator.ValidateObject(
                fabricante,
                new ValidationContext(fabricante),
                validateAllProperties: true);

            // Comprueba que el identificador sea positivo.
            if (fabricante.Id <= 0)
                throw new ArgumentException(
                    "El ID del fabricante debe ser positivo.",
                    nameof(fabricante));

            // Busca el fabricante por su identificador.
            var existente = Fabricantes.Find(fabricante.Id);

            // Devuelve false si no encuentra el registro.
            if (existente == null)
                return false;

            // Actualiza el prestigio del fabricante.
            existente.Prestigio = fabricante.Prestigio;

            // Actualiza la fecha de creación.
            existente.FechaCreacion = fabricante.FechaCreacion;

            // Guarda las modificaciones.
            SaveChanges();

            // Confirma que se actualizó el registro.
            return true;
        }

        /// <summary>
        /// Elimina un fabricante utilizando su identificador.
        /// </summary>
        /// <param name="id">
        /// Identificador del fabricante que se desea eliminar.
        /// </param>
        /// <returns>
        /// true si el fabricante se elimina;
        /// false si no existe.
        /// </returns>
        public bool eliminarFabricante(int id)
        {
            // Busca el fabricante por su identificador.
            var existente = Fabricantes.Find(id);

            // Comprueba si el registro existe.
            if (existente == null)
                return false;

            // Marca el fabricante para eliminarlo.
            Fabricantes.Remove(existente);

            // Guarda la eliminación en la base de datos.
            SaveChanges();

            // Confirma que se eliminó el registro.
            return true;
        }

        #endregion
    }
}
