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
using System.Drawing;

namespace EstacionamientoUDCI.modules.admin
{
    public partial class ParkingBoxes : System.Web.UI.Page
    {
        private SqlConnection conn = null;
        private SqlCommand sqlCmd = null;
        private DataTable dt = null;
        private string query = String.Empty;
        private string connectionStr = ConfigurationManager.ConnectionStrings["EstacionamientoConnStr"].ToString();

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

        //colores en el grid segun el estado del cajon (libre, reservado, ocupado)
        protected void ParkingBoxesGV_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //cajon ocupado /amarillo
                if (e.Row.Cells[2].Text == "Si")
                {
                    e.Row.BackColor = Color.LightYellow;
                }
                //cajon libre /verde
                else
                {
                    e.Row.BackColor = Color.LightGreen;
                }

                //cajon reservado /azul
                if (e.Row.Cells[2].Text == "Si" && e.Row.Cells[3].Text == "N/A") e.Row.BackColor = Color.DeepSkyBlue;
            }
        }

        //click en boton de registrar nuevo cajon
        protected void RegisterButton_Click(object sender, EventArgs e)
        {
            string alias = NameTextBox.Text; //nombre

            //obtiene si el cajon ingresado ya existe
            int exists = new DataObjectClass().CheckParkingBox(alias);

            //validamos cajon
            if (exists == 0)
            {
                int result = -1;
                try
                {
                    conn = new SqlConnection(connectionStr);
                    conn.Open();
                    if (conn.State == ConnectionState.Open)
                    {
                        string turn = "M";
                        
                        //registra el mismo cajon dos veces (turno matutino y vespertino)
                        for (int i = 0; i < 2; i++)
                        {
                            if (i == 1) turn = "V";

                            query = @"INSERT INTO CAJONES (alias,turno,ocupado,auto)
                                      VALUES
                                      (@alias,@turno,@ocupado,@auto)";

                            sqlCmd = new SqlCommand(query, conn);
                            sqlCmd.Parameters.AddWithValue("@alias", alias);
                            sqlCmd.Parameters.AddWithValue("@turno", turn);
                            sqlCmd.Parameters.AddWithValue("@ocupado", 0);
                            sqlCmd.Parameters.AddWithValue("@auto", "");

                            result = sqlCmd.ExecuteNonQuery();

                            if (result != 1)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Error al registrar.')", true);
                            }
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Cajon registrado.')", true);
                            }
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
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('El cajón con el nombre ingresado ya está registrado.')", true);
            }

            //limpia campos y actualiza grid
            NameTextBox.Text = "";
            ParkingBoxesGV.DataBind();
        }

        //click en boton para liberar cajones
        protected void FreeParkingBoxesButton_Click(object sender, EventArgs e)
        {
            //obtenemos el numero de cajones registrados
            int count = new DataObjectClass().CheckParkingBoxesRegistered();

            //validamos que haya cajones registrados
            if (count > 0)
            {
                //obtenemos ciclo escolar activo
                DataTable dt = new DataObjectClass().SelectScholarCycle_Active();

                //validamos ciclo escolar activo
                if (dt.Rows.Count > 0)
                {
                    string scholarCycle = dt.Rows[0]["ciclo"].ToString(); //ciclo escolar

                    //obtenemos si hay reservaciones/pagos pendientes para el ciclo escolar actual
                    int pendingCounter = new DataObjectClass().CheckPendingPayments_Requests(scholarCycle);

                    //validamos procesos pendientes
                    if (pendingCounter > 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Hay reservaciones pendientes para este ciclo escolar.')", true);
                    }
                    else
                    {
                        //libera todos los cajones y acutaliza el grid
                        new DataObjectClass().FreeParkingBoxes();
                        ParkingBoxesGV.DataBind();
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Cajones de estacionamiento liberados correctamente.')", true);
                    }
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('No hay cajones registrados.')", true);
            }
        }
    }
}