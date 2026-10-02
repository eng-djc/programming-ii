using System.ComponentModel.DataAnnotations;

namespace ProyectoWeb.Models
{
    /// <summary>
    /// Datos del formulario Agregar Usuario.
    /// Los nombres conservan exactamente los de la vista, incluido su uso de minúsculas.
    /// </summary>
    public class Usuario
    {
        /// <summary>Nombre completo: admite letras, tildes, ñ, ü y espacios.</summary>
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [RegularExpression(@"[A-Za-zÁÉÍÓÚáéíóúÑñÜü ]+", ErrorMessage = "El nombre solo puede contener letras y espacios.")]
        public string nombre { get; set; } = string.Empty;

        /// <summary>Edad entera no negativa. Null permite detectar un campo vacío.</summary>
        [Required(ErrorMessage = "La edad es obligatoria.")]
        [Range(0, int.MaxValue, ErrorMessage = "La edad debe ser un entero no negativo.")]
        public int? edad { get; set; }

        /// <summary>Fecha seleccionada; null representa un campo sin completar.</summary>
        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime? fechaNacimiento { get; set; }

        /// <summary>Perfil seleccionado: administrativo, plataforma o docente.</summary>
        [Required(ErrorMessage = "Seleccione un perfil.")]
        [RegularExpression("administrativo|plataforma|docente", ErrorMessage = "Seleccione un perfil válido.")]
        public string perfil { get; set; } = string.Empty;

        /// <summary>Opción del formulario: "si" o "no"; se conserva como texto.</summary>
        [Required(ErrorMessage = "Seleccione los permisos extras.")]
        [RegularExpression("si|no", ErrorMessage = "Seleccione una opción válida de permisos extras.")]
        public string permisosExtras { get; set; } = string.Empty;

        /// <summary>Observaciones opcionales.</summary>
        public string? otros { get; set; }

        /// <summary>Nombre de usuario indicado en el formulario.</summary>
        [Required(ErrorMessage = "El usuario es obligatorio.")]
        public string usuario { get; set; } = string.Empty;

        /// <summary>Clave recibida para este ejercicio; no se almacena ni se muestra.</summary>
        [Required(ErrorMessage = "La clave es obligatoria.")]
        [DataType(DataType.Password)]
        public string clave { get; set; } = string.Empty;

        /// <summary>
        /// Crea un modelo vacío. MVC usa este constructor al vincular los campos del formulario.
        /// Las propiedades públicas con get y set permiten esa vinculación y su lectura en la vista.
        /// </summary>
        public Usuario()
        {
        }

        /// <summary>Crea un modelo con los ocho valores del formulario.</summary>
        /// <param name="nombre">Nombre completo.</param>
        /// <param name="edad">Edad entera no negativa.</param>
        /// <param name="fechaNacimiento">Fecha de nacimiento.</param>
        /// <param name="perfil">Perfil seleccionado.</param>
        /// <param name="permisosExtras">Opción si/no.</param>
        /// <param name="otros">Observaciones opcionales.</param>
        /// <param name="usuario">Nombre de usuario.</param>
        /// <param name="clave">Clave introducida.</param>
        public Usuario(string nombre, int? edad, DateTime? fechaNacimiento,
            string perfil, string permisosExtras, string? otros, string usuario, string clave)
        {
            this.nombre = nombre;
            this.edad = edad;
            this.fechaNacimiento = fechaNacimiento;
            this.perfil = perfil;
            this.permisosExtras = permisosExtras;
            this.otros = otros;
            this.usuario = usuario;
            this.clave = clave;
        }
    }
}
