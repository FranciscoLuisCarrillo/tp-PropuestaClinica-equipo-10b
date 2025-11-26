using Clinica.Dominio;
using Clinica.Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Presentacion.Medicos
{
    public partial class AgendaMedico : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                txtFecha.Text = DateTime.Today.ToString("yyyy-MM-dd"); 
                CargarAgenda();
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarAgenda();
        }
        private void CargarAgenda()
        {
           
            if (Session["usuario"] == null) { Response.Redirect("~/Login.aspx"); return; }

            Usuario usuario = (Usuario)Session["usuario"];

            
            if (!usuario.IdMedico.HasValue)
            {
                Response.Write("<script>alert('Acceso denegado. Usted no es un médico registrado.'); window.location='../Default.aspx';</script>");
                return;
            }

            TurnoNegocio negocio = new TurnoNegocio();
            DateTime fecha;
            if (!DateTime.TryParse(txtFecha.Text, out fecha)) fecha = DateTime.Today;

            
            var lista = negocio.ListarAgendaMedico(usuario.IdMedico.Value, fecha);

            gvTurnos.DataSource = lista.Select(x => new
            {
                x.IdTurno,
                x.Hora,
                x.Paciente,
                Definicion = x.Diagnostico,
                x.Estado
            });
            gvTurnos.DataBind();
        }
    }
}