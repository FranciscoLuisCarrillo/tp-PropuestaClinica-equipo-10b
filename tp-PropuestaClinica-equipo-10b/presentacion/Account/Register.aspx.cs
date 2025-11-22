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
            UsuarioNegocio usuarioNegocio = new UsuarioNegocio();
            try
            {
                Usuario nuevo = new Usuario();
                nuevo.Nombre = txtNombre.Text.Trim();
                nuevo.Apellido = txtApellido.Text.Trim();
                nuevo.Email = txtEmail.Text.Trim();
                nuevo.Password = txtPass.Text.Trim();
                nuevo.Rol = "Paciente";
                nuevo.Perfil = Perfil.Paciente;
                nuevo.Activo = true;

                usuarioNegocio.Agregar(nuevo);


                Session["UsuarioEmail"] = nuevo.Email;
                Session["Perfil"] = nuevo.Perfil;

                Response.Redirect("~/Pacientes/Dashboard.aspx");
            }
            catch (Exception ex)
            {
                valSummary.HeaderText = ex.Message;
            }
        }
    }
    
}



