using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinica.Dominio
{
    public class Turno
    {
        public int IdTurno { get; set; }
        public int IdPaciente { get; set; }
        public int IdMedico { get; set; }
        public int IdEspecialidad { get; set; }
        public DateTime FechaHoraInicio { get; set; }
        public DateTime FechaHoraFin { get; set; }
        public string Observaciones { get; set; }
        public string DiagnosticoMedico { get; set; }

        public string Estado { get; set; } // Programado, Cancelado, Realizado
        
        

        //relaciones
        public Paciente Paciente { get; set; }
        public Medico Medico { get; set; }
        public Especialidad Especialidad { get; set; }

    }
}
