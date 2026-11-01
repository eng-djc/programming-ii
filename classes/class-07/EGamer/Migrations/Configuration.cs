using System.Data.Entity.Migrations;
namespace EGamer.Migrations;
internal sealed class Configuration : DbMigrationsConfiguration<EGamer.Services.Service>
{
    public Configuration() { AutomaticMigrationsEnabled = true; }
    protected override void Seed(EGamer.Services.Service context) { }
}