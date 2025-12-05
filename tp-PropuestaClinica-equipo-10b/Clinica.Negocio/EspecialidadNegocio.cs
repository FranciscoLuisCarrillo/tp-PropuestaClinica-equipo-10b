using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clinica.Datos;
using Clinica.Dominio;

namespace Clinica.Negocio
{
    // Clase que contiene la lógica de negocio y validaciones para las Especialidades.
    public class EspecialidadNegocio
    {
        private EspecialidadDatos especialidadDatos;

        public EspecialidadNegocio()
        {
            especialidadDatos = new EspecialidadDatos();
        }

        /// <summary>
        /// Agrega una nueva especialidad al sistema previa validación.
        /// </summary>
        /// <param name="nueva">Objeto Especialidad con el nombre a agregar.</param>
        /// <returns>True si se agregó, False si ya existía.</returns>
        /// <exception cref="Exception">Lanza una excepción si ocurre un error de DB.</exception>
        public bool Agregar(Especialidad nueva)
        {
            if (string.IsNullOrWhiteSpace(nueva.Nombre))
            {
                
                throw new ArgumentException("El nombre de la especialidad no puede estar vacío.");
            }

        
            if (especialidadDatos.ExistePorNombre(nueva.Nombre))
            {
                // Si ya existe, no se agrega y se informa al llamador.
                // El mensaje de "ya existe" se gestionaría en la capa de Presentación.
                return false;
            }

            
            int filasAfectadas = especialidadDatos.Agregar(nueva);

            return filasAfectadas > 0;
        }

        public List<Especialidad> ListarTodas()
        {
            return especialidadDatos.ListarTodas();
        }

        public void CambiarEstado(int especialidadId, bool nuevoEstado)
        {
            EspecialidadDatos datos = new EspecialidadDatos();
            datos.ActualizarEstado(especialidadId, nuevoEstado);
        }

        public List<Especialidad> ListarPorMedico(int medicoId)
        {
            EspecialidadDatos datos = new EspecialidadDatos();
            return datos.ListarPorMedico(medicoId);
        }

    }
}
