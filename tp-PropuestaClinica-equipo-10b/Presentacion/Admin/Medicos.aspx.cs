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

        protected void btnGuarda_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            try
            {
                Medico nuevo = new Medico();

                nuevo.Nombre = txtNombre.Text.Trim();
                nuevo.Apellido = txtApellido.Text.Trim();
                nuevo.Email = txtEmail.Text.Trim();
                nuevo.Telefono = string.IsNullOrWhiteSpace(txtTelefono.Text) ? null : txtTelefono.Text.Trim();
                nuevo.Matricula = string.IsNullOrWhiteSpace(txtMatricula.Text) ? null : txtMatricula.Text.Trim();

                int idTurno = int.Parse(ddlTurnoTrabajo.SelectedValue);
                nuevo.TurnoTrabajoId = idTurno;
                nuevo.Turno = new TurnoTrabajo { TurnoTrabajoId = idTurno };

                int cantidadSeleccionada = 0;
                nuevo.Especialidades = new List<Especialidad>();

                foreach (ListItem item in chkEspecialidades.Items)
                {
                    if (item.Selected)
                    {
                        cantidadSeleccionada++;
                        nuevo.Especialidades.Add(new Especialidad
                        {
                            EspecialidadId = int.Parse(item.Value),
                            Nombre = item.Text
                        });
                    }
                }

                if (cantidadSeleccionada == 0)
                {
                    throw new Exception("Debe seleccionar al menos una especialidad.");
                }

                if (cantidadSeleccionada > 2)
                {
                    throw new Exception("Solo puede seleccionar un máximo de 2 especialidades.");
                }

                nuevo.Activo = true;

                int idMedicoGenerado = medicoNegocio.Agregar(nuevo);

                UsuarioNegocio usuarioNegocio = new UsuarioNegocio();
                Usuario usuarioMedico = new Usuario
                {
                    Email = nuevo.Email,
                    Pass = txtPassword.Text.Trim(),
                    Perfil = Perfil.Medico,
                    Rol = "Medico",
                    IdMedico = idMedicoGenerado,
                    Nombre = nuevo.Nombre,
                    Apellido = nuevo.Apellido,
                    Activo = true
                };

                usuarioNegocio.Agregar(usuarioMedico);

                ValidarMedico.HeaderText = "Médico y Usuario creados correctamente.";
                CargarMedicos();
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                ValidarMedico.HeaderText = ex.Message;
            }
        }

        private void LimpiarFormulario()
        {
            txtNombre.Text = string.Empty;
            txtApellido.Text = string.Empty;
            txtTelefono.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtMatricula.Text = string.Empty;
            ddlTurnoTrabajo.SelectedIndex = 0;

            foreach (ListItem item in chkEspecialidades.Items)
            {
                item.Selected = false;
            }
        }
    }
}