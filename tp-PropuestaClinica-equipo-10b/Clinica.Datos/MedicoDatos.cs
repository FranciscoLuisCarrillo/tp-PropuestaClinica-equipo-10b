using System;
using System.Collections.Generic;
using Clinica.Dominio;

namespace Clinica.Datos
{
    public class MedicoDatos
    {
        /// <summary>
        /// Lista los datos básicos de los médicos (sin JOIN).
        /// </summary>
        public List<Medico> Listar()
        {
            List<Medico> lista = new List<Medico>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                // Consulta simple, solo a la tabla Medicos.
                string consulta = @"SELECT
                                    M.MedicoId, M.Nombre, M.Apellido, M.Matricula, M.Email, M.Telefono,
                                    M.TurnoTrabajoId
                                FROM Medicos M";

                datos.SetearConsulta(consulta);
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Medico aux = new Medico();
                    aux.Id = (int)datos.Lector["MedicoId"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Apellido = (string)datos.Lector["Apellido"];
                    aux.Matricula = (string)datos.Lector["Matricula"];
                    aux.Email = (string)datos.Lector["Email"];
                    aux.Telefono = (string)(datos.Lector["Telefono"] ?? DBNull.Value);
                    /*
                    // Solo guardamos el ID del turno (no cargamos el objeto completo)
                    
                    aux.Turno = new TurnoTrabajo();
                    if (datos.Lector["TurnoTrabajoId"] != DBNull.Value)
                    {
                        aux.Turno.TurnoTrabajoId = (int)datos.Lector["TurnoTrabajoId"];
                    }
                    */
                    if(datos.Lector["TurnoTrabajoId"] != DBNull.Value)
                    {
                        aux.IdTurnoTrabajo = (int)datos.Lector["TurnoTrabajoId"];
                    }

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
        /// Guarda el médico solo en la tabla 'Medicos'.
        /// Devuelve el ID del médico generado.
        /// </summary>
        public int Agregar(Medico nuevo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                // --- PASO 1: Guardar el Médico en la tabla Medicos ---
                string consulta = @"INSERT INTO Medicos 
                                (Nombre, Apellido, Matricula, Email, Telefono, TurnoTrabajoId) 
                                VALUES 
                                (@Nombre, @Apellido, @Matricula, @Email, @Telefono, @TurnoTrabajoId);
                                SELECT SCOPE_IDENTITY();"; // Devuelve el ID generado

                datos.SetearConsulta(consulta);
                datos.SetearParametro("@Nombre", nuevo.Nombre);
                datos.SetearParametro("@Apellido", nuevo.Apellido);
                datos.SetearParametro("@Matricula", nuevo.Matricula);
                datos.SetearParametro("@Email", nuevo.Email);
                datos.SetearParametro("@Telefono", (object)nuevo.Telefono ?? DBNull.Value);
                datos.SetearParametro("@TurnoTrabajoId",
                (object)nuevo.IdTurnoTrabajo ?? DBNull.Value);

                // Ejecutamos y obtenemos el nuevo ID
                int idMedicoGenerado = Convert.ToInt32(datos.EjecutarEscalar());

                // --- PASO 2 (Eliminado) ---
                // Se quitó la lógica de guardar las especialidades para simplificar.

                return idMedicoGenerado;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar médico en la base de datos.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        // --- Métodos de Validación (Necesarios para Negocio) ---

        public bool ExistePorMatricula(string matricula)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("SELECT 1 FROM Medicos WHERE Matricula COLLATE Latin1_General_CI_AI = @Matricula");
                datos.SetearParametro("@Matricula", matricula);
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
                datos.SetearConsulta("SELECT 1 FROM Medicos WHERE Email COLLATE Latin1_General_CI_AI = @Email");
                datos.SetearParametro("@Email", email);
                datos.EjecutarLectura();
                return datos.Lector.Read();
            }
            finally { datos.CerrarConexion(); }
        }
    }
}