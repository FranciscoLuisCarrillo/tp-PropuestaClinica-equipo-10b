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

        public int? IdTurnoTrabajo { get; set; }
        public string NombreTurnoTrabajo { get; set; } // Mañana, Tarde, Noche

        public List<Especialidad> Especialidades { get; set; } = new List<Especialidad>();

        public string EspecialidadTexto
        {
            get
            {
                if (Especialidades != null && Especialidades.Count > 0)
                {
                    return string.Join(", ", Especialidades.Select(e => e.Nombre));
                }
                return "Sin especialidades";
            }
        }
    }
}
