using System;
using System.Collections.Generic;
using Clinica.Dominio;

namespace Clinica.Datos
{
    public class TurnoTrabajoDatos
    {
        /// <summary>
        /// Obtiene la lista completa de Turnos de Trabajo desde la base de datos.
        /// </summary>
        public List<TurnoTrabajo> Listar()
        {
            List<TurnoTrabajo> lista = new List<TurnoTrabajo>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("SELECT TurnoTrabajoId, Nombre, HoraEntrada, HoraSalida FROM TurnosTrabajo");
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    TurnoTrabajo aux = new TurnoTrabajo();
                    aux.TurnoTrabajoId = (int)datos.Lector["TurnoTrabajoId"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.HoraEntrada = (TimeSpan)datos.Lector["HoraEntrada"];
                    aux.HoraSalida = (TimeSpan)datos.Lector["HoraSalida"];

                    lista.Add(aux);
                }

                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        /// <summary>
        /// Inserta un nuevo turno de trabajo en la base de datos.
        /// </summary>
        public void Agregar(TurnoTrabajo nuevo)
        {
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.SetearConsulta("INSERT INTO TurnosTrabajo (Nombre, HoraEntrada, HoraSalida) VALUES (@Nombre, @HoraEntrada, @HoraSalida)");
                datos.SetearParametro("@Nombre", nuevo.Nombre);
                datos.SetearParametro("@HoraEntrada", nuevo.HoraEntrada);
                datos.SetearParametro("@HoraSalida", nuevo.HoraSalida);

                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        /// <summary>
        /// Actualiza los datos de un turno existente.
        /// </summary>
        public void Modificar(TurnoTrabajo turno)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("UPDATE TurnosTrabajo SET Nombre = @Nombre, HoraEntrada = @HoraEntrada, HoraSalida = @HoraSalida WHERE TurnoTrabajoId = @Id");

                datos.SetearParametro("@Nombre", turno.Nombre);
                datos.SetearParametro("@HoraEntrada", turno.HoraEntrada);
                datos.SetearParametro("@HoraSalida", turno.HoraSalida);
                datos.SetearParametro("@Id", turno.TurnoTrabajoId);

                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        /// <summary>
        /// Elimina un turno de trabajo por su ID.
        /// </summary>
        public void Eliminar(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("DELETE FROM TurnosTrabajo WHERE TurnoTrabajoId = @Id");
                datos.SetearParametro("@Id", id);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                // Nota: Esto podría fallar si un Médico tiene este TurnoTrabajoId asignado
                // (por una restricción de Clave Foránea).
                // La capa de Negocio debería manejar esa validación antes de llamar a Eliminar.
                throw ex;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }
    }
}