using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using Microsoft.Extensions.Configuration;
using ParanormalAgency.Models;

namespace ParanormalAgency.Services
{
    public class Service : DbContext
    {
        // Configura la actualización del esquema de la base de datos mediante EF6.
        static Service()
        {
            System.Data.Entity.Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<Service, ParanormalAgency.Migrations.Configuration>(useSuppliedContext: true));
        }

        public Service() : base(GetConnection()) { }

        // Obtiene la conexión configurada o utiliza SQL Server LocalDB.
        private static string GetConnection()
        {
            var config = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true).AddEnvironmentVariables().Build();
            return config.GetConnectionString("BloggingDatabase")
                ?? @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ParanormalAgency;Integrated Security=True;";
        }

        public DbSet<Investigation> Investigations { get; set; } = null!;

        // Asocia las investigaciones con su tabla y define la precisión monetaria.
        protected override void OnModelCreating(DbModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Investigation>().ToTable("Investigations");
            builder.Entity<Investigation>().Property(x => x.BaseBudget).HasPrecision(18, 2);
            builder.Entity<Investigation>().Property(x => x.TotalCost).HasPrecision(18, 2);
        }

        // Recupera las investigaciones ordenadas por identificador.
        public List<Investigation> GetInvestigations() => Investigations.OrderBy(x => x.Id).ToList();
        // Busca una investigación por su identificador.
        public Investigation? FindInvestigation(int id) => Investigations.Find(id);

        // Valida los datos y calcula el costo antes de guardar.
        private static void ValidateInvestigation(Investigation item)
        {
            Validator.ValidateObject(item, new ValidationContext(item), validateAllProperties: true);
            var errors = item.Validate(new ValidationContext(item)).ToList();
            if (errors.Count > 0) throw new ValidationException(errors[0].ErrorMessage);
            item.CalculateTotalCost();
        }

        // Registra una investigación en la base de datos.
        public void AddInvestigation(Investigation item)
        {
            ValidateInvestigation(item);
            Investigations.Add(item);
            SaveChanges();
        }

        // Actualiza los datos y el costo de una investigación existente.
        public bool UpdateInvestigation(Investigation item)
        {
            ValidateInvestigation(item);
            var current = FindInvestigation(item.Id);
            if (current == null) return false;
            current.CaseName = item.CaseName;
            current.ClientName = item.ClientName;
            current.PhenomenonType = item.PhenomenonType;
            current.RiskLevel = item.RiskLevel;
            current.BaseBudget = item.BaseBudget;
            current.RequiresSpecializedEquipment = item.RequiresSpecializedEquipment;
            current.InvestigationDate = item.InvestigationDate;
            current.CalculateTotalCost();
            SaveChanges();
            return true;
        }

        // Elimina una investigación existente por su identificador.
        public bool DeleteInvestigation(int id)
        {
            var item = FindInvestigation(id);
            if (item == null) return false;
            Investigations.Remove(item);
            SaveChanges();
            return true;
        }
    }
}