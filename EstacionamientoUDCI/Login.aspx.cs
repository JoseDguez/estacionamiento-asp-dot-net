using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Web.Security;
using EstacionamientoUDCI.App_Code;

namespace EstacionamientoUDCI
{
    public partial class Login : System.Web.UI.Page
    {
        private SqlConnection conn = null;
        private SqlCommand sqlCmd = null;
        private DataTable dt = null;
        private string query = String.Empty;
        private string connectionStr = ConfigurationManager.ConnectionStrings["EstacionamientoConnStr"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["registerOk"] = null;
        }

        //click en boton de login
        protected void LoginButton_Click(object sender, EventArgs e)
        {
            string enrollmentNumber = EnrollmentNumberTextBox.Text; //matricula

            //se encripta la contraseña con el algoritmo MD5
            string password = FormsAuthentication.HashPasswordForStoringInConfigFile(PassTextBox.Text, System.Web.Configuration.FormsAuthPasswordFormat.MD5.ToString());

            try
            {
                conn = new SqlConnection(connectionStr);
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    query = @"SELECT * FROM ALUMNOS WHERE matricula = @matricula";

                    sqlCmd = new SqlCommand(query, conn);
                    sqlCmd.Parameters.AddWithValue("@matricula", enrollmentNumber);

                    dt = new DataTable();
                    
                    //obtenemos los datos del usuario ingresado
                    dt.Load(sqlCmd.ExecuteReader());

                    //validamos que el usuario exista
                    if (dt.Rows.Count > 0)
                    {
                        //validamos contraseña
                        string dbPassword = dt.Rows[0]["contrasena"].ToString();
                        if (password == dbPassword)
                        {
                            //datos del usuario
                            UserData user = new UserData()
                            {
                                EnrollmentNumber = dt.Rows[0]["matricula"].ToString(),
                                Name = dt.Rows[0]["nombre"].ToString(),
                                Lastname = dt.Rows[0]["apellidos"].ToString(),
                                Email = dt.Rows[0]["email"].ToString(),
                                Turn = dt.Rows[0]["turno"].ToString(),
                                Rol = dt.Rows[0]["rol"].ToString()
                            };

                            //se crea la sesion del usuario
                            Session["user"] = user;
                            Session["matricula"] = dt.Rows[0]["matricula"].ToString();

                            //redirecionamos a la pantalla de inicio
                            Response.Redirect("~/modules/Default.aspx");
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('La contraseña es incorrecta.')", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('No se encontraron datos.')", true);
                    }
                }
            }
            catch(Exception ex)
            {
                Response.Write("ERROR: " + ex.ToString());
            }
            finally
            {
                if (conn.State == ConnectionState.Open) { conn.Close(); }
            }
        }
    }
}