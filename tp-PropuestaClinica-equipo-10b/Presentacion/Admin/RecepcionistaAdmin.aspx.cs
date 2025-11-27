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
                // 1. Crear Objeto Recepcionista (Datos de negocio)
                Recepcionista nuevoRecepcionista = new Recepcionista();
                nuevoRecepcionista.Nombre = txtNombre.Text.Trim();
                nuevoRecepcionista.Apellido = txtApellido.Text.Trim();
                nuevoRecepcionista.Email = txtEmail.Text.Trim();

                // Manejo de nulos para teléfono
                nuevoRecepcionista.Telefono = string.IsNullOrWhiteSpace(txtTelefono.Text) ? null : txtTelefono.Text.Trim();

                // Manejo de nulos para el DropDown
                if (!string.IsNullOrEmpty(ddlTurno.SelectedValue))
                    nuevoRecepcionista.TurnoTrabajoId = int.Parse(ddlTurno.SelectedValue);
                else
                    nuevoRecepcionista.TurnoTrabajoId = null;

                nuevoRecepcionista.Activo = chkActivo.Checked;

                // 2. Instanciar Negocios
                RecepcionistaNegocio negocio = new RecepcionistaNegocio();
                UsuarioNegocio usuarioNegocio = new UsuarioNegocio();

                // 3. Guardar Recepcionista y OBTENER EL ID
                int idRecepcionistaGenerado = negocio.Agregar(nuevoRecepcionista);

                // 4. Guardar Usuario (Datos de Login) - ACTUALIZADO SEGÚN TU CLASE
                Usuario usuario = new Usuario();

                // Asignaciones básicas
                usuario.Email = txtEmail.Text.Trim();
                usuario.Password = txtPass.Text.Trim(); // CORREGIDO: Usamos 'Password' no 'Pass'
                usuario.Perfil = Perfil.Recepcionista;

                // Asignaciones nuevas gracias a tu clase actualizada
                usuario.Rol = "Recepcionista";
                usuario.Activo = true;
                usuario.Nombre = txtNombre.Text.Trim(); // Opcional: duplicar nombre en usuario
                usuario.Apellido = txtApellido.Text.Trim(); // Opcional: duplicar apellido en usuario

                // VINCULACIÓN IMPORTANTE: Guardamos el ID que acabamos de generar
                usuario.IdRecepcionista = idRecepcionistaGenerado;

                // Guardamos el usuario
                usuarioNegocio.Agregar(usuario);

                // 5. Feedback y Limpieza
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