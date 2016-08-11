using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using EstacionamientoUDCI.App_Code;
using System.Data;
using System.Text;

namespace EstacionamientoUDCI
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //validamos que el usuario haya iniciado sesion
            if (Session["user"] != null)
            {
                UserData user = (UserData)Session["user"]; //datos del usuario

                //validamos usuario de tipo administrador
                //se ocultan opciones no disponibles para usuarios que no sean administradores
                if (user.Rol != "administrador")
                {
                    liAdm.Visible = false;
                    liCycle.Visible = false;
                }

                //se ocultan opciones disponibles unicamente para lumnos
                if (user.Rol == "administrador")
                {
                    liCar.Visible = false;
                    liBox.Visible = false;
                }

                //se muestra nombre de usuario
                Username.InnerHtml = user.Name + ' ' + user.Lastname + " <span class='caret'>";

                //si el alumno ya tiene un cajon reservado, se oculta la opcion para solicitar otro cajon
                int assigned = new DataObjectClass().CheckAssignedParkingBox(user.EnrollmentNumber);
                if (assigned == 1) liBox.Visible = false;
            }
            else
            {
                //si no se ha iniciado sesion se oculta el menu principal
                UserActions.Visible = false;
                Menu.Visible = false;
            }
        }

        //click en boton para cerrar sesion
        protected void LogOutButton_Click(object sender, EventArgs e)
        {
            //se destruye la sesion y se redirecciona al login
            Session["user"] = null;
            Response.Redirect("~/Login.aspx");
        }

        //click en boton para reservar cajon de estacionamiento
        protected void ReserveButton_Click(object sender, EventArgs e)
        {
            UserData user = (UserData)Session["user"]; //datos del usuario

            //obtenemos si el usuario tiene un auto registrado
            int carExists = new DataObjectClass().CheckCar(user.EnrollmentNumber);
            //obtenemos el ciclo escolar activo registrado
            DataTable dt = new DataObjectClass().SelectScholarCycle_Active();

            //validamos ciclo escolar activo
            if (dt.Rows.Count <= 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('No hay ciclo escolar activo.')", true);
            }
            else
            {
                string scholarCycle = dt.Rows[0]["ciclo"].ToString(); //ciclo escolar

                //validamos que el usuario tenga un auto registrado
                if (carExists == 0)
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('No tienes un auto registrado.')", true);
                }
                else
                {
                    //obtenemos si el usuario ya tiene un cajon reservado
                    int alreadyReserved = new DataObjectClass().CheckIfReserved(user.EnrollmentNumber, scholarCycle);

                    //validamos que el usuario solo tenga un cajon reservado
                    if (alreadyReserved > 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Ya tienes un cajón reservado.')", true);
                    }
                    else
                    {
                        //obtenemos el siguiente cajon disponible
                        dt = new DataObjectClass().GetNextAvailableParking(user.Turn);

                        //validamos que haya cajones disponibles
                        if (dt.Rows.Count == 0)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Todos los cajones de tu turno están ocupados o no hay cajones registrados.')", true);
                        }
                        else
                        {
                            string parkingBox = dt.Rows[0]["alias"].ToString(); //cajon
                            string turn = dt.Rows[0]["turno"].ToString();       //turno

                            //reserva el cajon para el turno
                            int reserved = new DataObjectClass().ReserveParkingBox(parkingBox, turn);

                            if (reserved == 1)
                            {
                                //registra pago
                                new DataObjectClass().RegisterPayment(user.EnrollmentNumber, parkingBox, turn, scholarCycle);
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Se reservó el cajón con nombre: " + parkingBox + " (" + turn + ") correctamente. El administrador debe aprobar esta reservación para realizar el pago.')", true);
                            }
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Ocurrió un error al reservar el cajón.')", true);
                            }
                        }
                    }
                }
            }
        }

        //click en boton para ciclo escolar
        protected void ScholarCycleButton_Click(object sender, EventArgs e)
        {
            //muestra la ventana modal
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("setTimeout(function() {");
            sb.Append("$('#ScholCycleModal').modal();");
            sb.Append("}, 500);");
            sb.Append(@"</script>");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ModalScript", sb.ToString(), false);
        }

        //click en boton registro de ciclo escolar
        protected void RegisterScholarCycleButton_Click(object sender, EventArgs e)
        {
            string scholarCycle = ScholarCycleTextBox.Text; //ciclo escolar

            //obtiene si el ciclo escolar ingresado ya existe
            int counter = new DataObjectClass().CheckScholarCycle(scholarCycle);

            //validamos ciclo escolar
            if (counter > 0)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('El ciclo escolar ingresado ya está registrado.')", true);
            }
            else
            {
                //obtiene el ciclo escolar activo
                DataTable dt = new DataObjectClass().SelectScholarCycle_Active();

                if (dt.Rows.Count > 0)
                {
                    string currentScholarCycle = dt.Rows[0]["ciclo"].ToString(); //ciclo escolar actual

                    //obtenemos si hay reservaciones/pagos pendientes para el ciclo escolar actual
                    int pendingCounter = new DataObjectClass().CheckPendingPayments_Requests(currentScholarCycle);

                    //validamos procesos pendientes
                    if (pendingCounter > 0)
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Hay reservaciones pendientes para el ciclo escolar activo.')", true);
                    }
                    else
                    {
                        //reinicia todos los ciclos escolares (los desactiva)
                        new DataObjectClass().ResetScholarCycles();
                        //registra ciclo escolar
                        new DataObjectClass().RegisterScholarCycle(scholarCycle);
                        //libera todos los cajones del estacionamiento
                        new DataObjectClass().FreeParkingBoxes();
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), " ", "alert('Ciclo escolar registrado correctamente.'); window.location='" + Request.ApplicationPath + "modules/Default.aspx';", true);
                    }
                }
                else
                {
                    //reinicia todos los ciclos escolares (los desactiva)
                    new DataObjectClass().ResetScholarCycles();
                    //registra ciclo escolar
                    new DataObjectClass().RegisterScholarCycle(scholarCycle);
                }
            }
        }
    }
}