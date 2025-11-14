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
            lblMsg.Text = string.Empty;
            lblMsg.CssClass = "";
            try
            {
                var nombre = (txtNombreEspecialidad.Text ?? "").Trim();
                if (string.IsNullOrEmpty(nombre))
                {
                    lblMsg.Text = "Ingresa un nombre.";
                    lblMsg.CssClass = "text-danger d-block mt-2";
                    return;
                }
                EspecialidadNegocio negocio = new EspecialidadNegocio();
                var ok = negocio.Agregar(new Clinica.Dominio.Especialidad { Nombre = nombre });
                if (ok)
                {
                    lblMsg.Text = "Especialidad agregada Correctamente";
                    txtNombreEspecialidad.Text = "";
                    CargarEspecialidades();

                }
                else
                {
                    lblMsg.Text = "La especialidad ya existe.";
                    lblMsg.CssClass = "text-warning d-block mt-2";
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error: " + ex.Message;
                lblMsg.CssClass = "text-danger d-block mt-2";
            }
        }
    }
}