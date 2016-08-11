<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="MyCar.aspx.cs" Inherits="EstacionamientoUDCI.modules.MyCar" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    Mi Auto
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <div class="panel panel-primary">
        <div class="panel-heading"><h1 class="panel-title">Mi Auto</h1></div>
        <div class="panel-body">
            <div id="divNew" runat="server" class="container-fluid">
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                </asp:ScriptManager>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div class="form-group">
                            <label>Marca</label>
                            <asp:DropDownList ID="BrandDropdown" runat="server" DataSourceID="SqlBrandDS" DataValueField="id" 
                                DataTextField="marca" CssClass="form-control" OnDataBound="BrandDropdown_DataBound"
                                AutoPostBack="true" OnSelectedIndexChanged="BrandDropdown_SelectedIndexChanged"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="La marca es requerida."
                                ControlToValidate="BrandDropdown" Display="Dynamic" ForeColor="Red" ValidationGroup="Validation1"></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group">
                            <label>Modelo</label>
                            <asp:DropDownList ID="ModelDropdown" runat="server" AutoPostBack = "true" CssClass="form-control"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="El modelo es requerido."
                                ControlToValidate="ModelDropdown" Display="Dynamic" ForeColor="Red" ValidationGroup="Validation1"></asp:RequiredFieldValidator>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div class="form-group">
                    <label>Año</label>
                    <asp:TextBox ID="YearTextBox" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="El año es requerido"
                        ControlToValidate="YearTextBox" Display="Dynamic" ForeColor="Red" ValidationGroup="Validation1"></asp:RequiredFieldValidator>
                </div>
                <div class="form-group">
                    <label>Color</label>
                    <asp:TextBox ID="ColorTextBox" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="El color es requerido."
                        ControlToValidate="ColorTextBox" Display="Dynamic" ForeColor="Red" ValidationGroup="Validation1"></asp:RequiredFieldValidator>
                </div>
                <asp:Button ID="RegisterButton" runat="server" Text="Registrar" onclick="RegisterButton_Click" ValidationGroup="Validation1"/>
            </div>
            <div id="divGV" runat="server">
                <asp:GridView ID="CarGV" runat="server" DataSourceID="CarObjDS" DataKeyNames="id" AutoGenerateColumns="false"
                 Width="100%" EmptyDataText="Sin Información..."
                 HeaderStyle-BackColor="AliceBlue" HeaderStyle-HorizontalAlign="Center">
                 <Columns>
                    <asp:BoundField DataField="marca" HeaderText="Marca" />
                    <asp:BoundField DataField="modelo" HeaderText="Modelo" />
                    <asp:BoundField DataField="ano" HeaderText="Año" />
                    <asp:BoundField DataField="color" HeaderText="Color" />
                 </Columns>
                </asp:GridView>
                <asp:ObjectDataSource ID="CarObjDS" runat="server" TypeName="EstacionamientoUDCI.App_Code.DataObjectClass"
                    SelectMethod="SelectCar">
                    <SelectParameters>
                        <asp:SessionParameter SessionField="matricula" Name="matricula" />
                    </SelectParameters> 
                </asp:ObjectDataSource>
                <asp:SqlDataSource ID="SqlBrandDS" runat="server" ConnectionString='<%$ ConnectionStrings:EstacionamientoConnStr %>'
                     SelectCommand="SELECT id, marca FROM MARCAS_AUTO ORDER BY marca"></asp:SqlDataSource>
            </div>
        </div>
    </div>
</asp:Content>
