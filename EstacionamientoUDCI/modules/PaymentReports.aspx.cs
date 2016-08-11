using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Data;
using EstacionamientoUDCI.App_Code;
using System.Text;

namespace EstacionamientoUDCI.modules
{
    public partial class PaymentReports : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //validamos que el usuario haya iniciado sesion
            if (Session["user"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
        }

        //click en boton de reporte
        protected void PaymentsGridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int rowIndex = Convert.ToInt32(e.CommandArgument.ToString());               //index del renglon
            int paymentId = Convert.ToInt32(PaymentsGridView.DataKeys[rowIndex].Value); //id del reporte (pago)

            //reiniciamos el report viewer y cargamos el formato del reporte
            ReportViewer.Reset();
            ReportViewer.LocalReport.ReportPath = @"reports/PaymentReport.rdlc";

            //obtenemos el detalle del pago
            DataTable dt = new DataObjectClass().SelectPaymentDetail(paymentId);

            //pasamos los parametros al reporte
            ReportParameter[] reportParameters = new ReportParameter[] {
                new ReportParameter("folio", dt.Rows[0]["folio"].ToString()),
                new ReportParameter("matricula", dt.Rows[0]["matricula"].ToString()),
                new ReportParameter("nombre", dt.Rows[0]["nombre"].ToString()),
                new ReportParameter("turno", dt.Rows[0]["turno"].ToString()),
                new ReportParameter("marca", dt.Rows[0]["marca"].ToString()),
                new ReportParameter("modelo", dt.Rows[0]["modelo"].ToString()),
                new ReportParameter("ano", dt.Rows[0]["ano"].ToString()),
                new ReportParameter("color", dt.Rows[0]["color"].ToString()),
                new ReportParameter("cajon", dt.Rows[0]["cajon"].ToString()),
                new ReportParameter("monto", "$ " + dt.Rows[0]["monto"].ToString()),
            };

            //cargamos los parametros y actualizamos el reporte
            ReportViewer.LocalReport.SetParameters(reportParameters);
            ReportViewer.LocalReport.Refresh();

            //se muestra la ventana modal con el reporte
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<script type='text/javascript'>");
            sb.Append("setTimeout(function() {");
            sb.Append("$('#ReportModal').modal();");
            sb.Append("}, 500);");
            sb.Append(@"</script>");

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ModalScript", sb.ToString(), false);
        }
    }
}