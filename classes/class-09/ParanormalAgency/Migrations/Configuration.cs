using System.Data.Entity.Migrations;
namespace ParanormalAgency.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<ParanormalAgency.Services.Service>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = false;
        }
    }
}