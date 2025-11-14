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
        /*
        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            if(!Page.IsValid)
            {
                return;
            }
            Paciente paciente = new Paciente();
            {
                paciente.Nombre = txtNombre.Text.Trim();
                paciente.Apellido = txtApellido.Text.Trim();
                paciente.FechaNacimiento = Convert.ToDateTime(txtFechaNacimiento.Text);
                paciente.Genero = ddlGenero.SelectedValue;
                paciente.Dni = txtDni.Text.Trim();
                paciente.Direccion = txtDireccion.Text.Trim();
                paciente.Telefono = txtTelefono.Text.Trim();
                paciente.ObraSocial = txtObraSocial.Text.Trim();
                paciente.Activo = true;
              };
            PacienteNegocio pacienteNegocio = new PacienteNegocio();
            int idPaciente = pacienteNegocio.Registrar(paciente);
           
            Usuario usuario = new Usuario();
            {
                usuario.IdPaciente = idPaciente;
                usuario.Email = txtEmail.Text.Trim();
                usuario.Password = txtPass.Text.Trim();
                usuario.Rol = "Paciente";
                usuario.Activo = true;
            }
            ;

            Response.Redirect("~/Account/Login.aspx?registro=ok");
        }
        */
    }
}