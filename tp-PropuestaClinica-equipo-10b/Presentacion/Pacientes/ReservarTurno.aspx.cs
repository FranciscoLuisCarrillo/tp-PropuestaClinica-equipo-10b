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
        // Propiedad para guardar el ID si estamos reprogramando
        public int? IdTurnoReprogramar
        {
            get { return (int?)ViewState["IdTurnoReprogramar"]; }
            set { ViewState["IdTurnoReprogramar"] = value; }
        }

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
                CargarHorasBase();

                txtFecha.Attributes["min"] = DateTime.Now.ToString("yyyy-MM-dd");

                // Verificamos si venimos a REPROGRAMAR
                string idReprog = Request.QueryString["reprogramar"];
                if (!string.IsNullOrEmpty(idReprog) && int.TryParse(idReprog, out int idTurno))
                {
                    PrepararModoReprogramacion(idTurno);
                }
            }
        }

        private void PrepararModoReprogramacion(int idTurno)
        {
            var turnoNegocio = new TurnoNegocio();
            var turno = turnoNegocio.ObtenerPorId(idTurno);
            var usuario = (Usuario)Session["usuario"];

            // Validaciones de seguridad básicas
            if (turno == null || turno.Paciente.PacienteId != usuario.IdPaciente)
            {
                Response.Redirect("MisTurnos.aspx");
                return;
            }

            // Guardamos el ID en ViewState para usarlo al dar click en Confirmar
            IdTurnoReprogramar = idTurno;

            // Ajustes Visuales
            lblTitulo.Text = "Reprogramar Turno";
            btnCrearTurno.Text = "Confirmar Reprogramación";
            btnCrearTurno.CssClass = "btn btn-warning px-4 text-dark"; // Cambiar color para diferenciar

            // Pre-seleccionar valores del turno actual
            if (ddlEspecialidades.Items.FindByValue(turno.Especialidad.EspecialidadId.ToString()) != null)
            {
                ddlEspecialidades.SelectedValue = turno.Especialidad.EspecialidadId.ToString();
                // Forzamos la carga de médicos filtrados
                ddlEspecialidades_SelectedIndexChanged(null, null);
            }

            if (ddlMedico.Items.FindByValue(turno.Medico.Id.ToString()) != null)
            {
                ddlMedico.SelectedValue = turno.Medico.Id.ToString();
                // Forzamos la carga de horario del médico y rango
                ddlMedico_SelectedIndexChanged(null, null);
            }

            // Llenamos observaciones previas
            if (!string.IsNullOrEmpty(turno.MotivoConsulta))
                txtObs.Text = turno.MotivoConsulta;

            // Nota: No preseleccionamos fecha/hora porque la idea es cambiarlas, 
            // pero el usuario ya ve "Médico" y "Especialidad" listos.
        }

        private void CargarHorasBase()
        {
            ddlHora.Items.Clear();
            ddlHora.Items.Insert(0, new ListItem("-- Seleccionar hora --", ""));
        }

        private void CargarHorasDisponibles()
        {
            ddlHora.Items.Clear();
            ddlHora.Items.Insert(0, new ListItem("-- Seleccionar hora --", ""));

            if (!int.TryParse(ddlMedico.SelectedValue, out int medicoId) || medicoId <= 0)
                return;

            if (!DateTime.TryParse(txtFecha.Text, out DateTime fecha))
                return;

            var turnoNegocio = new TurnoNegocio();
            var libres = turnoNegocio.HorasDisponibles(medicoId, fecha);

            foreach (var hhmm in libres)
                ddlHora.Items.Add(new ListItem(hhmm, hhmm));
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

                if (cargarTodos) lista = negocio.Listar();
                else lista = listaFiltrada ?? new List<Medico>();

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
                lblRangoHorario.Text = "";
                string especialidadPrevia = ddlEspecialidades.SelectedValue;

                if (ddlMedico.SelectedIndex == 0 || ddlMedico.SelectedValue == "")
                {
                    CargarEspecialidades();
                }
                else
                {
                    int idMedico = int.Parse(ddlMedico.SelectedValue);

                    // Mostrar Rango Horario
                    MedicoNegocio medNegocio = new MedicoNegocio();
                    Medico medico = medNegocio.ObtenerPorId(idMedico);

                    TurnoTrabajoNegocio ttNegocio = new TurnoTrabajoNegocio();
                    var listaTurnos = ttNegocio.Listar();
                    var turnoTrabajo = listaTurnos.Find(t => t.TurnoTrabajoId == medico.TurnoTrabajoId);

                    if (turnoTrabajo != null)
                    {
                        lblRangoHorario.Text = $"Atiende de {turnoTrabajo.HoraEntrada:hh\\:mm} a {turnoTrabajo.HoraSalida:hh\\:mm}";
                    }

                    EspecialidadNegocio negocio = new EspecialidadNegocio();
                    var listaEspecialidades = negocio.ListarPorMedico(idMedico);

                    CargarEspecialidades(listaFiltrada: listaEspecialidades);

                    if (listaEspecialidades.Count == 1)
                        ddlEspecialidades.SelectedIndex = 1;
                }

                if (!string.IsNullOrEmpty(especialidadPrevia) && ddlEspecialidades.Items.FindByValue(especialidadPrevia) != null)
                {
                    ddlEspecialidades.SelectedValue = especialidadPrevia;
                }
                CargarHorasDisponibles();
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
                lblRangoHorario.Text = "";
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

                    // Restaurar label de rango si se mantuvo el médico
                    int idMedico = int.Parse(medicoPrevio);
                    MedicoNegocio medNegocio = new MedicoNegocio();
                    Medico medico = medNegocio.ObtenerPorId(idMedico);
                    TurnoTrabajoNegocio ttNegocio = new TurnoTrabajoNegocio();
                    var listaTurnos = ttNegocio.Listar();
                    var turnoTrabajo = listaTurnos.Find(t => t.TurnoTrabajoId == medico.TurnoTrabajoId);
                    if (turnoTrabajo != null)
                        lblRangoHorario.Text = $"Atiende de {turnoTrabajo.HoraEntrada:hh\\:mm} a {turnoTrabajo.HoraSalida:hh\\:mm}";
                }
                CargarHorasDisponibles();
            }
            catch (Exception ex)
            {
                Session.Add("error", "Error al filtrar médicos: " + ex.Message);
            }
        }

        protected void txtFecha_TextChanged(object sender, EventArgs e)
        {
            CargarHorasDisponibles();
        }

        protected void btnCrearTurno_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (!Page.IsValid) return;

            var usuarioActual = Session["usuario"] as Usuario;
            if (usuarioActual == null) { Response.Redirect("~/Login.aspx", false); return; }

            if (!usuarioActual.IdPaciente.HasValue)
            {
                GuardarReservaPendienteYIrACompletarPerfil("Tu usuario no está asociado a un paciente. Completá tu perfil.");
                return;
            }

            try
            {
                // Parseos básicos
                int especialidadId = int.Parse(ddlEspecialidades.SelectedValue);
                int medicoId = int.Parse(ddlMedico.SelectedValue);
                DateTime fechaBase = DateTime.ParseExact(txtFecha.Text, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
                TimeSpan horaSel = TimeSpan.ParseExact(ddlHora.SelectedValue, "hh\\:mm", System.Globalization.CultureInfo.InvariantCulture);

                // Objeto Turno Nuevo
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

                if (nuevoTurno.FechaHoraInicio < DateTime.Now)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "past", "window.__queueToast = window.__queueToast || []; __queueToast.push({ m: 'No podés reservar en el pasado.', t: 'danger', d: 2200 });", true);
                    return;
                }

                var turnoNegocio = new TurnoNegocio();

                // --- LÓGICA DIFERENCIADA: REPROGRAMAR vs NUEVO ---
                if (IdTurnoReprogramar.HasValue)
                {
                    // Estamos Reprogramando: Cancelamos el anterior y creamos este nuevo
                    // Usamos ReprogramarTurnoCreandoNuevo que ya existe en Negocio
                    turnoNegocio.ReprogramarTurnoCreandoNuevo(IdTurnoReprogramar.Value, nuevoTurno);

                    ScriptManager.RegisterStartupScript(this, GetType(), "okReprog",
                       "window.__queueToast = window.__queueToast || []; " +
                       "__queueToast.push({ m: '¡Turno reprogramado con éxito!', t: 'success', d: 1500, redirect: 'MisTurnos.aspx' });",
                       true);
                }
                else
                {
                    // Alta normal
                    turnoNegocio.AgendarTurno(nuevoTurno);

                    ScriptManager.RegisterStartupScript(this, GetType(), "okReserva",
                        "window.__queueToast = window.__queueToast || []; " +
                        "__queueToast.push({ m: '¡Turno reservado con éxito!', t: 'success', d: 1500, redirect: 'Default.aspx' });",
                        true);
                }
            }
            catch (Exception ex)
            {
                var msgUser = (ex.InnerException?.Message ?? ex.Message ?? "Error").Replace("'", "").Replace("\r", " ").Replace("\n", " ");
                ScriptManager.RegisterStartupScript(this, GetType(), "errReserva",
                    $"window.__queueToast = window.__queueToast || []; __queueToast.push({{ m: 'Error: {msgUser}', t: 'danger', d: 4000 }});",
                    true);
            }
        }

        void GuardarReservaPendienteYIrACompletarPerfil(string motivoToast)
        {
            var pend = new ReservaPendiente
            {
                EspecialidadId = int.TryParse(ddlEspecialidades.SelectedValue, out var esp) ? esp : (int?)null,
                MedicoId = int.TryParse(ddlMedico.SelectedValue, out var med) ? med : (int?)null,
                Fecha = DateTime.TryParse(txtFecha.Text, out var f) ? f : (DateTime?)null,
                Hora = string.IsNullOrWhiteSpace(ddlHora.SelectedValue) ? null : ddlHora.SelectedValue,
                Observaciones = string.IsNullOrWhiteSpace(txtObs.Text) ? null : txtObs.Text.Trim()
            };
            Session["ReservaPendiente"] = pend;

            var ret = Server.UrlEncode(ResolveUrl("~/Pacientes/ReservarTurno.aspx"));
            // Nota: Si estábamos reprogramando, deberíamos guardar ese estado también, pero es un caso borde complejo.
            // Asumimos flujo normal para completar perfil.

            ScriptManager.RegisterStartupScript(this, GetType(), "redirAddPerfil",
                $"window.__queueToast = window.__queueToast || []; __queueToast.push({{ m: '{motivoToast}', t: 'warning', d: 1200, redirect: '{ResolveUrl("~/Pacientes/Add.aspx")}?return={ret}' }});",
                true);
        }
    }
}