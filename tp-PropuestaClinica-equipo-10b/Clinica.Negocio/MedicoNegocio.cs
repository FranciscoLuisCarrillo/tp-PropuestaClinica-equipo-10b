using System;
using System.Collections.Generic;
using Clinica.Datos;
using Clinica.Dominio;

namespace Clinica.Negocio
{
    public class MedicoNegocio
    {
        private MedicoDatos datos;

        public MedicoNegocio()
        {
            datos = new MedicoDatos();
        }

        /// <summary>
        /// Llama a la capa de datos para obtener el listado de médicos.
        /// </summary>
        public List<Medico> Listar()
        {
            return datos.Listar();
        }

        /// <summary>
        /// Lógica de negocio para agregar un nuevo médico.
        /// </summary>
        public int Agregar(Medico nuevo)
        {
            // --- INICIO DE REGLAS DE NEGOCIO (Validaciones) ---

            if (nuevo == null)
                throw new ArgumentNullException("El objeto Médico no puede ser nulo.");

            if (string.IsNullOrWhiteSpace(nuevo.Matricula))
                throw new ArgumentException("La Matrícula es obligatoria.");

            if (nuevo.Turno == null || nuevo.Turno.TurnoTrabajoId == 0)
              throw new ArgumentException("Debe seleccionar un Turno de Trabajo.");

            // --- Regla de Especialidades (Eliminada) ---
             if (nuevo.Especialidades == null || nuevo.Especialidades.Count == 0)
               throw new ArgumentException("Debe seleccionar al menos una Especialidad.");

            // Validar duplicados contra la DB
            if (datos.ExistePorMatricula(nuevo.Matricula))
                throw new Exception("La Matrícula ingresada ya existe.");

            if (datos.ExistePorEmail(nuevo.Email))
                throw new Exception("El Email ingresado ya existe (puede ser de un Paciente o Médico).");

            // --- FIN DE REGLAS DE NEGOCIA ---

            // Llama a la versión simple de Agregar (solo guarda en tabla Medicos)
            return datos.Agregar(nuevo);
        }
    }
}