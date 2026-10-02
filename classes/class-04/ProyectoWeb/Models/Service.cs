namespace ProyectoWeb.Models
{
    public class Service
    {
        // Lista compartida por toda la aplicación.
        private static List<Usuario> usuarios = new List<Usuario>();

        // Constructor sin parámetros.
        public Service()
        {
        }

        // Constructor con parámetros: agrega el usuario recibido.
        public Service(Usuario usuarito)
        {
            agregar(usuarito);
        }

        // Agrega un usuario y verifica que no esté registrado.
        public static void agregar(Usuario usuarito) {
            foreach (Usuario aux in usuarios)
                if (aux.User == usuarito.User)
                    throw new Exception("Este usuario ya esta registrado");
            usuarios.Add(usuarito);
        }

        // Actualiza el usuario reemplazándolo por los datos recibidos.
        public static void actualizar(Usuario usuarito)
        {
            foreach (Usuario aux in usuarios)
            {
                if (aux.User == usuarito.User)
                {
                    usuarios.Remove(aux);
                    usuarios.Add(usuarito);
                    // Termina el método sin seguir recorriendo la lista modificada.
                    return;
                }
            }
            throw new Exception("Usuario no encontrado");
        }

        // Devuelve la lista de usuarios.
        public static List<Usuario> mostrar() {
            return usuarios;
        }
    }
}
