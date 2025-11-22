using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinica.Dominio
{
    public class Medico
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }

        public string Matricula { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }

        public int? TurnoTrabajoId { get; set; }
        public string NombreTurnoTrabajo { get; set; } = "-"; // Mañana, Tarde, Noche
            
       public TurnoTrabajo Turno { get; set; }
       public  bool Activo { get; set; }

        public List<Especialidad> Especialidades { get; set; } = new List<Especialidad>();

        public string EspecialidadTexto =>
          Especialidades != null && Especialidades.Count > 0
          ? string.Join(", ", Especialidades.Select(e => e.Nombre))
          : "-";

       
    }
}
