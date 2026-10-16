using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zara.Models
{
    public class Fabricante : IValidatableObject
    {
        // Atributos privados
        private int id;
        private string nombre = "";
        private string prestigio = "";
        private DateTime? fechaCreacion;

        // Constructor con parámetros
        public Fabricante(int id, string prestigio, DateTime? fechaCreacion, string nombre = "")
        {
            this.Id = id;
            this.Nombre = nombre;
            this.Prestigio = prestigio;
            this.FechaCreacion = fechaCreacion;
        }

        // Constructor sin parámetros
        public Fabricante()
        {
            this.Id = 0;
            this.Nombre = "";
            this.Prestigio = "";
            this.FechaCreacion = null;
        }

        // Propiedades públicas con Get y Set

        // Clave primaria
        [Key]
        public int Id { get => id; set => id = value; }

        // Columna opcional en SQL para conservar fabricantes ya registrados.
        // Se exige un nombre al crear o editar mediante IValidatableObject.
        [MaxLength(100, ErrorMessage = "El nombre no puede superar 100 caracteres.")]
        public string Nombre { get => nombre; set => nombre = value; }

        // Estas reglas se ejecutan en el servidor aunque se manipule el select HTML.
        // Required rechaza null, vacío y espacios; la expresión permite solo las tres opciones.
        [Required(ErrorMessage = "Seleccione un prestigio.")]
        [MaxLength(5, ErrorMessage = "El prestigio no puede superar cinco caracteres.")]
        [RegularExpression(@"^(Bajo|Medio|Alto)$",
            ErrorMessage = "El prestigio debe ser Bajo, Medio o Alto.")]
        public string Prestigio { get => prestigio; set => prestigio = value; }

        // Fecha obligatoria, almacenada sin hora
        [Required(ErrorMessage = "Seleccione una fecha de creación.")]
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        public DateTime? FechaCreacion { get => fechaCreacion; set => fechaCreacion = value; }

        // Column("date") define el almacenamiento, pero no valida una petición HTTP.
        // Esta regla evita descartar silenciosamente una hora enviada fuera del formulario.
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Nombre))
            {
                yield return new ValidationResult(
                    "Ingrese el nombre del fabricante.",
                    new[] { nameof(Nombre) });
            }

            if (FechaCreacion.HasValue && FechaCreacion.Value.TimeOfDay != TimeSpan.Zero)
            {
                yield return new ValidationResult(
                    "La fecha de creación no debe incluir una hora.",
                    new[] { nameof(FechaCreacion) });
            }
        }
    }
}
