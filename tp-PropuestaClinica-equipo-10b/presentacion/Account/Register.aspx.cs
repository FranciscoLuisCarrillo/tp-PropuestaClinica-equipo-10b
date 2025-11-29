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

        protected void BtnCrearCuenta_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;


            PacienteNegocio pacienteNegocio = new PacienteNegocio();
            

            try
            {
                var paciente = new Paciente
                {
                    Nombre = txtNombre.Text.Trim(),
                    Apellido = txtApellido.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Dni = null,                
                    FechaNacimiento = DateTime.MinValue,
                    Telefono = null,
                    Domicilio = null,
                    ObraSocial = null,
                    Activo = true
                };


                int idPacienteGenerado = pacienteNegocio.Agregar(paciente);

                UsuarioNegocio usuarioNegocio = new UsuarioNegocio();
                var usuario = new Usuario
                {
                    Email = txtEmail.Text.Trim(),
                    Pass = txtPass.Text.Trim(),
                    Perfil = Perfil.Paciente,
                    Rol = "Paciente",
                    Activo = true,
                    IdPaciente = idPacienteGenerado
                };
                Session["usuario"] = usuario;

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