using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Clinica.Dominio;
using Clinica.Negocio;

namespace Presentacion.Admin
{
    public partial class Especialidades : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarEspecialidades();
            }
        }

        private void CargarEspecialidades()
        {
            EspecialidadNegocio negocio = new EspecialidadNegocio();
            try
            {
                gvEspecialidades.DataSource = negocio.ListarTodas();
                gvEspecialidades.DataBind();
            }
            catch (Exception ex)
            {
                // Manejo de errores (puedes mostrar un mensaje en la página si lo deseas)
                throw new Exception("Error al cargar las especialidades.", ex);
            }
        }

        protected void btnAgregarEspecialidad_Click(object sender, EventArgs e)
        {

        }
    }
}