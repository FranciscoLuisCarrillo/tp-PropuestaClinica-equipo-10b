using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clinica.Dominio;



namespace Clinica.Datos
{
    public class EspecialidadDatos
    {
        // ----------------------------------------------------
        // 1. Método para verificar si una especialidad ya existe (Ignorando mayúsculas/minúsculas).
        // ----------------------------------------------------
        public bool ExistePorNombre(string nombre)
        {
            AccesoDatos datos = new AccesoDatos();
            bool existe = false;

            try
            {
                // Usamos COLLATE Latin1_General_CI_AI para hacer la comparación case-insensitive (CI) y accent-insensitive (AI).
                string consulta = "SELECT EspecialidadId FROM Especialidades WHERE Nombre COLLATE Latin1_General_CI_AI = @Nombre";
                datos.SetearConsulta(consulta);
                datos.SetearParametro("@Nombre", nombre);
                datos.EjecutarLectura();

                if (datos.Lector.Read())
                {
                    existe = true;
                }

                return existe;
            }
            catch (Exception ex)
            {
                // En caso de error, relanzar con contexto
                throw new Exception("Error al verificar si la especialidad existe en la base de datos.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        // ----------------------------------------------------
        // 2. Método para agregar una nueva especialidad (Asume que la validación ya se hizo en la capa de Negocio).
        // ----------------------------------------------------
        public int Agregar(Especialidad nueva)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                // Consulta de inserción simple
                string consulta = "INSERT INTO Especialidades (Nombre) VALUES (@Nombre)";
                datos.SetearConsulta(consulta);
                datos.SetearParametro("@Nombre", nueva.Nombre);

                // Ejecutar la acción.
                return datos.EjecutarAccion(); // Devuelve el número de filas afectadas
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar la especialidad a la base de datos.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }
    }
}
