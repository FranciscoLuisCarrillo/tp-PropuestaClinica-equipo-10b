using System;
using System.Collections.Generic;
using Clinica.Dominio;

namespace Clinica.Datos
{
    public class UsuarioDatos
    {
        public List<Usuario> Listar()
        {
            List<Usuario> lista = new List<Usuario>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("SELECT UsuarioId, Email, Pass, Perfil, Activo, IdRecepcionista, IdMedico, IdPaciente, Nombre, Apellido, Rol FROM Usuarios");
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Usuario aux = new Usuario();
                    aux.IdUsuario = (int)datos.Lector["UsuarioId"];
                    aux.Email = (string)datos.Lector["Email"];
                    aux.Pass = (string)datos.Lector["Pass"];
                    aux.Perfil = (Perfil)(int)datos.Lector["Perfil"];
                    aux.Activo = (bool)datos.Lector["Activo"];

                    if (!(datos.Lector["IdRecepcionista"] is DBNull))
                        aux.IdRecepcionista = (int)datos.Lector["IdRecepcionista"];
                    if (!(datos.Lector["IdMedico"] is DBNull))
                        aux.IdMedico = (int)datos.Lector["IdMedico"];
                    if (!(datos.Lector["IdPaciente"] is DBNull))
                        aux.IdPaciente = (int)datos.Lector["IdPaciente"];
                    if (!(datos.Lector["Nombre"] is DBNull))
                        aux.Nombre = (string)datos.Lector["Nombre"];
                    if (!(datos.Lector["Apellido"] is DBNull))
                        aux.Apellido = (string)datos.Lector["Apellido"];
                    if (!(datos.Lector["Rol"] is DBNull))
                        aux.Rol = (string)datos.Lector["Rol"];
                   
                   

                    lista.Add(aux);
                }
                return lista;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

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

                datos.SetearParametro("@Email", nuevo.Email);
                datos.SetearParametro("@Pass", nuevo.Pass); 
                datos.SetearParametro("@Perfil", (int)nuevo.Perfil);
                datos.SetearParametro("@Activo", nuevo.Activo);

                datos.SetearParametro("@IdRecepcionista", (object)nuevo.IdRecepcionista ?? DBNull.Value);
                datos.SetearParametro("@IdMedico", (object)nuevo.IdMedico ?? DBNull.Value);
                datos.SetearParametro("@IdPaciente", (object)nuevo.IdPaciente ?? DBNull.Value);

                datos.SetearParametro("@Nombre", (object)nuevo.Nombre ?? DBNull.Value);
                datos.SetearParametro("@Apellido", (object)nuevo.Apellido ?? DBNull.Value);
                datos.SetearParametro("@Rol", (object)nuevo.Rol ?? DBNull.Value);

