using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using EstacionamientoUDCI.App_Code;

namespace EstacionamientoUDCI.modules.admin
{
    public partial class Users : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //validamos que el usuario haya iniciado sesion
            if (Session["user"] != null)
            {
                UserData user = (UserData)Session["user"]; //datos del usuario

                //validamos que el usuario sea administrador
                if (user.Rol != "administrador")
                {
                    Response.Redirect("~/modules/Default.aspx");
                }
            }
            else
            {
                Response.Redirect("~/Login.aspx");
            }
        }
    }
}