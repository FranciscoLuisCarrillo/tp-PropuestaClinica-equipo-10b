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
            return datos.Listar();
        }

        public int Agregar(Paciente nuevo)
        {
            if (nuevo == null)
                throw new ArgumentNullException("El objeto Paciente no puede ser nulo.");

            if (string.IsNullOrWhiteSpace(nuevo.Nombre) ||
                string.IsNullOrWhiteSpace(nuevo.Apellido) ||
                string.IsNullOrWhiteSpace(nuevo.Dni) ||
                string.IsNullOrWhiteSpace(nuevo.Email))
            {
                throw new ArgumentException("Nombre, Apellido, DNI y Email son campos obligatorios.");
            }

            if (!nuevo.Email.Contains("@") || !nuevo.Email.Contains("."))
            {
                throw new ArgumentException("El formato del Email no es válido.");
            }

            if (datos.ExistePorDNI(nuevo.Dni))
            {
                throw new Exception("El DNI ingresado ya se encuentra registrado.");
            }

            if (datos.ExistePorEmail(nuevo.Email))
            {
                throw new Exception("El Email ingresado ya se encuentra registrado.");
            }

            return datos.Agregar(nuevo);
        }

        // ESTE ES EL MÉTODO QUE TE FALTABA
        public void Eliminar(int id)
        {
            datos.Eliminar(id);
        }
    }
}