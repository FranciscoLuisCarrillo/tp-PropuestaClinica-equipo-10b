using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clinica.Dominio;

namespace Clinica.Datos
{
    public class TurnoDatos
    {

        public void Agregar(Turno nuevo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = @"INSERT INTO Turnos (PacienteId, MedicoId, EspecialidadId, FechaHoraInicio, FechaHoraFin, MotivoConsulta, Estado)
                            VALUES (@PacienteId, @MedicoId, @EspecialidadId, @FechaInicio, @FechaFin, @Motivo, @Estado)";

                datos.SetearConsulta(consulta);
                datos.SetearParametro("@PacienteId", nuevo.Paciente.PacienteId);
                datos.SetearParametro("@MedicoId", nuevo.Medico.Id);
                datos.SetearParametro("@EspecialidadId", nuevo.Especialidad.EspecialidadId);
                datos.SetearParametro("@FechaInicio", nuevo.FechaHoraInicio);
                datos.SetearParametro("@FechaFin", nuevo.FechaHoraInicio.AddHours(1));
                datos.SetearParametro("@Motivo", nuevo.MotivoConsulta ?? (object)DBNull.Value);
                datos.SetearParametro("@Estado", 0); // 0 = Nuevo

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

        public List<TurnoAgendaMedico> ListarAgendaPorMedicoYFecha(int medicoId, DateTime fecha)
        {
            List<TurnoAgendaMedico> lista = new List<TurnoAgendaMedico>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                string consulta = @"
                    SELECT 
                        T.TurnoId,
                        CONVERT(VARCHAR(5), T.FechaHoraInicio, 108) AS Hora,
                        P.Nombre + ' ' + P.Apellido AS Paciente,
                         P.ObraSocial AS ObraSocial, 
                        E.Nombre AS Especialidad,
                        T.Estado,
                        CASE T.Estado
                            WHEN 0 THEN 'Nuevo'
                            WHEN 1 THEN 'Reprogramado'
                            WHEN 2 THEN 'Cancelado'
                            WHEN 3 THEN 'No Asistió'
                            WHEN 4 THEN 'Cerrado'
                            ELSE 'Desconocido'
                        END AS EstadoTexto,
                        T.DiagnosticoMedico
                    FROM Turnos T
                    INNER JOIN Pacientes P ON P.PacienteId = T.PacienteId
                    INNER JOIN Especialidades E ON E.EspecialidadId = T.EspecialidadId
                    WHERE T.MedicoId = @MedicoId
                      AND CAST(T.FechaHoraInicio AS DATE) = @Fecha
                    ORDER BY T.FechaHoraInicio;";

                datos.SetearConsulta(consulta);
                datos.SetearParametro("@MedicoId", medicoId);
                datos.SetearParametro("@Fecha", fecha.Date);
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    var t = new TurnoAgendaMedico
                    {
                        IdTurno = (int)datos.Lector["TurnoId"],
                        Hora = datos.Lector["Hora"].ToString(),
                        Paciente = datos.Lector["Paciente"].ToString(),
                        ObraSocial = datos.Lector["ObraSocial"].ToString(),
                        Especialidad = datos.Lector["Especialidad"].ToString(),
                        Estado = datos.Lector["EstadoTexto"].ToString(),
                        Diagnostico = datos.Lector["DiagnosticoMedico"] == DBNull.Value
                            ? ""
                            : datos.Lector["DiagnosticoMedico"].ToString()
                    };

                    lista.Add(t);
                }

                return lista;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }



        public void ActualizarEstadoYDiagnostico(int idTurno, int estadoValor, string diagnostico)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = @"
                    UPDATE Turnos
                    SET Estado = @Estado,
                        DiagnosticoMedico = @DiagnosticoMedico
                    WHERE TurnoId = @IdTurno";

                datos.SetearConsulta(consulta);
                datos.SetearParametro("@Estado", estadoValor);
                datos.SetearParametro("@DiagnosticoMedico",
                    string.IsNullOrWhiteSpace(diagnostico) ? (object)DBNull.Value : diagnostico);
                datos.SetearParametro("@IdTurno", idTurno);

                datos.EjecutarAccion();
            }
            finally
            {
                datos.CerrarConexion();
            }
        }
    }
}