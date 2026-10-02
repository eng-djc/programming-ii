using System.ComponentModel.DataAnnotations;

namespace ProyectoWeb.Models
{
    /// <summary>
    /// Valida usuarios y administra una lista compartida en memoria para el ejercicio.
    /// </summary>
    public class Service
    {
        // private: únicamente Service modifica la lista.
        // static: todas las instancias comparten la misma lista durante la ejecución.
        private static List<Usuario> usuarios = new List<Usuario>();

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
        /// Devuelve una copia de la lista para mostrar los usuarios.
        /// Los datos permanecen en memoria y se pierden al reiniciar la aplicación.
        /// </summary>
        public List<Usuario> MostrarUsuarios()
        {
            // lock evita leer la lista mientras otra petición agrega un elemento.
            lock (usuarios)
            {
                return usuarios.ToList();
            }
        }

        /// <summary>Valida y agrega el usuario recibido a la lista compartida.</summary>
        /// <param name="usuario">Usuario que se desea agregar.</param>
        /// <exception cref="ArgumentNullException">El usuario es null.</exception>
        /// <exception cref="ValidationException">El modelo incumple sus reglas.</exception>
        public void AgregarUsuario(Usuario usuario)
        {
            ArgumentNullException.ThrowIfNull(usuario);
            // Mantener las reglas también cuando se llama al servicio fuera del controlador.
            Validator.ValidateObject(usuario, new ValidationContext(usuario), validateAllProperties: true);
            lock (usuarios)
            {
                usuarios.Add(usuario);
            }
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
