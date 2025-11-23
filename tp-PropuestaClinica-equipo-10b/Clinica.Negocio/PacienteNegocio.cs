using System;
using System.Collections.Generic;
using Clinica.Datos;
using Clinica.Dominio;

namespace Clinica.Negocio
{
    public class PacienteNegocio
    {
        private PacienteDatos datos;

        public PacienteNegocio()
        {
            datos = new PacienteDatos();
        }

        public List<Paciente> Listar()
        {
            // Llama al método de la capa de datos que hace el SELECT
            return datos.Listar();
        }

        /// <summary>
        /// Lógica de negocio para registrar un nuevo paciente.
        /// Devuelve el ID del paciente si tiene éxito.
        /// Lanza excepciones si la validación falla.
        /// </summary>
        public int Agregar(Paciente nuevo)
        {
            // --- INICIO DE REGLAS DE NEGOCIO (Validaciones) ---

            // 1. Validar campos requeridos
            if (nuevo == null)
                throw new ArgumentNullException("El objeto Paciente no puede ser nulo.");

            if (string.IsNullOrWhiteSpace(nuevo.Nombre) ||
                string.IsNullOrWhiteSpace(nuevo.Apellido) ||
                string.IsNullOrWhiteSpace(nuevo.Email))
               {
                        throw new ArgumentException("Nombre, Apellido y Email son campos obligatorios.");
               }

            // 2. Validar formato (ejemplo simple)
            if (!nuevo.Email.Contains("@") || !nuevo.Email.Contains("."))
            {
                throw new ArgumentException("El formato del Email no es válido.");
            }

            // IMPORTANTE: Solo validamos si existe el DNI si el usuario realmente escribió uno.
            // Si viene vacío o nulo, saltamos esta validación.
            if (!string.IsNullOrEmpty(nuevo.Dni))
            {
                if (datos.ExistePorDNI(nuevo.Dni))
                {
                    throw new Exception("El DNI ingresado ya se encuentra registrado.");
                }
            }

            if (datos.ExistePorEmail(nuevo.Email))
            {
                throw new Exception("El Email ingresado ya se encuentra registrado.");
            }
            // --- FIN DE REGLAS DE NEGOCIO ---

            // Si todas las validaciones pasan, se llama a la capa de datos para guardar.
            return datos.Agregar(nuevo);
        }
    }
}