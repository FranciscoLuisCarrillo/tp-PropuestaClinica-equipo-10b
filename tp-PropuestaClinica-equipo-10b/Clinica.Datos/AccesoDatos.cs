using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Clinica.Datos
{
    // Clase central para la ejecución de comandos SQL.
    public class AccesoDatos
    {
        private SqlConnection conexion;
        private SqlCommand comando;
        private SqlDataReader lector;

        public SqlDataReader Lector
        {
            get { return lector; }
        }

        public AccesoDatos()
        {
            // Inicializa la conexión usando la clase de utilidad.
            conexion = ConexionDB.ObtenerConexion();
            comando = new SqlCommand();
        }

        // ------------------------------------------
        // Métodos de Configuración
        // ------------------------------------------

        // Configura el comando con la consulta SQL.
        public void SetearConsulta(string consulta)
        {
            comando.CommandType = CommandType.Text;
            comando.CommandText = consulta;
        }

        // Agrega parámetros al comando.
        public void SetearParametro(string nombre, object valor)
        {
            comando.Parameters.AddWithValue(nombre, valor);
        }

        // Limpia todos los parámetros después de una ejecución.
        public void LimpiarParametros()
        {
            comando.Parameters.Clear();
        }

        // ------------------------------------------
        // Métodos de Ejecución
        // ------------------------------------------

        // Método para ejecutar una lectura (SELECT) y obtener un SqlDataReader.
        public void EjecutarLectura()
        {
            try
            {
                comando.Connection = conexion;
                conexion.Open();
                lector = comando.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar la lectura en la base de datos.", ex);
            }
        }

        // Método para ejecutar acciones (INSERT, UPDATE, DELETE). Devuelve el número de filas afectadas.
        public int EjecutarAccion()
        {
            try
            {
                comando.Connection = conexion;
                conexion.Open();
                // ExecuteNonQuery devuelve el número de filas afectadas.
                return comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar la acción en la base de datos (INSERT/UPDATE/DELETE).", ex);
            }
            finally
            {
                // Aseguramos que la conexión se cierre después de la acción.
                CerrarConexion();
            }
        }

        // Método para ejecutar acciones que devuelven un valor escalar (ej: COUNT, MAX, o ID de un INSERT).
        public object EjecutarEscalar()
        {
            try
            {
                comando.Connection = conexion;
                conexion.Open();
                return comando.ExecuteScalar();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al ejecutar el escalar en la base de datos.", ex);
            }
            finally
            {
                CerrarConexion();
            }
        }

        // ------------------------------------------
        // Método de Cierre
        // ------------------------------------------

        // Cierra el lector y la conexión de manera segura.
        public void CerrarConexion()
        {
            if (lector != null && !lector.IsClosed)
                lector.Close();

            if (conexion != null && conexion.State == ConnectionState.Open)
                conexion.Close();
        }
    }
}