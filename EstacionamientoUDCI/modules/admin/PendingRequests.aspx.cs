using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EstacionamientoUDCI.App_Code;

namespace EstacionamientoUDCI.modules.admin
{
    public partial class PendingRequests : System.Web.UI.Page
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

        //click en botones de aprobar/cancelar del grid
        protected void PendingsGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument.ToString());               //index del renglon
            int pendingId = Convert.ToInt32(PendingsGridView.DataKeys[rowIndex].Value); //id del registro
            string commandName = e.CommandName;                                         //nombre del comando (boton)
            
            string enrollmentNumber = PendingsGridView.Rows[rowIndex].Cells[1].Text; //matricula
            string parkingBox_Turn = PendingsGridView.Rows[rowIndex].Cells[3].Text;  //cajon(turno)
            string status = PendingsGridView.Rows[rowIndex].Cells[4].Text;           //estatus

            //separamos el string CAJON (TURNO) en CAJON y TURNO
            string[] tmp1 = parkingBox_Turn.Split('(');
            string[] tmp2 = tmp1[1].Split(')');

            string parkingBox = tmp1[0].Trim();
            string turn = tmp2[0].Trim();

            //si el boton presionado fue para aprobar
            if (commandName == "Approve")
            {
                //si el estatus actual es pendiente
                if (status == "pendiente")
                {
                    //actualiza el registro a aprobado
                    new DataObjectClass().UpdateParkingRequest(pendingId, status);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Registro aprobado, imprimir concepto de pago.')", true);
                }
                //si el estatus actual es aprobado
                else if (status == "aprobado")
                {
                    //actualiza el registro a pagado
                    new DataObjectClass().UpdateParkingRequest(pendingId, status);

                    //obtiene el auto del usuario
                    int carId = Convert.ToInt32(new DataObjectClass().SelectCar(enrollmentNumber).Rows[0]["id"]);

                    //se registra el auto en el cajon del estacionamiento
                    new DataObjectClass().UpdateParkingBox_RegisterCar(parkingBox, turn, carId);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Registro aprobado, el cajón ha sido asignado correctamente.')", true);
                }
            }
            //si el boton presionado fue para cancelar
            else
            {
                //cancela el registro
                new DataObjectClass().CancelParkingRequest(pendingId);
                //libera el cajon reservado
                new DataObjectClass().FreeParkingBox(parkingBox, turn);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Registro cancelado correctamente.')", true);
            }

            PendingsGridView.DataBind(); //actualiza el grid
        }
    }
}