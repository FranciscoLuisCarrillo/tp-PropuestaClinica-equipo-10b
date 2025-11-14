using System;
using System.Collections.Generic;
using Clinica.Datos;
using Clinica.Dominio;

namespace Clinica.Negocio
{
    public class TurnoTrabajoNegocio
    {
        private TurnoTrabajoDatos datos;

        public TurnoTrabajoNegocio()
        {
            datos = new TurnoTrabajoDatos();
        }

        /// <summary>
        /// Llama a la capa de datos para obtener la lista completa de turnos de trabajo.
        /// </summary>
        public List<TurnoTrabajo> Listar()
        {
            // Simplemente pasa la llamada a la capa de datos
            return datos.Listar();
        }

        /// <summary>
        /// Agrega un nuevo turno de trabajo, previa validación.
        /// </summary>
        public void Agregar(TurnoTrabajo nuevo)
        {
            // --- INICIO DE REGLAS DE NEGOCIO ---

            // Validación 1: Que el nombre no esté vacío.
            if (string.IsNullOrWhiteSpace(nuevo.Nombre))
            {
                throw new ArgumentException("El nombre del turno no puede estar vacío.");
            }

            // Validación 2: Que la hora de salida sea posterior a la de entrada.
            if (nuevo.HoraSalida <= nuevo.HoraEntrada)
            {
                throw new ArgumentException("La hora de salida debe ser posterior a la hora de entrada.");
            }

            // --- FIN DE REGLAS DE NEGOCIO ---

            // Si pasó las validaciones, llama a la capa de datos para guardarlo.
            datos.Agregar(nuevo);
        }
    }
}