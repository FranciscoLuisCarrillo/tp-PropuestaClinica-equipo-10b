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
        protected void chkActiva_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            GridViewRow row = (GridViewRow)chk.NamingContainer;
            int especialidadId = Convert.ToInt32(gvEspecialidades.DataKeys[row.RowIndex].Value);
            bool nuevoEstado = chk.Checked;
            EspecialidadNegocio negocio = new EspecialidadNegocio();
            negocio.CambiarEstado(especialidadId, nuevoEstado);
            CargarEspecialidades();
        }
        protected void gvEspecialidades_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Especialidad especialidad = (Especialidad)e.Row.DataItem;
                CheckBox chkActiva = (CheckBox)e.Row.FindControl("chkActiva");
                chkActiva.Checked = especialidad.Activa;
            }
        }
    }
}