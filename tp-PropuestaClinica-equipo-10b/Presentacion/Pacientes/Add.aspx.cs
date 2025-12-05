using Clinica.Dominio;
using Clinica.Negocio;
using System;
using System.Web.UI;

namespace Presentacion.Pacientes
{
    public partial class Add : System.Web.UI.Page
    {
        private readonly PacienteNegocio pacienteNegocio = new PacienteNegocio();
        private readonly UsuarioNegocio usuarioNegocio = new UsuarioNegocio();

        protected void Page_Load(object sender, EventArgs e)
        {
            var usuario = Session["usuario"] as Usuario;
            if (usuario == null)
            {
                Response.Redirect("../Login.aspx", false);
                return;
            }

            if (!IsPostBack)
                CargarDatos(usuario);
        }

        private void CargarDatos(Usuario usuario)
        {
            if (usuario.IdPaciente.HasValue && usuario.IdPaciente.Value > 0)
            {
                var p = pacienteNegocio.ObtenerPorId(usuario.IdPaciente.Value);
                if (p != null)
                {
                    txtNombre.Text = p.Nombre ?? "";
                    txtApellido.Text = p.Apellido ?? "";
                    txtEmail.Text = p.Email ?? (usuario.Email ?? "");
                    txtEmail.ReadOnly = true;

                    txtDni.Text = p.Dni ?? "";
                    txtTelefono.Text = p.Telefono ?? "";
                    txtDireccion.Text = p.Domicilio ?? "";
                    txtObraSocial.Text = p.ObraSocial ?? "";

                    if (p.FechaNacimiento != DateTime.MinValue)
                        txtFechaNacimiento.Text = p.FechaNacimiento.ToString("yyyy-MM-dd");

                    if (!string.IsNullOrEmpty(p.Genero))
                        ddlGenero.SelectedValue = p.Genero;
                    return;
                }
            }

            txtEmail.Text = usuario.Email ?? "";
            txtEmail.ReadOnly = true;
        }

        protected void BtnGuardar_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (!Page.IsValid) return;

            try
            {
                var usuario = Session["usuario"] as Usuario;
                if (usuario == null)
                {
                    Response.Redirect("../Login.aspx", false);
                    return;
                }

                DateTime fechaNac = DateTime.MinValue;
                if (!string.IsNullOrWhiteSpace(txtFechaNacimiento.Text))
                    DateTime.TryParse(txtFechaNacimiento.Text, out fechaNac);

                var paciente = new Paciente
                {
                    PacienteId = (usuario.IdPaciente ?? 0),
                    Nombre = (txtNombre.Text ?? "").Trim(),
                    Apellido = (txtApellido.Text ?? "").Trim(),
                    Dni = string.IsNullOrWhiteSpace(txtDni.Text) ? null : txtDni.Text.Trim(),
                    FechaNacimiento = fechaNac == DateTime.MinValue ? DateTime.MinValue : fechaNac,
                    Genero = ddlGenero.SelectedValue,
                    Telefono = string.IsNullOrWhiteSpace(txtTelefono.Text) ? null : txtTelefono.Text.Trim(),
                    Domicilio = string.IsNullOrWhiteSpace(txtDireccion.Text) ? null : txtDireccion.Text.Trim(),
                    ObraSocial = string.IsNullOrWhiteSpace(txtObraSocial.Text) ? null : txtObraSocial.Text.Trim(),
                    Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? usuario.Email : txtEmail.Text.Trim(),
                    Activo = true
                };

                //  alta + linkear al usuario
                if (usuario.IdPaciente.HasValue && usuario.IdPaciente.Value > 0)
                {
                    pacienteNegocio.Modificar(paciente);
                }
                else
                {
                    int nuevoId = pacienteNegocio.Agregar(paciente);

                    var uDb = usuarioNegocio.ObtenerPorId(usuario.IdUsuario);
                    if (uDb == null) throw new Exception("No se pudo obtener el usuario para vincular el paciente.");

                    uDb.IdPaciente = nuevoId;
                    uDb.Nombre = paciente.Nombre;
                    uDb.Apellido = paciente.Apellido;

                    usuarioNegocio.Modificar(uDb, actualizarPassword: false);

                    // refrescar sesión
                    usuario.IdPaciente = nuevoId;
                    usuario.Nombre = uDb.Nombre;
                    usuario.Apellido = uDb.Apellido;
                    Session["usuario"] = usuario;
                }


                var ret = Request.QueryString["return"];
                var redirect = string.IsNullOrWhiteSpace(ret)
                    ? ResolveUrl("~/Pacientes/ReservarTurno.aspx")   // por defecto, volver a reservar
                    : ret;

                
                redirect = redirect.Replace("'", "\\'");

                ScriptManager.RegisterStartupScript(
                    this, GetType(), "okPerfil",
                    "window.__queueToast = window.__queueToast || []; " +
                    $"__queueToast.push({{ m:'Datos actualizados correctamente.', t:'success', d:1200, redirect:'{redirect}' }});",
                    true
                );
                return;
            }
            catch (Exception ex)
            {
                var msg = (ex.Message ?? "Error").Replace("'", "").Replace("\r", " ").Replace("\n", " ");
                ScriptManager.RegisterStartupScript(
                    this, GetType(), "errPerfil",
                    "window.__queueToast = window.__queueToast || []; " +
                    $"__queueToast.push({{ m:'Error al guardar: {msg}', t:'danger', d:3500 }});",
                    true
                );
            }
        }
    }
}
