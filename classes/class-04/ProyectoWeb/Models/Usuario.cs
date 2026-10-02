using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ProyectoWeb.Models
{
    /// <summary>
    /// Datos del formulario Agregar Usuario.
    /// Campos privados con los nombres del formulario y propiedades públicas encapsuladas.
    /// </summary>
    public class Usuario
    {
        // Estado interno: private permite acceso únicamente desde esta clase.
        // Los textos parten de string.Empty para garantizar su inicialización (CS8618).
        // Se conservan las asignaciones mediante propiedades en ambos constructores.
        private string nombre = string.Empty;
        private int edad;
        private DateTime fechaNacimiento;
        private string perfil = string.Empty;
        private string permisosExtras = string.Empty;
        private string otros = string.Empty;
        private string usuario = string.Empty;
        private string clave = string.Empty;

        /// <summary>Nombre completo: admite letras, tildes, ñ, ü y espacios.</summary>
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [RegularExpression(@"[A-Za-zÁÉÍÓÚáéíóúÑñÜü ]+", ErrorMessage = "El nombre solo puede contener letras y espacios.")]
        public string Nombre { get => nombre; set => nombre = value; }

        /// <summary>Edad entera no negativa; MVC detecta valores vacíos o no numéricos.</summary>
        [Required(ErrorMessage = "La edad es obligatoria.")]
        [Range(0, int.MaxValue, ErrorMessage = "La edad debe ser un entero no negativo.")]
        public int Edad { get => edad; set => edad = value; }

        /// <summary>Fecha de nacimiento seleccionada; MVC detecta valores vacíos o inválidos.</summary>
        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get => fechaNacimiento; set => fechaNacimiento = value; }

        /// <summary>Perfil seleccionado: administrativo, plataforma o docente.</summary>
        [Required(ErrorMessage = "Seleccione un perfil.")]
        [RegularExpression("administrativo|plataforma|docente", ErrorMessage = "Seleccione un perfil válido.")]
        public string Perfil { get => perfil; set => perfil = value; }

        /// <summary>Opción del formulario: "si" o "no"; se conserva como texto.</summary>
        [Required(ErrorMessage = "Seleccione los permisos extras.")]
        [RegularExpression("si|no", ErrorMessage = "Seleccione una opción válida de permisos extras.")]
        public string PermisosExtras { get => permisosExtras; set => permisosExtras = value; }

        /// <summary>Observaciones opcionales.</summary>
        public string Otros { get => otros; set => otros = value; }

        /// <summary>Nombre de usuario indicado en el formulario.</summary>
        [Required(ErrorMessage = "El usuario es obligatorio.")]
        // User sigue el nombre usado por la profesora en Service.
        // El alias conserva name="usuario" para la vinculación del formulario.
        [ModelBinder(Name = "usuario")]
        public string User { get => usuario; set => usuario = value; }

        /// <summary>Clave recibida para este ejercicio; no se almacena ni se muestra.</summary>
        [Required(ErrorMessage = "La clave es obligatoria.")]
        [DataType(DataType.Password)]
        public string Clave { get => clave; set => clave = value; }

        /// <summary>
        /// Crea un modelo vacío. MVC usa este constructor al vincular los campos del formulario.
        /// Los getters leen los campos privados y los setters reciben value para actualizarlos.
        /// </summary>
        public Usuario()
        {
            this.Nombre = string.Empty;
            this.Edad = 0;
            this.FechaNacimiento = DateTime.MinValue;
            this.Perfil = string.Empty;
            this.PermisosExtras = string.Empty;
            this.Otros = string.Empty;
            this.User = string.Empty;
            this.Clave = string.Empty;
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
        public Usuario(string nombre, int edad, DateTime fechaNacimiento,
            string perfil, string permisosExtras, string otros, string usuario, string clave)
        {
            this.Nombre = nombre;
            this.Edad = edad;
            this.FechaNacimiento = fechaNacimiento;
            this.Perfil = perfil;
            this.PermisosExtras = permisosExtras;
            this.Otros = otros;
            this.User = usuario;
            this.Clave = clave;
        }
    }
}
