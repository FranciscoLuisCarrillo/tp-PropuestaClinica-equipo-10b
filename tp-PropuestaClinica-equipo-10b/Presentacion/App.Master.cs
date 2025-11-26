using Clinica.Dominio;
using Clinica.Negocio;
using System;
using System.Web.UI;

namespace Presentacion
{
    public partial class Public : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (Session["usuario"] == null)
            {
                
                phAdmin.Visible = false;
                phRecepcion.Visible = false;
                phMedico.Visible = false;
                phPaciente.Visible = false;

                
                pnlUsuarioLogueado.Visible = false;
                pnlAnonimo.Visible = true;
            }
            else
            {
                
                Usuario usuario = (Usuario)Session["usuario"];

                // 1. Mostramos panel de usuario y ocultamos el anónimo
                pnlUsuarioLogueado.Visible = true;
                pnlAnonimo.Visible = false;

                // 2. Seteamos el nombre en el Literal (Email o Nombre)
                litUsuario.Text = usuario.Email;

                // 3. Lógica del Logo Principal (Tu pedido de mejora)
                // Dependiendo el rol, el logo lleva a un lugar distinto
                switch (usuario.Perfil)
                {
                    case Perfil.Administrador:
                        linkLogo.HRef = "~/Admin/Default.aspx";
                        phAdmin.Visible = true;
                        break;
                    case Perfil.Medico:
                        linkLogo.HRef = "~/Medicos/Default.aspx";
                        phMedico.Visible = true;
                        break;
                    case Perfil.Paciente:
                        linkLogo.HRef = "~/Pacientes/Default.aspx"; // Al menú nuevo que hicimos
                        phPaciente.Visible = true;
                        break;
                    case Perfil.Recepcionista:
                        linkLogo.HRef = "~/Recepcion/Default.aspx";
                        phRecepcion.Visible = true;
                        break;
                }
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();       
            Session.Abandon();     
            Response.Redirect("~/Account/Default.aspx"); 
        }
    }
}