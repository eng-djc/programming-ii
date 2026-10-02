# Semana 4 — Formulario MVC de usuarios

Copia de semana 3; el formulario está en `ProyectoWeb/Views/Home/AgregarUsuario.cshtml`.

## Ejecutar

```bash
cd classes/class-04/ProyectoWeb
dotnet build ProyectoWeb.csproj
dotnet run --project ProyectoWeb.csproj
```

Abrir la URL de la consola seguida de `/Home/AgregarUsuario`.

## Formato de la profesora

El modelo usa campos privados, propiedades públicas encapsuladas y constructores
con y sin parámetros. Las propiedades siguen el formato generado por
«Encapsular campos (y usar propiedad)» de Visual Studio:

```csharp
private string nombre;
public string Nombre { get => nombre; set => nombre = value; }
```

`get` devuelve el campo privado; `set` asigna el valor recibido en `value`.
Ambos constructores asignan mediante las propiedades, como en la refactorización de clase.
El constructor vacío inicializa textos con string.Empty, edad con 0 y fecha con DateTime.MinValue.
La vista muestra esa fecha inicial como un campo vacío.

| Nombre en formulario / campo privado | Propiedad pública | Tipo |
| --- | --- | --- |
| nombre | Nombre | string |
| edad | Edad | int |
| fechaNacimiento | FechaNacimiento | DateTime |
| perfil | Perfil | string |
| permisosExtras | PermisosExtras | string |
| otros | Otros | string |
| usuario | NombreUsuario | string |
| clave | Clave | string |

C# prohíbe que una propiedad tenga el mismo nombre que su clase (CS0542).
Por ello se usa NombreUsuario en lugar de Usuario, que aparece marcado como error en la foto.
Su atributo ModelBinder conserva la vinculación con el campo HTML usuario.
Los demás nombres se vinculan sin distinguir mayúsculas y minúsculas.

Las reglas DataAnnotations validan los datos; MVC detecta errores de conversión
y campos vacíos para int y DateTime. La vista conserva el filtro inmediato del nombre.
Service tiene ambos constructores, una referencia private readonly y el método público Validar.
El POST valida el token antifalsificación y los datos y agrega el usuario a la lista estática.
La lista private static es compartida por todas las instancias de Service.
AgregarUsuario(Usuario usuario) valida e inserta; MostrarUsuarios() devuelve una copia de la lista.
Ambos métodos son public para su uso desde el controlador y protegen la lista con lock.
La ruta /Home/MostrarUsuarios muestra la tabla sin incluir claves.
Los usuarios se conservan únicamente en memoria y se pierden al reiniciar la aplicación.
