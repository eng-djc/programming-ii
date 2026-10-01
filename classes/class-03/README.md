# Class 03

This folder contains the Week 3 project source and the corresponding homework exercise.

## Class material

`ProyectoWeb/` contains the ASP.NET Core MVC source from the provided `Semana3.zip`.

Generated or machine-specific content is intentionally excluded: `.vs/`, `bin/`, `obj/`, and `ProyectoWeb.csproj.user`. The template's third-party `wwwroot/lib/` files are also not duplicated here.

## Homework — Agregar Usuario

File: `homework/agregar-usuario.html`

The form implements only the fields requested in the assignment:

| Field | HTML control |
| --- | --- |
| Nombre | `input type="text"` |
| Edad | `input type="number"` |
| Fecha de nacimiento | `input type="date"` |
| Perfil | Three radio buttons: Administrativo, Plataforma, Docente |
| Permisos extras | `select` with Sí / No |
| Otros | `textarea` |
| Usuario | `input type="text"` |
| Clave | `input type="password"` |

The password is masked by the browser. Perfil is one radio-button group, so only one of the three profiles can be selected. Permisos extras is one select control with the two requested choices.
