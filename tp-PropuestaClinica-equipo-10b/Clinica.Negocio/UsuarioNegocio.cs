using System;
using System.Collections.Generic;
using System.Linq;
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

        public List<Usuario> ListarRecepcionistas()
        {
            List<Usuario> todos = datos.Listar();
            List<Usuario> soloRecepcionistas = todos.FindAll(x => x.Perfil == Perfil.Recepcionista);
            return soloRecepcionistas;
        }

        public void Agregar(Usuario nuevo)
        {
            // --- INICIO REGLAS DE NEGOCIO ---

            if (nuevo == null)
                throw new ArgumentNullException("El objeto Usuario no puede ser nulo.");

            if (string.IsNullOrWhiteSpace(nuevo.Email) || string.IsNullOrWhiteSpace(nuevo.Pass))
                throw new ArgumentException("Email y Contraseña son obligatorios.");

            if (datos.ExistePorEmail(nuevo.Email))
            {
                throw new Exception("El email ingresado ya existe en el sistema.");
            }

            datos.Agregar(nuevo);
        }

        public void ModificarPasswordPorEmail(string email, string nuevaPass)
        {
            if (!datos.ExistePorEmail(email))
            {
                throw new Exception("No se encontró ningún usuario con ese email.");
            }

            datos.ModificarPassword(email, nuevaPass);
        }

        public Usuario Login(string email, string password)
        {
            return datos.Login(email, password);
        }
        public bool ExistePorEmail(string email)
        {
            return datos.ExistePorEmail(email);
        }

        public void CambiarActivoPorEmail(string email, bool activo)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email inválido");
            datos.CambiarActivoPorEmail(email, activo);
        }
        public Usuario ObtenerPorPacienteId(int pacienteId)
        {
            return datos.ObtenerPorPacienteId(pacienteId);
        }

        public Usuario ObtenerPorId(int id)
        {
            return datos.ObtenerPorId(id);
        }

        
        public void Modificar(Usuario u, bool actualizarPassword = false)
        {
            if (u == null || u.IdUsuario <= 0) throw new ArgumentException("Usuario inválido.");
            datos.Modificar(u, actualizarPassword);
        }

    }
}