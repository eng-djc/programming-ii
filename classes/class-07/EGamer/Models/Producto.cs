using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EGamer.Models;

/// <summary>Clase lógica: propiedades, validación y todos los cálculos monetarios.</summary>
[Table("Producto")]
public class Producto : IValidatableObject
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required(ErrorMessage = "Seleccione fecha de factura."), DataType(DataType.Date), Column(TypeName = "date"), Display(Name = "Fecha de factura")]
    public DateTime? FechaFactura { get; set; }

    [Required(ErrorMessage = "Ingrese nombre."), StringLength(100), Display(Name = "Nombre del producto")]
    public string Nombre { get; set; } = "";

    [Required(ErrorMessage = "Seleccione proveedor."), RegularExpression(@"^(p1|p2|p3|p4|p5)$", ErrorMessage = "Proveedor inválido.")]
    public string Proveedor { get; set; } = "";

    [Required(ErrorMessage = "Ingrese precio."), Range(typeof(decimal), "0", "999999999999", ErrorMessage = "El precio debe ser no negativo.")]
    public decimal? Precio { get; set; }

    [Required(ErrorMessage = "Seleccione calidad."), RegularExpression(@"^(baja|media|alta)$", ErrorMessage = "Calidad inválida.")]
    public string Calidad { get; set; } = "";

    [Required(ErrorMessage = "Ingrese descripción."), StringLength(2000), Display(Name = "Descripción de objeto")]
    public string Descripcion { get; set; } = "";

    [Required(ErrorMessage = "Seleccione entrega."), RegularExpression(@"^(si|no)$", ErrorMessage = "Seleccione si o no."), Display(Name = "Entrega a domicilio")]
    public string EntregaDomicilio { get; set; } = "";

    [NotMapped]
    public decimal PorcentajeCalidad => Calidad switch { "baja" => 0.02m, "media" => 0.04m, "alta" => 0.06m, _ => 0m };

    // El recargo y el envío forman parte de la base gravable: supuesto documentado en README.
    [NotMapped]
    public decimal MontoPagar
    {
        get
        {
            var subtotal = (Precio ?? 0m) * (1m + PorcentajeCalidad) + (EntregaDomicilio == "si" ? 10m : 0m);
            return decimal.Round(subtotal * 1.13m, 2, MidpointRounding.AwayFromZero);
        }
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext context)
    {
        if (string.IsNullOrWhiteSpace(Nombre))
            yield return new ValidationResult("El nombre es obligatorio.", new[] { nameof(Nombre) });
        if (string.IsNullOrWhiteSpace(Descripcion))
            yield return new ValidationResult("La descripción es obligatoria.", new[] { nameof(Descripcion) });
        if (FechaFactura.HasValue && FechaFactura.Value.TimeOfDay != TimeSpan.Zero)
            yield return new ValidationResult("La fecha no debe incluir hora.", new[] { nameof(FechaFactura) });
    }
}