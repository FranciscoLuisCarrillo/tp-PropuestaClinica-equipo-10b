using Clinica.Dominio;
using Clinica.Negocio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Presentacion.Pacientes
{
    public partial class ReservarTurno : System.Web.UI.Page
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
            // 1) Validación ASP.NET
            Page.Validate();
            if (!Page.IsValid) return;

            // 2) Validar sesión/usuario
            var usuarioActual = Session["usuario"] as Usuario;
            if (usuarioActual == null)
            {
                Response.Redirect("~/Login.aspx", false);
                return;
            }

            // 3) Validar que el usuario tenga Paciente asociado
            if (!usuarioActual.IdPaciente.HasValue)
            {
                ScriptManager.RegisterStartupScript(
                    this, GetType(), "errNoPaciente",
                    "window.__queueToast = window.__queueToast || []; " +
                    "__queueToast.push({ m: 'Tu usuario no está asociado a un paciente. Completá tu perfil.', t: 'warning', d: 1200, redirect: 'Add.aspx' });",
                    true
                );
                return;
            }

            try
            {
                // 4) Verificar datos obligatorios del perfil
                var pacienteNegocio = new PacienteNegocio();
                var datos = pacienteNegocio.ObtenerPorId(usuarioActual.IdPaciente.Value);
                if (datos == null || string.IsNullOrEmpty(datos.Dni) || datos.FechaNacimiento == DateTime.MinValue)
                {
                    ScriptManager.RegisterStartupScript(
                        this, GetType(), "faltanDatos",
                        "window.__queueToast = window.__queueToast || []; " +
                        "__queueToast.push({ m: 'Faltan datos en tu perfil. Por favor complétalos.', t: 'warning', d: 1200, redirect: 'Add.aspx' });",
                        true
                    );
                    return;
                }

                // 5) Validar y parsear Especialidad / Médico
                if (string.IsNullOrWhiteSpace(ddlEspecialidades.SelectedValue) ||
                    string.IsNullOrWhiteSpace(ddlMedico.SelectedValue))
                {
                    ScriptManager.RegisterStartupScript(
                        this, GetType(), "reqCombos",
                        "window.__queueToast = window.__queueToast || []; " +
                        "__queueToast.push({ m: 'Seleccioná especialidad y médico.', t: 'danger', d: 2000 });",
                        true
                    );
                    return;
                }

                if (!int.TryParse(ddlEspecialidades.SelectedValue, out int especialidadId))
                {
                    ScriptManager.RegisterStartupScript(
                        this, GetType(), "badEsp",
                        "window.__queueToast = window.__queueToast || []; " +
                        "__queueToast.push({ m: 'Especialidad inválida.', t: 'danger', d: 2000 });",
                        true
                    );
                    return;
                }

                if (!int.TryParse(ddlMedico.SelectedValue, out int medicoId))
                {
                    ScriptManager.RegisterStartupScript(
                        this, GetType(), "badMed",
                        "window.__queueToast = window.__queueToast || []; " +
                        "__queueToast.push({ m: 'Médico inválido.', t: 'danger', d: 2000 });",
                        true
                    );
                    return;
                }

                // 6) Validar y parsear Fecha
                if (string.IsNullOrWhiteSpace(txtFecha.Text))
                {
                    ScriptManager.RegisterStartupScript(
                        this, GetType(), "reqFecha",
                        "window.__queueToast = window.__queueToast || []; " +
                        "__queueToast.push({ m: 'Fecha requerida.', t: 'danger', d: 2000 });",
                        true
                    );
                    return;
                }

                if (!DateTime.TryParseExact(txtFecha.Text, "yyyy-MM-dd",
                                            System.Globalization.CultureInfo.InvariantCulture,
                                            System.Globalization.DateTimeStyles.None,
                                            out DateTime fechaBase))
                {
                    ScriptManager.RegisterStartupScript(
                        this, GetType(), "badFecha",
                        "window.__queueToast = window.__queueToast || []; " +
                        "__queueToast.push({ m: 'Formato de fecha inválido.', t: 'danger', d: 2000 });",
                        true
                    );
                    return;
                }

                // 7) Validar y parsear Hora
                if (ddlHora.SelectedIndex <= 0 || string.IsNullOrWhiteSpace(ddlHora.SelectedValue))
                {
                    ScriptManager.RegisterStartupScript(
                        this, GetType(), "reqHora",
                        "window.__queueToast = window.__queueToast || []; " +
                        "__queueToast.push({ m: 'Seleccioná una hora.', t: 'danger', d: 2000 });",
                        true
                    );
                    return;
                }

                if (!TimeSpan.TryParseExact(ddlHora.SelectedValue, "hh\\:mm",
                                            System.Globalization.CultureInfo.InvariantCulture, out TimeSpan horaSel))
                {
                    ScriptManager.RegisterStartupScript(
                        this, GetType(), "badHora",
                        "window.__queueToast = window.__queueToast || []; " +
                        "__queueToast.push({ m: 'Formato de hora inválido.', t: 'danger', d: 2000 });",
                        true
                    );
                    return;
                }

                // 8) Construir Turno
                var nuevoTurno = new Turno
                {
                    Medico = new Medico { Id = medicoId },
                    Especialidad = new Especialidad { EspecialidadId = especialidadId },
                    Paciente = new Paciente { PacienteId = usuarioActual.IdPaciente.Value },
                    FechaHoraInicio = fechaBase.Date.Add(horaSel),
                    FechaHoraFin = fechaBase.Date.Add(horaSel).AddHours(1),
                    MotivoConsulta = (txtObs.Text ?? string.Empty).Trim(),
                    Estado = EstadoTurno.Nuevo
                };

                // 9) No reservar en el pasado
                if (nuevoTurno.FechaHoraInicio < DateTime.Now)
                {
                    ScriptManager.RegisterStartupScript(
                        this, GetType(), "past",
                        "window.__queueToast = window.__queueToast || []; " +
                        "__queueToast.push({ m: 'No podés reservar en el pasado.', t: 'danger', d: 2200 });",
                        true
                    );
                    return;
                }

                // 10) Guardar
                var turnoNegocio = new TurnoNegocio();
                turnoNegocio.AgendarTurno(nuevoTurno);

                ScriptManager.RegisterStartupScript(
                    this, GetType(), "okReserva",
                    "window.__queueToast = window.__queueToast || []; " +
                    "__queueToast.push({ m: '¡Turno reservado con éxito!', t: 'success', d: 1500, redirect: 'Default.aspx' });",
                    true
                );
            }
            catch (System.Threading.ThreadAbortException)
            {
               
            }
            catch (Exception ex)
            {
              
                Session["error"] = ex.ToString(); 

                var msgUser = (ex.InnerException?.Message ?? ex.Message ?? "Error").Replace("'", "").Replace("\r", " ").Replace("\n", " ");

                ScriptManager.RegisterStartupScript(
                    this, GetType(), "errReserva",
                    "window.__queueToast = window.__queueToast || []; " +
                    $"__queueToast.push({{ m: 'Error al reservar: {msgUser}', t: 'danger', d: 4000 }});",
                    true
                );
            }
        }


    }
}