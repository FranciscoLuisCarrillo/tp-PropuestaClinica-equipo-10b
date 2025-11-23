using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Clinica.Negocio;

namespace Presentacion.Admin
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                CargarMetricas();
            }
        }
        private void CargarMetricas()
        {
            MedicoNegocio medicoNegocio = new MedicoNegocio();
            lblMedicos.Text = medicoNegocio.Listar().Count.ToString();
            TurnoNegocio turnoNegocio = new TurnoNegocio();
            DateTime hoy = DateTime.Today;
            
        }
    }
}