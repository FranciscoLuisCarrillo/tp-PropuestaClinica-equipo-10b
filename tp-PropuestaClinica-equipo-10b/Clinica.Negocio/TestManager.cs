using Clinica.Dominio;
using System;
using System.Diagnostics; // Importación necesaria para Debug.WriteLine
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinica.Negocio.Pruebas
{
    // Clase estática para ejecutar pruebas de las funcionalidades de negocio.
    public static class TestManager
    {
        /// <summary>
        /// Ejecuta una prueba simple de la funcionalidad de Agregar Especialidad.
        /// </summary>
        public static void ProbarAgregarEspecialidad(string nombreEspecialidad)
        {
            // Usamos Debug.WriteLine para asegurar la salida en la ventana de depuración (Output -> Debug).
            Debug.WriteLine($"--- Ejecutando Prueba: Agregar Especialidad '{nombreEspecialidad}' ---");

            // Instancia de la clase de Negocio (la lógica a probar)
            EspecialidadNegocio negocio = new EspecialidadNegocio();
            Especialidad nuevaEspecialidad = new Especialidad { Nombre = nombreEspecialidad };

            try
            {
                // Intento de agregar
                bool agregado = negocio.Agregar(nuevaEspecialidad);

                if (agregado)
                {
                    Debug.WriteLine($"[EXITO] La especialidad '{nombreEspecialidad}' fue agregada correctamente.");
                }
                else
                {
                    Debug.WriteLine($"[VALIDACIÓN] La especialidad '{nombreEspecialidad}' ya existe (o no se pudo agregar).");
                }
            }
            catch (ArgumentException ex)
            {
                // Captura la excepción si el nombre es vacío/nulo
                Debug.WriteLine($"[ERROR DE LÓGICA] {ex.Message}");
            }
            catch (Exception ex)
            {
                // Captura cualquier otro error (ej: error de conexión a DB)
                Debug.WriteLine($"[ERROR DE SISTEMA] Ocurrió un error inesperado: {ex.Message}");
            }
            finally
            {
                // No se usa Console.ResetColor() ni Console.ForegroundColor ya que no tienen efecto en Debug Output
                Debug.WriteLine("-------------------------------------------------------------------");
            }
        }
    }
}