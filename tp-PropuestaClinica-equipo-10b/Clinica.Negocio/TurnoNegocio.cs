using Clinica.Datos;
using Clinica.Dominio;
using System;
using System.Collections.Generic;
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
        public void AgendarTurno(Turno nuevo)
        {
            datos.Agregar(nuevo);
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
    
    public List<TurnoAgendaMedico> ListarPorPaciente(int pacienteId)
        {
           
            return datos.ListarPorPaciente(pacienteId);
        }

        public Turno ObtenerPorId(int turnoId)
        {
            return datos.ObtenerPorId(turnoId);
        }

        }
    }