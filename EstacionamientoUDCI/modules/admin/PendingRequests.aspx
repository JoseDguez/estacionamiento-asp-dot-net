<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="PendingRequests.aspx.cs" Inherits="EstacionamientoUDCI.modules.admin.PendingRequests" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    Solicitudes Pendientes
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <div class="panel panel-primary">
        <div class="panel-heading"><h1 class="panel-title">Cajones de Estacionamiento</h1></div>
        <div class="panel-body">
            <asp:GridView ID="PendingsGridView" runat="server" DataSourceID="PendingsObjDS" Width="100%" EmptyDataText="Sin Información..."
             AutoGenerateColumns="false" DataKeyNames="id" OnRowCommand="PendingsGridView_RowCommand"
             HeaderStyle-BackColor="AliceBlue" HeaderStyle-HorizontalAlign="Center">
                <Columns>
                    <asp:BoundField DataField="id" HeaderText="ID" Visible="false" />
                    <asp:BoundField DataField="matricula" HeaderText="Matrícula" />
                    <asp:BoundField DataField="alumno" HeaderText="Nombre" />
                    <asp:BoundField DataField="cajon" HeaderText="Cajon (Turno)" />
                    <asp:BoundField DataField="estatus" HeaderText="Estatus" />
                    <asp:ButtonField ButtonType="Image" CommandName="Approve" HeaderText="Aprobar" ImageUrl="~/assets/img/approve.png">
                        <ControlStyle CssClass="ImgButton"/>
                        <ItemStyle HorizontalAlign="Center" />     
                    </asp:ButtonField>
                    <asp:ButtonField ButtonType="Image" CommandName="Cancel" HeaderText="Cancelar" ImageUrl="~/assets/img/cancel.jpg">
                        <ControlStyle CssClass="ImgButton"/>
                        <ItemStyle HorizontalAlign="Center" />     
                    </asp:ButtonField>
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="PendingsObjDS" runat="server" TypeName="EstacionamientoUDCI.App_Code.DataObjectClass" SelectMethod="SelectPendingRequests">
            </asp:ObjectDataSource>
        </div>
    </div>
</asp:Content>
