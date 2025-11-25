using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinica.Dominio
{
    public class TurnoAgendaMedico
    {
        public int IdTurno { get; set; }
        public string Hora { get; set; }
        public string Paciente { get; set; }
        public string Medico { get; set; }

        public string ObraSocial { get; set; }
        public string Especialidad { get; set; }
        public string Estado { get; set; }
        public string Diagnostico { get; set; }
    }
}
