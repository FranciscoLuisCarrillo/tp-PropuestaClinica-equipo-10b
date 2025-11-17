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
    public partial class TurnosTrabajo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                CargarTurnos();
            }
        }
        private void CargarTurnos()
        {
            try
            {
                var lista = new TurnoTrabajoNegocio().Listar();
                gvTurnosTrabajo.DataSource = lista;
                gvTurnosTrabajo.DataBind();

            }
            catch (Exception ex)
            {
                valTurno.CssClass = "alert alert-danger d-block mt-2";
                valTurno.HeaderText = "Error al cargar los turnos de trabajo: " + ex.Message;
            }
        }
        protected void btnGuardarTurno_Click(object sender, EventArgs e)
        {
            if(!Page.IsValid)
                return;
            try
            {
                if (!TimeSpan.TryParse(txtHoraEntrada.Text, out TimeSpan horaEntrada))
                {
                    valTurno.HeaderText = "Hora de entrada inválida.";
                    return;
                }
                if (!TimeSpan.TryParse(txtHoraSalida.Text, out TimeSpan horaSalida))
                {
                    valTurno.HeaderText = "Hora de salida inválida.";
                    return;
                }
                TurnoTrabajo nuevoTurno = new TurnoTrabajo
                {
                    Nombre = txtNombreTurno.Text.Trim(),
                    HoraEntrada = horaEntrada,
                    HoraSalida = horaSalida
                };
                TurnoTrabajoNegocio negocio = new TurnoTrabajoNegocio();
                negocio.Agregar(nuevoTurno);
                valTurno.CssClass = "alert alert-success d-block mt-2";
                valTurno.HeaderText = "Turno de trabajo agregado correctamente.";

                LimpiarFormulario();
                CargarTurnos();

            }
            catch (Exception ex)
            {
                valTurno.CssClass = "alert alert-danger d-block mt-2";
                valTurno.HeaderText = ex.Message;
            }
          }
        private void LimpiarFormulario()
            {
            txtNombreTurno.Text = "";
            txtHoraEntrada.Text = "";
            txtHoraSalida.Text = "";
        }
    }
}