using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinica.Dominio
{
    public class Medico : Persona
    {
        public string Matricula { get; set; }

        public int? TurnoTrabajoId { get; set; }
        public string NombreTurnoTrabajo { get; set; } = "-"; // Mañana, Tarde, Noche
            
       // public TurnoTrabajo Turno { get; set; }


        public List<Especialidad> Especialidades { get; set; } = new List<Especialidad>();

        public string EspecialidadTexto =>
          Especialidades != null && Especialidades.Count > 0
          ? string.Join(", ", Especialidades.Select(e => e.Nombre))
          : "-";

       
    }
}
