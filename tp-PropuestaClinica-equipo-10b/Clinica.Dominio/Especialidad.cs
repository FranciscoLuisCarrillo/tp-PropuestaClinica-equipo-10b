using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinica.Dominio
{
    // Clase que representa la entidad Especialidad.
    public class Especialidad
    {
        public int EspecialidadId { get; set; }
        public string Nombre { get; set; }
        public bool Activa { get; set; }

    }
}
