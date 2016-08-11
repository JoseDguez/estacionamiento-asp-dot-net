<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Users.aspx.cs" Inherits="EstacionamientoUDCI.modules.admin.Users" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    Usuarios
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <div class="panel panel-primary">
        <div class="panel-heading"><h1 class="panel-title">Usuarios</h1></div>
        <div class="panel-body">
            <asp:GridView ID="UsersGridView" runat="server" DataSourceID="UsersObjDS" Width="100%" EmptyDataText="Sin Información..."
             AutoGenerateColumns="false" AutoGenerateEditButton="true" DataKeyNames="matricula"
             HeaderStyle-BackColor="AliceBlue" HeaderStyle-HorizontalAlign="Center">
                <Columns>
                    <asp:BoundField DataField="matricula" HeaderText="Matrícula" ReadOnly="true" />
                    <asp:BoundField DataField="nombre" HeaderText="Nombre(s)" />
                    <asp:BoundField DataField="apellidos" HeaderText="Apellido(s)" />
                    <asp:TemplateField HeaderText="Carrera" SortExpression="descripcion">
                        <EditItemTemplate>
                            <asp:DropDownList ID="careerDropdown" runat="server" DataSourceID="CareerObjDS" DataValueField="id" 
                                DataTextField="descripcion" SelectedValue='<%# Bind("idCarrera") %>'>
                            </asp:DropDownList>      
                       </EditItemTemplate>
                       <ItemTemplate>
                           <asp:Label Runat="server" Text='<%# Bind("carrera") %>' ID="Label1"></asp:Label>
                       </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="email" HeaderText="E-Mail" />
                    <asp:TemplateField HeaderText="Rol">
                        <EditItemTemplate>
                            <asp:DropDownList ID="rolDropdown" runat="server" SelectedValue='<%# Bind("rol") %>'>
                                <asp:ListItem Value="administrador" Text="Administrador"></asp:ListItem>
                                <asp:ListItem Value="alumno" Text="Alumno"></asp:ListItem>
                            </asp:DropDownList>      
                       </EditItemTemplate>
                       <ItemTemplate>
                           <asp:Label Runat="server" Text='<%# Bind("rol") %>' ID="Label2"></asp:Label>
                       </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="UsersObjDS" runat="server" TypeName="EstacionamientoUDCI.App_Code.DataObjectClass" SelectMethod="SelectAlumns" UpdateMethod="UpdateAlumn">
                <UpdateParameters>
                    <asp:Parameter Name="nombre" />
                    <asp:Parameter Name="apellidos" />
                    <asp:Parameter Name="email" />
                    <asp:Parameter Name="rol" />
                    <asp:Parameter Name="matricula" />
                </UpdateParameters>
            </asp:ObjectDataSource>
            <asp:ObjectDataSource ID="CareerObjDS" runat="server" TypeName="EstacionamientoUDCI.App_Code.DataObjectClass" SelectMethod="SelectCareers"></asp:ObjectDataSource>
        </div>
    </div>
</asp:Content>
