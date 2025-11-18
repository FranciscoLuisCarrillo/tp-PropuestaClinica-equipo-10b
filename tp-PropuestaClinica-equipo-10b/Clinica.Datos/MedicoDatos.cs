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
                string consulta = @"SELECT M.MedicoId, M.Nombre, M.Apellido, M.Matricula, M.Email, M.Telefono,
                                  M.TurnoTrabajoId,
                                  T.Nombre AS TurnoNombre
                                  FROM Medicos M
                                  LEFT JOIN TurnosTrabajo T ON T.TurnoTrabajoId = M.TurnoTrabajoId";

                datos.SetearConsulta(consulta);
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Medico aux = new Medico();

                    aux.Id = (int)datos.Lector["MedicoId"];
                    aux.Nombre = datos.Lector["Nombre"] == DBNull.Value ? "" : datos.Lector["Nombre"].ToString();
                    aux.Apellido = datos.Lector["Apellido"] == DBNull.Value ? "" : datos.Lector["Apellido"].ToString();
                    aux.Matricula = datos.Lector["Matricula"] == DBNull.Value ? "" : datos.Lector["Matricula"].ToString();
                    aux.Email = datos.Lector["Email"] == DBNull.Value ? "" : datos.Lector["Email"].ToString();

                    aux.Telefono = datos.Lector["Telefono"] == DBNull.Value
                        ? null
                        : datos.Lector["Telefono"].ToString();

                    aux.IdTurnoTrabajo = datos.Lector["TurnoTrabajoId"] == DBNull.Value
                        ? (int?)null
                        : Convert.ToInt32(datos.Lector["TurnoTrabajoId"]);

                    aux.NombreTurnoTrabajo = datos.Lector["TurnoNombre"] == DBNull.Value
                        ? "-"
                        : datos.Lector["TurnoNombre"].ToString();
                    

                    aux.Especialidades = ObtenerEspecialidadesPorMedico(aux.Id);

                    aux.Activo = datos.Lector["Activo"] == DBNull.Value ? true : (bool)datos.Lector["Activo"];
                    /*
                    // Solo guardamos el ID del turno (no cargamos el objeto completo)
                    
                    aux.Turno = new TurnoTrabajo();
                    if (datos.Lector["TurnoTrabajoId"] != DBNull.Value)
                    {
                        aux.Turno.TurnoTrabajoId = (int)datos.Lector["TurnoTrabajoId"];
                    }
                    */


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

        private List<Especialidad> ObtenerEspecialidadesPorMedico(int medicoId)
        {
            List<Especialidad> especialidades = new List<Especialidad>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = @"SELECT E.EspecialidadId, E.Nombre
                                    FROM MedicoEspecialidades ME
                                    INNER JOIN Especialidades E ON E.EspecialidadId = ME.EspecialidadId
                                    WHERE ME.MedicoId = @MedicoId";
                datos.SetearConsulta(consulta);
                datos.SetearParametro("@MedicoId", medicoId);
                datos.EjecutarLectura();
                while (datos.Lector.Read())
                {
                    Especialidad esp = new Especialidad
                    {
                        EspecialidadId = (int)datos.Lector["EspecialidadId"],
                        Nombre = datos.Lector["Nombre"].ToString()
                    };
                    especialidades.Add(esp);
                }
                return especialidades;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener especialidades del médico.", ex);
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
                datos.CerrarConexion();

                if(nuevo.Especialidades != null && nuevo.Especialidades.Count > 0)
                {
                    foreach(var especialidad in nuevo.Especialidades)
                    {
                        AccesoDatos datosEspecialidad = new AccesoDatos();
                        datosEspecialidad.SetearConsulta(@"INSERT INTO Medico_Especialidades (MedicoId, EspecialidadId) 
                                                        VALUES (@MedicoId, @EspecialidadId)");
                        datosEspecialidad.SetearParametro("@MedicoId", idMedicoGenerado);
                        datosEspecialidad.SetearParametro("@EspecialidadId", especialidad.EspecialidadId);
                        datosEspecialidad.EjecutarAccion();
                        datosEspecialidad.CerrarConexion();
                    }
                }

                return idMedicoGenerado;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar médico en la base de datos.", ex);
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