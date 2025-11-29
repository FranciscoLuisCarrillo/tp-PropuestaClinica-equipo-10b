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
            if (!Page.IsValid) return;

            var email = txtEmail.Text.Trim();

            var pacienteNegocio = new PacienteNegocio();
            var usuarioNegocio = new UsuarioNegocio();

            // Evitá mails duplicados en ambos lados
            if (usuarioNegocio.ExistePorEmail(email) || pacienteNegocio.ExistePorEmail(email))
            {
                valSummary.HeaderText = "El email ya está registrado.";
                return;
            }

          
            var paciente = new Paciente
            {
                Nombre = txtNombre.Text.Trim(),
                Apellido = txtApellido.Text.Trim(),
                Email = email,
                Dni = null,
                FechaNacimiento = DateTime.MinValue,
                Telefono = null,
                Domicilio = null,
                ObraSocial = null,
                Activo = true
            };

            int idPacienteGenerado = pacienteNegocio.Agregar(paciente);

           
            var usuario = new Usuario
            {
                Email = email,
                Pass = txtPass.Text.Trim(),   
                Perfil = Perfil.Paciente,
                Rol = "Paciente",
                Activo = true,
                IdPaciente = idPacienteGenerado
            };
            usuarioNegocio.Agregar(usuario);     

            Session["usuario"] = usuario;
            Response.Redirect("~/Pacientes/Default.aspx", false);
        }


    }

}