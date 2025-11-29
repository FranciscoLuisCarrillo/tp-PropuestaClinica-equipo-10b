using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clinica.Datos;
using Clinica.Dominio;

namespace Clinica.Negocio
{
    public class UsuarioNegocio
    {
        private UsuarioDatos datos;

        public UsuarioNegocio()
        {
            datos = new UsuarioDatos();
        }

        /// <summary>
        /// Llama a la capa de datos para listar los usuarios.
        /// (Para el "Listado de Recepcionistas")
        /// </summary>
        public List<Usuario> Listar()
        {
            return datos.Listar();
        }
        public List<Usuario> ListarRecepcionistas()
        {
            List<Usuario> todos = datos.Listar();

            // Usamos FindAll (o Where con Linq) para filtrar solo el perfil 1
            List<Usuario> soloRecepcionistas = todos.FindAll(x => x.Perfil == Perfil.Recepcionista);

            return soloRecepcionistas;
        }

        /// <summary>
        /// Lógica de negocio para el "Guardado de la Recepcionista".
        /// </summary>
        public void Agregar(Usuario nuevo)
        {
            // --- INICIO REGLAS DE NEGOCIO ---

            if (nuevo == null)
                throw new ArgumentNullException("El objeto Usuario no puede ser nulo.");

            if (string.IsNullOrWhiteSpace(nuevo.Email) || string.IsNullOrWhiteSpace(nuevo.Pass))
                throw new ArgumentException("Email y Contraseña son obligatorios.");

            // Validación de duplicados
            if (datos.ExistePorEmail(nuevo.Email))
                throw new Exception("El Email ingresado ya se encuentra registrado.");

            // --- FIN REGLAS DE NEGOCIO ---

            datos.Agregar(nuevo);
        }
        public Usuario Login(string email, string pass)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(pass))
                throw new Exception("Email y contraseña son obligatorios");

            return datos.Login(email, pass);
        }


        public void ModificarPasswordPorEmail(string email, string nuevaPassword)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email inválido.");
            if (string.IsNullOrWhiteSpace(nuevaPassword)) throw new ArgumentException("La nueva contraseña no puede estar vacía.");

            datos.ModificarPasswordEmail(email, nuevaPassword);
        }

    }
}