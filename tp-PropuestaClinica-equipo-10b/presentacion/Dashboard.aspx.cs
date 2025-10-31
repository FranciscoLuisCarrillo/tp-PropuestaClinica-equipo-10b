using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Clinica.Negocio.Pruebas;

namespace presentacion
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // --- PRUEBA 1: AGREGAR ESPECIALIDAD NUEVA ---
            TestManager.ProbarAgregarEspecialidad("Odontología");

            // --- PRUEBA 2: AGREGAR LA MISMA ESPECIALIDAD (debería fallar la validación) ---
            TestManager.ProbarAgregarEspecialidad("odontología"); // Prueba case-insensitivity

            // --- PRUEBA 3: AGREGAR OTRA NUEVA ESPECIALIDAD ---
            TestManager.ProbarAgregarEspecialidad("Cardiología");

            // --- PRUEBA 4: NOMBRE VACÍO (debería lanzar ArgumentException) ---
            TestManager.ProbarAgregarEspecialidad(string.Empty);

        }
    }
}