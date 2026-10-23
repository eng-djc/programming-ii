# Programación II — Clase 7: E Gamer

Proyecto **ASP.NET Core MVC / .NET 8** desarrollado siguiendo la arquitectura y metodología de **Zara Clase 6**: Entity Framework 6.5.2, contexto `Service`, controlador, modelos Razor y SQL Server LocalDB.

## Organización

- `Models/Producto.cs`: propiedades, validación y **todos los cálculos matemáticos**.
- `Services/Service.cs`: contexto EF6 y CRUD, sin cálculos monetarios.
- `Controllers/ProductoController.cs`: acciones MVC HTTP GET/POST, antiforgery, ModelState.
- `Views/Producto`: Index, Create, Edit.
- `Migrations/Configuration.cs`: migraciones automáticas EF6, como Zara Clase 6.
- `docs/uml.md`: UML de clases.

## Campos requeridos

Fecha de factura (`date`), Nombre (`text`), Proveedor (`radio` p1/p2/p3/p4/p5), Precio (`number`), Calidad (`select` baja/media/alta), Descripción (`textarea`) y Entrega (`select` si/no) tanto en creación como edición. Index lista los valores y el monto total.

## Reglas matemáticas

La clase `Producto` determina el porcentaje de calidad (baja 2%, media 4%, alta 6%). Envío opcional $10. IVA 13%. **Supuesto necesario:** el IVA se calcula sobre precio + recargo por calidad + entrega, porque el enunciado no define explícitamente el orden:

`MontoPagar = Round((precio * (1 + porcentajeCalidad) + (entrega == "si" ? 10 : 0)) * 1.13, 2)`

Ejemplo: precio $100, calidad media, entrega sí: $100 + $4 + $10 = $114; IVA $14.82; total **$128.82**. Redondeo a dos decimales con `MidpointRounding.AwayFromZero`.

## Abrir y ejecutar en Visual Studio

En Windows, abrir `EGamer.sln` con Visual Studio 2022 compatible con .NET 8. Restaurar y compilar:

```powershell
dotnet restore
dotnet build
dotnet run --launch-profile http
```

URL: `http://localhost:5267/Producto`.

## Configuración de base de datos

SQL Server Express LocalDB en Windows; la base debe llamarse **EGamer** y su tabla **Producto** (PK Id). `Service.cs` configura la cadena de conexión directamente como Clase 6; `appsettings.json` refleja esa configuración, pero EF6 no la lee automáticamente.

En Visual Studio: Tools → NuGet Package Manager → Package Manager Console, seleccionar EGamer y ejecutar:

```powershell
Update-Database -ConfigurationTypeName EGamer.Migrations.Configuration -ConnectionString "Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=EGamer;Integrated Security=True;" -ConnectionProviderName "System.Data.SqlClient" -Verbose
```

La configuración incluye `AutomaticMigrationsEnabled = true` y `MigrateDatabaseToLatestVersion` igual al proyecto de referencia. Comprobar desde SQL Server Object Explorer:

```sql
USE EGamer;
SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE';
SELECT TOP (10) * FROM dbo.Producto;
```

## Validaciones / fuzzing HTML

Comprobar los campos obligatorios, precios negativos o no numéricos, fechas imposibles, opciones alteradas en radio y select, IDs inconsistentes o inexistentes, y cadenas con HTML/JS. `ModelState.IsValid`, atributos del modelo, verificación también en Service, `Bind` y antiforgery en POST. Razor codifica en HTML los campos al mostrarlos; no se interpreta HTML enviado como texto.

## Estado de verificación

Estructura generada y lista para pruebas; **no se ha ejecutado** la compilación, Visual Studio ni SQL Server LocalDB en este entorno. Ver también `docs/VERIFICACION.md`. No se alteran Clase 5 ni Clase 6.

## Presentación visual (plantilla Visual Studio)

Se conserva la estructura del layout de Zara Clase 6 (`Home`, `Privacy`, navegación y pie de página), con reemplazo exclusivo del nombre Zara por EGamer y la entidad Prendas por Productos. `Views/Home/Index.cshtml` y `Views/Home/Privacy.cshtml` siguen la plantilla predeterminada MVC. Los recursos `wwwroot` se reutilizan de Clase 6 sin rediseño. Para probar estas rutas: `/Home/Index`, `/Home/Privacy`, `/Producto/Index`. El listado usa tabla `table table-striped` y botón Bootstrap como Zara.
