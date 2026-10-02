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

        // Devuelve la lista de usuarios.
        public static List<Usuario> mostrar() {
            return usuarios;
        }
    }
}
