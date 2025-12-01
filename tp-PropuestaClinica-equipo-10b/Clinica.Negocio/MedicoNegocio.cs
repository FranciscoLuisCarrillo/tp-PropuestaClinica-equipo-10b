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
            if (nuevo == null) throw new ArgumentNullException("Médico nulo.");

            if (string.IsNullOrWhiteSpace(nuevo.Matricula))
                throw new ArgumentException("La Matrícula es obligatoria.");

            if (!nuevo.TurnoTrabajoId.HasValue)
                throw new ArgumentException("Debe seleccionar un Turno de Trabajo.");

            if (nuevo.Especialidades == null || nuevo.Especialidades.Count == 0)
                throw new ArgumentException("Debe seleccionar al menos una Especialidad.");

            if (nuevo.Especialidades.Count > 2)
                throw new ArgumentException("Solo puede seleccionar hasta 2 especialidades.");

            if (datos.ExistePorMatricula(nuevo.Matricula))
                throw new Exception("La Matrícula ingresada ya existe.");

            if (datos.ExistePorEmail(nuevo.Email))
                throw new Exception("El Email ingresado ya existe.");

            return datos.Agregar(nuevo);
        }
        public List<Medico> ListarPorEspecialidad(int especialidadId)
        {
            return datos.ListarPorEspecialidad(especialidadId);
        }
        public Medico ObtenerPorId(int id) => datos.ObtenerPorId(id);

        public void Modificar(Medico m)
        {
            if (m == null || m.Id <= 0) throw new Exception("Médico inválido.");
            if (string.IsNullOrWhiteSpace(m.Nombre) || string.IsNullOrWhiteSpace(m.Apellido))
                throw new Exception("Nombre y Apellido son obligatorios.");
            if (string.IsNullOrWhiteSpace(m.Email)) throw new Exception("Email obligatorio.");
            if (string.IsNullOrWhiteSpace(m.Matricula)) throw new Exception("Matrícula obligatoria.");
            if (m.Especialidades == null || m.Especialidades.Count == 0)
                throw new Exception("Debe seleccionar al menos una especialidad.");
            if (m.Especialidades.Count > 2)
                throw new Exception("Solo puede seleccionar hasta 2 especialidades.");

            if (datos.EmailEnUso(m.Email, m.Id)) throw new Exception("El email ya está en uso por otro médico.");
            if (datos.MatriculaEnUso(m.Matricula, m.Id)) throw new Exception("La matrícula ya está en uso por otro médico.");

            datos.Modificar(m);

            
        }
    }
}