                datos.EjecutarAccion();
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public bool ExistePorEmail(string email)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("SELECT 1 FROM Usuarios WHERE LOWER(Email) = LOWER(@Email)");
                datos.SetearParametro("@Email", email);
                datos.EjecutarLectura();
                return datos.Lector.Read();
            }
            finally { datos.CerrarConexion(); }
        }

        public void ModificarPassword(string email, string nuevaPass)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("UPDATE Usuarios SET Pass = @Pass WHERE Email = @Email");
                datos.SetearParametro("@Pass", nuevaPass);
                datos.SetearParametro("@Email", email);
                datos.EjecutarAccion();
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public Usuario Login(string email, string password)
        {
            var datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta(@"
            SELECT  UsuarioId, Email, Pass, Perfil, Activo,
                    IdPaciente, IdMedico, IdRecepcionista,
                    Nombre, Apellido, Rol
            FROM    Usuarios
            WHERE   LOWER(Email) = LOWER(@Email) AND Pass = @Pass
        ");
                datos.SetearParametro("@Email", email);
                datos.SetearParametro("@Pass", password);
                datos.EjecutarLectura();

                if (!datos.Lector.Read()) return null;

                var u = new Usuario
                {
                    IdUsuario = (int)datos.Lector["UsuarioId"],
                    Email = (string)datos.Lector["Email"],
                    Pass = (string)datos.Lector["Pass"],
                    Perfil = (Perfil)(int)datos.Lector["Perfil"],
                    Activo = datos.Lector["Activo"] != DBNull.Value && (bool)datos.Lector["Activo"],
                    IdPaciente = datos.Lector["IdPaciente"] == DBNull.Value ? (int?)null : (int)datos.Lector["IdPaciente"],
                    IdMedico = datos.Lector["IdMedico"] == DBNull.Value ? (int?)null : (int)datos.Lector["IdMedico"],
                    IdRecepcionista = datos.Lector["IdRecepcionista"] == DBNull.Value ? (int?)null : (int)datos.Lector["IdRecepcionista"],
                    Nombre = datos.Lector["Nombre"] == DBNull.Value ? null : (string)datos.Lector["Nombre"],
                    Apellido = datos.Lector["Apellido"] == DBNull.Value ? null : (string)datos.Lector["Apellido"],
                    Rol = datos.Lector["Rol"] == DBNull.Value ? null : (string)datos.Lector["Rol"],
                };
                return u;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public void CambiarActivoPorEmail(string email, bool activo)
        {
            var acc = new AccesoDatos();
            try
            {
                acc.SetearConsulta("UPDATE Usuarios SET Activo=@A WHERE LOWER(Email)=LOWER(@E)");
                acc.SetearParametro("@A", activo);
                acc.SetearParametro("@E", email);
                acc.EjecutarAccion();
            }
            finally { acc.CerrarConexion(); }
        }
        public Usuario ObtenerPorPacienteId(int pacienteId)
        {
            var acc = new AccesoDatos();
            try
            {
                acc.SetearConsulta(@"
            SELECT TOP 1 UsuarioId, Email, Pass, Perfil, Activo,
                    IdPaciente, IdMedico, IdRecepcionista,
                    Nombre, Apellido, Rol
            FROM Usuarios
            WHERE IdPaciente = @P");
                acc.SetearParametro("@P", pacienteId);
                acc.EjecutarLectura();
                if (acc.Lector.Read())
                {
                    return new Usuario
                    {
                        IdUsuario = (int)acc.Lector["UsuarioId"],
                        Email = acc.Lector["Email"].ToString(),
                        Pass = acc.Lector["Pass"].ToString(),
                        Perfil = (Perfil)(int)acc.Lector["Perfil"],
                        Rol = acc.Lector["Rol"].ToString(),
                        IdPaciente = acc.Lector["IdPaciente"] == DBNull.Value ? (int?)null : (int)acc.Lector["IdPaciente"],
                        IdMedico = acc.Lector["IdMedico"] == DBNull.Value ? (int?)null : (int)acc.Lector["IdMedico"],
                        Nombre = acc.Lector["Nombre"].ToString(),
                        Apellido = acc.Lector["Apellido"].ToString(),
                        Activo = (bool)acc.Lector["Activo"]
                    };
                }
                return null;
            }
            finally { acc.CerrarConexion(); }
        }

        public Usuario ObtenerPorId(int id)
        {
            var acc = new AccesoDatos();
            try
            {
                acc.SetearConsulta(@"
            SELECT UsuarioId, Email, Pass, Perfil, Activo,
                    IdPaciente, IdMedico, IdRecepcionista,
                    Nombre, Apellido, Rol
            FROM Usuarios
            WHERE UsuarioId = @Id");
                acc.SetearParametro("@Id", id);
                acc.EjecutarLectura();
                if (acc.Lector.Read())
                {
                    return new Usuario
                    {
                        IdUsuario = (int)acc.Lector["UsuarioId"],
                        Email = acc.Lector["Email"].ToString(),
                        Pass = acc.Lector["Pass"].ToString(),
                        Perfil = (Perfil)(int)acc.Lector["Perfil"],
                        Rol = acc.Lector["Rol"].ToString(),
                        IdPaciente = acc.Lector["IdPaciente"] == DBNull.Value ? (int?)null : (int)acc.Lector["IdPaciente"],
                        IdMedico = acc.Lector["IdMedico"] == DBNull.Value ? (int?)null : (int)acc.Lector["IdMedico"],
                        Nombre = acc.Lector["Nombre"].ToString(),
                        Apellido = acc.Lector["Apellido"].ToString(),
                        Activo = (bool)acc.Lector["Activo"]
                    };
                }
                return null;
            }
            finally { acc.CerrarConexion(); }
        }

        public void Modificar(Usuario u, bool actualizarPassword)
        {
            var acc = new AccesoDatos();
            try
            {
                acc.SetearConsulta(@"
            UPDATE Usuarios
            SET Email = @E,
                Perfil = @Perf,
                Rol = @Rol,
                IdPaciente = @Pid,
                IdMedico = @Mid,
                IdRecepcionista = @Rid,
                Nombre = @Nom,
                Apellido = @Ape,
                Activo = @Act
                " + (actualizarPassword ? ", Pass = @P" : "") + @"
            WHERE UsuarioId = @Id;");

                acc.SetearParametro("@E", u.Email);
                acc.SetearParametro("@Perf", (int)u.Perfil);
                acc.SetearParametro("@Rol", (object)u.Rol ?? DBNull.Value);

                acc.SetearParametro("@Pid", u.IdPaciente.HasValue ? (object)u.IdPaciente.Value : DBNull.Value);
                acc.SetearParametro("@Mid", u.IdMedico.HasValue ? (object)u.IdMedico.Value : DBNull.Value);
                acc.SetearParametro("@Rid", u.IdRecepcionista.HasValue ? (object)u.IdRecepcionista.Value : DBNull.Value);

                acc.SetearParametro("@Nom", (object)u.Nombre ?? DBNull.Value);
                acc.SetearParametro("@Ape", (object)u.Apellido ?? DBNull.Value);
                acc.SetearParametro("@Act", u.Activo);

                if (actualizarPassword)
                    acc.SetearParametro("@P", u.Pass);

                
                acc.SetearParametro("@Id", u.IdUsuario);

                acc.EjecutarAccion();
            }
            finally { acc.CerrarConexion(); }
        }



    }
}
