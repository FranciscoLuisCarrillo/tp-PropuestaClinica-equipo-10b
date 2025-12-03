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

        // === NUEVO: validación por paciente+hora ===
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
        public List<string> HorasDisponibles(int medicoId, DateTime fecha) => datos.HorasDisponibles(medicoId, fecha);
    }
}