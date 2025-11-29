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
    public partial class RecepcionistaAdmin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarTurnos();
                CargarRecepcionistas();
            }
        }

        private void CargarTurnos()
        {
            try
            {
                TurnoTrabajoNegocio turnoNegocio = new TurnoTrabajoNegocio();
                ddlTurno.DataSource = turnoNegocio.Listar();
                ddlTurno.DataTextField = "Nombre";
                ddlTurno.DataValueField = "TurnoTrabajoId";
                ddlTurno.DataBind();
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error al cargar turnos: " + ex.Message;
                lblMensaje.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void CargarRecepcionistas()
        {
            try
            {
                RecepcionistaNegocio negocio = new RecepcionistaNegocio();
                gvRecepcionistas.DataSource = negocio.Listar();
                gvRecepcionistas.DataBind();
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error al listar: " + ex.Message;
                lblMensaje.ForeColor = System.Drawing.Color.Red;
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            try
            {
                Recepcionista nuevoRecepcionista = new Recepcionista();
                nuevoRecepcionista.Nombre = txtNombre.Text.Trim();
                nuevoRecepcionista.Apellido = txtApellido.Text.Trim();
                nuevoRecepcionista.Email = txtEmail.Text.Trim();
                nuevoRecepcionista.Telefono = string.IsNullOrWhiteSpace(txtTelefono.Text) ? null : txtTelefono.Text.Trim();

                if (!string.IsNullOrEmpty(ddlTurno.SelectedValue))
                    nuevoRecepcionista.TurnoTrabajoId = int.Parse(ddlTurno.SelectedValue);
                else
                    nuevoRecepcionista.TurnoTrabajoId = null;

                nuevoRecepcionista.Activo = chkActivo.Checked;

                RecepcionistaNegocio negocio = new RecepcionistaNegocio();
                UsuarioNegocio usuarioNegocio = new UsuarioNegocio();

                int idRecepcionistaGenerado = negocio.Agregar(nuevoRecepcionista);

                Usuario usuario = new Usuario();
                usuario.Email = txtEmail.Text.Trim();
                usuario.Pass = txtPass.Text.Trim();
                usuario.Perfil = Perfil.Recepcionista;
                usuario.Rol = "Recepcionista";
                usuario.Activo = true;
                usuario.Nombre = txtNombre.Text.Trim();
                usuario.Apellido = txtApellido.Text.Trim();
                usuario.IdRecepcionista = idRecepcionistaGenerado;

                usuarioNegocio.Agregar(usuario);

                lblMensaje.Text = "Recepcionista y Usuario creados exitosamente.";
                lblMensaje.ForeColor = System.Drawing.Color.Green;

                CargarRecepcionistas();
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                lblMensaje.Text = "Error: " + ex.Message;
                lblMensaje.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void LimpiarFormulario()
        {
            txtNombre.Text = "";
            txtApellido.Text = "";
            txtTelefono.Text = "";
            txtEmail.Text = "";
            txtPass.Text = "";
            ddlTurno.SelectedIndex = 0;
            chkActivo.Checked = true;
        }
    }
}