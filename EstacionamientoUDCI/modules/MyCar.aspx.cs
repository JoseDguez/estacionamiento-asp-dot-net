using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EstacionamientoUDCI.App_Code;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace EstacionamientoUDCI.modules
{
    public partial class MyCar : System.Web.UI.Page
    {
        private SqlConnection conn = null;
        private SqlCommand sqlCmd = null;
        private DataTable dt = null;
        private string query = String.Empty;
        private string connectionStr = ConfigurationManager.ConnectionStrings["EstacionamientoConnStr"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            //validamos que el usuario haya iniciado sesion
            if (Session["user"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
            else
            {
                //obtenemos si el usuario tiene un auto registrado
                int count = new DataObjectClass().CheckCar(Session["matricula"].ToString());

                //si ya tiene un auto registrado mostramos los datos, si no, mostramos formulario para registro
                if (count > 0)
                {
                    divNew.Visible = false;
                    divGV.Visible = true;
                }
                else 
                {
                    divNew.Visible = true;
                    divGV.Visible = false;
                }
            }

            //opcion default para la lista de modelos
            if (!IsPostBack)
            {
                ModelDropdown.Enabled = false;
                ModelDropdown.Items.Insert(0, new ListItem("--- Selecciona un modelo ---", String.Empty));
            }
        }

        protected void BrandDropdown_DataBound(Object sender, EventArgs e)
        {
            BrandDropdown.Items.Insert(0, new ListItem("--- Selecciona una marca ---", String.Empty));
        }

        //evento cuando se selecciona una marca de auto
        protected void BrandDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            int brandId = int.Parse(BrandDropdown.SelectedItem.Value); //id marca

            //validamos seleccion, cargamos la lista de modelos para la marca seleccionada
            if (brandId > 0)
            {
                try
                {
                    conn = new SqlConnection(connectionStr);
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        query = @"SELECT id, modelo FROM MODELOS_AUTO WHERE marca = @marca ORDER BY modelo";

                        sqlCmd = new SqlCommand(query, conn);
                        sqlCmd.Parameters.AddWithValue("@marca", brandId);

                        ModelDropdown.DataSource = sqlCmd.ExecuteReader();
                        ModelDropdown.DataTextField = "modelo";
                        ModelDropdown.DataValueField = "id";
                        ModelDropdown.DataBind();
                        ModelDropdown.Items.Insert(0, new ListItem("--- Selecciona un modelo ---", String.Empty));
                        ModelDropdown.Enabled = true;
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
        }

        //click en el boton de registro
        protected void RegisterButton_Click(object sender, EventArgs e)
        {
            string brand = BrandDropdown.SelectedValue.ToString(); //marca
            string model = ModelDropdown.SelectedValue.ToString(); //modelo
            string year = YearTextBox.Text;   //año
            string color = ColorTextBox.Text; //color

            int result = -1;
            try
            {
                //registramos el auto
                conn = new SqlConnection(connectionStr);
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    query = @"INSERT INTO AUTOS (alumno,marca,modelo,ano,color)
                            VALUES
                            (@alumno,@marca,@modelo,@ano,@color)";

                    sqlCmd = new SqlCommand(query, conn);
                    sqlCmd.Parameters.AddWithValue("@alumno", Session["matricula"].ToString());
                    sqlCmd.Parameters.AddWithValue("@marca", brand);
                    sqlCmd.Parameters.AddWithValue("@modelo", model);
                    sqlCmd.Parameters.AddWithValue("@ano", Convert.ToInt32(year));
                    sqlCmd.Parameters.AddWithValue("@color", color);

                    result = sqlCmd.ExecuteNonQuery();

                    if (result != 1)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Error al registrar.')", true);
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Auto registrado.')", true);
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

            //se actualiza el grid y se muestran los registros
            CarGV.DataBind();

            int count = new DataObjectClass().CheckCar(Session["matricula"].ToString());
            if (count > 0)
            {
                divNew.Visible = false;
                divGV.Visible = true;
            }
            else
            {
                divNew.Visible = true;
                divGV.Visible = false;
            }
        }
    }
}