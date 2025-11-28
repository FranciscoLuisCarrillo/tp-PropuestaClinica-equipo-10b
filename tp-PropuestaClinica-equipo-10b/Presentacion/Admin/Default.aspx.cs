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
            if (!IsPostBack)
            {
                CargarMetricas();
                CargarUltimosTurnos();
            }
        }

        private void CargarMetricas()
        {
            try
            {
                var medicos = new MedicoNegocio();
                var pacientes = new PacienteNegocio();
                var turnos = new TurnoNegocio();

                lblPacientes.Text = pacientes.Listar().Count.ToString(); 
                lblMedicos.Text = medicos.Listar().Count.ToString();
                lblTurnosHoy.Text = turnos.ContarTurnosHoy().ToString();
            }
            catch
            {
                lblPacientes.Text = "0";
                lblMedicos.Text = "0";
                lblTurnosHoy.Text = "0";
            }
        }

        private void CargarUltimosTurnos()
        {
            try
            {
                var turnos = new TurnoNegocio();
                gvUltimosTurnos.DataSource = turnos.ListarUltimosResumen(10);
                gvUltimosTurnos.DataBind();
            }
            catch
            {
                gvUltimosTurnos.DataSource = null;
                gvUltimosTurnos.DataBind();
            }
        }
    }
}