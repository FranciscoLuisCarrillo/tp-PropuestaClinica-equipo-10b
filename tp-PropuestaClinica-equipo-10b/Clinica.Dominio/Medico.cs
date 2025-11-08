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

        public int IdEspecialidad { get; set; }
        public int? IdTurnoTrabajo { get; set; }
        public string NombreEspecialidad { get; set; }
        public string NombreTurnoTrabajo { get; set; } // Mañana, Tarde, Noche
    }
}
