using Clinica.Datos;
using Clinica.Dominio;
using Clinica.Negocio;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Presentacion.Admin
{
    public partial class Pacientes : System.Web.UI.Page
    {
        PacienteNegocio negocio = new PacienteNegocio();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CargarPacientes();
        }

        private void CargarPacientes()
        {
            try
            {
                List<Paciente> lista = negocio.Listar();

                if (!string.IsNullOrWhiteSpace(txtFiltro.Text))
                {
                    string f = txtFiltro.Text.Trim().ToUpper();
                    lista = lista.FindAll(x =>
                        (x.Nombre ?? "").ToUpper().Contains(f) ||
                        (x.Apellido ?? "").ToUpper().Contains(f) ||
                        (x.Dni ?? "").Contains(f));
                }

                repPacientes.DataSource = lista;
                repPacientes.DataBind();

                litVacio.Text = (lista.Count == 0)
                    ? "<p class='text-muted text-center my-4'>No se encontraron pacientes registrados.</p>"
                    : "";
            }
            catch (Exception ex)
            {
                Session["error"] = ex.Message;
                litVacio.Text = "<p class='text-danger text-center my-4'>Error al cargar pacientes.</p>";
            }
        }

        protected void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            CargarPacientes();
        }

        protected void btnGuardarPass_Click(object sender, EventArgs e)
        {
            try
            {
                string email = hfEmailDestino.Value?.Trim();
                string nuevaPass = txtNuevaPass.Text?.Trim();

                if (string.IsNullOrWhiteSpace(email))
                {
                    ClientScript.RegisterStartupScript(GetType(), "alert", "alert('Falta el email de destino.');", true);
                    return;
                }
                if (string.IsNullOrWhiteSpace(nuevaPass))
                {
                    ClientScript.RegisterStartupScript(GetType(), "alert", "alert('La contraseña no puede estar vacía.');", true);
                    return;
                }

                var un = new UsuarioNegocio();
                un.ModificarPasswordPorEmail(email, nuevaPass);

                txtNuevaPass.Text = string.Empty;

                ClientScript.RegisterStartupScript(
                    GetType(), "pwdok",
                    "(() => { var el = document.getElementById('modalPassword'); if (el){ var m = bootstrap.Modal.getInstance(el) || new bootstrap.Modal(el); m.hide(); } alert('Contraseña actualizada correctamente.'); })();",
                    true
                );
            }
            catch (Exception ex)
            {
                ClientScript.RegisterStartupScript(GetType(), "err", $"alert('Error al cambiar clave: {ex.Message}');", true);
            }
        }
    }
}