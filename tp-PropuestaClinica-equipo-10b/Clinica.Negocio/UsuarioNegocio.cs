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
            if (string.IsNullOrWhiteSpace(nuevo.Email) || string.IsNullOrWhiteSpace(nuevo.Password))
            {
                throw new Exception("El usuario y la contraseña son obligatorios.");
            }

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

        public bool Login(Usuario usuario)
        {
            return datos.Login(usuario);
        }
    }
}