using Clinica.Dominio;
using Clinica.Negocio;
using System;
using System.Web.UI;

namespace Presentacion.Admin
{
    public partial class AddPaciente : System.Web.UI.Page
    {
        private readonly PacienteNegocio pacienteNegocio = new PacienteNegocio();
        private readonly UsuarioNegocio usuarioNegocio = new UsuarioNegocio();

        protected void Page_Load(object sender, EventArgs e)
        {
            var u = Session["usuario"] as Usuario;
            if (u == null || u.Perfil != Perfil.Administrador)
            {
                Response.Redirect("~/Login.aspx", false);
                return;
            }

            if (!IsPostBack)
            {
                var id = Request.QueryString["id"];
                if (!string.IsNullOrWhiteSpace(id) && int.TryParse(id, out int pid))
                {
                    CargarPaciente(pid);
                    litTitulo.Text = "Editar paciente";
                    Page.Title = "Editar paciente — Admin";
                }
                else
                {
                    chkActivo.Checked = true;
                    litTitulo.Text = "Nuevo paciente";
                    Page.Title = "Nuevo paciente — Admin";
                }
            }
        }

        private void CargarPaciente(int pacienteId)
        {
            var p = pacienteNegocio.ObtenerPorId(pacienteId);
            if (p == null)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "noPaciente",
                    "window.__queueToast = window.__queueToast || []; __queueToast.push({ m:'No se encontró el paciente.', t:'danger', d:2500 });",
                    true);
                Response.Redirect("~/Admin/Pacientes.aspx", false);
                return;
            }

            hfPacienteId.Value = p.PacienteId.ToString();
            txtNombre.Text = p.Nombre ?? "";
            txtApellido.Text = p.Apellido ?? "";
            txtDni.Text = p.Dni ?? "";
            txtTelefono.Text = p.Telefono ?? "";
            txtEmail.Text = p.Email ?? "";
            txtObraSocial.Text = p.ObraSocial ?? "";
            if (p.FechaNacimiento != DateTime.MinValue) txtNacimiento.Text = p.FechaNacimiento.ToString("yyyy-MM-dd");

            chkActivo.Checked = p.Activo;

            var user = usuarioNegocio.ObtenerPorPacienteId(pacienteId); 
            if (user != null)
            {
                hfUsuarioId.Value = user.IdUsuario.ToString();
                if (!IsPostBack && user.Activo == false) chkActivo.Checked = false;
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                var p = new Paciente
                {
                    PacienteId = string.IsNullOrWhiteSpace(hfPacienteId.Value) ? 0 : int.Parse(hfPacienteId.Value),
                    Nombre = (txtNombre.Text ?? "").Trim(),
                    Apellido = (txtApellido.Text ?? "").Trim(),
                    Dni = string.IsNullOrWhiteSpace(txtDni.Text) ? null : txtDni.Text.Trim(),
                    Telefono = string.IsNullOrWhiteSpace(txtTelefono.Text) ? null : txtTelefono.Text.Trim(),
                    Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim(),
                    ObraSocial = string.IsNullOrWhiteSpace(txtObraSocial.Text) ? null : txtObraSocial.Text.Trim(),
                    FechaNacimiento = string.IsNullOrWhiteSpace(txtNacimiento.Text) ? DateTime.MinValue : DateTime.Parse(txtNacimiento.Text),
                    Activo = chkActivo.Checked
                };

                bool esEdicion = p.PacienteId > 0;

                if (esEdicion)
                    pacienteNegocio.Modificar(p);
                else
                    p.PacienteId = pacienteNegocio.Agregar(p);

                var tienePasswordNuevo = !string.IsNullOrWhiteSpace(txtPass.Text);
                Usuario usu = null;

                if (!string.IsNullOrWhiteSpace(hfUsuarioId.Value))
                {
                    int uid = int.Parse(hfUsuarioId.Value);
                    usu = usuarioNegocio.ObtenerPorId(uid);
                    if (usu != null)
                    {
                        usu.Email = p.Email ?? usu.Email;
                        usu.Activo = chkActivo.Checked;
                        if (tienePasswordNuevo)
                            usu.Pass = txtPass.Text.Trim();

                        usuarioNegocio.Modificar(usu, actualizarPassword: tienePasswordNuevo);
                    }
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(p.Email))
                    {
                        usu = new Usuario
                        {
                            Email = p.Email,
                            Pass = tienePasswordNuevo ? txtPass.Text.Trim() : GenerarPasswordTemporal(),
                            Perfil = Perfil.Paciente,
                            Rol = "Paciente",
                            IdPaciente = p.PacienteId,
                            Nombre = p.Nombre,
                            Apellido = p.Apellido,
                            Activo = chkActivo.Checked
                        };
                        usuarioNegocio.Agregar(usu);
                        hfUsuarioId.Value = usu.IdUsuario.ToString();
                    }
                }

                ScriptManager.RegisterStartupScript(
                    this, GetType(), "okSave",
                    "window.__queueToast = window.__queueToast || []; " +
                    "__queueToast.push({ m:'Paciente guardado correctamente.', t:'success', d:1500, redirect:'" + ResolveUrl("~/Admin/Pacientes.aspx") + "' });",
                    true
                );
            }
            catch (Exception ex)
            {
                var msg = (ex.Message ?? "Error").Replace("'", "").Replace("\r", " ").Replace("\n", " ");
                ScriptManager.RegisterStartupScript(
                    this, GetType(), "errSave",
                    "window.__queueToast = window.__queueToast || []; " +
                    $"__queueToast.push({{ m:'Error al guardar: {msg}', t:'danger', d:4000 }});",
                    true
                );
                valResumen.HeaderText = msg;
            }
        }

        private string GenerarPasswordTemporal()
        {
            return "Clinica" + DateTime.Now.Ticks.ToString().Substring(10);
        }
    }
}
