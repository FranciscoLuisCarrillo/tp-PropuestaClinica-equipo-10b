using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinica.Dominio
{
    public class TurnoResumen
    {
        public int NumeroTurno { get; set; }
        public int TurnoId { get; set; }
        public int MedicoId { get; set; }
        public DateTime Fecha { get; set; }
        public string Hora { get; set; }
        public string Paciente { get; set; }
        public string Medico { get; set; }
        public string Especialidad { get; set; }
        public string Estado { get; set; }
    }
}
