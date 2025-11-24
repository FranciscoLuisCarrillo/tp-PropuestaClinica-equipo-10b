using Clinica.Dominio;
using Clinica.Negocio;
using Presentacion.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Presentacion.Pacientes
{
    public partial class Dashboard : System.Web.UI.Page
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
                CargarEspecialidades(); 
                CargarMedicos(true);    
                CargarHoras();

                
                txtFecha.Attributes["min"] = DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        private void CargarHoras()
        {
            ddlHora.Items.Clear();
            ddlHora.Items.Insert(0, new ListItem("-- Seleccionar hora --", ""));

            
            for (int h = 8; h <= 20; h++)
            {
                string hora = h.ToString("00") + ":00";
                ddlHora.Items.Add(new ListItem(hora, hora));
            }
        }

        
        private void CargarEspecialidades(List<Especialidad> listaFiltrada = null)
        {
            try
            {
                EspecialidadNegocio negocio = new EspecialidadNegocio();
                var lista = listaFiltrada ?? negocio.ListarTodas();

                ddlEspecialidades.DataSource = lista;
                ddlEspecialidades.DataTextField = "Nombre";
                ddlEspecialidades.DataValueField = "EspecialidadId";
                ddlEspecialidades.DataBind();

                ddlEspecialidades.Items.Insert(0, new ListItem("-- Seleccionar especialidad --", ""));
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
            }
        }

        
        private void CargarMedicos(bool cargarTodos = false, List<Medico> listaFiltrada = null)
        {
            try
            {
                MedicoNegocio negocio = new MedicoNegocio();
                List<Medico> lista;

                if (cargarTodos)
                    lista = negocio.Listar();
                else
                    lista = listaFiltrada ?? new List<Medico>();

                
                ddlMedico.DataSource = lista;
                ddlMedico.DataTextField = "Nombre"; 
                ddlMedico.DataValueField = "Id";    
                ddlMedico.DataBind();

                ddlMedico.Items.Insert(0, new ListItem("-- Seleccionar médico --", ""));
            }
            catch (Exception ex)
            {
                Session.Add("error", ex.ToString());
            }
        }


        protected void ddlMedico_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string especialidadPrevia = ddlEspecialidades.SelectedValue;

                if (ddlMedico.SelectedIndex == 0 || ddlMedico.SelectedValue == "")
                {
                    CargarEspecialidades();
                }
                else
                {
                    int idMedico = int.Parse(ddlMedico.SelectedValue);
                    EspecialidadNegocio negocio = new EspecialidadNegocio();
                    var listaEspecialidades = negocio.ListarPorMedico(idMedico);

                    CargarEspecialidades(listaFiltrada: listaEspecialidades);

                 
                    if (listaEspecialidades.Count == 1)
                    {
                        ddlEspecialidades.SelectedIndex = 1;
                    }
                }

                if (!string.IsNullOrEmpty(especialidadPrevia) && ddlEspecialidades.Items.FindByValue(especialidadPrevia) != null)
                {
                    ddlEspecialidades.SelectedValue = especialidadPrevia;
                }

            }
            catch (Exception ex)
            {
                Session.Add("error", "Error al filtrar especialidades: " + ex.Message);
            }
        }

        protected void ddlEspecialidades_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                
                string medicoPrevio = ddlMedico.SelectedValue;

                if (ddlEspecialidades.SelectedIndex == 0 || ddlEspecialidades.SelectedValue == "")
                {
                    CargarMedicos(true);
                }
                else
                {
                    int idEspecialidad = int.Parse(ddlEspecialidades.SelectedValue);
                    MedicoNegocio negocio = new MedicoNegocio();
                    var listaMedicos = negocio.ListarPorEspecialidad(idEspecialidad);

                    CargarMedicos(cargarTodos: false, listaFiltrada: listaMedicos);
                }

                
                if (!string.IsNullOrEmpty(medicoPrevio) && ddlMedico.Items.FindByValue(medicoPrevio) != null)
                {
                    ddlMedico.SelectedValue = medicoPrevio;
                }
            }
            catch (Exception ex)
            {
                Session.Add("error", "Error al filtrar médicos: " + ex.Message);
            }
        }


        protected void btnCrearTurno_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (!Page.IsValid) return;
            Usuario usuarioActual = (Usuario)Session ["usuario"];
            try
            {
                PacienteNegocio pacienteNegocio = new PacienteNegocio();
                Paciente datos = pacienteNegocio.ObtenerPorId((int)usuarioActual.IdPaciente);
                if(datos ==null || string.IsNullOrEmpty(datos.Dni) || datos.FechaNacimiento == null)
                {
                    Response.Write("<script>alert('Por favor complete sus datos personales antes de reservar un turno.'); window.location='Add.aspx';</script>");
                    return;
                }
            }catch (Exception ex)
            {
                Response.Write("<script>alert('Error al verificar datos personales: " + ex.Message + "');</script>");
                return;
            }




            if (ddlHora.SelectedIndex == 0 || string.IsNullOrEmpty(ddlHora.SelectedValue))
            {
                Response.Write("<script>alert('Por favor seleccione una hora.');</script>");
                return;
            }
            try
            {
                TurnoNegocio turnoNegocio = new TurnoNegocio();
                Turno nuevoTurno = new Turno();

                nuevoTurno.Medico = new Medico();
                nuevoTurno.Medico.Id = int.Parse(ddlMedico.SelectedValue);

                nuevoTurno.Especialidad = new Especialidad();
                nuevoTurno.Especialidad.EspecialidadId = int.Parse(ddlEspecialidades.SelectedValue);

                
                nuevoTurno.Paciente = new Paciente();
                nuevoTurno.Paciente.PacienteId = (int)usuarioActual.IdPaciente; 
                DateTime fecha = DateTime.Parse(txtFecha.Text);
                TimeSpan hora = TimeSpan.Parse(ddlHora.SelectedValue);
                
                nuevoTurno.FechaHoraInicio = fecha.Add(hora);
                nuevoTurno.FechaHoraFin = nuevoTurno.FechaHoraInicio.AddHours(1);

                nuevoTurno.MotivoConsulta = txtObs.Text;
                nuevoTurno.Estado = EstadoTurno.Nuevo;

                turnoNegocio.AgendarTurno(nuevoTurno);

                Response.Write("<script>alert('¡Turno reservado con éxito!'); window.location='Dashboard.aspx';</script>");
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error al reservar: " + ex.Message + "');</script>");
            }
        }
    }
}