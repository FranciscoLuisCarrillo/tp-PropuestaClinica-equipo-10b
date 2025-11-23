using Clinica.Dominio;
using Clinica.Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Presentacion.Medicos
{
    public partial class Default : System.Web.UI.Page
    {
        private List<TurnoAgendaMedico> AgendaDelDia
        {
            get => ViewState["AgendaDelDia"] as List<TurnoAgendaMedico>;
            set => ViewState["AgendaDelDia"] = value;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            

            if (!IsPostBack)
            {
                txtFecha.Text = DateTime.Now.ToString("yyyy-MM-dd");
                CargarAgenda();
            }

        }
        private int MedicoIdActual()
        {
            return Convert.ToInt32(Session["MedicoId"]);
        }
        private DateTime FechaSeleccionada()
        {
            if (DateTime.TryParse(txtFecha.Text, out var f)) return f.Date;
            return DateTime.Today;
        }
        protected void btnVerFecha_Click(object sender, EventArgs e)
        {
            CargarAgenda();
        }
        protected void btnGuardarDiagnostico_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(hfTurnoId.Value))
                    return;

                int turnoId = int.Parse(hfTurnoId.Value);
                string estadoTexto = ddlEstadoTurno.SelectedValue; // “Nuevo”, “Cancelado”, etc.
                string diagnostico = (txtDiagnostico.Text ?? "").Trim();

                var turnoNeg = new TurnoNegocio();
                turnoNeg.GuardarDiagnostico(turnoId, estadoTexto, diagnostico);

                lblMsg.Text = "Turno actualizado correctamente.";
                lblMsg.CssClass = "text-success d-block mt-2";

                CargarAgenda();
                pnlDetalle.Visible = false;
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error al guardar: " + ex.Message;
                lblMsg.CssClass = "text-danger d-block mt-2";
            }
        }
        private void CargarAgenda()
        {
            try
            {
                TurnoNegocio turnoNegocio = new TurnoNegocio();
                int medicoId = MedicoIdActual();
                DateTime fecha = FechaSeleccionada();
                var agenda = turnoNegocio.ListarAgendaMedico(medicoId, fecha) ?? new List<TurnoAgendaMedico>();
                AgendaDelDia = agenda;

                gvTurnos.DataSource = agenda.Select(lista => new
                {
                    lista.IdTurno,
                    lista.Hora,
                    lista.Paciente,
                    lista.ObraSocial,
                    lista.Especialidad,
                    Estado = lista.Estado,
                    lista.Diagnostico
                }).ToList();
                gvTurnos.DataBind();
                pnlDetalle.Visible = false;
                lblMsg.Text = "";
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error cargando agenda: " + ex.Message;
                lblMsg.CssClass = "text-danger d-block mt-2";
            }
        }
        protected void gvTurnos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                int idTurno = Convert.ToInt32(gvTurnos.DataKeys[rowIndex].Value);

                var turno = AgendaDelDia.FirstOrDefault(t => t.IdTurno == idTurno);
                if (turno == null) return;

                hfTurnoId.Value = idTurno.ToString();
                lblPaciente.Text = turno.Paciente;

                lblFechaHora.Text = $"{FechaSeleccionada():dd/MM/yyyy} {turno.Hora}";
                lblEspecialidad.Text = turno.Especialidad;

                var estados = valorEstadoTexto(turno.Estado);

                var item = ddlEstadoTurno.Items.FindByValue(estados);
                if(item != null) ddlEstadoTurno.ClearSelection();
                if(item != null) item.Selected = true;

                txtDiagnostico.Text = turno.Diagnostico ?? string.Empty;

                pnlDetalle.Visible = true;
                lblMsg.Text = string.Empty;
                lblMsg.CssClass = "d-block mt-2";

            }
        }

        private string valorEstadoTexto(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto)) return "Nuevo";
            texto = texto.Trim();
            if (texto.Equals("No Asistió", StringComparison.OrdinalIgnoreCase)) return "No Asistio";
            return texto;
        }
    }
}