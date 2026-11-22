using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Zara.Models
{
    public class Funcionario : IValidatableObject
    {
        public static IReadOnlyList<string> Perfiles { get; } =
            Array.AsReadOnly(new[] { "Jefatura", "Profesional", "Técnico" });

        // Atributos privados
        private int id;
        private string nombre;
        private long? cedula;
        private string telefono;
        private DateTime? fechaNacimiento;
        private bool? activo;
        private string perfil;
        private string usuario;
        private string? clave;
        private string claveHash;

        // Constructor con parámetros
        public Funcionario(int id, string nombre, long cedula, string telefono, DateTime fechaNacimiento,
            bool activo, string perfil, string usuario, string clave)
        {
            this.Id = id;
            this.Nombre = nombre;
            this.Cedula = cedula;
            this.Telefono = telefono;
            this.FechaNacimiento = fechaNacimiento;
            this.Activo = activo;
            this.Perfil = perfil;
            this.Usuario = usuario;
            this.Clave = clave;
            this.ClaveHash = "";
        }

        // Constructor sin parámetros
        public Funcionario()
        {
            this.Id = 0;
            this.Nombre = "";
            this.Cedula = null;
            this.Telefono = "";
            this.FechaNacimiento = null;
            this.Activo = null;
            this.Perfil = "";
            this.Usuario = "";
            this.Clave = null;
            this.ClaveHash = "";
        }

        // Propiedades públicas
        [Key]
        public int Id { get => id; set => id = value; }

        [Required(ErrorMessage = "Ingrese el nombre.")]
        [MaxLength(100)]
        [RegularExpression(@"^[A-Za-zÁÉÍÓÚÜÑáéíóúüñ ]+$",
            ErrorMessage = "El nombre solo puede contener letras y espacios.")]
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
        public DateTime? FechaNacimiento
        {
            get => fechaNacimiento;
            set => fechaNacimiento = value;
        }

        [Required(ErrorMessage = "Seleccione el estado.")]
        [Display(Name = "Estado")]
        public bool? Activo { get => activo; set => activo = value; }

        [Required(ErrorMessage = "Seleccione el perfil.")]
        [MaxLength(20)]
        public string Perfil { get => perfil; set => perfil = value; }

        [Required(ErrorMessage = "Ingrese el usuario.")]
        [MaxLength(100)]
        [RegularExpression(@"^[A-Za-zÁÉÍÓÚÜÑáéíóúüñ]+$",
            ErrorMessage = "El usuario solo puede contener letras, sin espacios ni números.")]
        public string Usuario { get => usuario; set => usuario = value; }

        [NotMapped]
        [DataType(DataType.Password)]
        public string? Clave { get => clave; set => clave = value; }

        [BindNever]
        [ValidateNever]
        public string ClaveHash { get => claveHash; set => claveHash = value; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!Perfiles.Contains(Perfil))
            {
                yield return new ValidationResult(
                    "Seleccione un perfil válido.",
                    new[] { nameof(Perfil) });
            }
        }
    }
}
