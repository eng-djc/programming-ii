# Semana 4 — Formulario MVC de usuarios

Copia de la semana 3 en el commit `3cc3f61`. La semana 3 conserva su contenido original.
El formulario de homework se trasladó a `ProyectoWeb/Views/Home/AgregarUsuario.cshtml`.

## Ejecutar

```bash
cd classes/class-04/ProyectoWeb
dotnet build ProyectoWeb.csproj
dotnet run --project ProyectoWeb.csproj
```

Abrir la URL que muestra la consola seguida de `/Home/AgregarUsuario`.
La vista Razor se ejecuta desde ASP.NET Core; no se abre con explorer.exe.

## Vista, modelo y servicio

| Campo de la vista / propiedad | Tipo C# | Regla |
| --- | --- | --- |
| nombre | string | Obligatorio; letras y espacios |
| edad | int? | Obligatorio; entero no negativo |
| fechaNacimiento | DateTime? | Obligatoria |
| perfil | string | administrativo, plataforma o docente |
| permisosExtras | string | si o no |
| otros | string? | Opcional |
| usuario | string | Obligatorio |
| clave | string | Obligatoria; no se almacena ni se devuelve |

Se conservan exactamente los nombres del formulario a petición del ejercicio.
Aunque las propiedades C# suelen usar PascalCase, aquí se mantiene la correspondencia literal.
Las propiedades usan `public get; set;` para la vinculación de MVC.
Los tipos nullable permiten distinguir un campo vacío de un valor numérico o de fecha.

`Usuario` tiene un constructor vacío y otro con los ocho campos.
`Service` tiene un constructor vacío y otro que recibe un Usuario.
Su referencia interna es `private readonly` y su método de validación es `public`.

El controlador ofrece GET y POST, valida el token antifalsificación y los datos del modelo.
El servicio reutiliza las reglas DataAnnotations; MVC detecta también conversiones inválidas.
Se conserva el filtro inmediato de números en Nombre.
Un envío válido muestra una confirmación de validación: no crea cuentas ni persiste datos.
