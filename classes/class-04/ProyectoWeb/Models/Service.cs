namespace ProyectoWeb.Models
{
    /// <summary>
    /// Administra usuarios en memoria siguiendo el ejemplo de la profesora.
    /// La lista y las operaciones son estáticas y compartidas por toda la aplicación.
    /// </summary>
    public class Service
    {
        // private: el acceso directo a la lista queda dentro de Service.
        // static: la lista es compartida; no se crea una lista por cada objeto Service.
        private static List<Usuario> usuarios = new List<Usuario>();

        /// <summary>Crea un servicio sin agregar usuarios a la lista.</summary>
        public Service()
        {
        }

        /// <summary>Crea el servicio y agrega el usuario recibido usando la misma operación.</summary>
        /// <param name="usuarito">Usuario inicial que se agregará a la lista compartida.</param>
        public Service(Usuario usuarito)
        {
            agregar(usuarito);
        }

        /// <summary>
        /// Agrega un usuario si su User todavía no existe.
        /// public permite llamar desde el controlador; static evita crear un Service.
        /// </summary>
        /// <param name="usuarito">Usuario que se desea registrar.</param>
        /// <exception cref="Exception">Ya existe un usuario con el mismo User.</exception>
        public static void agregar(Usuario usuarito) {
            // Recorrer los usuarios existentes antes de insertar el nuevo.
            foreach (Usuario aux in usuarios)
                if (aux.User == usuarito.User)
                    throw new Exception("Este usuario ya esta registrado");
            usuarios.Add(usuarito);
        }

        /// <summary>
        /// Devuelve la lista, como en el ejemplo de clase.
        /// Los usuarios permanecen solo en memoria hasta que se reinicia la aplicación.
        /// </summary>
        public static List<Usuario> GetAll() {
            return usuarios;
        }
    }
}
