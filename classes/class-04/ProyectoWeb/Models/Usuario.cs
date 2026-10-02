using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ProyectoWeb.Models
{
    public class Usuario
    {
        // Atributos privados.
        private string nombre = string.Empty;
        private int edad;
        private DateTime fechaNacimiento;
        private string perfil = string.Empty;
        private string permisosExtras = string.Empty;
        private string otros = string.Empty;
        private string usuario = string.Empty;
        private string clave = string.Empty;

        // Propiedades públicas con get y set.
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [RegularExpression(@"[A-Za-zÁÉÍÓÚáéíóúÑñÜü ]+", ErrorMessage = "El nombre solo puede contener letras y espacios.")]
        public string Nombre { get => nombre; set => nombre = value; }

        [Required(ErrorMessage = "La edad es obligatoria.")]
        [Range(0, int.MaxValue, ErrorMessage = "La edad debe ser un entero no negativo.")]
        public int Edad { get => edad; set => edad = value; }

        [Required(ErrorMessage = "La fecha de nacimiento es obligatoria.")]
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get => fechaNacimiento; set => fechaNacimiento = value; }

        [Required(ErrorMessage = "Seleccione un perfil.")]
        [RegularExpression("administrativo|plataforma|docente", ErrorMessage = "Seleccione un perfil válido.")]
        public string Perfil { get => perfil; set => perfil = value; }

        [Required(ErrorMessage = "Seleccione los permisos extras.")]
        [RegularExpression("si|no", ErrorMessage = "Seleccione una opción válida de permisos extras.")]
        public string PermisosExtras { get => permisosExtras; set => permisosExtras = value; }

        public string Otros { get => otros; set => otros = value; }

        [Required(ErrorMessage = "El usuario es obligatorio.")]
        [RegularExpression("[A-Za-z0-9]+", ErrorMessage = "El usuario solo puede contener letras y números, sin espacios ni caracteres especiales.")]
        // Vincula User con el campo usuario del formulario.
        [ModelBinder(Name = "usuario")]
        public string User { get => usuario; set => usuario = value; }

        [Required(ErrorMessage = "La clave es obligatoria.")]
        [DataType(DataType.Password)]
        public string Clave { get => clave; set => clave = value; }

        // Constructor sin parámetros: inicializa los atributos.
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

        // Constructor con parámetros: recibe los datos del usuario.
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
