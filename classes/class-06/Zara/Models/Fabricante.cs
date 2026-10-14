using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Zara.Models
{
    /// <summary>Entidad persistida en SQL Server como tabla Fabricantes.</summary>
    public class Fabricante
    {
        private int id;
        private string prestigio = "";
        private DateTime? fechaCreacion;

        // Constructor requerido por Entity Framework y el model binding de MVC.
        public Fabricante() { }

        // Constructor de conveniencia para crear instancias desde código C#.
        public Fabricante(int id, string prestigio, DateTime? fechaCreacion)
        {
            Id = id;
            Prestigio = prestigio;
            FechaCreacion = fechaCreacion;
        }

        // SQL Server genera el ID automáticamente (IDENTITY); no se pide en Create.
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get => id; set => id = value; }

        // Lista cerrada: una sola selección entre Bajo, Medio o Alto.
        [Required(ErrorMessage = "Seleccione el prestigio del fabricante.")]
        [RegularExpression(@"^(Bajo|Medio|Alto)$", ErrorMessage = "Seleccione un prestigio válido.")]
        [MaxLength(5)]
        public string Prestigio { get => prestigio; set => prestigio = value; }

        // DateTime? + Required evita que una fecha vacía pase como 01/01/0001.
        // SQL 'date' guarda únicamente la fecha, sin componente de hora.
        [Required(ErrorMessage = "La fecha de creación es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de creación")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "date")]
        public DateTime? FechaCreacion { get => fechaCreacion; set => fechaCreacion = value; }
    }
}
