using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinica.Negocio
{
    public class ReservaPendiente
    {
        public int? EspecialidadId { get; set; }
        public int? MedicoId { get; set; }
        public DateTime? Fecha { get; set; }
        public string Hora { get; set; }         
        public string Observaciones { get; set; } 
    }
}
