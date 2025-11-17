using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clinica.Dominio;

namespace Clinica.Datos
{
    public class UsuarioDatos
    {
        /// <summary>
        /// Lista todos los usuarios (Admins, Recepcionistas, Médicos)
        /// </summary>
        public List<Usuario> Listar()
        {
            List<Usuario> lista = new List<Usuario>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("SELECT UsuarioId, Email, Perfil FROM Usuarios");
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Usuario aux = new Usuario();
                    aux.IdUsuario = (int)datos.Lector["UsuarioId"];
                    aux.Email = (string)datos.Lector["Email"];

                    // Convertimos el INT de la DB al ENUM de C#
                    aux.Perfil = (Perfil)(int)datos.Lector["Perfil"];

                    lista.Add(aux);
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        /// <summary>
        /// Agrega un nuevo usuario (Recepcionista, Admin, etc.)
        /// </summary>
        public void Agregar(Usuario nuevo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("INSERT INTO Usuarios (Email, Pass, Perfil) VALUES (@Email, @Pass, @Perfil)");
                datos.SetearParametro("@Email", nuevo.Email);
                datos.SetearParametro("@Pass", nuevo.Password);

                // Convertimos el ENUM de C# al INT de la DB
                datos.SetearParametro("@Perfil", (int)nuevo.Perfil);

                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        /// <summary>
        /// Verifica si un email ya existe en la base de datos de Usuarios.
        /// </summary>
        public bool ExistePorEmail(string email)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("SELECT 1 FROM Usuarios WHERE Email COLLATE Latin1_General_CI_AI = @Email");
                datos.SetearParametro("@Email", email);
                datos.EjecutarLectura();
                return datos.Lector.Read();
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public Usuario Login(string email, string pass)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("SELECT UsuarioId, Email, Pass, Perfil FROM Usuarios WHERE Email COLLATE Latin1_General_CI_AI = @Email AND Pass = @Pass");
                datos.SetearParametro("@Email", email);
                datos.SetearParametro("@Pass", pass);
                datos.EjecutarLectura();
                if (datos.Lector.Read())
                {
                    Usuario usuario = new Usuario
                    {
                        IdUsuario = (int)datos.Lector["UsuarioId"],
                        Email = (string)datos.Lector["Email"],
                        Password = (string)datos.Lector["Pass"],
                        Perfil = (Perfil)(int)datos.Lector["Perfil"]
                    };
                    return usuario;
                }
                return null;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

    }
}