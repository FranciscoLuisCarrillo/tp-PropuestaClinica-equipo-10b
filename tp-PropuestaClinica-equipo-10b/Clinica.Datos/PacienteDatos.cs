using System;
using System.Collections.Generic;
using Clinica.Dominio;

namespace Clinica.Datos
{
    public class PacienteDatos
    {
        public List<Paciente> Listar()
        {
            List<Paciente> lista = new List<Paciente>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("SELECT PacienteId, Nombre, Apellido, DNI, FechaNacimiento, Telefono, Email, Domicilio, ObraSocial, Genero, Activo FROM Pacientes");

                while (datos.Lector.Read())
                {
                    var aux = new Paciente
                    {
                        PacienteId = (int)datos.Lector["PacienteId"],
                        Nombre = datos.Lector["Nombre"] as string ?? "",
                        Apellido = datos.Lector["Apellido"] as string ?? "",
                        Dni = datos.Lector["DNI"] as string ?? "",
                        Email = datos.Lector["Email"] as string ?? "",
                        Telefono = datos.Lector["Telefono"] as string ?? "",
                        Domicilio = datos.Lector["Domicilio"] as string ?? "",
                        ObraSocial = datos.Lector["ObraSocial"] as string ?? "",
                        Genero = datos.Lector["Genero"] as string ?? "",  
                        FechaNacimiento = datos.Lector["FechaNacimiento"] == DBNull.Value ? DateTime.MinValue : (DateTime)datos.Lector["FechaNacimiento"],
                        Activo = datos.Lector["Activo"] == DBNull.Value ? true : (bool)datos.Lector["Activo"]
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

        public int Agregar(Paciente nuevo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = @"INSERT INTO Pacientes 
            (Nombre, Apellido, DNI, FechaNacimiento, Telefono, Email, Domicilio, ObraSocial, Activo) 
            VALUES 
            (@Nombre, @Apellido, @DNI, @FechaNacimiento, @Telefono, @Email, @Domicilio, @ObraSocial, 1);
            SELECT SCOPE_IDENTITY();";

                datos.SetearConsulta(consulta);

                datos.SetearParametro("@Nombre", nuevo.Nombre);
                datos.SetearParametro("@Apellido", nuevo.Apellido);
                datos.SetearParametro("@DNI",
                    string.IsNullOrWhiteSpace(nuevo.Dni) ? (object)DBNull.Value : nuevo.Dni);

             
                if (nuevo.FechaNacimiento == default(DateTime))
                    datos.SetearParametro("@FechaNacimiento", DBNull.Value);
                else
                    datos.SetearParametro("@FechaNacimiento", nuevo.FechaNacimiento);

                datos.SetearParametro("@Telefono",
                    string.IsNullOrWhiteSpace(nuevo.Telefono) ? (object)DBNull.Value : nuevo.Telefono);

                datos.SetearParametro("@Email", nuevo.Email);

                datos.SetearParametro("@Domicilio",
                    string.IsNullOrWhiteSpace(nuevo.Domicilio) ? (object)DBNull.Value : nuevo.Domicilio);

                datos.SetearParametro("@ObraSocial",
                    string.IsNullOrWhiteSpace(nuevo.ObraSocial) ? (object)DBNull.Value : nuevo.ObraSocial);

                object idGenerado = datos.EjecutarEscalar();
                return Convert.ToInt32(idGenerado);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }


        // NUEVO MÉTODO ELIMINAR
        public void Eliminar(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("UPDATE Pacientes SET Activo = 0 WHERE PacienteId = @Id");
                datos.SetearParametro("@Id", id);
                datos.EjecutarAccion();
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public bool ExistePorDNI(string dni)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("SELECT 1 FROM Pacientes WHERE DNI = @DNI");
                datos.SetearParametro("@DNI", dni);
                datos.EjecutarLectura();
                return datos.Lector.Read();
            }
            finally { datos.CerrarConexion(); }
        }

        public bool ExistePorEmail(string email)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("SELECT 1 FROM Pacientes WHERE LOWER(Email) = LOWER(@Email)");
                datos.SetearParametro("@Email", email);
                datos.EjecutarLectura();
                return datos.Lector.Read();
            }
            finally { datos.CerrarConexion(); }
        }

