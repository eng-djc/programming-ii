# Guía breve para la exposición — ParanormalAgency

**Objetivo:** explicar la aplicación recorriendo la pantalla y el código, sin leer requisitos, siglas ni diagramas. Duración sugerida: **8 minutos de código, 2 minutos de navegador y 10 minutos de preguntas**, según la rúbrica.

## Recorrido por el código — 8 minutos

| Tiempo | Abrir en Visual Studio | Qué mostrar | Qué decir |
|---|---|---|---|
| 0:00–1:30 | `Models/Investigation.cs` | Atributos privados, propiedades y dos constructores | «Esta clase representa una investigación. Guarda los datos del caso y define sus validaciones. Tiene un constructor vacío para la base de datos y otro para inicializar un caso.» |
| 1:30–3:00 | `Models/Investigation.cs` → `CalculateTotalCost()` | Las opciones de fenómeno, riesgo y suma final | «Primero identifico el porcentaje del fenómeno, luego el monto fijo del riesgo. Si requiere equipo, agrego un 20% del presupuesto base. Sumo todo y redondeo a dos decimales.» |
| 3:00–4:30 | `Controllers/InvestigationController.cs` | `Index`, `Create`, `Edit`, `Delete` | «El controlador recibe las solicitudes, comprueba que los datos sean válidos, llama al servicio y decide qué pantalla mostrar.» |
| 4:30–5:45 | `Services/Service.cs` | `DbContext`, `Investigations`, métodos CRUD | «El servicio es el encargado de consultar y guardar en SQL Server LocalDB. El controlador no contiene instrucciones SQL.» |
| 5:45–7:00 | `Views/Investigation/` | `Index.cshtml`, `Create.cshtml`, `Edit.cshtml`, `Delete.cshtml`, `_Form.cshtml` | «Las vistas son las pantallas. Create y Edit reutilizan los mismos campos; Index muestra los casos y Delete confirma antes de eliminar.» |
| 7:00–8:00 | `Views/Shared/_Layout.cshtml` y `Program.cs` | Menú, ruta inicial, estructura MVC | «Conservé la estructura de Class 08 y eliminé Home/Privacy porque el examen lo pide. Toda la interfaz propia está en español y no agregué JavaScript.» |

## Demostración en navegador — 2 minutos

1. **Lista de investigaciones:** mostrar los registros, costo y botones **Actualizar** y **Eliminar**.
2. **Agregar investigación:** registrar un caso breve con los siguientes valores:

   | Campo | Valor |
   |---|---|
   | Nombre del caso | Casa embrujada 2 - Cartago |
   | Nombre del cliente | María Solórzano |
   | Fenómeno | Poltergeist |
   | Riesgo | Alto |
   | Presupuesto | 1000 |
   | Equipo especializado | Sí |
   | Fecha de investigación | 15/11/2026 |

3. **Guardar:** señalar el costo total esperado: **USD 3,450.00**.
4. **Actualizar:** cambiar un dato y guardar para mostrar el recálculo.
5. **Eliminar:** abrir la confirmación; confirmar solo si hay tiempo y se desea cerrar el recorrido.

## Cálculo del costo — explicación de 20 segundos

**Costo final = base + fenómeno + riesgo + equipo.**

- Base: USD 1,000.
- Poltergeist: 25% de la base = USD 250.
- Riesgo Alto: monto fijo = USD 2,000.
- Equipo especializado: 20% de la base = USD 200.
- **Total: USD 3,450.**

**Importante:** el porcentaje del fenómeno y el del equipo siempre se aplican a la base, no al total acumulado. La fórmula vive en `Investigation.CalculateTotalCost()`, no en la vista ni en el controlador. En C#, `0.25m` representa el 25% como decimal; se usa `decimal` para montos monetarios.

### Valores oficiales para responder preguntas

| Fenómeno | Porcentaje | Riesgo | Adicional fijo |
|---|---:|---|---:|
| Aparición | 15% | Bajo | USD 500 |
| Poltergeist | 25% | Medio | USD 1,000 |
| Actividad demoníaca | 45% | Alto | USD 2,000 |
| Objeto maldito | 30% | Crítico | USD 4,000 |

El equipo agrega 20% de la base si se selecciona **Sí**; con **No**, agrega cero.

## Puntos críticos para defender — preguntas y respuestas breves

| Pregunta probable | Respuesta sencilla | Archivo |
|---|---|---|
| ¿Qué hace el modelo? | Guarda los datos, valida las reglas y calcula el costo. | `Models/Investigation.cs` |
| ¿Para qué sirven los constructores? | Uno permite crear el objeto vacío y el otro inicializarlo con sus datos. | `Models/Investigation.cs` |
| ¿Por qué el cálculo está en el modelo? | Es una regla del caso y el examen exige calcularla en la clase lógica. | `CalculateTotalCost()` |
| ¿Qué hace el controlador? | Recibe la solicitud, valida y coordina la vista con el servicio. | `InvestigationController.cs` |
| ¿Qué hace el servicio? | Consulta, agrega, actualiza y elimina registros en la base de datos. | `Services/Service.cs` |
| ¿Cómo se guardan los datos? | Entity Framework 6 usa `DbContext` y `SaveChanges()` con LocalDB. | `Services/Service.cs` |
| ¿Cómo se evita guardar un presupuesto negativo? | Se valida en el HTML y también en el servidor. | `_Form.cshtml`, `Investigation.cs` |
| ¿Por qué hay validación doble? | La del navegador ayuda al usuario; la del servidor protege los datos aunque se omita el HTML. | `Investigation.cs` |
| ¿Cómo se evitan nombres incorrectos? | El caso admite hasta 50 caracteres y el cliente hasta 80; el cliente solo permite letras y espacios. | `Investigation.cs`, `_Form.cshtml` |
| ¿Por qué la fecha no tiene hora? | El examen solicita fecha de investigación y expresamente indica que no debe mostrar hora. | `InvestigationDate`, vistas |
| ¿Por qué no hay Home ni Privacy? | Es una instrucción expresa del examen; se conserva el resto del formato de Class 08. | `Program.cs`, `_Layout.cshtml` |
| ¿Se usa JavaScript? | No se agregó JavaScript a las vistas ni a la lógica del proyecto. | Vistas Razor |

## Mapa mental de la arquitectura

```text
Pantalla (Views)
      ↓
Controlador (InvestigationController)
      ↓
Servicio (Service / EF6)
      ↓
Base de datos (SQL Server LocalDB)

Modelo (Investigation):
  datos + validaciones + cálculo del costo
```

**Frase de cierre:** «La aplicación registra investigaciones paranormales, valida sus datos y calcula automáticamente el costo. Separé las pantallas, el controlador, la lógica y la base de datos siguiendo el MVC de Class 08.»

**Preparación:** abrir la solución en Visual Studio, ejecutar la aplicación con LocalDB y verificar un registro antes de compartir pantalla.
