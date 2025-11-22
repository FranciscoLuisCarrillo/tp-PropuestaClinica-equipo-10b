using Clinica.Negocio;
using Presentacion.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Presentacion.Pacientes
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarEspecialidades();
                CargarMedicos();
                CargarHoras();
            }
        }
        private void CargarHoras()
        {
            ddlHora.Items.Clear();
            ddlHora.Items.Insert(0, new ListItem("-- Seleccionar hora --", ""));

            for (int h = 0; h < 24; h++)
            {
                string hora = h.ToString("00") + ":00";
                ddlHora.Items.Add(new ListItem(hora, hora));
            }
        }

        private void CargarEspecialidades()
        {
            try
            {
                EspecialidadNegocio especialidadNegocio = new EspecialidadNegocio();
                var lista = especialidadNegocio.ListarTodas();
                ddlEspecialidades.DataSource = lista;
                ddlEspecialidades.DataTextField = "Nombre";
                ddlEspecialidades.DataValueField = "EspecialidadId";
                ddlEspecialidades.DataBind();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cargar las especialidades.", ex);
            }
        }
        private void CargarMedicos()
        {
            try
            {
                MedicoNegocio medicoNegocio = new MedicoNegocio();
                var lista = medicoNegocio.Listar();
                ddlMedico.DataTextField = "Nombre";       
                ddlMedico.DataSource = lista;
                ddlMedico.DataBind();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}