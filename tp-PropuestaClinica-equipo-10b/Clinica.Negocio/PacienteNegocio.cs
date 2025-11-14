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
                string.IsNullOrWhiteSpace(nuevo.Dni) ||
                string.IsNullOrWhiteSpace(nuevo.Email))
            {
                throw new ArgumentException("Nombre, Apellido, DNI y Email son campos obligatorios.");
            }

            // 2. Validar formato (ejemplo simple)
            if (!nuevo.Email.Contains("@") || !nuevo.Email.Contains("."))
            {
                throw new ArgumentException("El formato del Email no es válido.");
            }

            // 3. Validar duplicados (Regla de negocio CRÍTICA para un registro)
            if (datos.ExistePorDNI(nuevo.Dni))
            {
                // Usamos Exception (no ArgumentException) porque es una regla de negocio, no un error de formato.
                throw new Exception("El DNI ingresado ya se encuentra registrado.");
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