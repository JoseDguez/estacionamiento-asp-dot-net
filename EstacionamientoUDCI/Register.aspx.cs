using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Web.Security;

namespace EstacionamientoUDCI
{
    public partial class Register : System.Web.UI.Page
    {
        private SqlConnection conn = null;
        private SqlCommand sqlCmd = null;
        private DataTable dt = null;
        private string query = String.Empty;
        private string connectionStr = ConfigurationManager.ConnectionStrings["EstacionamientoConnStr"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void CareerDropdown_DataBound(Object sender, EventArgs e)
        {
            CareerDropdown.Items.Insert(0, new ListItem("--- Selecciona una carrera ---", String.Empty));
        }

        //click en el boton de registro
        protected void RegisterButton_Click(object sender, EventArgs e)
        {
            string enrollmentNumber = EnrollmentNumberTextBox.Text;  //matricula
            string career = CareerDropdown.SelectedValue.ToString(); //carrera
            string turn = TurnDropdown.SelectedValue.ToString();     //turno
            string name = NameTextBox.Text;                          //nombre
            string lastname = LastnameTextBox.Text;                  //apellido
            string email = EmailTextBox.Text;                        //email

            //se encripta la contraseña con el algoritmo MD5
            string password = FormsAuthentication.HashPasswordForStoringInConfigFile(PasswordTextBox.Text, System.Web.Configuration.FormsAuthPasswordFormat.MD5.ToString());

            //validar si el usuario ingresado ya existe
            int exists = checkIfUserExists(enrollmentNumber, email);
            if (exists != 1)
            {
                //si no existe se registra el usuario
                int result = -1;
                try
                {
                    conn = new SqlConnection(connectionStr);
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        query = @"INSERT INTO ALUMNOS (matricula,nombre,apellidos,carrera,turno,email,contrasena,rol)
                                  VALUES
                                  (@matricula,@nombre,@apellidos,@carrera,@turno,@email,@contrasena,@rol)";

                        sqlCmd = new SqlCommand(query, conn);
                        sqlCmd.Parameters.AddWithValue("@matricula", enrollmentNumber);
                        sqlCmd.Parameters.AddWithValue("@nombre", name);
                        sqlCmd.Parameters.AddWithValue("@apellidos", lastname);
                        sqlCmd.Parameters.AddWithValue("@email", email);
                        sqlCmd.Parameters.AddWithValue("@carrera", Convert.ToInt32(career));
                        sqlCmd.Parameters.AddWithValue("@turno", turn);
                        sqlCmd.Parameters.AddWithValue("@contrasena", password);
                        sqlCmd.Parameters.AddWithValue("@rol", "alumno");

                        result = sqlCmd.ExecuteNonQuery();

                        if (result != 1)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Error al registrar.')", true);
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Usuario registrado.'); window.location='" + Request.ApplicationPath + "Login.aspx';" , true);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Response.Write("ERROR: " + ex.ToString());
                }
                finally
                {
                    if (conn.State == ConnectionState.Open) { conn.Close(); }
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('La matrícula y/o el correo electrónico ingresados ya están registrados.')", true);
            }

            //limpiar campos
            EnrollmentNumberTextBox.Text = "";
            CareerDropdown.SelectedIndex = 0;
            NameTextBox.Text = "";
            LastnameTextBox.Text = "";
            EmailTextBox.Text = "";
            PasswordTextBox.Text = "";
        }

        //devuelve 1 si el correo o la matricula ya estan registrados
        protected int checkIfUserExists(string enrollmentNumber, string email)
        {
            int result = -1;
            try
            {
                conn = new SqlConnection(connectionStr);
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    query = @"SELECT COUNT(*) FROM ALUMNOS WHERE matricula = @matricula OR email = @email";

                    sqlCmd = new SqlCommand(query, conn);
                    sqlCmd.Parameters.AddWithValue("@matricula", enrollmentNumber);
                    sqlCmd.Parameters.AddWithValue("@email", email);
                    
                    result = Convert.ToInt32(sqlCmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                Response.Write("ERROR: " + ex.ToString());
            }
            finally
            {
                if (conn.State == ConnectionState.Open) { conn.Close(); }
            }
            return result;
        }
    }
}