        public void Modificar(Paciente paciente)
        {
            var datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta(@"
            UPDATE Pacientes 
            SET  Nombre          = @Nombre,
                 Apellido        = @Apellido,
                 DNI             = @DNI,
                 FechaNacimiento = @Fecha,
                 Telefono        = @Tel,
                 Domicilio       = @Dom,
                 Genero          = @Genero,
                 Email           = @Email,
                 ObraSocial      = @ObraSocial,
                 Activo          = @Activo
            WHERE PacienteId     = @Id;");

                datos.SetearParametro("@Nombre", paciente.Nombre ?? (object)DBNull.Value);
                datos.SetearParametro("@Apellido", paciente.Apellido ?? (object)DBNull.Value);
                datos.SetearParametro("@DNI", string.IsNullOrWhiteSpace(paciente.Dni) ? (object)DBNull.Value : paciente.Dni);
                datos.SetearParametro("@Fecha", paciente.FechaNacimiento == DateTime.MinValue ? (object)DBNull.Value : paciente.FechaNacimiento);
                datos.SetearParametro("@Tel", string.IsNullOrWhiteSpace(paciente.Telefono) ? (object)DBNull.Value : paciente.Telefono);
                datos.SetearParametro("@Dom", string.IsNullOrWhiteSpace(paciente.Domicilio) ? (object)DBNull.Value : paciente.Domicilio);

                
                datos.SetearParametro("@Genero", string.IsNullOrWhiteSpace(paciente.Genero) ? (object)DBNull.Value : paciente.Genero);
                datos.SetearParametro("@Email", string.IsNullOrWhiteSpace(paciente.Email) ? (object)DBNull.Value : paciente.Email);
                datos.SetearParametro("@ObraSocial", string.IsNullOrWhiteSpace(paciente.ObraSocial) ? (object)DBNull.Value : paciente.ObraSocial);
                datos.SetearParametro("@Activo", paciente.Activo);

                datos.SetearParametro("@Id", paciente.PacienteId);
                datos.EjecutarAccion();
            }
            finally { datos.CerrarConexion(); }
        }
        public void CambioEstado(int id, bool estado)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("UPDATE Pacientes SET Activo = @Estado WHERE PacienteId = @Id");
                datos.SetearParametro("@Estado", estado);
                datos.SetearParametro("@Id", id);
                datos.EjecutarAccion();
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public Paciente ObtenerPorId(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("SELECT * FROM Pacientes WHERE PacienteId = @Id");
                datos.SetearParametro("@Id", id);
                datos.EjecutarLectura();

                if (datos.Lector.Read())
                {
                    var aux = new Paciente
                    {
                        PacienteId = (int)datos.Lector["PacienteId"],
                        Nombre = datos.Lector["Nombre"] == DBNull.Value ? "" : datos.Lector["Nombre"].ToString(),
                        Apellido = datos.Lector["Apellido"] == DBNull.Value ? "" : datos.Lector["Apellido"].ToString(),
                        Dni = datos.Lector["DNI"] == DBNull.Value ? "" : datos.Lector["DNI"].ToString(),
                        Genero = datos.Lector["Genero"] == DBNull.Value ? "" : datos.Lector["Genero"].ToString(),
                        Email = datos.Lector["Email"] == DBNull.Value ? "" : datos.Lector["Email"].ToString(),
                        Telefono = datos.Lector["Telefono"] == DBNull.Value ? "" : datos.Lector["Telefono"].ToString(),
                        Domicilio = datos.Lector["Domicilio"] == DBNull.Value ? "" : datos.Lector["Domicilio"].ToString(),
                        ObraSocial = datos.Lector["ObraSocial"] == DBNull.Value ? "" : datos.Lector["ObraSocial"].ToString(),
                        FechaNacimiento = datos.Lector["FechaNacimiento"] == DBNull.Value ? DateTime.MinValue : (DateTime)datos.Lector["FechaNacimiento"],
                        Activo = datos.Lector["Activo"] == DBNull.Value ? true : (bool)datos.Lector["Activo"]
                    };
                    return aux;
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