using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cherkessia.Models
{
    public class Instrumento : IValidatableObject
    {
        private int codigo;
        private string nombre = "";
        private DateTime? fechaRegistro;
        private decimal? precio;
        private int garantia;

        public Instrumento() { }

        public Instrumento(int codigo, string nombre, DateTime fechaRegistro, decimal precio, int garantia)
        {
            this.Codigo = codigo;
            this.Nombre = nombre;
            this.FechaRegistro = fechaRegistro;
            this.Precio = precio;
            this.Garantia = garantia;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Range(1, int.MaxValue, ErrorMessage = "Ingrese un código entero positivo.")]
        [Display(Name = "Código")]
        public int Codigo { get => codigo; set => codigo = value; }

        [Required(ErrorMessage = "Ingrese el nombre del instrumento.")]
        [MaxLength(100, ErrorMessage = "El nombre admite hasta 100 caracteres.")]
        public string Nombre { get => nombre; set => nombre = value; }

        [Required(ErrorMessage = "Seleccione la fecha de registro.")]
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        [Display(Name = "Fecha de registro")]
        public DateTime? FechaRegistro { get => fechaRegistro; set => fechaRegistro = value; }

        [Required(ErrorMessage = "Ingrese el precio en dólares.")]
        [Range(typeof(decimal), "0", "9999999999999999.99", ErrorMessage = "El precio debe ser no negativo y caber en decimal(18,2).")]
        [Display(Name = "Precio (USD)")]
        public decimal? Precio { get => precio; set => precio = value; }

        [Range(1, 5, ErrorMessage = "La garantía debe ser de 1 a 5 años.")]
        [Display(Name = "Garantía (años)")]
        public int Garantia { get => garantia; set => garantia = value; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FechaRegistro.HasValue && (FechaRegistro.Value.TimeOfDay != TimeSpan.Zero || FechaRegistro.Value.Date > DateTime.Today))
                yield return new ValidationResult("Use una fecha sin hora que no sea futura.", new[] { nameof(FechaRegistro) });
            if (Precio.HasValue && decimal.Round(Precio.Value, 2) != Precio.Value)
                yield return new ValidationResult("El precio admite como máximo dos decimales.", new[] { nameof(Precio) });
        }

        // Años completos; AddYears contempla el aniversario del 29 de febrero.
        public int calcularAntiguedad(DateTime fechaActual)
        {
            if (!FechaRegistro.HasValue || FechaRegistro.Value.Date > fechaActual.Date)
                throw new ValidationException("La fecha de registro no permite calcular una antigüedad válida.");
            DateTime fecha = FechaRegistro.Value.Date;
            int anios = fechaActual.Year - fecha.Year;
            if (fecha.AddYears(anios) > fechaActual.Date) anios--;
            return anios;
        }

        public bool calcularGarantia(DateTime fechaActual)
        {
            return calcularAntiguedad(fechaActual) <= Garantia;
        }

        public int calcularPuntos(DateTime fechaActual)
        {
            int antiguedad = calcularAntiguedad(fechaActual);
            if (antiguedad <= 2) return 3;
            if (antiguedad < 5) return 2;
            if (antiguedad == 5) return 1;
            return 0;
        }
    }
}
