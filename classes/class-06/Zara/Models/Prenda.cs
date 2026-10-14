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
        [MaxLength(25)]
        public string Marca { get => marca; set => marca = value; }

        public string Talla { get => talla; set => talla = value; }

        public double Precio { get => precio; set => precio = value; }

        public string Genero { get => genero; set => genero = value; }
    }
}
