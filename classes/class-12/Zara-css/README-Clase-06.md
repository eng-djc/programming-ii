# Programación II – Clase 6: Zara, Prendas y Fabricantes

Proyecto ASP.NET Core MVC (.NET 8) que conserva la Clase 5 y agrega la entidad **Fabricante**.

## Modelo MVC y CRUD

- `Models/Fabricante.cs`: `Id` autoincremental (`IDENTITY`); `Nombre` obligatorio en formularios, máximo 100 caracteres (columna nullable para conservar registros previos); `Prestigio` obligatorio, selección **Bajo / Medio / Alto**; `FechaCreacion` obligatoria, tipo SQL `date` y sin hora en HTML.
- `Services/Service.cs`: `DbSet<Prenda> Prendas` + `DbSet<Fabricante> Fabricantes` y cinco operaciones CRUD por entidad.
- `Controllers/FabricanteController.cs`: GET `Index`, `Details`, `Create`, `Edit`; POST `Create`, `Edit`, `Delete` con antiforgery y validación. Eliminar desde Index sin confirmación.
- `Views/Fabricante/`: Index (sin ID ni enlace Detalles, con botón Eliminar por POST), Details (acceso directo), Create y Edit. `select` obliga a elegir exactamente una opción; `input type=date` solicita solo fecha.
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
   SELECT TOP (10) Id, Nombre, Prestigio, FechaCreacion FROM dbo.Fabricantes;
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

## Validación de Fabricante en el servidor

El navegador facilita la entrada, pero un cliente puede modificar el HTML o enviar
un POST directamente. Las mismas reglas deben comprobarse antes de guardar:

| Campo | Regla | Dónde se comprueba |
| --- | --- | --- |
| Nombre | Obligatorio al crear o editar; máximo 100 caracteres; nueva columna nullable para preservar registros anteriores | Validación MVC, IValidatableObject y MaxLength |
| Prestigio | Obligatorio; exactamente Bajo, Medio o Alto, respetando mayúsculas y sin espacios adicionales | DataAnnotations del modelo |
| FechaCreacion | Obligatoria; fecha convertible a DateTime y sin componente horario distinto de cero | Model binding de MVC, Required e IValidatableObject |
| Id al crear | SQL Server lo genera; no se recibe en el Bind | Controlador y servicio |
| Id al editar | Positivo e igual al ID de la ruta | Controlador; positividad también en el servicio |

No se impone una fecha máxima: el formulario original no prohíbe fechas futuras.
Una fecha imposible, como 2026-02-30, produce un error de conversión en MVC.

### Recorrido del código

1. MVC convierte los campos HTTP y valida las anotaciones de Fabricante. Los errores
   quedan en ModelState. IValidatableObject agrega la regla de fecha sin hora.
2. Los POST Create y Edit revisan ModelState.IsValid **antes** de llamar al servicio.
   Cuando falla, View(fabricante) conserva la entrada y muestra los errores mediante
   asp-validation-for y asp-validation-summary.
3. Edit toma el ID de la URL con FromRoute y lo compara con el campo oculto.
   Un ID no positivo o diferente devuelve HTTP 400; un registro inexistente devuelve 404.
4. El servicio usa Validator.ValidateObject antes de modificar entidades, para aplicar
   las mismas reglas a llamadas desde código que no pasen por el controlador.
   Una entrada inválida lanza ValidationException. La conversión de texto HTTP pertenece
   a MVC; el servicio recibe una fecha ya tipada.
5. Bind limita las propiedades recibidas y ValidateAntiForgeryToken comprueba el token
   del formulario. Ninguno sustituye las reglas de validación de valores.

### Verificación manual

Ejecutar con .NET 8 y SQL Server LocalDB configurado:

```bash
dotnet build Zara.csproj
dotnet run --project Zara.csproj
```

Probar tanto Create como Edit. Para probar valores que el navegador bloquea,
modificar el formulario desde las herramientas de desarrollo y usar novalidate,
conservando el token antiforgery válido.

| Entrada o acción | Resultado esperado |
| --- | --- |
| Bajo / Medio / Alto y 2026-10-15 | Guardado correcto |
| Prestigio vacío, espacios, Otro, bajo o Alto con un espacio final | Error de prestigio; no guardar |
| Prestigio de más de cinco caracteres | Error de validación; no guardar |
| Fecha vacía o 2026-02-30 | Error de fecha; no guardar |
| Fecha 2026-10-15T12:30:00 enviada manualmente | Error de fecha sin hora; no guardar |
| ID de la URL distinto del campo oculto al editar | HTTP 400; no guardar |
| ID cero, negativo o no numérico | HTTP 400; no modificar datos |
| ID positivo inexistente, con los demás campos válidos | HTTP 404 |
| Corregir un formulario rechazado | Guardar y redirigir al listado |
| Llamar agregarFabricante/actualizarFabricante con prestigio Otro desde C# | ValidationException antes de modificar entidades |

Después de un envío inválido, consultar el listado para confirmar que no se insertó
ni se actualizó el registro.
