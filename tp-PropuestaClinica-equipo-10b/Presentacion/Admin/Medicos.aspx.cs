using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Clinica.Dominio;
using Clinica.Negocio;

namespace Presentacion.Admin
{
    public partial class Medicos : System.Web.UI.Page
    {
        private readonly MedicoNegocio medicoNegocio = new MedicoNegocio();
        private readonly EspecialidadNegocio especialidadNegocio = new EspecialidadNegocio();
        private readonly TurnoTrabajoNegocio turnoTrabajoNegocio = new TurnoTrabajoNegocio();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarTurnos();
                CargarMedicos();
                CargarEspecialidades();
            }
        }

        // Password obligatoria solo en ALTAS
        protected void Page_PreRender(object sender, EventArgs e)
        {
            rfvPassword.Enabled = string.IsNullOrWhiteSpace(hfMedicoId.Value);
        }

        private void CargarTurnos()
        {
            try
            {
                ddlTurnoTrabajo.DataTextField = "Nombre";
                ddlTurnoTrabajo.DataValueField = "TurnoTrabajoId";
                ddlTurnoTrabajo.DataSource = turnoTrabajoNegocio.Listar();
                ddlTurnoTrabajo.DataBind();
            }
            catch (Exception ex)
            {
                ValidarMedico.HeaderText = "Error al cargar turnos: " + ex.Message;
            }
        }

        private void CargarMedicos()
        {
            try
            {
                var lista = medicoNegocio.Listar();
                gvMedicos.DataSource = lista;
                gvMedicos.DataBind();
            }
            catch (Exception ex)
            {
                ValidarMedico.HeaderText = "Error al cargar médicos: " + ex.Message;
            }
        }

        private void CargarEspecialidades()
        {
            try
            {
                var lista = especialidadNegocio.ListarTodas();
                chkEspecialidades.DataSource = lista;
                chkEspecialidades.DataTextField = "Nombre";
                chkEspecialidades.DataValueField = "EspecialidadId";
                chkEspecialidades.DataBind();
            }
            catch (Exception ex)
            {
                ValidarMedico.HeaderText = "Error al cargar especialidades: " + ex.Message;
            }
        }

        // Validador servidor: 1..2 especialidades
        protected void cvEspecialidades_ServerValidate(object source, ServerValidateEventArgs args)
        {
            int cant = 0;
            foreach (ListItem item in chkEspecialidades.Items)
                if (item.Selected) cant++;

            args.IsValid = (cant >= 1 && cant <= 2);
        }

        protected void btnGuarda_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                ClientScript.RegisterStartupScript(
                this.GetType(), "ShowFormEdit",
                "if (window.mostrarFormularioMedico) { mostrarFormularioMedico(); }", true);
                return;
            }

            try
            {
                var m = new Medico
                {
                    Nombre = txtNombre.Text.Trim(),
                    Apellido = txtApellido.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Telefono = string.IsNullOrWhiteSpace(txtTelefono.Text) ? null : txtTelefono.Text.Trim(),
                    Matricula = string.IsNullOrWhiteSpace(txtMatricula.Text) ? null : txtMatricula.Text.Trim(),
                    TurnoTrabajoId = string.IsNullOrEmpty(ddlTurnoTrabajo.SelectedValue) ? (int?)null : int.Parse(ddlTurnoTrabajo.SelectedValue),
                    Activo = true,
                    Especialidades = new List<Especialidad>()
                };

                foreach (ListItem item in chkEspecialidades.Items)
                    if (item.Selected)
                        m.Especialidades.Add(new Especialidad { EspecialidadId = int.Parse(item.Value), Nombre = item.Text });

                bool esEdicion = !string.IsNullOrWhiteSpace(hfMedicoId.Value);

                if (esEdicion)
                {
                    m.Id = int.Parse(hfMedicoId.Value);
                    medicoNegocio.Modificar(m);
                    ClientScript.RegisterStartupScript(this.GetType(), "okUpd", "alert('Médico actualizado.');", true);
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(txtPassword.Text))
                        throw new Exception("La contraseña es obligatoria para crear el usuario del médico.");

                    int idMedico = medicoNegocio.Agregar(m);

                    var usuarioNegocio = new UsuarioNegocio();
                    usuarioNegocio.Agregar(new Usuario
                    {
                        Email = m.Email,
                        Pass = txtPassword.Text.Trim(),
                        Perfil = Perfil.Medico,
                        Rol = "Medico",
                        IdMedico = idMedico,
                        Nombre = m.Nombre,
                        Apellido = m.Apellido,
                        Activo = true
                    });

                    ClientScript.RegisterStartupScript(this.GetType(), "okAdd", "alert('Médico y usuario creados.');", true);
                }

                CargarMedicos();
                LimpiarFormulario();
                ClientScript.RegisterStartupScript(this.GetType(), "HideForm",
                "if (window.ocultarFormularioMedico){ocultarFormularioMedico();}", true);
            }
            catch (Exception ex)
            {
                string msg = ex.Message.Replace("'", "").Replace("\r", "").Replace("\n", " ");
                ValidarMedico.HeaderText = msg;
                ClientScript.RegisterStartupScript(
                this.GetType(), "ShowFormEdit",
                "if (window.mostrarFormularioMedico) { mostrarFormularioMedico(); }", true);
            }
        }

        private void LimpiarFormulario()
        {
            hfMedicoId.Value = string.Empty;
            txtNombre.Text = txtApellido.Text = txtTelefono.Text = txtEmail.Text = txtPassword.Text = txtMatricula.Text = string.Empty;
            ddlTurnoTrabajo.SelectedIndex = 0;
            foreach (ListItem item in chkEspecialidades.Items) item.Selected = false;
            btnGuarda.Text = "Guardar médico";
            rfvPassword.Enabled = true;
        }

        protected void gvMedicos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                int medicoId = int.Parse(e.CommandArgument.ToString());
                CargarFormularioEdicion(medicoId);
                ClientScript.RegisterStartupScript(
                this.GetType(), "ShowFormEdit",
                "if (window.mostrarFormularioMedico) { mostrarFormularioMedico(); }", true);
            }
        }

        private void CargarFormularioEdicion(int medicoId)
        {
            try
            {
                var m = medicoNegocio.ObtenerPorId(medicoId);
                if (m == null) { ValidarMedico.HeaderText = "No se encontró el médico."; return; }

                hfMedicoId.Value = m.Id.ToString();
                txtNombre.Text = m.Nombre;
                txtApellido.Text = m.Apellido;
                txtTelefono.Text = m.Telefono ?? "";
                txtEmail.Text = m.Email ?? "";
                txtMatricula.Text = m.Matricula ?? "";

                ddlTurnoTrabajo.ClearSelection();
                if (m.TurnoTrabajoId.HasValue)
                {
                    var it = ddlTurnoTrabajo.Items.FindByValue(m.TurnoTrabajoId.Value.ToString());
                    if (it != null) it.Selected = true;
                }

                foreach (ListItem it in chkEspecialidades.Items) it.Selected = false;
                if (m.Especialidades != null)
                    foreach (var esp in m.Especialidades)
                    {
                        var it = chkEspecialidades.Items.FindByValue(esp.EspecialidadId.ToString());
                        if (it != null) it.Selected = true;
                    }

                btnGuarda.Text = "Guardar cambios";
                rfvPassword.Enabled = false;
            }
            catch (Exception ex)
            {
                ValidarMedico.HeaderText = "Error al cargar médico: " + ex.Message;
            }
        }
    }
}
