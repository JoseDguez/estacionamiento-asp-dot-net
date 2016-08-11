<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="EstacionamientoUDCI.Register" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    Registro
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <div class="col-lg-7 col-lg-offset-2 col-md-8 col-md-offset-2 col-sm-10 col-sm-offset-2 col-xs-7 col-xs-offset-3">
        <div class="panel panel-primary">
            <div class="panel-heading"><h1 class="panel-title">Registro de Usuario</h1></div>
            <div class="panel-body">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-sm-6">
                            <div class="form-group">
                                <label>Nombre(s)</label>
                                <asp:TextBox ID="NameTextBox" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="El nombre es requerido."
                                 ControlToValidate="NameTextBox" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-sm-6">
                            <div class="form-group">
                                <label>Apellido(s)</label>
                                <asp:TextBox ID="LastnameTextBox" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="El apellido es requerido."
                                 ControlToValidate="LastnameTextBox" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label>Matrícula</label>
                                <asp:TextBox ID="EnrollmentNumberTextBox" runat="server" CssClass="form-control"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="La matrícula es requerida."
                                 ControlToValidate="EnrollmentNumberTextBox" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label>Carrera</label>
                                <asp:DropDownList ID="CareerDropdown" runat="server" DataSourceID="SqlCareerDS" DataValueField="id" 
                                 DataTextField="descripcion" CssClass="form-control" OnDataBound="CareerDropdown_DataBound"></asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ErrorMessage="La carrera es requerida." 
                                 ControlToValidate="CareerDropdown" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="col-sm-4">
                            <div class="form-group">
                                <label>Turno</label>
                                <asp:DropDownList ID="TurnDropdown" runat="server" CssClass="form-control">
                                    <asp:ListItem Value="" Text="--- Seleeciona el turno ---" Selected="true"></asp:ListItem>
                                    <asp:ListItem Value="M" Text="Matutino"></asp:ListItem>
                                    <asp:ListItem Value="V" Text="Vespertino"></asp:ListItem>
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ErrorMessage="El turno es requerido." 
                                 ControlToValidate="TurnDropdown" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                    </div>
                    <div class="form-group">
                        <label>Correo Electrónico</label>
                        <asp:TextBox ID="EmailTextBox" runat="server" CssClass="form-control" TextMode="Email"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="El correo electrónico es requerido."
                         ControlToValidate="EmailTextBox" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="El correo electrónico no es valido."
                         ControlToValidate="EmailTextBox" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="Dynamic" ForeColor="Red"></asp:RegularExpressionValidator>
                    </div>
                    <div class="row">
                        <div class="form-group col-sm-6">
                            <label>Contraseña</label>
                            <asp:TextBox ID="PasswordTextBox" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="La contraseña es requerida."
                             ControlToValidate="PasswordTextBox" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group col-sm-6">
                            <label>Confirmar Contraseña</label>
                            <asp:TextBox ID="ConfirmPassTextBox" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Confirmar la contraseña es requerido."
                             ControlToValidate="ConfirmPassTextBox" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Las contraseñas no coinciden."
                             ControlToValidate="PasswordTextBox" ControlToCompare="ConfirmPassTextBox" ForeColor="Red" Display="Dynamic"></asp:CompareValidator>
                        </div>
                    </div>
                    <asp:Button ID="RegisterButton" runat="server" Text="Registrar" onclick="RegisterButton_Click"/>
                    <asp:SqlDataSource ID="SqlCareerDS" runat="server" ConnectionString='<%$ ConnectionStrings:EstacionamientoConnStr %>'
                     SelectCommand="SELECT id, descripcion FROM CARRERAS ORDER BY descripcion"></asp:SqlDataSource>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
