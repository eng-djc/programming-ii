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

        // Máximo 25 caracteres
        [Required(ErrorMessage = "Ingrese la marca.")]
        [MaxLength(25, ErrorMessage = "La marca no puede superar 25 caracteres.")]
        [RegularExpression(@"^[A-Za-zÁÉÍÓÚÜÑáéíóúüñ ]+$",
            ErrorMessage = "La marca solo puede contener letras y espacios.")]
        public string Marca { get => marca; set => marca = value; }

        // La talla debe coincidir con las opciones del formulario.
        [Required(ErrorMessage = "Seleccione una talla.")]
        [RegularExpression(@"^(XS|S|M|L|XL|XXL)$",
            ErrorMessage = "Seleccione una talla válida.")]
        public string Talla { get => talla; set => talla = value; }

        // Se limita el precio al rango permitido en el formulario.
        [Range(0.01, 999999999.99,
            ErrorMessage = "El precio debe estar entre 0.01 y 999999999.99.")]
        public double Precio { get => precio; set => precio = value; }

        // El género debe coincidir con las categorías del formulario.
        [Required(ErrorMessage = "Seleccione una categoría.")]
        [RegularExpression(@"^(Hombre|Mujer|Unisex)$",
            ErrorMessage = "Seleccione una categoría válida.")]
        public string Genero { get => genero; set => genero = value; }
    }
}
