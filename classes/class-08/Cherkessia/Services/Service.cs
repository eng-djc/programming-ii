using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using Microsoft.Extensions.Configuration;
using Cherkessia.Models;

namespace Cherkessia.Services
{
    public class Service : DbContext
    {
        static Service()
        {
            System.Data.Entity.Database.SetInitializer(new MigrateDatabaseToLatestVersion<Service,
                Cherkessia.Migrations.Configuration>(useSuppliedContext: true));
        }

        // Constructor directo usado por Zara/EGamer. Permite sustituir la conexión
        // mediante appsettings o variables ASPNETCORE para pruebas aisladas.
        public Service() : base(ObtenerConexion()) { }

        private static string ObtenerConexion()
        {
            var configuracion = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            return configuracion.GetConnectionString("BloggingDatabase")
                ?? @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Cherkessia;Integrated Security=True;";
        }

        public DbSet<Instrumento> Instrumentos { get; set; } = null!;
        public DbSet<Volcan> Volcanes { get; set; } = null!;

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Instrumento>().ToTable("Instrumentos");
            modelBuilder.Entity<Instrumento>().Property(i => i.Precio).HasPrecision(18, 2);
            modelBuilder.Entity<Volcan>().ToTable("Volcanes");
            modelBuilder.Entity<Volcan>().HasRequired(v => v.Instrumento)
                .WithMany().HasForeignKey(v => v.InstrumentoCodigo).WillCascadeOnDelete(false);
        }

        #region CRUD de instrumentos
        public List<Instrumento> mostrarInstrumentos() => Instrumentos.OrderBy(i => i.Codigo).ToList();

        public Instrumento? buscarInstrumento(int codigo) => Instrumentos.Find(codigo);

        public void agregarInstrumento(Instrumento instrumento)
        {
            Validator.ValidateObject(instrumento, new ValidationContext(instrumento), true);
            if (Instrumentos.Any(i => i.Codigo == instrumento.Codigo))
                throw new ValidationException("Ya existe un instrumento con ese código.");
            Instrumentos.Add(instrumento);
            SaveChanges();
        }

        public bool actualizarInstrumento(Instrumento instrumento)
        {
            Validator.ValidateObject(instrumento, new ValidationContext(instrumento), true);
            var existente = buscarInstrumento(instrumento.Codigo);
            if (existente == null) return false;
            existente.Nombre = instrumento.Nombre;
            existente.FechaRegistro = instrumento.FechaRegistro;
            existente.Precio = instrumento.Precio;
            existente.Garantia = instrumento.Garantia;
            SaveChanges();
            return true;
        }
        #endregion

        #region CRUD de volcanes
        public List<Volcan> mostrarVolcanes() => Volcanes.Include(v => v.Instrumento).OrderBy(v => v.Nombre).ToList();

        public Volcan? buscarVolcan(string nombre) => Volcanes.Include(v => v.Instrumento).FirstOrDefault(v => v.Nombre == nombre);

        private Instrumento validarVolcan(Volcan volcan)
        {
            Validator.ValidateObject(volcan, new ValidationContext(volcan), true);
            var instrumento = buscarInstrumento(volcan.InstrumentoCodigo);
            if (instrumento == null) throw new ValidationException("El instrumento seleccionado no existe.");
            return instrumento;
        }

        public void agregarVolcan(Volcan volcan)
        {
            var instrumento = validarVolcan(volcan);
            if (Volcanes.Any(v => v.Nombre == volcan.Nombre))
                throw new ValidationException("Ya existe un volcán con ese nombre.");
            volcan.calcularIRV(instrumento, DateTime.Today);
            Volcanes.Add(volcan);
            SaveChanges();
        }

        public bool actualizarVolcan(Volcan volcan)
        {
            var instrumento = validarVolcan(volcan);
            var existente = buscarVolcan(volcan.Nombre);
            if (existente == null) return false;
            existente.Provincia = volcan.Provincia;
            existente.Activo = volcan.Activo;
            existente.Gases = volcan.Gases;
            existente.InstrumentoCodigo = volcan.InstrumentoCodigo;
            existente.Instrumento = instrumento;
            existente.calcularIRV(instrumento, DateTime.Today);
            SaveChanges();
            return true;
        }

        public bool eliminarVolcan(string nombre)
        {
            var existente = buscarVolcan(nombre);
            if (existente == null) return false;
            Volcanes.Remove(existente);
            SaveChanges();
            return true;
        }
        #endregion
    }
}
