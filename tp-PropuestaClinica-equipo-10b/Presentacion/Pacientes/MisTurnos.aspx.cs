using Clinica.Dominio;
using Clinica.Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Presentacion.Pacientes
{
    public partial class MisTurnos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuario"] == null) Response.Redirect("~/Login.aspx");

            if (!IsPostBack)
            {
                CargarMisTurnos();
            }
        }

        private void CargarMisTurnos()
        {
            Usuario usuario = (Usuario)Session["usuario"];
            TurnoNegocio negocio = new TurnoNegocio();

            
            gvMisTurnos.DataSource = negocio.ListarPorPaciente((int)usuario.IdPaciente);
            gvMisTurnos.DataBind();
        }
    }
}