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
    public partial class RecepcionistaAdmin : System.Web.UI.Page
    {
     

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarTurnos();
                CargarRecepcionistas();
            }

        }
        private void CargarTurnos()
        {
            TurnoTrabajoNegocio turnoTrabajo = new TurnoTrabajoNegocio();
            ddlTurno.DataSource = turnoTrabajo.Listar();
            ddlTurno.DataTextField = "Nombre";
            ddlTurno.DataValueField = "TurnoTrabajoId";
            ddlTurno.DataBind();
        }
        private void CargarRecepcionistas()
        {
          RecepcionistaNegocio negocio = new RecepcionistaNegocio();
            try
            {
                gvRecepcionistas.DataSource = negocio.Listar();
                gvRecepcionistas.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            if(!Page.IsValid)
            {
                return;
            }
          
            
            Recepcionista nuevoRecepcionista = new Recepcionista();
           
            try
            {
                nuevoRecepcionista.Nombre = txtNombre.Text.Trim();
                nuevoRecepcionista.Apellido = txtApellido.Text.Trim();
                nuevoRecepcionista.Email = txtEmail.Text.Trim();
                nuevoRecepcionista.Telefono = string.IsNullOrWhiteSpace(txtTelefono.Text) ? null : txtTelefono.Text.Trim();
                nuevoRecepcionista.TurnoTrabajoId = string.IsNullOrEmpty(ddlTurno.SelectedValue) ? (int?)null : int.Parse(ddlTurno.SelectedValue);
                nuevoRecepcionista.Activo = chkActivo.Checked;
                
                RecepcionistaNegocio negocio = new RecepcionistaNegocio();
                UsuarioNegocio usuarioNegocio = new UsuarioNegocio();

                int idRecepcionista = negocio.Agregar(nuevoRecepcionista);
                
                Usuario usuario = new Usuario();
                {
                    usuario.IdRecepcionista = idRecepcionista;
                    usuario.Email = txtEmail.Text.Trim();
                    usuario.Password = txtPass.Text.Trim();
                    usuario.Rol = "Recepcionista";
                    usuario.Perfil = Perfil.Recepcionista;
                    usuario.Activo = true;
                }
                
                usuarioNegocio.Agregar(usuario);

                CargarRecepcionistas();
                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void LimpiarFormulario()
        {
            txtNombre.Text = "";
            txtApellido.Text = "";
            txtTelefono.Text = "";
            txtEmail.Text = "";
            txtPass.Text = "";
            chkActivo.Checked = true;
        }

    }
}