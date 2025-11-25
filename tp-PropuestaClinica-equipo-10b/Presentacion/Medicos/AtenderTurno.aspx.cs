using Clinica.Negocio;
using Clinica.Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Presentacion.Medicos
{
    public partial class AtenderTurno : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string id = Request.QueryString["id"];
                if (!string.IsNullOrEmpty(id))
                {
                    CargarDatos(int.Parse(id));
                }
            }
        }

        private void CargarDatos(int idTurno)
        {
            TurnoNegocio negocio = new TurnoNegocio();
            Turno turno = negocio.ObtenerPorId(idTurno);
            if (turno != null)
            {
                lblPaciente.Text = turno.Paciente.Apellido + ", " + turno.Paciente.Nombre + " (DNI: " + turno.Paciente.Dni + ")";
                lblHorario.Text = turno.FechaHoraInicio.ToString("dd/MM/yyyy HH:mm");
                lblMotivo.Text = turno.MotivoConsulta;
                txtDiagnostico.Text = turno.DiagnosticoMedico;

                // Configurar el DropDownList de estado
                string valorEstado = turno.Estado.ToString();// Convertir el enum a string
                if (valorEstado == "NoAsistio") valorEstado = "No Asistio";// Ajuste para coincidir con el valor del DropDown
                // Seleccionar el valor correspondiente en el DropDownList
                ddlEstado.SelectedValue = valorEstado;
            }
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                int idTurno = int.Parse(Request.QueryString["id"]);
                TurnoNegocio negocio = new TurnoNegocio();

                // Guardar
                negocio.GuardarDiagnostico(idTurno, ddlEstado.SelectedValue, txtDiagnostico.Text);

                // Redirigir a la agenda
                Response.Redirect("AgendaMedico.aspx");
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }
        }
    }
}