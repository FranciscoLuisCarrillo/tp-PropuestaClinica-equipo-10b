using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinica.Dominio
{
    public class Medico : Persona
    {
        string Matricula { get; set; }
        string Especialidad { get; set; }
    }
}
