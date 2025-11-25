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
    public partial class Add : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Session["Usuario"] == null)
            {
                Response.Redirect("../Login.aspx", false);
                return;
            }

            if (!IsPostBack)
            {
                CargarDatos();
            }
        }
        private void CargarDatos()
        {
            // Recuperar el ID del paciente desde la sesión
            Usuario usuario = (Usuario)Session["usuario"];
            PacienteNegocio negocio = new PacienteNegocio();

            Paciente miPerfil = negocio.ObtenerPorId((int)usuario.IdPaciente);

            if (miPerfil != null)
            {
                txtNombre.Text = miPerfil.Nombre;
                txtApellido.Text = miPerfil.Apellido;
                txtEmail.Text = miPerfil.Email;
                txtEmail.Enabled = false;

                // Cargamos lo demás si existe
                txtDni.Text = miPerfil.Dni;
                txtTelefono.Text = miPerfil.Telefono;
                txtDireccion.Text = miPerfil.Domicilio;
                txtObraSocial.Text = miPerfil.ObraSocial;

                // Manejo de Fecha y DropDown
                if (miPerfil.FechaNacimiento != DateTime.MinValue)
                    txtFechaNacimiento.Text = miPerfil.FechaNacimiento.ToString("yyyy-MM-dd");

                if (!string.IsNullOrEmpty(miPerfil.Genero))
                    ddlGenero.SelectedValue = miPerfil.Genero;
            }
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            Page.Validate();
            if (!Page.IsValid) return;

            try
            {
                Usuario usuario = (Usuario)Session["usuario"];
                PacienteNegocio negocio = new PacienteNegocio();
                Paciente pacienteActualizado = new Paciente();

             
                pacienteActualizado.PacienteId = (int)usuario.IdPaciente;

                // Cargamos datos del formulario
                pacienteActualizado.Nombre = txtNombre.Text;
                pacienteActualizado.Apellido = txtApellido.Text;
                pacienteActualizado.Dni = txtDni.Text;
                pacienteActualizado.FechaNacimiento = DateTime.Parse(txtFechaNacimiento.Text);
                pacienteActualizado.Genero = ddlGenero.SelectedValue;
                pacienteActualizado.Telefono = txtTelefono.Text;
                pacienteActualizado.Domicilio = txtDireccion.Text;
                pacienteActualizado.ObraSocial = txtObraSocial.Text;

                // Guardamos en BD
                negocio.Modificar(pacienteActualizado);

                Response.Write("<script>alert('Datos actualizados correctamente.'); window.location='Default.aspx';</script>");
            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('Error al guardar: " + ex.Message + "');</script>");
            }
        
    }
    }
}