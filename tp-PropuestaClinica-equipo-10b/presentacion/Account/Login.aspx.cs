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
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                UsuarioNegocio usuarioNegocio = new UsuarioNegocio();
                Usuario usuario = usuarioNegocio.Login(txtEmail.Text.Trim(), txtPass.Text.Trim());
                if (usuario == null)
                {
                    valSum.HeaderText = "Email o contraseña incorrectos.";
                    return;
                }
                Session["Usuario"] = usuario;
                switch (usuario.Perfil)
                {
                    case Perfil.Administrador:
                        Response.Redirect("~/Admin/Default.aspx");
                        break;
                    case Perfil.Recepcionista:
                        Response.Redirect("~/Recepcion/Default.aspx");
                        break;
                    case Perfil.Medico:
                        Response.Redirect("~/Medicos/Default.aspx");
                        break;
                    case Perfil.Paciente:
                        Response.Redirect("~/Pacientes/Default.aspx");
                        break;
                    default:
                        valSum.HeaderText = "Perfil de usuario no reconocido.";
                        break;
                }
            }
            catch (Exception ex)
            {
                valSum.HeaderText = ex.Message;

            }
        }
    }
}