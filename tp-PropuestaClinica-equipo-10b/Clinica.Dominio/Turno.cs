using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinica.Dominio
{

    public enum EstadoTurno
    {
        Nuevo = 0,
        Reprogramado = 1,
        Cancelado = 2,
        NoAsistio = 3,
        Cerrado = 4
    }
    public class Turno
    {
        public int IdTurno { get; set; }
        public int IdPaciente { get; set; }
        public int IdMedico { get; set; }
        public int IdEspecialidad { get; set; }
        public DateTime FechaHoraInicio { get; set; }
        public DateTime FechaHoraFin { get; set; }
        public string MotivoConsulta { get; set; }
        public string DiagnosticoMedico { get; set; }

        public EstadoTurno Estado { get; set; } // Programado, Cancelado, Realizado
        
        

        //relaciones
        public Paciente Paciente { get; set; }
        public Medico Medico { get; set; }
        public Especialidad Especialidad { get; set; }

    }
}
