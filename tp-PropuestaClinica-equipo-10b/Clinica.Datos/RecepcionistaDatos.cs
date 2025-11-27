using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clinica.Dominio;

namespace Clinica.Datos
{
    public class RecepcionistaDatos
    {
        // ... (Tu método Listar existente se mantiene igual) ...
        public List<Recepcionista> Listar()
        {
            List<Recepcionista> lista = new List<Recepcionista>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = @"SELECT R.RecepcionistaId, R.Nombre, R.Apellido, R.Email, R.Telefono,
                                    R.TurnoTrabajoId, R.Activo, T.Nombre AS TurnoNombre
                                    FROM Recepcionistas R
                                    LEFT JOIN TurnosTrabajo T ON T.TurnoTrabajoId = R.TurnoTrabajoId
                                    WHERE R.Activo = 1  -- Opcional: Si solo quieres listar los activos
                                    ORDER BY R.Apellido, R.Nombre;";
                datos.SetearConsulta(consulta);
                datos.EjecutarLectura();
                while (datos.Lector.Read())
                {
                    lista.Add(Mapear(datos));
                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar Recepcionistas.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public Recepcionista ObtenerPorId(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = @"SELECT R.RecepcionistaId, R.Nombre, R.Apellido, R.Email, R.Telefono,
                                    R.TurnoTrabajoId, R.Activo, T.Nombre AS TurnoNombre
                                    FROM Recepcionistas R
                                    LEFT JOIN TurnosTrabajo T ON T.TurnoTrabajoId = R.TurnoTrabajoId
                                    WHERE R.RecepcionistaId = @Id";
                datos.SetearConsulta(consulta);
                datos.SetearParametro("@Id", id);
                datos.EjecutarLectura();

                if (datos.Lector.Read())
                {
                    return Mapear(datos);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener Recepcionista por ID.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        // ... (Tu método Agregar existente se mantiene igual) ...
        public int Agregar(Recepcionista nuevo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = @"
            INSERT INTO Recepcionistas (Nombre, Apellido, Email, Telefono, TurnoTrabajoId, Activo)
            VALUES (@Nombre, @Apellido, @Email, @Telefono, @TurnoTrabajoId, @Activo);
            SELECT SCOPE_IDENTITY();";

                datos.SetearConsulta(consulta);
                datos.SetearParametro("@Nombre", nuevo.Nombre);
                datos.SetearParametro("@Apellido", nuevo.Apellido);
                datos.SetearParametro("@Email", nuevo.Email);
                datos.SetearParametro("@Telefono", (object)nuevo.Telefono ?? DBNull.Value);
                datos.SetearParametro("@TurnoTrabajoId", (object)nuevo.TurnoTrabajoId ?? DBNull.Value);
                datos.SetearParametro("@Activo", nuevo.Activo);

                return Convert.ToInt32(datos.EjecutarEscalar());
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar nueva recepcionista.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public void Modificar(Recepcionista modificar)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = @"UPDATE Recepcionistas 
                                    SET Nombre = @Nombre, 
                                        Apellido = @Apellido, 
                                        Email = @Email, 
                                        Telefono = @Telefono, 
                                        TurnoTrabajoId = @TurnoTrabajoId,
                                        Activo = @Activo
                                    WHERE RecepcionistaId = @Id";

                datos.SetearConsulta(consulta);
                datos.SetearParametro("@Nombre", modificar.Nombre);
                datos.SetearParametro("@Apellido", modificar.Apellido);
                datos.SetearParametro("@Email", modificar.Email);
                datos.SetearParametro("@Telefono", (object)modificar.Telefono ?? DBNull.Value);
                datos.SetearParametro("@TurnoTrabajoId", (object)modificar.TurnoTrabajoId ?? DBNull.Value);
                datos.SetearParametro("@Activo", modificar.Activo);
                datos.SetearParametro("@Id", modificar.Id);

                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al modificar recepcionista.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public void EliminarLogico(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("UPDATE Recepcionistas SET Activo = 0 WHERE RecepcionistaId = @Id");
                datos.SetearParametro("@Id", id);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar recepcionista.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        // Verifica si el email existe en CUALQUIER registro
        public bool ExisteEmail(string email)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("SELECT COUNT(1) FROM Recepcionistas WHERE Email = @Email");
                datos.SetearParametro("@Email", email);
                datos.EjecutarLectura();
                if (datos.Lector.Read())
                {
                    return (int)datos.Lector[0] > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al verificar email.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        // Verifica si el email existe en OTRO registro que no sea el actual (para Modificar)
        public bool ExisteEmailEnOtroUsuario(string email, int idActual)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("SELECT COUNT(1) FROM Recepcionistas WHERE Email = @Email AND RecepcionistaId != @Id");
                datos.SetearParametro("@Email", email);
                datos.SetearParametro("@Id", idActual);
                datos.EjecutarLectura();
                if (datos.Lector.Read())
                {
                    return (int)datos.Lector[0] > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al verificar duplicado de email.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        // Método auxiliar privado para no repetir código de lectura
        private Recepcionista Mapear(AccesoDatos datos)
        {
            Recepcionista aux = new Recepcionista();
            aux.Id = (int)datos.Lector["RecepcionistaId"];
            aux.Nombre = datos.Lector["Nombre"] == DBNull.Value ? "" : datos.Lector["Nombre"].ToString();
            aux.Apellido = datos.Lector["Apellido"] == DBNull.Value ? "" : datos.Lector["Apellido"].ToString();
            aux.Email = datos.Lector["Email"] == DBNull.Value ? "" : datos.Lector["Email"].ToString();
            aux.Telefono = datos.Lector["Telefono"] == DBNull.Value ? null : datos.Lector["Telefono"].ToString();
            aux.TurnoTrabajoId = datos.Lector["TurnoTrabajoId"] == DBNull.Value ? (int?)null : Convert.ToInt32(datos.Lector["TurnoTrabajoId"]);
            aux.NombreTurnoTrabajo = datos.Lector["TurnoNombre"] == DBNull.Value ? "-" : datos.Lector["TurnoNombre"].ToString();
            aux.Activo = datos.Lector["Activo"] == DBNull.Value ? true : (bool)datos.Lector["Activo"];
            return aux;
        }
    }
}