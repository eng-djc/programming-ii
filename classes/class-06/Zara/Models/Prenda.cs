using System.ComponentModel.DataAnnotations;

namespace Zara.Models
{
    public class Prenda : IValidatableObject
    {
        // Atributos privados
        private int id;
        private string marca = "";
        private string talla = "";
        private double precio;
        private string genero = "";

        // Constructor con parámetros
        public Prenda(int id, string marca, string talla, double precio, string genero)
        {
            this.Id = id;
            this.Marca = marca;
            this.Talla = talla;
            this.Precio = precio;
            this.Genero = genero;
        }

        // Constructor sin parámetros
        public Prenda()
        {
            this.Id = 0;
            this.Marca = "";
            this.Talla = "";
            this.Precio = 0;
            this.Genero = "";
        }

        // Propiedades públicas con Get y Set

        // Clave primaria
        [Key]
        public int Id { get => id; set => id = value; }

        // Máximo 25 caracteres
        [MaxLength(25, ErrorMessage = "La marca no puede superar 25 caracteres.")]
        [RegularExpression(@"^[A-Za-zÁÉÍÓÚÜÑáéíóúüñ ]+$",
            ErrorMessage = "La marca solo puede contener letras y espacios.")]
        public string Marca { get => marca; set => marca = value; }

        // La talla debe coincidir con las opciones del formulario.
        [RegularExpression(@"^(XS|S|M|L|XL|XXL)$",
            ErrorMessage = "Seleccione una talla válida.")]
        public string Talla { get => talla; set => talla = value; }

        // Se limita el precio al rango permitido en el formulario.
        [Range(0.01, 999999999.99,
            ErrorMessage = "El precio debe estar entre 0.01 y 999999999.99.")]
        public double Precio { get => precio; set => precio = value; }

        // El género debe coincidir con las categorías del formulario.
        [RegularExpression(@"^(Hombre|Mujer|Unisex)$",
            ErrorMessage = "Seleccione una categoría válida.")]
        public string Genero { get => genero; set => genero = value; }

        // Valida campos obligatorios sin modificar la nulabilidad
        // de las columnas de la base de datos existente.
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Marca))
            {
                yield return new ValidationResult(
                    "Ingrese la marca.", new[] { nameof(Marca) });
            }

            if (string.IsNullOrWhiteSpace(Talla))
            {
                yield return new ValidationResult(
                    "Seleccione una talla.", new[] { nameof(Talla) });
            }

            if (string.IsNullOrWhiteSpace(Genero))
            {
                yield return new ValidationResult(
                    "Seleccione una categoría.", new[] { nameof(Genero) });
            }
        }
    }
}
