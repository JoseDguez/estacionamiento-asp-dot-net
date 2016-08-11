<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ParkingBoxes.aspx.cs" Inherits="EstacionamientoUDCI.modules.admin.ParkingBoxes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    Cajones de Estacionamiento
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <div class="panel panel-primary">
        <div class="panel-heading"><h1 class="panel-title">Cajones de Estacionamiento</h1></div>
        <div class="panel-body">
            <div class="form-inline">
                Registrar nuevo cajón.<br />
                <div class="row">
                    <div class="form-group col-md-5">
                        <label>Nombre:</label>
                        <asp:TextBox ID="NameTextBox" class="form-control" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="El nombre es requerido."
                         ControlToValidate="NameTextBox" ValidationGroup="Grupo1" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:Button ID="RegisterButton" runat="server" Text="Registrar" onclick="RegisterButton_Click" ValidationGroup="Grupo1"/>
                    </div>
                    <div class="form-group col-md-7 nopadding">
                        <asp:Button ID="FreeParkingBoxesButton" runat="server" CssClass="pull-right" Text="Liberar Cajones" onclick="FreeParkingBoxesButton_Click"/>
                    </div>
                </div>
            </div>
            <hr />
            <asp:GridView ID="ParkingBoxesGV" runat="server" DataSourceID="ParkingBoxesObjDS" AutoGenerateColumns="false" Width="100%" EmptyDataText="Sin Información..."
             HeaderStyle-BackColor="AliceBlue" HeaderStyle-HorizontalAlign="Center" AllowPaging="true" OnRowDataBound="ParkingBoxesGV_RowDataBound">
                <PagerStyle HorizontalAlign="right" CssClass="GridPager" Height="25px"/>
                <Columns>
                    <asp:BoundField DataField="alias" HeaderText="Nombre" />
                    <asp:BoundField DataField="turno" HeaderText="Turno" />
                    <asp:BoundField DataField="ocupado" HeaderText="Ocupado" />
                    <asp:BoundField DataField="Auto" HeaderText="Auto" />
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="ParkingBoxesObjDS" runat="server" TypeName="EstacionamientoUDCI.App_Code.DataObjectClass"
             SelectMethod="SelectParkingBoxes"></asp:ObjectDataSource>
        </div>
    </div>
</asp:Content>
