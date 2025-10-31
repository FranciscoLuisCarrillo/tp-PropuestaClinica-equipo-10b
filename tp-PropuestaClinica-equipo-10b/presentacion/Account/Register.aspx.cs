using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Presentacion.Account
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               rbPaciente.Checked = true;
                cambiarRol();
            }
        }
        protected void cambioRolMarcado(object sender, EventArgs e)
        {
            cambiarRol();
        }
        private void cambiarRol()
        {
            bool esMedico = rbMedico.Checked;
            grpMatricula.Visible = esMedico;
            reqMatricula.Enabled = esMedico;

            grpObra.Visible = !esMedico;
            reqObra.Enabled = !esMedico;

        }
        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            string rol = rbMedico.Checked ? "Medico" : "Paciente";
        }
    }
}