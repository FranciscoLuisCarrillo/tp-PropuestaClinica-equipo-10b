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
           

            return datos.Agregar(nuevo);
        }

        public void Eliminar(int id)
        {
            datos.Eliminar(id);
        }

        public Paciente ObtenerPorId(int id)
        {
            return datos.ObtenerPorId(id);
        }

        public void Modificar(Paciente paciente)
        {
            datos.Modificar(paciente);
        }

        public void CambioEstado(int id, bool estado)
        {
            datos.CambioEstado(id, estado);
        }
        public bool ExistePorEmail(string email)
        {
            return datos.ExistePorEmail(email);
        }
    }
}