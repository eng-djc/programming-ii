using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zara.Models
{
    public class Fabricante
    {
        // Atributos privados
        private int id;
        private string prestigio = "";
        private DateTime? fechaCreacion;

        // Constructor con parámetros
        public Fabricante(int id, string prestigio, DateTime? fechaCreacion)
        {
            this.Id = id;
            this.Prestigio = prestigio;
            this.FechaCreacion = fechaCreacion;
        }

        // Constructor sin parámetros
        public Fabricante()
        {
            this.Id = 0;
            this.Prestigio = "";
            this.FechaCreacion = null;
        }

        // Propiedades públicas con Get y Set

        // Clave primaria
        [Key]
        public int Id { get => id; set => id = value; }

        // Máximo 5 caracteres
        [Required]
        [MaxLength(5)]
        public string Prestigio { get => prestigio; set => prestigio = value; }

        // Fecha obligatoria, almacenada sin hora
        [Required]
        [Column(TypeName = "date")]
        public DateTime? FechaCreacion { get => fechaCreacion; set => fechaCreacion = value; }
    }
}
