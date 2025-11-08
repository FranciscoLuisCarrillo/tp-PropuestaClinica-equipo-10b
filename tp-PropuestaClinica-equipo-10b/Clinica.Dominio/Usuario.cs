using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinica.Dominio
{
    public class Usuario
    {
        int IdUsuario { get; set; }
        int? I { get; set; }
        string Email { get; set; }
        string Password { get; set; }
        string Rol { get; set; } // Admin, Medico, Recepcionista, Paciente
        bool Activo { get; set; }
        

        Persona Persona { get; set; }

    }
}
