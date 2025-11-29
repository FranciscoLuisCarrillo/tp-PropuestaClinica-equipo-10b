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

        public List<Medico> Listar()
        {
            return datos.Listar();
        }

        public int Agregar(Medico nuevo)
        {
            if (nuevo == null)
                throw new ArgumentNullException("El objeto Médico no puede ser nulo.");

            if (string.IsNullOrWhiteSpace(nuevo.Matricula))
                throw new ArgumentException("La Matrícula es obligatoria.");

            if (nuevo.Turno == null || nuevo.Turno.TurnoTrabajoId == 0)
                throw new ArgumentException("Debe seleccionar un Turno de Trabajo.");

            if (nuevo.Especialidades == null || nuevo.Especialidades.Count == 0)
                throw new ArgumentException("Debe seleccionar al menos una Especialidad.");

            if (datos.ExistePorMatricula(nuevo.Matricula))
                throw new Exception("La Matrícula ingresada ya existe.");

            if (datos.ExistePorEmail(nuevo.Email))
                throw new Exception("El Email ingresado ya existe (puede ser de un Paciente o Médico).");

            return datos.Agregar(nuevo);
        }

        public List<Medico> ListarPorEspecialidad(int especialidadId)
        {
            return datos.ListarPorEspecialidad(especialidadId);
        }
    }
}