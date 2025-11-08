using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinica.Dominio
{
    public class Turno
    {
        int IdTurno { get; set; }
        int IdPaciente { get; set; }
        int IdMedico { get; set; }
        int IdEspecialidad { get; set; }
        DateTime FechaHora { get; set; }
        string Estado { get; set; } // Programado, Cancelado, Realizado
        string Observaciones { get; set; }
        

        //relaciones
        Paciente Paciente { get; set; }
        Medico Medico { get; set; }
        string Especialidad { get; set; }

    }
}
