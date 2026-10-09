using System.ComponentModel.DataAnnotations;

namespace Zara.Models
{
    public class Prenda
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

        // Marca obligatoria: texto de hasta 25 caracteres, sin números ni símbolos.
        [Required(ErrorMessage = "La marca es obligatoria.")]
        [MaxLength(25, ErrorMessage = "La marca admite un máximo de 25 caracteres.")]
        [RegularExpression(@"^[A-Za-zÁÉÍÓÚÜÑáéíóúüñ ]+$",
            ErrorMessage = "La marca solo admite letras y espacios.")]
        public string Marca { get => marca; set => marca = value; }

        // Solo se admiten las tallas disponibles en el formulario.
        [Required(ErrorMessage = "Seleccione una talla.")]
        [RegularExpression(@"^(XS|S|M|L|XL|XXL)$",
            ErrorMessage = "Seleccione una talla válida.")]
        public string Talla { get => talla; set => talla = value; }

        // Precio numérico positivo con un límite de valor aceptado.
        [Range(0.01, 999999999.99,
            ErrorMessage = "El precio debe ser mayor que cero.")]
        public double Precio { get => precio; set => precio = value; }

        // Categoría de prenda: debe coincidir con una de las opciones del formulario.
        [Required(ErrorMessage = "Seleccione el género de la prenda.")]
        [RegularExpression(@"^(Hombre|Mujer|Unisex)$",
            ErrorMessage = "Seleccione un género válido.")]
        public string Genero { get => genero; set => genero = value; }
    }
}
