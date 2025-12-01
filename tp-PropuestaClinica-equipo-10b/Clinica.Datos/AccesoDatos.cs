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
        private SqlTransaction transaccion;

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
                if (conexion.State != ConnectionState.Open) conexion.Open();
                if (EnTransaccion) comando.Transaction = transaccion;
                lector = comando.ExecuteReader();
            }
            catch (Exception ex) { throw new Exception("Error al ejecutar la lectura.", ex); }
        }

        // Método para ejecutar acciones (INSERT, UPDATE, DELETE). Devuelve el número de filas afectadas.
        public int EjecutarAccion()
        {
            try
            {
                comando.Connection = conexion;
                if (conexion.State != ConnectionState.Open) conexion.Open();
                if (EnTransaccion) comando.Transaction = transaccion;
                return comando.ExecuteNonQuery();
            }
            catch (Exception ex) { throw new Exception("Error al ejecutar la acción.", ex); }
            finally
            {
                
                if (!EnTransaccion) CerrarConexion();
                
                LimpiarParametros();
            }
        }

        // Método para ejecutar acciones que devuelven un valor escalar (ej: COUNT, MAX, o ID de un INSERT).
        public object EjecutarEscalar()
        {
            try
            {
                comando.Connection = conexion;
                if (conexion.State != ConnectionState.Open) conexion.Open();
                if (EnTransaccion) comando.Transaction = transaccion;
                return comando.ExecuteScalar();
            }
            catch (Exception ex) { throw new Exception("Error al ejecutar escalar.", ex); }
            finally
            {
                if (!EnTransaccion) CerrarConexion();
                LimpiarParametros();
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
        public void IniciarTransaccion()
        {
            if (conexion.State != ConnectionState.Open) conexion.Open();
            transaccion = conexion.BeginTransaction();
            comando.Connection = conexion;
            comando.Transaction = transaccion;
        }

        public void ConfirmarTransaccion()
        {
            transaccion?.Commit();
            transaccion = null;
            
        }
       public void CancelarTransaccion()
        {
            try { transaccion?.Rollback(); }
            finally { transaccion = null; }
        }

      
        private bool EnTransaccion => transaccion != null;
        public void Commit()
        {
            if (transaccion != null)
            {
                transaccion.Commit();
                transaccion.Dispose();
                transaccion = null;
            }
        }

        public void Rollback()
        {
            if (transaccion != null)
            {
                transaccion.Rollback();
                transaccion.Dispose();
                transaccion = null;
            }
        }
    }
}