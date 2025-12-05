using Clinica.Dominio;
using Clinica.Negocio;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Presentacion.Pacientes
{
    public partial class MisTurnos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["usuario"] == null)
            {
                Response.Redirect("~/Login.aspx", false);
                return;
            }

            if (!IsPostBack)
            {
                CargarMisTurnos();
            }
        }

        private void CargarMisTurnos()
        {
            var usuario = (Usuario)Session["usuario"];
            var negocio = new TurnoNegocio();

            gvMisTurnos.DataSource = negocio.ListarPorPaciente((int)usuario.IdPaciente);
            gvMisTurnos.DataBind();
        }

        protected void gvMisTurnos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            // Redirección para Reprogramar
            if (e.CommandName == "Reprogramar")
            {
                string idTurno = e.CommandArgument.ToString();
                // Enviamos el parametro "reprogramar" con el ID del turno
                Response.Redirect($"ReservarTurno.aspx?reprogramar={idTurno}");
                return;
            }

            // Lógica de Cancelación existente
            if (e.CommandName == "Cancelar")
            {
                try
                {
                    int turnoId = Convert.ToInt32(e.CommandArgument);
                    var usuario = (Usuario)Session["usuario"];
                    var negocio = new TurnoNegocio();
                    var turno = negocio.ObtenerPorId(turnoId);

                    if (turno == null || turno.Paciente.PacienteId != usuario.IdPaciente)
                        throw new Exception("Turno inválido o no pertenece al usuario actual.");

                    if (turno.Estado != EstadoTurno.Nuevo && turno.Estado != EstadoTurno.Reprogramado)
                        throw new Exception("Solo se pueden cancelar turnos pendientes.");

                    negocio.CancelarTurno(turnoId);
                    CargarMisTurnos();

                    ScriptManager.RegisterStartupScript(
                        this, GetType(), "okCancel",
                        "window.__queueToast = window.__queueToast || []; " +
                        "__queueToast.push({ m:'Turno cancelado.', t:'warning', d:1800 });",
                        true
                    );
                }
                catch (Exception ex)
                {
                    var msg = (ex.Message ?? "Error al cancelar").Replace("'", "").Replace("\r", " ").Replace("\n", " ");
                    ScriptManager.RegisterStartupScript(
                        this, GetType(), "errCancel",
                        "window.__queueToast = window.__queueToast || []; " +
                        $"__queueToast.push({{ m:'No se pudo cancelar: {msg}', t:'danger', d:3000 }});",
                        true
                    );
                }
            }
        }
    }
}