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

    // Porcentaje adicional según la calidad del producto.
    // La letra m indica que trabajamos con números decimales (dinero).
    [NotMapped]
    public decimal PorcentajeCalidad
    {
        get
        {
            if (Calidad == "baja") return 0.02m;  // 2%
            if (Calidad == "media") return 0.04m; // 4%
            if (Calidad == "alta") return 0.06m;  // 6%

            return 0m;
        }
    }

    // Esta propiedad calcula el total, sin guardar otro valor en la tabla.
    // El IVA se aplica al precio más el recargo por calidad y el envío.
    [NotMapped]
    public decimal MontoPagar
    {
        get
        {
            // 1. Precio del producto.
            decimal precio = Precio ?? 0m;

            // 2. Recargo por calidad: precio multiplicado por 2%, 4% o 6%.
            decimal recargoCalidad = precio * PorcentajeCalidad;

            // 3. Entrega a domicilio: $10 si se solicita, $0 si no.
            decimal envio = EntregaDomicilio == "si" ? 10m : 0m;

            // 4. Sumar el precio, la calidad y el envío.
            decimal subtotal = precio + recargoCalidad + envio;

            // 5. Calcular el 13% de IVA sobre el subtotal.
            decimal iva = subtotal * 0.13m;

            // 6. Sumar el IVA y redondear a dos decimales.
            decimal total = subtotal + iva;
            return decimal.Round(total, 2, MidpointRounding.AwayFromZero);
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