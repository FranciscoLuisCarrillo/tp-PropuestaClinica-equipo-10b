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
              
                datos.SetearConsulta("SELECT PacienteId, Nombre, Apellido, DNI, FechaNacimiento, Telefono, Email, Domicilio, ObraSocial, Activo FROM Pacientes");
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Paciente aux = new Paciente();
                    aux.PacienteId = (int)datos.Lector["PacienteId"];
                    aux.Nombre = datos.Lector["Nombre"] == DBNull.Value ? "" : (string)datos.Lector["Nombre"];
                    aux.Apellido = datos.Lector["Apellido"] == DBNull.Value ? "" : (string)datos.Lector["Apellido"];
                    aux.Dni = datos.Lector["DNI"] == DBNull.Value ? "" : (string)datos.Lector["DNI"];
                    aux.Email = datos.Lector["Email"] == DBNull.Value ? "" : (string)datos.Lector["Email"];

                    aux.Telefono = datos.Lector["Telefono"] == DBNull.Value ? "" : (string)datos.Lector["Telefono"];
                    aux.Domicilio = datos.Lector["Domicilio"] == DBNull.Value ? "" : (string)datos.Lector["Domicilio"];
                    aux.ObraSocial = datos.Lector["ObraSocial"] == DBNull.Value ? "" : (string)datos.Lector["ObraSocial"];

                    // Leemos el estado
                    if (datos.Lector["Activo"] != DBNull.Value)
                        aux.Activo = (bool)datos.Lector["Activo"];
                    else
                        aux.Activo = true; 

                    lista.Add(aux);
                }
                return lista;
            }catch (Exception ex)
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
                datos.SetearParametro("@DNI", nuevo.Dni);
                datos.SetearParametro("@FechaNacimiento", nuevo.FechaNacimiento);
                datos.SetearParametro("@Telefono", (object)nuevo.Telefono ?? DBNull.Value);
                datos.SetearParametro("@Email", nuevo.Email);
                datos.SetearParametro("@Domicilio", (object)nuevo.Domicilio ?? DBNull.Value);
                datos.SetearParametro("@ObraSocial", (object)nuevo.ObraSocial ?? DBNull.Value);

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
                datos.SetearConsulta("SELECT 1 FROM Pacientes WHERE Email = @Email");
                datos.SetearParametro("@Email", email);
                datos.EjecutarLectura();
                return datos.Lector.Read();
            }
            finally { datos.CerrarConexion(); }
        }
        public void Modificar(Paciente paciente)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta(@"UPDATE Pacientes 
                               SET Nombre=@Nombre, Apellido=@Apellido, DNI=@DNI, 
                                   FechaNacimiento=@Fecha, Telefono=@Tel, Domicilio=@Dom
                               WHERE PacienteId=@Id");

                datos.SetearParametro("@Nombre", paciente.Nombre);
                datos.SetearParametro("@Apellido", paciente.Apellido);
                datos.SetearParametro("@DNI", paciente.Dni);
                datos.SetearParametro("@Fecha", paciente.FechaNacimiento);
                datos.SetearParametro("@Tel", paciente.Telefono);
                datos.SetearParametro("@Dom", paciente.Domicilio);
                datos.SetearParametro("@Id", paciente.PacienteId);

                datos.EjecutarAccion();
            }
            finally
            {
                datos.CerrarConexion();
            }
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

            }finally
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
                    Paciente aux = new Paciente();
                    aux.PacienteId = (int)datos.Lector["PacienteId"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Apellido = (string)datos.Lector["Apellido"];
                    aux.Email = (string)datos.Lector["Email"];

                    // Validamos nulos porque es un registro incompleto
                    if (!(datos.Lector["DNI"] is DBNull)) aux.Dni = (string)datos.Lector["DNI"];
                    if (!(datos.Lector["Telefono"] is DBNull)) aux.Telefono = (string)datos.Lector["Telefono"];
                    if (!(datos.Lector["FechaNacimiento"] is DBNull)) aux.FechaNacimiento = (DateTime)datos.Lector["FechaNacimiento"];
                    if (!(datos.Lector["Domicilio"] is DBNull)) aux.Domicilio = (string)datos.Lector["Domicilio"];

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
   