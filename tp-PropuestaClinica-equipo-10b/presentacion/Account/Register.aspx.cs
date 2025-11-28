using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Clinica.Dominio;
using Clinica.Negocio;

namespace Presentacion.Account
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        protected void btnCrearCuenta_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            PacienteNegocio pacienteNegocio = new PacienteNegocio();
            Paciente paciente = new Paciente();

            try
            {
                paciente.Nombre = txtNombre.Text.Trim();
                paciente.Apellido = txtApellido.Text.Trim();
                paciente.Email = txtEmail.Text.Trim();
                paciente.Activo = true;

                // --- CORRECCIÓN: Agregar campos obligatorios para SQL ---
                // Como no los pides en el formulario, asignamos valores por defecto
                // para que la base de datos acepte el registro.
                paciente.Dni = "S/D";
                paciente.FechaNacimiento = DateTime.Now;
                // -------------------------------------------------------

                int idPacienteGenerado = pacienteNegocio.Agregar(paciente);

                UsuarioNegocio usuarioNegocio = new UsuarioNegocio();
                Usuario nuevo = new Usuario();
                nuevo.Email = txtEmail.Text.Trim();
                nuevo.Password = txtPass.Text.Trim();
                nuevo.Perfil = Perfil.Paciente;
                nuevo.Rol = "Paciente";
                nuevo.Activo = true;

                // Copiamos datos personales al usuario también (si tu tabla lo requiere)
                nuevo.Nombre = txtNombre.Text.Trim();
                nuevo.Apellido = txtApellido.Text.Trim();

                nuevo.IdPaciente = idPacienteGenerado;

                usuarioNegocio.Agregar(nuevo);

                Session["usuario"] = nuevo;

                Response.Redirect("~/Pacientes/Default.aspx", false);
            }
            catch (Exception ex)
            {
                // Asegúrate de tener un control con ID="valSummary" en tu HTML
                // o cambia esto por un Label de error.
                if (valSummary != null)
                    valSummary.HeaderText = ex.Message;
            }
        }
    }
}