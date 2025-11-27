using System;
using System.Collections.Generic;
using Clinica.Dominio;

namespace Clinica.Datos
{
    public class PacienteDatos
    {
        public List<Paciente> Listar()
        {
            List<Paciente> lista = new List<Paciente>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                // Agregamos Activo a la consulta y filtramos solo los activos (Activo = 1)
                datos.SetearConsulta("SELECT PacienteId, Nombre, Apellido, DNI, FechaNacimiento, Telefono, Email, Domicilio, ObraSocial, Activo FROM Pacientes WHERE Activo = 1");
                datos.EjecutarLectura();

                while (datos.Lector.Read())
                {
                    Paciente aux = new Paciente();
                    aux.PacienteId = (int)datos.Lector["PacienteId"];
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Apellido = (string)datos.Lector["Apellido"];
                    aux.Dni = (string)datos.Lector["DNI"];
                    aux.FechaNacimiento = (DateTime)datos.Lector["FechaNacimiento"];
                    aux.Email = (string)datos.Lector["Email"];

                    if (!(datos.Lector["Telefono"] is DBNull))
                        aux.Telefono = (string)datos.Lector["Telefono"];

                    if (!(datos.Lector["Domicilio"] is DBNull))
                        aux.Domicilio = (string)datos.Lector["Domicilio"];

                    if (!(datos.Lector["ObraSocial"] is DBNull))
                        aux.ObraSocial = (string)datos.Lector["ObraSocial"];

                    // Leemos el estado
                    aux.Activo = (bool)datos.Lector["Activo"];

                    lista.Add(aux);
                }
                return lista;
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public int Agregar(Paciente nuevo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = @"INSERT INTO Pacientes 
                                (Nombre, Apellido, DNI, FechaNacimiento, Telefono, Email, Domicilio, ObraSocial, Activo) 
                                VALUES 
                                (@Nombre, @Apellido, @DNI, @FechaNacimiento, @Telefono, @Email, @Domicilio, @ObraSocial, 1);
                                SELECT SCOPE_IDENTITY();";

                datos.SetearConsulta(consulta);

                datos.SetearParametro("@Nombre", nuevo.Nombre);
                datos.SetearParametro("@Apellido", nuevo.Apellido);
                datos.SetearParametro("@DNI", nuevo.Dni);
                datos.SetearParametro("@FechaNacimiento", nuevo.FechaNacimiento);
                datos.SetearParametro("@Telefono", (object)nuevo.Telefono ?? DBNull.Value);
                datos.SetearParametro("@Email", nuevo.Email);
                datos.SetearParametro("@Domicilio", (object)nuevo.Domicilio ?? DBNull.Value);
                datos.SetearParametro("@ObraSocial", (object)nuevo.ObraSocial ?? DBNull.Value);

                object idGenerado = datos.EjecutarEscalar();
                return Convert.ToInt32(idGenerado);
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        // NUEVO MÉTODO ELIMINAR
        public void Eliminar(int id)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("UPDATE Pacientes SET Activo = 0 WHERE PacienteId = @Id");
                datos.SetearParametro("@Id", id);
                datos.EjecutarAccion();
            }
            finally
            {
                datos.CerrarConexion();
            }
        }

        public bool ExistePorDNI(string dni)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("SELECT 1 FROM Pacientes WHERE DNI = @DNI");
                datos.SetearParametro("@DNI", dni);
                datos.EjecutarLectura();
                return datos.Lector.Read();
            }
            finally { datos.CerrarConexion(); }
        }

        public bool ExistePorEmail(string email)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.SetearConsulta("SELECT 1 FROM Pacientes WHERE Email = @Email");
                datos.SetearParametro("@Email", email);
                datos.EjecutarLectura();
                return datos.Lector.Read();
            }
            finally { datos.CerrarConexion(); }
        }
    }
}