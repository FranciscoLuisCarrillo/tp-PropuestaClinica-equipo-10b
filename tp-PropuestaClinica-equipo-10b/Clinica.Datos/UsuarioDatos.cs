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
                    var aux = new Usuario
                    {
                        IdUsuario = (int)datos.Lector["UsuarioId"],
                        Email = (string)datos.Lector["Email"],
                        Perfil = (Perfil)(int)datos.Lector["Perfil"]
                    };
                   

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
                string consulta = @"
            INSERT INTO Usuarios 
            (Email, Pass, Perfil, Activo, IdRecepcionista, IdMedico, IdPaciente, Nombre, Apellido, Rol) 
            VALUES 
            (@Email, @Pass, @Perfil, @Activo, @IdRecepcionista, @IdMedico, @IdPaciente, @Nombre, @Apellido, @Rol)";

                datos.SetearConsulta(consulta);

                // Parámetros Básicos
                datos.SetearParametro("@Email", nuevo.Email);
                datos.SetearParametro("@Pass", nuevo.Pass); 
                datos.SetearParametro("@Perfil", (int)nuevo.Perfil);
                datos.SetearParametro("@Activo", nuevo.Activo);

                // Parámetros de Relación (Manejo de Nulos)
                datos.SetearParametro("@IdRecepcionista", (object)nuevo.IdRecepcionista ?? DBNull.Value);
                datos.SetearParametro("@IdMedico", (object)nuevo.IdMedico ?? DBNull.Value);
                datos.SetearParametro("@IdPaciente", (object)nuevo.IdPaciente ?? DBNull.Value);

                // Parámetros Nuevos
                datos.SetearParametro("@Nombre", (object)nuevo.Nombre ?? DBNull.Value);
                datos.SetearParametro("@Apellido", (object)nuevo.Apellido ?? DBNull.Value);
                datos.SetearParametro("@Rol", (object)nuevo.Rol ?? DBNull.Value);

                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar usuario: " + ex.Message);
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
                    var usuario = new Usuario
                    {
                        IdUsuario = (int)datos.Lector["UsuarioId"],
                        Email = (string)datos.Lector["Email"],
                        Pass = (string)datos.Lector["Pass"],          
                        Perfil = (Perfil)(int)datos.Lector["Perfil"],
                        Activo = datos.Lector["Activo"] != DBNull.Value && (bool)datos.Lector["Activo"],
                        IdPaciente = datos.Lector["IdPaciente"] == DBNull.Value ? (int?)null : (int)datos.Lector["IdPaciente"],
                        IdMedico = datos.Lector["IdMedico"] == DBNull.Value ? (int?)null : (int)datos.Lector["IdMedico"],
                        IdRecepcionista = datos.Lector["IdRecepcionista"] == DBNull.Value ? (int?)null : (int)datos.Lector["IdRecepcionista"]
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
        public void ModificarPasswordEmail(string email, string nuevaPass)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta(@"UPDATE Usuarios 
                               SET Pass = @Pass 
                               WHERE Email COLLATE Latin1_General_CI_AI = @Email");
                datos.SetearParametro("@Pass", nuevaPass);
                datos.SetearParametro("@Email", email);
                int afectados = datos.EjecutarAccion();

                if (afectados == 0)
                    throw new Exception("No se encontró un usuario con ese email.");
            }
            finally { datos.CerrarConexion(); }
        }

        

    }
}