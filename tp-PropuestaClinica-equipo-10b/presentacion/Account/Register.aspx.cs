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

                int idPacienteGenerado = pacienteNegocio.Agregar(paciente);

                UsuarioNegocio usuarioNegocio = new UsuarioNegocio();
                Usuario nuevo = new Usuario();
                nuevo.Email = txtEmail.Text.Trim();
                nuevo.Password = txtPass.Text.Trim();
                nuevo.Perfil = Perfil.Paciente;
                nuevo.Rol = "Paciente";
                nuevo.Activo = true;

                nuevo.IdPaciente = idPacienteGenerado;
                usuarioNegocio.Agregar(nuevo);


                Session["usuario"] = nuevo;

                Response.Redirect("~/Pacientes/Default.aspx");
            }
            catch (Exception ex)
            {
                valSummary.HeaderText = ex.Message;
            }
        }
    }
    
}



