# Programación II – Clase 6: Zara, Prendas y Fabricantes

Proyecto ASP.NET Core MVC (.NET 8) que conserva la Clase 5 y agrega la entidad **Fabricante**.

## Modelo MVC y CRUD

- `Models/Fabricante.cs`: `Id` autoincremental (`IDENTITY`); `Prestigio` obligatorio, selección **Bajo / Medio / Alto**; `FechaCreacion` obligatoria, tipo SQL `date` y sin hora en HTML.
- `Services/Service.cs`: `DbSet<Prenda> Prendas` + `DbSet<Fabricante> Fabricantes` y cinco operaciones CRUD por entidad.
- `Controllers/FabricanteController.cs`: GET `Index`, `Details`, `Create`, `Edit`, `Delete`; POST `Create`, `Edit`, `Delete` con antiforgery y validación.
- `Views/Fabricante/`: Index, Details, Create, Edit, Delete. `select` obliga a elegir exactamente una opción; `input type=date` solicita solo fecha.
- `Views/Shared/_Layout.cshtml`: menú para Prendas y Fabricantes.

## Pasos exigidos en el curso

1. **Clase lógica**: `Models/Fabricante.cs` (incluye `[Key]` y `[DatabaseGenerated(DatabaseGeneratedOption.Identity)]`).
2. **Instalar EF6 (YA REALIZADO)**: `Zara.csproj` conserva `EntityFramework 6.5.2` del proyecto de Clase 5; no reinstalar.
3. **Configurar appsettings.json**: `ConnectionStrings:BloggingDatabase` apunta a `(localdb)\\MSSQLLocalDB`, base `Zara`. **Importante:** EF6 no lee este archivo automáticamente; `Service.cs` usa explícitamente la misma cadena.
4. **Configurar Service**: contexto EF6 de `Prenda` y `Fabricante`, sobre la misma base de datos Zara.
5. **Enable-Migrations (YA REALIZADO)**: ya estaba habilitado en Clase 5 y se copió `Migrations/Configuration.cs`. No ejecute `Enable-Migrations` de nuevo sobre un proyecto que ya contiene esta configuración.
6. **Configuration a true (YA REALIZADO)**: `Migrations/Configuration.cs` contiene `AutomaticMigrationsEnabled = true`.
7. **Update-Database**: en **Visual Studio para Windows → Tools → NuGet Package Manager → Package Manager Console**, seleccionar proyecto Zara de **Clase 6** y ejecutar:

   ```powershell
   Update-Database -ConfigurationTypeName Zara.Migrations.Configuration -ConnectionString "Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Zara;Integrated Security=True;" -ConnectionProviderName "System.Data.SqlClient" -Verbose
   ```

   Alternativamente, el `Service` de Clase 6 configura `MigrateDatabaseToLatestVersion` para aplicar migraciones automáticas cuando se usa por primera vez el contexto. Esto NO implica que los comandos del paso 7 se hayan ejecutado ya: compruebe el resultado en SQL Server.
8. **Verificar BD**: conectar en SQL Server Object Explorer/SSMS a `(localdb)\\MSSQLLocalDB`, base `Zara` y comprobar `dbo.Prendas`, `dbo.Fabricantes` y `dbo.__MigrationHistory`. Consulta sugerida:

   ```sql
   USE Zara;
   SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE';
   SELECT TOP (10) Id, Prestigio, FechaCreacion FROM dbo.Fabricantes;
   ```

9. **CRUD en Service**: `agregarFabricante`, `mostrarFabricantes`, `buscarFabricante`, `actualizarFabricante`, `eliminarFabricante`.
10. **Controller**: `Controllers/FabricanteController.cs` implementa acciones GET/POST con validación y control de errores.
11. **Vistas**: `Views/Fabricante/*.cshtml` contiene formularios documentados en Razor.
12. **Menú**: `Views/Shared/_Layout.cshtml` contiene `Fabricantes`.

## Ejecución

Trabajar en **Windows/Visual Studio**, porque SQL Server LocalDB no se ejecuta nativamente en WSL. Detener otras instancias para evitar bloqueo de `Zara.exe`.

```powershell
cd C:\GitHub\programming-ii\classes\class-06\Zara
dotnet build Zara.csproj
dotnet run --launch-profile http
```

Páginas: `http://localhost:5260/Prenda` y `http://localhost:5260/Fabricante`.

**Evolución:** Clase 6 utiliza la misma base `Zara`: conserva la tabla `Prendas` y agrega `Fabricantes` mediante migraciones de EF6. No eliminar ni recrear la base.
