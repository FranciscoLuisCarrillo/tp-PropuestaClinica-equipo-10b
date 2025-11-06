using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinica.Dominio
{
    public class Persona
    {
        // DATOS PERSONALES, LEY DE PROTECCION DE DATOS. BORRARLOS SIEMPRE despues de usarlos

        int Id { get; set; }
        string Nombre { get; set; }
        string Apellido { get; set; }
        DateTime FechaNacimiento { get; set; }
        string Genero { get; set; }
        string Dni { get; set; }
        string Direccion { get; set; }
        string Telefono { get; set; }
        string Email { get; set; }


    }
}
