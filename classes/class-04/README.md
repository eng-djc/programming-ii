# Semana 4 — Usuarios

Proyecto copiado de semana 3 y adaptado al formato de la profesora.

## Ejecutar

```bash
cd classes/class-04/ProyectoWeb
dotnet build ProyectoWeb.csproj
dotnet run --project ProyectoWeb.csproj
```

Abrir la dirección de la consola seguida de /Home/AgregarUsuario.
La lista se muestra en /Home/MostrarUsuarios.

## Modelo y propiedades

Los campos privados usan inicial minúscula y las propiedades públicas inicial mayúscula.
Cada propiedad encapsula su campo mediante get y set:

```csharp
private string usuario = string.Empty;
public string User { get => usuario; set => usuario = value; }
```

| Campo privado y nombre del formulario | Propiedad | Tipo |
| --- | --- | --- |
| nombre | Nombre | string |
| edad | Edad | int |
| fechaNacimiento | FechaNacimiento | DateTime |
| perfil | Perfil | string |
| permisosExtras | PermisosExtras | string |
| otros | Otros | string |
| usuario | User | string |
| clave | Clave | string |

User coincide con el ejemplo de Service de la profesora.
ModelBinder conserva la vinculación de User con el campo HTML usuario.
Los textos se inicializan con string.Empty para evitar advertencias CS8618.
Usuario conserva los constructores sin parámetros y con los ocho parámetros.

## Service

La declaración coincide con la foto de clase:

```csharp
private static List<Usuario> usuarios = new List<Usuario>();
```

- agregar(Usuario usuarito) es public static void. Recorre la lista con foreach,
  compara aux.User con usuarito.User y lanza una Exception si ya está registrado.
  Si no está repetido, usa usuarios.Add(usuarito).
- GetAll() es public static List<Usuario> y devuelve directamente usuarios.
- El constructor vacío no agrega datos; el constructor con Usuario llama a agregar.

El controlador utiliza Service.agregar(modelo) y Service.GetAll().
MVC valida los datos del formulario y el token antifalsificación.
Si agregar lanza una excepción, el controlador muestra su mensaje en el formulario.
La tabla muestra los usuarios sin incluir las claves.

La lista estática existe únicamente en memoria y se pierde al reiniciar la aplicación.
