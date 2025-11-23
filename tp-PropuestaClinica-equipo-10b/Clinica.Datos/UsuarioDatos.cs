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
                datos.SetearConsulta(@"INSERT INTO Usuarios 
                                    (Email, Pass, Perfil, Activo, IdPaciente, IdMedico, IdRecepcionista) 
                                    VALUES 
                                    (@Email, @Pass, @Perfil, 1, @IdPaciente, @IdMedico, @IdRecepcionista)");


                datos.SetearParametro("@Email", nuevo.Email);
                datos.SetearParametro("@Pass", nuevo.Password);
                datos.SetearParametro("@Perfil", (int)nuevo.Perfil);

                datos.SetearParametro("@IdPaciente", nuevo.IdPaciente.HasValue ? (object)nuevo.IdPaciente.Value : DBNull.Value);
                datos.SetearParametro("@IdMedico", nuevo.IdMedico.HasValue ? (object)nuevo.IdMedico.Value : DBNull.Value);
                datos.SetearParametro("@IdRecepcionista", nuevo.IdRecepcionista.HasValue ? (object)nuevo.IdRecepcionista.Value : DBNull.Value);

                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear el usuario: " + ex.Message);
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
                datos.SetearConsulta(@"SELECT UsuarioId, Email, Pass, Perfil, IdPaciente, IdMedico, IdRecepcionista, Activo
                                    FROM Usuarios
                                    WHERE Email = @Email AND Pass = @Pass");
                datos.SetearParametro("@Email", email);
                datos.SetearParametro("@Pass", pass);
                datos.EjecutarLectura();
                if (datos.Lector.Read())
                {
                    Usuario usuario = new Usuario();
                    usuario.IdUsuario = (int)datos.Lector["UsuarioId"];
                    usuario.Email = (string)datos.Lector["Email"];
                    usuario.Password = (string)datos.Lector["Pass"];
                    usuario.Perfil = (Perfil)(int)datos.Lector["Perfil"];
                    usuario.Activo = datos.Lector["Activo"] != DBNull.Value ? (bool)datos.Lector["Activo"] : true;

                    if(datos.Lector["IdPaciente"] != DBNull.Value)
                        usuario.IdPaciente = (int)datos.Lector["IdPaciente"];
                    if (datos.Lector["IdMedico"] != DBNull.Value)
                        usuario.IdMedico = (int)datos.Lector["IdMedico"];
                    if (datos.Lector["IdRecepcionista"] != DBNull.Value)
                        usuario.IdRecepcionista = (int)datos.Lector["IdRecepcionista"];
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