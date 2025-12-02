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

             
                if (turnoNegocio.ExisteTurnoEnHorario(medicoId, fechaHora))
                    throw new Exception("Ese horario ya está ocupado para el médico seleccionado.");

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

                ScriptManager.RegisterStartupScript(this, GetType(), "okAlta",
                    "alert('Turno creado correctamente.');", true);
            }
            catch (Exception ex)
            {
                valAlta.HeaderText = ex.Message;
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
        }

        protected void btnBuscarTurnosDia_Click(object sender, EventArgs e)
        {
            BindTurnosDelDia();
        }

        // ================== REPROG/CANCEL ==================
        protected void gvTurnosDia_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Reprogramar")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                var keys = gvTurnosDia.DataKeys[index];
                int idTurno = (int)keys["IdTurno"];
                hfReprogId.Value = idTurno.ToString();

                // por default: nueva fecha = misma fecha listada
                txtReprogFecha.Text = txtFechaListado.Text;
                ddlReprogHora.SelectedIndex = 0;

                ScriptManager.RegisterStartupScript(this, GetType(), "showReprog", "abrirModalReprog();", true);
            }
            else if (e.CommandName == "Cancelar")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                var keys = gvTurnosDia.DataKeys[index];
                int idTurno = (int)keys["IdTurno"];

                turnoNegocio.CancelarTurno(idTurno);
                BindTurnosDelDia();
            }
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
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "reprogErr",
                    $"alert('{ex.Message.Replace("'", "")}');", true);
            }
        }
    }
}
