using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Cherkessia.Models
{
    public class Funcionario : IValidatableObject
    {
        private int id;
        private string nombre = "";
        private long? cedula;
        private string telefono = "";
        private DateTime? fechaNacimiento;
        private bool? activo;
        private string perfil = "";
        private string usuario = "";

        public static IReadOnlyList<string> Perfiles { get; } =
            Array.AsReadOnly(new[] { "Jefatura", "Profesional", "Técnico" });

        public Funcionario() { }

        public Funcionario(string nombre, long cedula, string telefono, DateTime fechaNacimiento,
            bool activo, string perfil, string usuario, string clave)
        {
            Nombre = nombre;
            Cedula = cedula;
            Telefono = telefono;
            FechaNacimiento = fechaNacimiento;
            Activo = activo;
            Perfil = perfil;
            Usuario = usuario;
            Clave = clave;
        }

        [Key]
        public int Id { get => id; set => id = value; }

        [Required(ErrorMessage = "Ingrese el nombre.")]
        [MaxLength(100)]
        public string Nombre { get => nombre; set => nombre = value; }

        [Required(ErrorMessage = "Ingrese la cédula.")]
        [Range(typeof(long), "1", "9223372036854775807", ErrorMessage = "Ingrese una cédula válida.")]
        [Display(Name = "Cédula")]
        public long? Cedula { get => cedula; set => cedula = value; }

        [Required(ErrorMessage = "Ingrese el teléfono.")]
        [MaxLength(30)]
        [Display(Name = "Teléfono")]
        public string Telefono { get => telefono; set => telefono = value; }

        [Required(ErrorMessage = "Seleccione la fecha de nacimiento.")]
        [DataType(DataType.Date)]
        [Column(TypeName = "date")]
        [Display(Name = "Fecha de nacimiento")]
        public DateTime? FechaNacimiento { get => fechaNacimiento; set => fechaNacimiento = value; }

        [Required(ErrorMessage = "Seleccione el estado.")]
        [Display(Name = "Estado")]
        public bool? Activo { get => activo; set => activo = value; }

        [Required(ErrorMessage = "Seleccione el perfil.")]
        [MaxLength(20)]
        public string Perfil { get => perfil; set => perfil = value; }

        [Required(ErrorMessage = "Ingrese el usuario.")]
        [MaxLength(100)]
        public string Usuario { get => usuario; set => usuario = value; }

        [NotMapped]
        [DataType(DataType.Password)]
        public string? Clave { get; set; }

        [BindNever]
        [ValidateNever]
        public string ClaveHash { get; set; } = "";

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!Perfiles.Contains(Perfil))
                yield return new ValidationResult("Seleccione un perfil válido.", new[] { nameof(Perfil) });
        }
    }
}
