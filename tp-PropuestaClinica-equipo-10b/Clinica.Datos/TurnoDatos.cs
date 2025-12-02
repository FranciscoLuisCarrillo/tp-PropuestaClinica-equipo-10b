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
                if (nuevo.FechaHoraInicio == DateTime.MinValue)
                {
                    throw new Exception("La fecha y hora del turno no son válidas. Verifique los campos.");
                }
                datos.SetearParametro("@PacienteId", nuevo.Paciente.PacienteId);
                datos.SetearParametro("@MedicoId", nuevo.Medico.Id);
                datos.SetearParametro("@EspecialidadId", nuevo.Especialidad.EspecialidadId);
                datos.SetearParametro("@FechaInicio", nuevo.FechaHoraInicio);
                datos.SetearParametro("@FechaFin", nuevo.FechaHoraInicio.AddHours(1));
                datos.SetearParametro("@Motivo", nuevo.MotivoConsulta ?? (object)DBNull.Value);
                datos.SetearParametro("@Estado", 0);

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

        public List<TurnoAgendaMedico> ListarPorPaciente(int pacienteId)
        {
            List<TurnoAgendaMedico> lista = new List<TurnoAgendaMedico>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = @"SELECT T.TurnoId, 
                                   T.FechaHoraInicio, 
                                   M.Apellido + ', ' + M.Nombre as MedicoNombre, 
                                   E.Nombre as Especialidad,
                                   T.Estado
                            FROM Turnos T
                            INNER JOIN Medicos M ON M.MedicoId = T.MedicoId
                            INNER JOIN Especialidades E ON E.EspecialidadId = T.EspecialidadId
                            WHERE T.PacienteId = @IdPaciente
                            ORDER BY T.FechaHoraInicio DESC";

                datos.SetearConsulta(consulta);
                datos.SetearParametro("@IdPaciente", pacienteId);
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    var turno = new TurnoAgendaMedico();
                    turno.IdTurno = (int)datos.Lector["TurnoId"];

                    DateTime fecha = (DateTime)datos.Lector["FechaHoraInicio"];
                    turno.Hora = fecha.ToString("dd/MM/yyyy HH:mm");

                    turno.Medico = datos.Lector["MedicoNombre"].ToString();
                    turno.Especialidad = datos.Lector["Especialidad"].ToString();

                    int estado = (int)datos.Lector["Estado"];
                    if (estado == 0 || estado == 1) turno.Estado = "Pendiente";
                    else if (estado == 2) turno.Estado = "Cancelado";
                    else turno.Estado = "Finalizado";

                    lista.Add(turno);
                }
                return lista;
            }
            finally { datos.CerrarConexion(); }

        }
        public bool ExisteTurnoEnHorario(int medicoId, DateTime fechaHoraInicio)
        {
            var datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta(@"
            SELECT 1
            FROM Turnos
            WHERE MedicoId = @MedicoId
              AND FechaHoraInicio = @Inicio
              AND Estado <> 2; -- 2 = Cancelado
        ");
                datos.SetearParametro("@MedicoId", medicoId);
                datos.SetearParametro("@Inicio", fechaHoraInicio);
                datos.EjecutarLectura();
                return datos.Lector.Read();
            }
            finally { datos.CerrarConexion(); }
        }
        public List<TurnoResumen> ListarResumenPorFecha(DateTime fecha)
        {
            var lista = new List<TurnoResumen>();
            var datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta(@"
            SELECT  t.TurnoId,
                    t.MedicoId,
                    CAST(t.FechaHoraInicio AS date) AS Fecha,
                    FORMAT(t.FechaHoraInicio, 'HH:mm') AS Hora,
                    CONCAT(p.Apellido, ', ', p.Nombre) AS Paciente,
                    CONCAT(m.Apellido, ', ', m.Nombre) AS Medico,
                    e.Nombre AS Especialidad,
                    t.Estado
            FROM Turnos t
            INNER JOIN Pacientes p ON p.PacienteId = t.PacienteId
            INNER JOIN Medicos   m ON m.MedicoId   = t.MedicoId
            INNER JOIN Especialidades e ON e.EspecialidadId = t.EspecialidadId
            WHERE CAST(t.FechaHoraInicio AS date) = @Fecha
            ORDER BY t.FechaHoraInicio;
        ");
                datos.SetearParametro("@Fecha", fecha.Date);
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    int estado = (int)datos.Lector["Estado"];
                    lista.Add(new TurnoResumen
                    {
                        TurnoId = (int)datos.Lector["TurnoId"],
                        MedicoId = (int)datos.Lector["MedicoId"],
                        Fecha = (DateTime)datos.Lector["Fecha"],
                        Hora = (string)datos.Lector["Hora"],
                        Paciente = (string)datos.Lector["Paciente"],
                        Medico = (string)datos.Lector["Medico"],
                        Especialidad = (string)datos.Lector["Especialidad"],
                        Estado = EstadoTurnoToTexto((EstadoTurno)estado)
                    });
                }
                return lista;
            }
            finally { datos.CerrarConexion(); }
        }

        public Turno ObtenerPorId(int idTurno)
        {
            var datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta(@"
            SELECT T.TurnoId, T.FechaHoraInicio, T.MotivoConsulta, T.DiagnosticoMedico, T.Estado,
                   T.MedicoId, T.EspecialidadId, T.PacienteId,
                   P.Nombre, P.Apellido, P.DNI
            FROM Turnos T
            INNER JOIN Pacientes P ON T.PacienteId = P.PacienteId
            WHERE T.TurnoId = @Id");
                datos.SetearParametro("@Id", idTurno);
                datos.EjecutarLectura();

                if (datos.Lector.Read())
                {
                    var turno = new Turno
                    {
                        IdTurno = (int)datos.Lector["TurnoId"],
                        FechaHoraInicio = (DateTime)datos.Lector["FechaHoraInicio"],
                        MotivoConsulta = datos.Lector["MotivoConsulta"] == DBNull.Value ? null : datos.Lector["MotivoConsulta"].ToString(),
                        DiagnosticoMedico = datos.Lector["DiagnosticoMedico"] == DBNull.Value ? null : datos.Lector["DiagnosticoMedico"].ToString(),
                        Estado = (EstadoTurno)(int)datos.Lector["Estado"],
                        Paciente = new Paciente
                        {
                            PacienteId = (int)datos.Lector["PacienteId"],
                            Nombre = datos.Lector["Nombre"].ToString(),
                            Apellido = datos.Lector["Apellido"].ToString(),
                            Dni = datos.Lector["DNI"] == DBNull.Value ? "-" : datos.Lector["DNI"].ToString()
                        },
                        Medico = new Medico { Id = (int)datos.Lector["MedicoId"] },
                        Especialidad = new Especialidad { EspecialidadId = (int)datos.Lector["EspecialidadId"] }
                    };
                    return turno;
                }
                return null;
            }
            finally { datos.CerrarConexion(); }
        }
        public int ContarTurnosHoy()
        {
            var datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta(@"
                    SELECT COUNT(*)
                    FROM Turnos
                    WHERE FechaHoraInicio >= @Hoy
                      AND FechaHoraInicio < @Maniana;");
                var hoy = DateTime.Today;
                datos.SetearParametro("@Hoy", hoy);
                datos.SetearParametro("@Maniana", hoy.AddDays(1));

                object r = datos.EjecutarEscalar();
                return Convert.ToInt32(r);
            }
            finally { datos.CerrarConexion(); }
        }

        
        public List<TurnoResumen> ListarUltimosResumen(int top)
        {
            var lista = new List<TurnoResumen>();
            var datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta(@"
                    SELECT TOP (@TopN)
                        t.TurnoId                                      AS NumeroTurno,
                        CAST(t.FechaHoraInicio AS date)                AS Fecha,
                        FORMAT(t.FechaHoraInicio, 'HH:mm')             AS Hora,
                        CONCAT(p.Apellido, ', ', p.Nombre)            AS Paciente,
                        CONCAT(m.Apellido, ', ', m.Nombre)            AS Medico,
                        e.Nombre                                       AS Especialidad,
                        t.Estado                                       AS Estado
                    FROM Turnos t
                    INNER JOIN Pacientes p       ON p.PacienteId      = t.PacienteId
                    INNER JOIN Medicos m         ON m.MedicoId        = t.MedicoId
                    INNER JOIN Especialidades e  ON e.EspecialidadId  = t.EspecialidadId
                    ORDER BY t.FechaHoraInicio DESC;");
                datos.SetearParametro("@TopN", top);
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    int estado = (int)datos.Lector["Estado"];
                    lista.Add(new TurnoResumen
                    {
                        NumeroTurno = (int)datos.Lector["NumeroTurno"],
                        Fecha = (DateTime)datos.Lector["Fecha"],
                        Hora = (string)datos.Lector["Hora"],
                        Paciente = (string)datos.Lector["Paciente"],
                        Medico = (string)datos.Lector["Medico"],
                        Especialidad = (string)datos.Lector["Especialidad"],
                        Estado = EstadoTurnoToTexto((EstadoTurno)estado)
                    });
                }
                return lista;
            }
            finally { datos.CerrarConexion(); }
        }

        public List<TurnoResumen> ListarPorFecha(DateTime fecha)
        {
            var lista = new List<TurnoResumen>();
            var datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta(@"
            SELECT  t.TurnoId,
                    t.FechaHoraInicio,
                    CONCAT(p.Apellido, ', ', p.Nombre)   AS Paciente,
                    CONCAT(m.Apellido, ', ', m.Nombre)   AS Medico,
                    e.Nombre                             AS Especialidad,
                    t.Estado
            FROM Turnos t
            INNER JOIN Pacientes p      ON p.PacienteId     = t.PacienteId
            INNER JOIN Medicos m        ON m.MedicoId       = t.MedicoId
            INNER JOIN Especialidades e ON e.EspecialidadId = t.EspecialidadId
            WHERE CAST(t.FechaHoraInicio AS date) = @F
            ORDER BY t.FechaHoraInicio ASC;");
                datos.SetearParametro("@F", fecha.Date);
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    var fh = (DateTime)datos.Lector["FechaHoraInicio"];
                    var estInt = (int)datos.Lector["Estado"];

                    var item = new TurnoResumen
                    {
                        NumeroTurno = (int)datos.Lector["TurnoId"], 
                        TurnoId = (int)datos.Lector["TurnoId"],   
                        Fecha = fh.Date,
                        Hora = fh.ToString("HH:mm"),
                        Paciente = (string)datos.Lector["Paciente"],
                        Medico = (string)datos.Lector["Medico"],
                        Especialidad = (string)datos.Lector["Especialidad"],
                        Estado = EstadoTurnoToTexto((EstadoTurno)estInt)
                    };
                    lista.Add(item);
                }
                return lista;
            }
            finally { datos.CerrarConexion(); }
        }

        // Reprogramar (actualiza fecha/hora)
        public void ReprogramarFecha(int idTurno, DateTime nuevaFechaHora)
        {
            var datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta(@"
            UPDATE Turnos
            SET FechaHoraInicio = @Nueva,
                FechaHoraFin    = DATEADD(hour, 1, @Nueva),
                Estado          = 1  -- Reprogramado
            WHERE TurnoId = @Id;");
                datos.SetearParametro("@Nueva", nuevaFechaHora);
                datos.SetearParametro("@Id", idTurno);
                datos.EjecutarAccion();
            }
            finally { datos.CerrarConexion(); }
        }

        // Cancelar (marca estado = 2)
        public void Cancelar(int idTurno)
        {
            var datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta(@"UPDATE Turnos SET Estado = 2 WHERE TurnoId = @Id;");
                datos.SetearParametro("@Id", idTurno);
                datos.EjecutarAccion();
            }
            finally { datos.CerrarConexion(); }
        }

        private List<string> HorasOcupadas(int medicoId, DateTime fecha)
        {
            var ocupadas = new List<string>();
            var datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta(@"
            SELECT FORMAT(FechaHoraInicio, 'HH:mm') AS Hora
            FROM Turnos
            WHERE MedicoId = @M
              AND CAST(FechaHoraInicio AS date) = @F
              AND Estado <> 2 -- excluir cancelados");
                datos.SetearParametro("@M", medicoId);
                datos.SetearParametro("@F", fecha.Date);
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                    ocupadas.Add((string)datos.Lector["Hora"]);

                return ocupadas;
            }
            finally { datos.CerrarConexion(); }
        }

        // Horas disponibles (08:00–18:00 cada 30’) menos ocupadas
        public List<string> HorasDisponibles(int medicoId, DateTime fecha)
        {
            var ocupadas = new HashSet<string>(HorasOcupadas(medicoId, fecha));
            var libres = new List<string>();

            var inicio = new DateTime(fecha.Year, fecha.Month, fecha.Day, 8, 0, 0);
            var fin = new DateTime(fecha.Year, fecha.Month, fecha.Day, 18, 0, 0);

            for (var h = inicio; h <= fin; h = h.AddMinutes(30))
            {
                var hhmm = h.ToString("HH:mm");
                if (!ocupadas.Contains(hhmm))
                    libres.Add(hhmm);
            }
            return libres;
        }

        private static string EstadoTurnoToTexto(EstadoTurno e)
        {
            switch (e)
            {
                case EstadoTurno.Nuevo: return "Nuevo";
                case EstadoTurno.Reprogramado: return "Reprogramado";
                case EstadoTurno.Cancelado: return "Cancelado";
                case EstadoTurno.NoAsistio: return "No asistió";
                case EstadoTurno.Cerrado: return "Cerrado";
                default: return "Desconocido";
            }
        }

    }
}