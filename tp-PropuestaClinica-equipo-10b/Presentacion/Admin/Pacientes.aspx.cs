using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Clinica.Negocio;

namespace Presentacion.Admin
{
    public partial class Pacientes : System.Web.UI.Page
    {
        PacienteNegocio negocio = new PacienteNegocio();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarPacientes();
            }
        }
        private void CargarPacientes()
        {
           // gvPacientes.DataSource = negocio.Listar();
            gvPacientes.DataBind();
        }

        protected void gvPacientes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if(e.CommandName == "ToggleActivo")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                int idPaciente = (int)gvPacientes.DataKeys[index].Value;

               //negocio.CambiarEstado(idPaciente, true);
                CargarPacientes();
            }
        }
    }
}