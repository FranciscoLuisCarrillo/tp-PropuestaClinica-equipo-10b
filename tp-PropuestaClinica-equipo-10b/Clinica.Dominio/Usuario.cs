using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinica.Dominio
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public int? IdRecepcionista { get; set; }

        public int? IdPaciente { get; set; }
        public int? IdMedico { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }

        public string Email { get; set; }
        public string Password { get; set; }
        public string Rol { get; set; } // Admin, Medico, Recepcionista, Paciente
        public bool Activo { get; set; }
        public Perfil Perfil { get; set; }


        Persona Persona { get; set; }

    }
}
