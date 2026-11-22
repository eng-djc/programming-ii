
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Zara.Models;

namespace Zara.Services
{
    /// <summary>
    /// Servicio encargado de las operaciones CRUD
    /// de las entidades Prenda, Fabricante y Funcionario.
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
        public DbSet<Prenda> Prendas { get; set; }

        /// <summary>
        /// Conjunto de registros de fabricantes.
        /// </summary>
        public DbSet<Fabricante> Fabricantes { get; set; }

        /// <summary>
        /// Conjunto de registros de funcionarios.
        /// </summary>
        public DbSet<Funcionario> Funcionarios { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Funcionario>().ToTable("Funcionarios");
            modelBuilder.Entity<Funcionario>().Property(f => f.ClaveHash).IsRequired();
        }

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
            Prendas.Add(prendita);
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
            var prendaBuscada = Prendas.FirstOrDefault(p => p.Id == id);

            if (prendaBuscada != null)
                return prendaBuscada;

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
            Prendas.Remove(prendita);
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
            var prendaBuscada = Prendas.FirstOrDefault(p => p.Id == prendita.Id);

            if (prendaBuscada != null)
            {
                prendaBuscada.Marca = prendita.Marca;
                prendaBuscada.Talla = prendita.Talla;
                prendaBuscada.Precio = prendita.Precio;
                prendaBuscada.Genero = prendita.Genero;
                SaveChanges();
            }

            else throw new Exception("No se encontraron registros de esa prenda");
        }

        #endregion

        #region CRUD de Funcionarios

        public List<Funcionario> mostrarFuncionarios()
        {
            return Funcionarios.OrderBy(f => f.Nombre).ToList();
        }

        public Funcionario? buscarFuncionario(int id)
        {
            return Funcionarios.Find(id);
        }

        public void agregarFuncionario(Funcionario funcionario)
        {
            funcionario.ClaveHash =
                new PasswordHasher<Funcionario>().HashPassword(funcionario, funcionario.Clave!);
            funcionario.Clave = null;

            Funcionarios.Add(funcionario);
            SaveChanges();
        }

        public bool actualizarFuncionario(Funcionario funcionario)
        {
            var existente = buscarFuncionario(funcionario.Id);
            if (existente == null)
                return false;

            existente.Nombre = funcionario.Nombre;
            existente.Cedula = funcionario.Cedula;
            existente.Telefono = funcionario.Telefono;
            existente.FechaNacimiento = funcionario.FechaNacimiento;
            existente.Activo = funcionario.Activo;
            existente.Perfil = funcionario.Perfil;
            existente.Usuario = funcionario.Usuario;

            if (!string.IsNullOrWhiteSpace(funcionario.Clave))
            {
                existente.ClaveHash =
                    new PasswordHasher<Funcionario>().HashPassword(existente, funcionario.Clave);
            }

            SaveChanges();
            return true;
        }

        public bool eliminarFuncionario(int id)
        {
            var existente = buscarFuncionario(id);
            if (existente == null)
                return false;

            Funcionarios.Remove(existente);
            SaveChanges();
            return true;
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
            Fabricantes.Add(fabricante);
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
            var existente = Fabricantes.Find(fabricante.Id);

            if (existente == null)
                return false;

            existente.Nombre = fabricante.Nombre;
            existente.Prestigio = fabricante.Prestigio;
            existente.FechaCreacion = fabricante.FechaCreacion;
            SaveChanges();

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
            var existente = Fabricantes.Find(id);

            if (existente == null)
                return false;

            Fabricantes.Remove(existente);
            SaveChanges();

            return true;
        }

        #endregion
    }
}
