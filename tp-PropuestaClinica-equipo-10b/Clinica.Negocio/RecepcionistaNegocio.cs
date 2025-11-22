using Clinica.Datos;
using Clinica.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinica.Negocio
{
    public class RecepcionistaNegocio
    {
        private RecepcionistaDatos datos;
        
        public RecepcionistaNegocio()
        {
            datos = new RecepcionistaDatos();
        }
        public List<Recepcionista> Listar()
        {  
            return datos.Listar();
        }

        public int Agregar(Recepcionista nuevo)
        {
            // --- INICIO REGLAS DE NEGOCIO ---
            if (nuevo == null)
                throw new ArgumentNullException("El objeto Recepcionista no puede ser nulo.");
            if (string.IsNullOrWhiteSpace(nuevo.Email))
                throw new ArgumentException("El Email es obligatorio.");
            // Validación de duplicados
            if (datos.ExisteEmail(nuevo.Email))
                throw new Exception("El Email ingresado ya se encuentra registrado.");
            // --- FIN REGLAS DE NEGOCIO ---
            return datos.Agregar(nuevo);
        }
    }
}