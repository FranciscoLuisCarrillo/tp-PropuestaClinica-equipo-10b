using System;
using System.Collections.Generic;
using Clinica.Dominio;

namespace Clinica.Datos
{
    public class PacienteDatos
    {
        /// <summary>
        /// Inserta un nuevo paciente en la base de datos.
        /// Devuelve el ID (PacienteId) generado.
        /// </summary>
        public int Agregar(Paciente nuevo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                // Agregamos "SELECT SCOPE_IDENTITY()" para que la consulta
                // devuelva el ID del paciente recién insertado.
                string consulta = @"INSERT INTO Pacientes 
                                    (Nombre, Apellido, Email, DNI, FechaNacimiento, Telefono, Domicilio, Activo) 
                                    VALUES 
                                    (@Nombre, @Apellido, @Email, @DNI, @FechaNacimiento, @Telefono, @Domicilio, 1);
                                    SELECT SCOPE_IDENTITY();";

                datos.SetearConsulta(consulta);

                // Seteamos los parámetros leyendo las propiedades públicas de 'nuevo'
                datos.SetearParametro("@Nombre", nuevo.Nombre);
                datos.SetearParametro("@Apellido", nuevo.Apellido);
                datos.SetearParametro("@Email", nuevo.Email);

                // Manejamos valores opcionales (pueden ser nulos en la DB)
                datos.SetearParametro("@DNI", (object)nuevo.Dni ?? DBNull.Value);
                datos.SetearParametro("@Telefono", (object)nuevo.Telefono ?? DBNull.Value);
                datos.SetearParametro("@Domicilio", (object)nuevo.Domicilio ?? DBNull.Value);

                if (nuevo.FechaNacimiento == DateTime.MinValue)
                {
                    datos.SetearParametro("@FechaNacimiento", DBNull.Value);
                }
                else
                {
                    datos.SetearParametro("@FechaNacimiento", nuevo.FechaNacimiento);
                }

                // Usamos EjecutarEscalar para obtener el ID devuelto por SCOPE_IDENTITY()
                object idGenerado = datos.EjecutarEscalar();
                return Convert.ToInt32(idGenerado);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar paciente en la base de datos.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public List<Paciente> Listar()
        {
            List<Paciente> lista = new List<Paciente>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                // Seleccionamos todos los campos necesarios
                datos.SetearConsulta("SELECT PacienteId, Nombre, Apellido, DNI, FechaNacimiento, Telefono, Email, Domicilio, ObraSocial FROM Pacientes");
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Paciente aux = new Paciente();
                    aux.PacienteId = (int)datos.Lector["PacienteId"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Apellido = (string)datos.Lector["Apellido"];
                    aux.Dni = (string)datos.Lector["DNI"];
                    aux.FechaNacimiento = (DateTime)datos.Lector["FechaNacimiento"];
                    aux.Email = (string)datos.Lector["Email"];

                    // Validaciones para campos que pueden ser NULL en la base de datos
                    if (!(datos.Lector["Telefono"] is DBNull))
                        aux.Telefono = (string)datos.Lector["Telefono"];

                    if (!(datos.Lector["Domicilio"] is DBNull))
                        aux.Domicilio = (string)datos.Lector["Domicilio"];

                    if (!(datos.Lector["ObraSocial"] is DBNull))
                        aux.ObraSocial = (string)datos.Lector["ObraSocial"];

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
        /// Verifica si un DNI ya existe en la base de datos.
        /// </summary>
        public bool ExistePorDNI(string dni)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                // Usamos COLLATE para ignorar mayúsculas/minúsculas (como en EspecialidadDatos)
                string consulta = "SELECT 1 FROM Pacientes WHERE DNI COLLATE Latin1_General_CI_AI = @DNI";
                datos.SetearConsulta(consulta);
                datos.SetearParametro("@DNI", dni);
                datos.EjecutarLectura();

                // Si Lector.Read() es true, significa que encontró una fila
                return datos.Lector.Read();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al verificar DNI.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        /// <summary>
        /// Verifica si un Email ya existe en la base de datos.
        /// </summary>
        public bool ExistePorEmail(string email)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = "SELECT 1 FROM Pacientes WHERE Email COLLATE Latin1_General_CI_AI = @Email";
                datos.SetearConsulta(consulta);
                datos.SetearParametro("@Email", email);
                datos.EjecutarLectura();

                return datos.Lector.Read();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al verificar Email.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }
        public void Modificar(Paciente paciente)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta(@"UPDATE Pacientes 
                            SET Nombre = @Nombre, 
                                Apellido = @Apellido, 
                                DNI = @DNI, 
                                FechaNacimiento = @FechaNacimiento, 
                                Genero = @Genero,
                                Telefono = @Telefono, 
                                Domicilio = @Domicilio,
                                ObraSocial = @ObraSocial
                            WHERE PacienteId = @Id");

                

                datos.SetearParametro("@Nombre", paciente.Nombre);
                datos.SetearParametro("@Apellido", paciente.Apellido);
                datos.SetearParametro("@DNI", paciente.Dni);
                datos.SetearParametro("@FechaNacimiento", paciente.FechaNacimiento);
                datos.SetearParametro("@Genero", paciente.Genero ?? (object)DBNull.Value);
                datos.SetearParametro("@Telefono", paciente.Telefono ?? (object)DBNull.Value);
                datos.SetearParametro("@Domicilio", paciente.Domicilio ?? (object)DBNull.Value);
                datos.SetearParametro("@ObraSocial", paciente.ObraSocial ?? (object)DBNull.Value);
                
                datos.SetearParametro("@Id", paciente.PacienteId);

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