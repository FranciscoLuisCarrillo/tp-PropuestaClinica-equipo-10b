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
    public partial class Recepcionista : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void CargarRecepcionistas()
        {
         //   RecepcionistaNegocio negocio = new RecepcionistaNegocio();
            try
            {
               // dgvRecepcionistas.DataSource = negocio.ListarConSP();
            //    dgvRecepcionistas.DataBind();
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
            Persona nuevoRecepcionista = new Persona();
          //  RecepcionistaNegocio negocio = new RecepcionistaNegocio();
           
            try
            {
                nuevoRecepcionista.Nombre = txtNombre.Text;
                nuevoRecepcionista.Apellido = txtApellido.Text;
                nuevoRecepcionista.FechaNacimiento = Convert.ToDateTime(txtFechaNacimiento.Text);
                nuevoRecepcionista.Genero = ddlGenero.SelectedValue;
                nuevoRecepcionista.Dni = txtDNI.Text;
                nuevoRecepcionista.Domicilio = txtDireccion.Text;
                nuevoRecepcionista.Telefono = txtTelefono.Text;
                nuevoRecepcionista.Email = txtEmail.Text;
                nuevoRecepcionista.Activo = true;
                
               /// int idRecepcionista = negocio.Agregar(nuevoRecepcionista);
                Usuario usuario = new Usuario();
                {
                    //usuario.IdRecepcionista = idRecepcionista;
                    usuario.Email = txtEmail.Text.Trim();
                    usuario.Password = txtPass.Text.Trim();
                    usuario.Rol = "Recepcionista";
                    usuario.Activo = true;
                }
                UsuarioNegocio usuarioNegocio = new UsuarioNegocio();
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
            txtFechaNacimiento.Text = "";
            ddlGenero.SelectedIndex = 0;
            txtDNI.Text = "";
            txtDireccion.Text = "";
            txtTelefono.Text = "";
            txtEmail.Text = "";
            txtPass.Text = "";
            txtConfirmarPass.Text = "";
            chkActivo.Checked = true;
        }

    }
}