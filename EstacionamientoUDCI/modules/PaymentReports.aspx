<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="PaymentReports.aspx.cs" Inherits="EstacionamientoUDCI.modules.PaymentReports" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    Reportes de Pago
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div class="panel panel-primary">
        <div class="panel-heading"><h1 class="panel-title">Cajones de Estacionamiento</h1></div>
        <div class="panel-body">
            <asp:UpdatePanel ID="UpdatePanel" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="ReportPanel" runat="server">
                        <asp:GridView ID="PaymentsGridView" runat="server" Width="100%" AutoGenerateColumns="false" DataSourceID="PaymentsObjDS" 
                         DataKeyNames="folio" OnRowCommand="PaymentsGridView_RowCommand" EmptyDataText="Sin Información..."
                         HeaderStyle-BackColor="AliceBlue" HeaderStyle-HorizontalAlign="Center">
                            <Columns>
                                <asp:BoundField DataField="folio" HeaderText="Folio" Visible="false" />
                                <asp:BoundField DataField="matricula" HeaderText="Matrícula" />
                                <asp:BoundField DataField="nombre" HeaderText="Nombre" />
                                <asp:BoundField DataField="turno" HeaderText="Turno" />
                                <asp:BoundField DataField="cajon" HeaderText="Cajon" />
                                <asp:BoundField DataField="monto" HeaderText="Monto" />
                                <asp:ButtonField ButtonType="Image" CommandName="PDF" HeaderText="PDF" ImageUrl="~/assets/img/pdf.jpg">
                                    <ControlStyle CssClass="ImgButton"/>
                                    <ItemStyle HorizontalAlign="Center" />     
                                </asp:ButtonField>
                            </Columns>
                        </asp:GridView>
                        <asp:ObjectDataSource ID="PaymentsObjDS" runat="server" TypeName="EstacionamientoUDCI.App_Code.DataObjectClass" SelectMethod="SelectPendingPayments"></asp:ObjectDataSource>
                    </asp:Panel>
                    <!-- Modal -->
                    <div id="ReportModal" class="modal fade" role="dialog">
                      <div class="modal-dialog modal-lg">
                        <!-- Modal content-->
                        <div class="modal-content">
                          <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Reporte de Pago</h4>
                          </div>
                          <div class="modal-body">
                            <rsweb:ReportViewer ID="ReportViewer" runat="server" ShowPrintButton="true" Width="100%"></rsweb:ReportViewer>
                          </div>
                          <div class="modal-footer"></div>
                        </div>
                      </div>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
