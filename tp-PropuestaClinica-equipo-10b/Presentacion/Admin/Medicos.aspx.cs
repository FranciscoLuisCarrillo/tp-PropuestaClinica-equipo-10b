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
    public partial class Medicos : System.Web.UI.Page
    {
        EspecialidadNegocio especialidadNegocio = new EspecialidadNegocio();
 
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                CargarEspecialidades();
            }   
        }
        private void CargarEspecialidades()
        {
            try
            {
                var lista = especialidadNegocio.ListarTodas();
                chkEspecialidades.DataSource = lista;
                chkEspecialidades.DataTextField = "Nombre";
                chkEspecialidades.DataValueField = "IdEspecialidad";
                chkEspecialidades.DataBind();
            }
            catch (Exception ex)
            {
                
                throw new Exception("Error al cargar las especialidades.", ex);
            }

        }

    }
}