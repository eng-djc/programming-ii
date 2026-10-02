# Semana 4 — Usuarios

## Ejecutar

```bash
cd classes/class-04/ProyectoWeb
dotnet build ProyectoWeb.csproj
dotnet run --project ProyectoWeb.csproj
```

- Formulario: /Home/AgregarUsuario.
- Lista: /Home/MostrarUsuarios.

## Modelo

Atributos privados en minúscula y propiedades públicas con inicial mayúscula,
get y set. Usuario tiene constructores con y sin parámetros.
La propiedad User corresponde al campo usuario del formulario.

## Servicio

- Lista privada estática de Usuario.
- agregar(Usuario usuarito): verifica duplicados y agrega el usuario.
- mostrar(): devuelve la lista.
- Constructor sin parámetros y constructor con Usuario que lo agrega.

Los datos permanecen en memoria hasta reiniciar la aplicación.
