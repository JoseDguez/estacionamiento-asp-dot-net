using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EstacionamientoUDCI.App_Code;
using System.Drawing;
using System.Data;

namespace EstacionamientoUDCI
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //validamos que el usuario haya iniciado sesion
            if (Session["user"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
            else
            {
                //obtenemos ciclo escolar activo
                DataTable dt = new DataObjectClass().SelectScholarCycle_Active();

                if (dt.Rows.Count > 0)
                {
                    //mostramos el ciclo escolar activo
                    string scholarCycle = dt.Rows[0]["ciclo"].ToString();
                    if (scholarCycle != null) ScholarCycleLabel.Text = "Ciclo escolar: " + scholarCycle;
                }
                else
                {
                    ScholarCycleLabel.Text = "No hay Ciclo Escolar registrado";
                    ScholarCycleLabel.ForeColor = Color.Red;
                }
            }
        }
    }
}