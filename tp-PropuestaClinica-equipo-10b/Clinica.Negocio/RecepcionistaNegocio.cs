using Clinica.Datos;
using Clinica.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Clinica.Negocio
{
    public class RecepcionistaNegocio
    {
        private RecepcionistaDatos datos;

        public RecepcionistaNegocio()
        {
            datos = new RecepcionistaDatos();
        }

        public List<Recepcionista> Listar()
        {
            return datos.Listar();
        }

        private void ValidarDatosTelefono(Recepcionista obj)
        {
            // ... (Tus validaciones anteriores de Nombre, Apellido, Email) ...

            if (obj == null) throw new ArgumentNullException("El objeto es nulo.");
            if (string.IsNullOrWhiteSpace(obj.Email)) throw new ArgumentException("El Email es obligatorio.");

            // --- VALIDACIÓN DE TELÉFONO NUEVA ---

            // 1. Validar si es obligatorio (asumimos que sí por tu pedido)
            if (string.IsNullOrWhiteSpace(obj.Telefono))
            {
                throw new ArgumentException("El teléfono es obligatorio.");
            }

            // 2. Validar formato: Solo números y mínimo 10 dígitos
            // Explicación Regex:
            // ^      -> Inicio del texto
            // \d     -> Dígito numérico (0-9)
            // {10,}  -> Se repite 10 veces o más (sin límite máximo)
            // $      -> Fin del texto
            string patronTelefono = @"^\d{10,}$";

            if (!Regex.IsMatch(obj.Telefono, patronTelefono))
            {
                throw new ArgumentException("El teléfono debe contener solo números y tener al menos 10 dígitos (sin guiones ni paréntesis).");
            }

            // ... (Resto de validaciones como formato de email, etc.) ...
        }
        public Recepcionista ObtenerPorId(int id)
        {
            if (id <= 0) throw new ArgumentException("El ID debe ser mayor a 0.");
            return datos.ObtenerPorId(id);
        }

        public int Agregar(Recepcionista nuevo)
        {
            ValidarDatos(nuevo);

            // Regla específica de Agregar: El email no puede existir en ningún registro
            if (datos.ExisteEmail(nuevo.Email))
            {
                throw new Exception("El Email ingresado ya se encuentra registrado.");
            }

            // Por defecto al agregar se pone activo
            nuevo.Activo = true;

            return datos.Agregar(nuevo);
        }

        public void Modificar(Recepcionista modificar)
        {
            if (modificar.Id <= 0)
                throw new ArgumentException("El ID del recepcionista no es válido.");

            ValidarDatos(modificar);

            // Regla específica de Modificar: El email no puede existir en OTRO usuario,
            // pero sí puede ser el mismo que ya tenía este usuario.
            if (datos.ExisteEmailEnOtroUsuario(modificar.Email, modificar.Id))
            {
                throw new Exception("El Email ingresado ya pertenece a otro usuario.");
            }

            datos.Modificar(modificar);
        }

        public void Eliminar(int id)
        {
            if (id <= 0) throw new ArgumentException("El ID debe ser mayor a 0.");

            // Aquí podrías validar si la recepcionista tiene turnos o tareas pendientes si fuera necesario

            datos.EliminarLogico(id);
        }

        // Método privado para centralizar validaciones comunes
        private void ValidarDatos(Recepcionista obj)
        {
            if (obj == null)
                throw new ArgumentNullException("El objeto Recepcionista no puede ser nulo.");

            if (string.IsNullOrWhiteSpace(obj.Nombre))
                throw new ArgumentException("El Nombre es obligatorio.");

            if (string.IsNullOrWhiteSpace(obj.Apellido))
                throw new ArgumentException("El Apellido es obligatorio.");

            if (string.IsNullOrWhiteSpace(obj.Email))
                throw new ArgumentException("El Email es obligatorio.");

            if (!EsEmailValido(obj.Email))
                throw new ArgumentException("El formato del Email no es válido.");

            // Validaciones opcionales:
            if (obj.Nombre.Length > 50) throw new ArgumentException("El nombre es demasiado largo.");
            if (obj.Apellido.Length > 50) throw new ArgumentException("El apellido es demasiado largo.");
        }

        private bool EsEmailValido(string email)
        {
            // Expresión regular simple para validar formato de email
            string patron = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, patron);
        }
    }

}