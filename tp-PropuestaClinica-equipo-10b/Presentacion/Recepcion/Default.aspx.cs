using Clinica.Dominio;
using Clinica.Negocio;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Presentacion.Recepcion
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly PacienteNegocio pacienteNegocio = new PacienteNegocio();
        private readonly EspecialidadNegocio especialidadNegocio = new EspecialidadNegocio();
        private readonly MedicoNegocio medicoNegocio = new MedicoNegocio();
        private readonly TurnoNegocio turnoNegocio = new TurnoNegocio();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarPacientes();
                CargarEspecialidades();
                CargarMedicos();  
                CargarHoras(ddlHora);
                CargarHoras(ddlReprogHora);

               
                txtFecha.Text = DateTime.Today.ToString("yyyy-MM-dd");
                txtFechaListado.Text = DateTime.Today.ToString("yyyy-MM-dd");
                BindTurnosDelDia();
            }
        }

        private void CargarPacientes()
        {
            ddlPaciente.Items.Clear();
            ddlPaciente.Items.Insert(0, new ListItem("-- Seleccionar --", ""));
            foreach (var p in pacienteNegocio.Listar())
                ddlPaciente.Items.Add(new ListItem($"{p.Apellido}, {p.Nombre}", p.PacienteId.ToString()));
        }

        private void CargarEspecialidades()
        {
            ddlEspecialidad.Items.Clear();
            ddlEspecialidad.Items.Insert(0, new ListItem("-- Seleccionar --", ""));
            foreach (var e in especialidadNegocio.ListarTodas())
                ddlEspecialidad.Items.Add(new ListItem(e.Nombre, e.EspecialidadId.ToString()));
        }

        private void CargarMedicos()
        {
            ddlMedico.Items.Clear();
            ddlMedico.Items.Insert(0, new ListItem("-- Seleccionar --", ""));

            int espId;

            
            if (int.TryParse(ddlEspecialidad.SelectedValue, out espId) && espId > 0)
            {
                var medicos = medicoNegocio.ListarPorEspecialidad(espId);

                foreach (var m in medicos)
                    ddlMedico.Items.Add(new ListItem($"{m.Apellido}, {m.Nombre}", m.Id.ToString()));

                return;
            }

            var todos = medicoNegocio.ListarPorEspecialidad(0);

            foreach (var m in todos)
                ddlMedico.Items.Add(new ListItem($"{m.Apellido}, {m.Nombre}", m.Id.ToString()));
        }

        private void CargarHoras(DropDownList ddl)
        {
            ddl.Items.Clear();
            ddl.Items.Insert(0, new ListItem("-- Seleccionar --", ""));
            for (int h = 8; h <= 20; h++) 
                ddl.Items.Add(new ListItem($"{h:00}:00", $"{h:00}:00"));
        }

        protected void ddlEspecialidad_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarMedicos();
            
            ddlHora.Items.Clear();
            ddlHora.Items.Insert(0, new ListItem("-- Seleccionar --", ""));
        }
        protected void ddlMedico_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            CargarHorasDisponibles();
        }
        private void CargarHorasDisponibles()
        {
            ddlHora.Items.Clear();
            ddlHora.Items.Insert(0, new ListItem("-- Seleccionar --", ""));

            if (!int.TryParse(ddlMedico.SelectedValue, out int medicoId) || medicoId <= 0)
                return;

            if (!DateTime.TryParse(txtFecha.Text, out DateTime fecha))
                return;

            
            var libres = turnoNegocio.HorasDisponibles(medicoId, fecha);
            foreach (var hhmm in libres)
                ddlHora.Items.Add(new ListItem(hhmm, hhmm));
        }
        protected void txtFecha_TextChanged(object sender, EventArgs e)
        {
            CargarHorasDisponibles();
        }

        
        // ================== ALTA ==================
        protected void btnCrearTurno_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid) return;

                int pacienteId = int.Parse(ddlPaciente.SelectedValue);
                int medicoId = int.Parse(ddlMedico.SelectedValue);
                int especialidadId = int.Parse(ddlEspecialidad.SelectedValue);

                var fecha = DateTime.Parse(txtFecha.Text);
                var hora = TimeSpan.Parse(ddlHora.SelectedValue);
                var fechaHora = fecha.Date + hora;

                
                if (turnoNegocio.ExisteTurnoPacienteEnHorario(pacienteId, fechaHora))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "dupPac",
                        "window.__queueToast = window.__queueToast || []; " +
                        "__queueToast.push({ m: 'Ese paciente ya tiene un turno en ese horario.', t: 'warning', d: 3000 });",
                        true
                    );
                    return;
                }

                if (turnoNegocio.ExisteTurnoEnHorario(medicoId, fechaHora))
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "dupMed",
                        "window.__queueToast = window.__queueToast || []; " +
                        "__queueToast.push({ m: 'Ese horario ya está ocupado para el médico.', t: 'danger', d: 3000 });",
                        true
                    );
                    return;
                }

                var t = new Turno
                {
                    Paciente = new Paciente { PacienteId = pacienteId },
                    Medico = new Medico { Id = medicoId },
                    Especialidad = new Especialidad { EspecialidadId = especialidadId },
                    FechaHoraInicio = fechaHora,
                    MotivoConsulta = string.IsNullOrWhiteSpace(txtObs.Text) ? null : txtObs.Text.Trim()
                };

                turnoNegocio.AgendarTurno(t);

                BindTurnosDelDia();
                txtObs.Text = "";
                ddlHora.SelectedIndex = 0;

                ScriptManager.RegisterStartupScript(
                    this, GetType(), "okAlta",
                    "window.__queueToast = window.__queueToast || []; " +
                    "__queueToast.push({ m: 'Turno creado correctamente.', t: 'success', d: 1800 });",
                    true
                );
            }
            catch (InvalidOperationException iox)
            {
                var msg = (iox.Message ?? "Error").Replace("'", "").Replace("\r", " ").Replace("\n", " ");
                valAlta.HeaderText = msg;
                ScriptManager.RegisterStartupScript(
                    this, GetType(), "bizAlta",
                    "window.__queueToast = window.__queueToast || []; " +
                    $"__queueToast.push({{ m: '{msg}', t: 'warning', d: 3200 }});",
                    true
                );
            }
            catch (Exception ex)
            {
                var msg = (ex.Message ?? "Error").Replace("'", "").Replace("\r", " ").Replace("\n", " ");
                valAlta.HeaderText = msg;
                ScriptManager.RegisterStartupScript(
                    this, GetType(), "errAlta",
                    "window.__queueToast = window.__queueToast || []; " +
                    $"__queueToast.push({{ m: 'Error al crear turno: {msg}', t: 'danger', d: 3000 }});",
                    true
                );
            }
        }

        // ================== LISTADO DÍA ==================
        private void BindTurnosDelDia()
        {
            var fecha = string.IsNullOrWhiteSpace(txtFechaListado.Text)
                ? DateTime.Today : DateTime.Parse(txtFechaListado.Text);

            var lista = turnoNegocio.ListarResumenPorFecha(fecha);
            gvTurnosDia.DataSource = lista;
            gvTurnosDia.DataBind();
            if (gvTurnosDia.HeaderRow != null)
            {
                gvTurnosDia.UseAccessibleHeader = true;
                gvTurnosDia.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void btnBuscarTurnosDia_Click(object sender, EventArgs e)
        {
            BindTurnosDelDia();
        }

        // ================== REPROG/CANCEL ==================
        protected void gvTurnosDia_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Reprogramar" && e.CommandName != "Cancelar") return;

            int index = Convert.ToInt32(e.CommandArgument);
            int idTurno = (int)gvTurnosDia.DataKeys[index]["TurnoId"];

            var turno = turnoNegocio.ObtenerPorId(idTurno);
            if (turno == null)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "noTurno",
                    "window.__queueToast = window.__queueToast || []; __queueToast.push({ m:'No se encontró el turno.', t:'danger', d:2500 });", true);
                return;
            }

            // Bloqueo por estado
            if (turno.Estado == EstadoTurno.Cerrado || turno.Estado == EstadoTurno.Cancelado)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "bloqueado",
                    "window.__queueToast = window.__queueToast || []; __queueToast.push({ m:'No se puede reprogramar/cancelar un turno cerrado o cancelado.', t:'warning', d:3000 });", true);
                return;
            }

            if (e.CommandName == "Reprogramar")
            {
                hfReprogId.Value = idTurno.ToString();
                txtReprogFecha.Text = txtFechaListado.Text;
                ddlReprogHora.SelectedIndex = 0;

                ScriptManager.RegisterStartupScript(this, GetType(), "showReprog",
                    "window.addEventListener('load', function(){ abrirModalReprog(); }, { once:true });", true);
            }
            else // Cancelar
            {
                turnoNegocio.CancelarTurno(idTurno);
                BindTurnosDelDia();
                ScriptManager.RegisterStartupScript(this, GetType(), "okCancel",
                    "window.__queueToast = window.__queueToast || []; __queueToast.push({ m:'Turno cancelado.', t:'warning', d:1800 });", true);
            }
        }
        protected void gvTurnosDia_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            var estado = DataBinder.Eval(e.Row.DataItem, "Estado") as string; // "Nuevo", "Cerrado", etc.
            bool bloquear = string.Equals(estado, "Cerrado", StringComparison.OrdinalIgnoreCase)
                         || string.Equals(estado, "Cancelado", StringComparison.OrdinalIgnoreCase);

          
            var btnReprog = e.Row.Cells[5].Controls.Count > 0 ? e.Row.Cells[5].Controls[0] as Button : null;
            var btnCancel = e.Row.Cells[6].Controls.Count > 0 ? e.Row.Cells[6].Controls[0] as Button : null;

            if (btnReprog != null) btnReprog.Enabled = !bloquear;
            if (btnCancel != null) btnCancel.Enabled = !bloquear;
        }
        protected void btnConfirmarReprog_Click(object sender, EventArgs e)
        {
            try
            {
                int turnoId = int.Parse(hfReprogId.Value);
                var turnoOrigen = turnoNegocio.ObtenerPorId(turnoId);
                if (turnoOrigen == null)
                    throw new Exception("No se encontró el turno a reprogramar.");

                var nuevaFecha = DateTime.Parse(txtReprogFecha.Text);
                var nuevaHora = TimeSpan.Parse(ddlReprogHora.SelectedValue);
                var fechaHoraNueva = nuevaFecha.Date + nuevaHora;

                turnoNegocio.ReprogramarTurno(turnoId, fechaHoraNueva, turnoOrigen.Medico.Id);

                BindTurnosDelDia();
                ScriptManager.RegisterStartupScript(
                this, GetType(), "okReprog",
                "window.__queueToast = window.__queueToast || []; " +
                "__queueToast.push({ m: 'Turno reprogramado.', t: 'success', d: 1800 });" +
                "var m = (window.bootstrap && document.getElementById('mdlReprog')) ? bootstrap.Modal.getInstance(document.getElementById('mdlReprog')) : null;" +
                "if (m) m.hide();",
                true
            );
            }
            catch (Exception ex)
            {
                var msg = (ex.Message ?? "Error").Replace("'", "").Replace("\r", " ").Replace("\n", " ");
                ScriptManager.RegisterStartupScript(
                    this, GetType(), "reprogErr",
                    "window.__queueToast = window.__queueToast || []; " +
                    $"__queueToast.push({{ m: 'Error al reprogramar: {msg}', t: 'danger', d: 3500 }});",
                    true
                );
            }
        }
    }
}
