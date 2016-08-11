<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="EstacionamientoUDCI.Login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    Iniciar Sesión
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <div class="col-lg-5 col-lg-offset-3 col-md-8 col-md-offset-2 col-sm-10 col-sm-offset-2 col-xs-7 col-xs-offset-3">
        <div class="panel panel-primary">
            <div class="panel-heading"><h1 class="panel-title">Iniciar Sesion</h1></div>
            <div class="panel-body">
                <div class="form-group">
                    <label>Matrícula</label>
                    <asp:TextBox ID="EnrollmentNumberTextBox" runat="server" CssClass="form-control"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="La matrícula es requerida."
                     ControlToValidate="EnrollmentNumberTextBox" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                </div>
                <div class="form-group">
                    <label>Contraseña</label>
                    <asp:TextBox ID="PassTextBox" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="La contraseña es requerida."
                     ControlToValidate="PassTextBox" Display="Dynamic" ForeColor="Red"></asp:RequiredFieldValidator>
                </div>
                <asp:Button ID="LoginButton" runat="server" Text="Iniciar Sesión" onclick="LoginButton_Click"/>
                <a href="Register.aspx">Registrarme</a>
            </div>
        </div>
    </div>
</asp:Content>
