using System.ComponentModel.DataAnnotations;

namespace Cherkessia.Models
{
    public class Volcan : IValidatableObject
    {
        private string nombre = "";
        private string provincia = "";
        private bool? activo;
        private string gases = "";
        private int instrumentoCodigo;
        private int irv;

        public static IReadOnlyList<string> Provincias { get; } = Array.AsReadOnly(new[]
            { "San José", "Alajuela", "Cartago", "Heredia", "Guanacaste", "Puntarenas", "Limón" });
        public static IReadOnlyList<string> OpcionesGases { get; } = Array.AsReadOnly(new[]
            { "Poco probable", "Probable", "Muy probable" });

        public Volcan() { }

        public Volcan(string nombre, string provincia, bool activo, string gases, int instrumentoCodigo)
        {
            this.Nombre = nombre;
            this.Provincia = provincia;
            this.Activo = activo;
            this.Gases = gases;
            this.InstrumentoCodigo = instrumentoCodigo;
        }

        [Key]
        [Required(ErrorMessage = "Ingrese el nombre del volcán.")]
        [MaxLength(100, ErrorMessage = "El nombre admite hasta 100 caracteres.")]
        [Display(Name = "Nombre del volcán")]
        public string Nombre { get => nombre; set => nombre = value; }

        [Required(ErrorMessage = "Seleccione una provincia.")]
        [MaxLength(10)]
        public string Provincia { get => provincia; set => provincia = value; }

        [Required(ErrorMessage = "Indique si el volcán está activo.")]
        public bool? Activo { get => activo; set => activo = value; }

        [Required(ErrorMessage = "Seleccione la probabilidad de expulsión de gases.")]
        [MaxLength(13)]
        [Display(Name = "Expulsa gases")]
        public string Gases { get => gases; set => gases = value; }

        [Range(1, int.MaxValue, ErrorMessage = "Seleccione un instrumento.")]
        [Display(Name = "Instrumento utilizado")]
        public int InstrumentoCodigo { get => instrumentoCodigo; set => instrumentoCodigo = value; }

        public Instrumento? Instrumento { get; set; }

        // No se recibe desde los formularios; se calcula antes de SaveChanges.
        public int IRV { get => irv; private set => irv = value; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!Provincias.Contains(Provincia))
                yield return new ValidationResult("Seleccione una de las siete provincias de Costa Rica.", new[] { nameof(Provincia) });
            if (!OpcionesGases.Contains(Gases))
                yield return new ValidationResult("Seleccione una opción de gases válida.", new[] { nameof(Gases) });
            if (!string.IsNullOrWhiteSpace(Nombre) && Nombre != Nombre.Trim())
                yield return new ValidationResult("El nombre no debe comenzar ni terminar con espacios.", new[] { nameof(Nombre) });
        }

        public int calcularIRV(Instrumento instrumento, DateTime fechaActual)
        {
            if (!Activo.HasValue) throw new ValidationException("Indique si el volcán está activo.");
            int puntosGases = Gases switch
            {
                "Poco probable" => 1,
                "Probable" => 2,
                "Muy probable" => 3,
                _ => throw new ValidationException("Seleccione una opción de gases válida.")
            };
            IRV = (Activo.Value ? 2 : 0) + puntosGases + instrumento.calcularPuntos(fechaActual);
            return IRV;
        }

        public string clasificarRiesgo()
        {
            return IRV switch
            {
                >= 1 and <= 2 => "Riesgo Muy Bajo",
                <= 4 and >= 3 => "Riesgo Bajo",
                <= 6 and >= 5 => "Riesgo Medio",
                <= 8 and >= 7 => "Riesgo Alto",
                _ => throw new ValidationException("El IRV no ha sido calculado correctamente.")
            };
        }
    }
}
