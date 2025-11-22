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

        public List<Recepcionista> Listar()
        {
            List<Recepcionista> lista = new List<Recepcionista>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = @"SELECT R.RecepcionistaId, R.Nombre, R.Apellido, R.Email, R.Telefono,
                                        R.TurnoTrabajoId, R.Activo,
                                        T.Nombre AS TurnoNombre
                                        FROM Recepcionistas R
                                        LEFT JOIN TurnosTrabajo T ON T.TurnoTrabajoId = R.TurnoTrabajoId
                                        ORDER BY R.Apellido, R.Nombre;";
                datos.SetearConsulta(consulta);
                datos.EjecutarLectura();
                while (datos.Lector.Read())
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
                    lista.Add(aux);

                }
                return lista;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar Recepcionistas desde la base de datos.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }
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

                int idGenerado = Convert.ToInt32(datos.EjecutarEscalar());

                return idGenerado;
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
                    int count = (int)datos.Lector[0];
                    return count > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al verificar existencia de email en la base de datos.", ex);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

    }
}
