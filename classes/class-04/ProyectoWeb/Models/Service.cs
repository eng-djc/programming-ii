using System.ComponentModel.DataAnnotations;

namespace ProyectoWeb.Models
{
    /// <summary>
    /// Servicio del ejercicio: valida un Usuario usando las reglas declaradas en el modelo.
    /// No guarda usuarios ni claves y no representa una base de datos.
    /// </summary>
    public class Service
    {
        // private impide acceder directamente al estado interno.
        // readonly impide reemplazar esta referencia después de construir el servicio.
        private readonly Usuario _usuario;

        /// <summary>Crea el servicio con un usuario vacío reutilizando el otro constructor.</summary>
        public Service() : this(new Usuario())
        {
        }

        /// <summary>Crea el servicio para validar el usuario recibido.</summary>
        /// <param name="usuario">Modelo que se validará.</param>
        /// <exception cref="ArgumentNullException">El usuario es null.</exception>
        public Service(Usuario usuario)
        {
            ArgumentNullException.ThrowIfNull(usuario);
            _usuario = usuario;
        }

        /// <summary>
        /// Devuelve los errores de validación; una lista vacía indica datos válidos.
        /// Es public porque el controlador necesita llamar a esta operación.
        /// </summary>
        public IReadOnlyList<ValidationResult> Validar()
        {
            var errores = new List<ValidationResult>();
            var contexto = new ValidationContext(_usuario);
            Validator.TryValidateObject(_usuario, contexto, errores, validateAllProperties: true);
            return errores;
        }
    }
}
