using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Clinica.Dominio;
using Clinica.Negocio;

namespace Presentacion.Admin
{
    public partial class Medicos : System.Web.UI.Page
    {
        private readonly MedicoNegocio medicoNegocio = new MedicoNegocio();
        private readonly EspecialidadNegocio especialidadNegocio = new EspecialidadNegocio();
       

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarMedicos();
                CargarEspecialidades();
                
            }
        }

        private void CargarMedicos()
        {
            try
            {
                var lista = medicoNegocio.Listar();
                gvMedicos.DataSource = lista;
                gvMedicos.DataBind();
            }
            catch (Exception ex)
            {

                ValidarMedico.HeaderText = "Error al cargar los médicos: " + ex.Message;
            }
        }

        private void CargarEspecialidades()
        {
            try
            {
                var lista = especialidadNegocio.ListarTodas();
                chkEspecialidades.DataSource = lista;
                chkEspecialidades.DataTextField = "Nombre";
                chkEspecialidades.DataValueField = "EspecialidadId";
                chkEspecialidades.DataBind();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al cargar las especialidades.", ex);
            }
        }

        

        protected void btnGuarda_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            try
            {

                Medico nuevo = new Medico();

                nuevo.Nombre = txtNombre.Text.Trim();
                nuevo.Apellido = txtApellido.Text.Trim();

                if (!string.IsNullOrEmpty(txtFechaNacimiento.Text))
                    nuevo.FechaNacimiento = DateTime.Parse(txtFechaNacimiento.Text);

                nuevo.Genero = ddlGenero.SelectedValue;
                nuevo.Dni = txtDNI.Text.Trim();
                nuevo.Domicilio = txtDireccion.Text.Trim();
                nuevo.Telefono = txtTelefono.Text.Trim();
                nuevo.Email = txtEmail.Text.Trim();
                nuevo.Matricula = txtMatricula.Text.Trim();


             

                nuevo.Especialidades = new List<Especialidad>();
                foreach (ListItem item in chkEspecialidades.Items)
                {
                    if (item.Selected)
                    {
                        nuevo.Especialidades.Add(new Especialidad
                        {
                            EspecialidadId = int.Parse(item.Value),
                            Nombre = item.Text
                        });
                    }
                }


                int idMedicoGenerado = medicoNegocio.Agregar(nuevo);


                ValidarMedico.HeaderText = "Médico agregado correctamente. ID: " + idMedicoGenerado;
                UsuarioNegocio usuarioNegocio = new UsuarioNegocio();
                Usuario usuarioMedico = new Usuario
                {
                    Email = nuevo.Email,
                    Password = txtPassword.Text.Trim(),                    
                    Perfil = Perfil.Medico,
                    Rol = "Medico",
                    Activo = true,
                };
                usuarioNegocio.Agregar(usuarioMedico);
                ValidarMedico.HeaderText += " Usuario médico creado correctamente.";

                CargarMedicos();


                LimpiarFormulario();
            }
            catch (Exception ex)
            {
                ValidarMedico.HeaderText = ex.Message;
            }
        }

        private void LimpiarFormulario()
        {
            txtNombre.Text = string.Empty;
            txtApellido.Text = string.Empty;
            txtFechaNacimiento.Text = string.Empty;
            ddlGenero.SelectedIndex = 0;
            txtDNI.Text = string.Empty;
            txtDireccion.Text = string.Empty;
            txtTelefono.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtMatricula.Text = string.Empty;

            

            foreach (ListItem item in chkEspecialidades.Items)
            {
                item.Selected = false;
                item.Enabled = true;
            }
        }
    }
}
