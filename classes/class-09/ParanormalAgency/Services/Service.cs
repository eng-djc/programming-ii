using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using Microsoft.Extensions.Configuration;
using ParanormalAgency.Models;

namespace ParanormalAgency.Services
{
    public class Service : DbContext
    {
        static Service()
        {
            System.Data.Entity.Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<Service, ParanormalAgency.Migrations.Configuration>(useSuppliedContext: true));
        }

        public Service() : base(GetConnection()) { }

        private static string GetConnection()
        {
            var config = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true).AddEnvironmentVariables().Build();
            return config.GetConnectionString("BloggingDatabase")
                ?? @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=ParanormalAgency;Integrated Security=True;";
        }

        public DbSet<Investigation> Investigations { get; set; } = null!;

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Investigation>().ToTable("Investigations");
            builder.Entity<Investigation>().Property(x => x.BaseBudget).HasPrecision(18, 2);
            builder.Entity<Investigation>().Property(x => x.TotalCost).HasPrecision(18, 2);
        }

        public List<Investigation> GetInvestigations() => Investigations.OrderBy(x => x.Id).ToList();
        public Investigation? FindInvestigation(int id) => Investigations.Find(id);

        private static void ValidateInvestigation(Investigation item)
        {
            Validator.ValidateObject(item, new ValidationContext(item), validateAllProperties: true);
            // Validator.ValidateObject no ejecuta IValidatableObject en todos los escenarios
            // con propiedades inválidas; se validan explícitamente sus reglas cruzadas.
            var errors = item.Validate(new ValidationContext(item)).ToList();
            if (errors.Count > 0) throw new ValidationException(errors[0].ErrorMessage);
            item.CalculateTotalCost();
        }

        public void AddInvestigation(Investigation item)
        {
            ValidateInvestigation(item);
            Investigations.Add(item);
            SaveChanges();
        }

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