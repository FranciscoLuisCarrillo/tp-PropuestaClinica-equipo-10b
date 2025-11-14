using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinica.Dominio
{
    public class TurnoTrabajo

    {
        public int TurnoTrabajoId { get; set; }
        public string Nombre { get; set; }

        /// <summary>
        /// TimeSpan tra el tipo TIME de SQL Server
        /// </summary>
        public TimeSpan HoraEntrada { get; set; }
        public TimeSpan HoraSalida { get; set; }
        public override string ToString()
        {
            return $"{Nombre} ({HoraEntrada:hh\\:mm} - {HoraSalida:hh\\:mm})";
        }

    }
}
