using Clinica.Datos;
using Clinica.Dominio;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinica.Negocio
{
    public class TurnoNegocio
    {
        private readonly TurnoDatos datos = new TurnoDatos();

        public List<TurnoAgendaMedico> ListarAgendaMedico(int medicoId, DateTime fecha)
        {
            if (medicoId <= 0)
                throw new ArgumentException("El médico no es válido.");

            return datos.ListarAgendaPorMedicoYFecha(medicoId, fecha.Date);
        }

        public void GuardarDiagnostico(int idTurno, string estadoTexto, string diagnostico)
        {
            if (string.IsNullOrWhiteSpace(estadoTexto))
                throw new ArgumentException("Debe seleccionar un estado.");

            int estadoValor = EstadoTextoAInt(estadoTexto);
            datos.ActualizarEstadoYDiagnostico(idTurno, estadoValor, diagnostico);
        }

        public bool ExisteTurnoPacienteEnHorario(int pacienteId, DateTime fechaHoraInicio)
            => datos.ExisteTurnoPacienteEnHorario(pacienteId, fechaHoraInicio);

        public bool ExisteTurnoEnHorario(int medicoId, DateTime fechaHoraInicio)
            => datos.ExisteTurnoEnHorario(medicoId, fechaHoraInicio);

        public void AgendarTurno(Turno nuevo)
        {

            if (nuevo == null) throw new ArgumentNullException(nameof(nuevo));
            if (nuevo.Paciente?.PacienteId <= 0) throw new ArgumentException("Paciente inválido.");
            if (nuevo.Medico?.Id <= 0) throw new ArgumentException("Médico inválido.");
            if (nuevo.FechaHoraInicio == default) throw new ArgumentException("Fecha/hora inválida.");


            if (ExisteTurnoPacienteEnHorario(nuevo.Paciente.PacienteId, nuevo.FechaHoraInicio))
                throw new InvalidOperationException("Ya tenés un turno reservado en ese horario.");


            if (ExisteTurnoEnHorario(nuevo.Medico.Id, nuevo.FechaHoraInicio))
                throw new InvalidOperationException("Ese horario ya está ocupado para el médico.");

            try
            {
                datos.Agregar(nuevo);
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {

                throw new InvalidOperationException("Conflicto de horario: ya existe un turno en ese horario.", ex);
            }
        }

        public void CancelarTurno(int idTurno)
        {
            datos.ActualizarEstadoYDiagnostico(idTurno, 2, "");
        }

        public void ReprogramarTurnoCreandoNuevo(int idTurnoAnterior, Turno nuevoTurno)
        {
            if (ExisteTurnoPacienteEnHorario(nuevoTurno.Paciente.PacienteId, nuevoTurno.FechaHoraInicio))
                throw new InvalidOperationException("El paciente ya tiene un turno en ese horario.");
            if (ExisteTurnoEnHorario(nuevoTurno.Medico.Id, nuevoTurno.FechaHoraInicio))
                throw new InvalidOperationException("El médico ya tiene un turno en ese horario.");

            try
            {
                datos.ActualizarEstadoYDiagnostico(idTurnoAnterior, 1, "");
                datos.Agregar(nuevoTurno);
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601)
            {
                throw new InvalidOperationException("Conflicto de horario al reprogramar.", ex);
            }
        }

        public void ReprogramarTurno(int idTurno, DateTime nuevaFechaHora, int medicoId)
        {
            var actual = datos.ObtenerPorId(idTurno);
            if (actual == null)
                throw new InvalidOperationException("No se encontró el turno.");

            if (actual.Estado == EstadoTurno.Cerrado || actual.Estado == EstadoTurno.Cancelado)
                throw new InvalidOperationException("No se puede reprogramar un turno cerrado o cancelado.");

            if (ExisteTurnoEnHorario(medicoId, nuevaFechaHora))
                throw new InvalidOperationException("El médico ya tiene un turno en ese horario.");

            datos.ReprogramarFecha(idTurno, nuevaFechaHora);
        }
        private int EstadoTextoAInt(string estado)
        {
            switch (estado)
            {
                case "Nuevo": return 0;
                case "Reprogramado": return 1;
                case "Cancelado": return 2;
                case "No Asistio": return 3;
                case "Cerrado": return 4;
                default: return 0;
            }
        }

        public List<TurnoAgendaMedico> ListarPorPaciente(int pacienteId) => datos.ListarPorPaciente(pacienteId);
        public Turno ObtenerPorId(int turnoId) => datos.ObtenerPorId(turnoId);
        public int ContarTurnosHoy() => datos.ContarTurnosHoy();
        public List<TurnoResumen> ListarUltimosResumen(int top) => datos.ListarUltimosResumen(top);
        public List<TurnoResumen> ListarResumenPorFecha(DateTime fecha) => datos.ListarResumenPorFecha(fecha);

        // --- MODIFICACIÓN: Buscamos al médico para saber su turno antes de consultar disponibles ---
        public List<string> HorasDisponibles(int medicoId, DateTime fecha)
        {
            // 1. Obtenemos datos del médico para saber su Turno de Trabajo
            MedicoNegocio medicoNegocio = new MedicoNegocio();
            Medico medico = medicoNegocio.ObtenerPorId(medicoId);

            if (medico != null && medico.TurnoTrabajoId.HasValue)
            {
                // Para obtener los horarios exactos de entrada/salida necesitamos la entidad TurnoTrabajo
                // Pero Medico.cs ya tiene "Turno" como propiedad de navegación si se cargó, 
                // o podemos usar los valores si cargamos TurnoTrabajo completo.
                // Asumiendo que MedicoDatos.ObtenerPorId trae los datos del join (lo cual hace),
                // pero necesitamos confirmar que trae HoraEntrada/Salida.
                // En MedicoDatos.ObtenerPorId actualmente trae "TurnoNombre", pero NO HoraEntrada/Salida.

                // Opción rápida: Usar TurnoTrabajoNegocio para obtener el detalle del turno
                TurnoTrabajoNegocio ttNegocio = new TurnoTrabajoNegocio();
                // Necesitamos un método ObtenerPorId en TurnoTrabajoNegocio, o filtramos la lista (menos eficiente pero sirve)
                var listaTurnos = ttNegocio.Listar();
                var turnoTrabajo = listaTurnos.Find(t => t.TurnoTrabajoId == medico.TurnoTrabajoId.Value);

                if (turnoTrabajo != null)
                {
                    return datos.HorasDisponibles(medicoId, fecha, turnoTrabajo.HoraEntrada, turnoTrabajo.HoraSalida);
                }
            }

            // Si no tiene turno asignado o no se encuentra, retornamos lista vacía o default 0-23 (fallback)
            // Para mantener consistencia con lo anterior, si falla usamos horario full, pero idealmente debería ser vacío.
            return new List<string>();
        }
    }
}