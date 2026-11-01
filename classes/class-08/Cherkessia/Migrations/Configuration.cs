using System.Data.Entity.Migrations;

namespace Cherkessia.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<Cherkessia.Services.Service>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = false;
        }
    }
}
