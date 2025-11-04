<%@ Page Title="Acceso — Clínica Médica" Language="C#"
    MasterPageFile="~/App.Master"
    AutoEventWireup="true" CodeBehind="Login.aspx.cs"
    Inherits="Presentacion.Account.Login" %>

<asp:Content ID="HeadSeo" ContentPlaceHolderID="HeadExtra" runat="server">
  <title>Acceso — Clínica Médica</title>
  <meta name="description" content="Ingresá con tu email y contraseña para acceder al sistema de turnos médicos online." />
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
  <div class="container" style="max-width: 480px;">
    <h1 class="h3 text-center mb-4">Iniciar sesión</h1>

    <asp:ValidationSummary ID="valSum" runat="server" CssClass="alert alert-danger" EnableClientScript="true" />

    <div class="mb-3">
      <label for="txtEmail" class="form-label">Email</label>
      <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
      <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmail" ErrorMessage="El email es obligatorio" CssClass="text-danger" />
    </div>

    <div class="mb-3">
      <label for="txtPass" class="form-label">Contraseña</label>
      <asp:TextBox ID="txtPass" runat="server" CssClass="form-control" TextMode="Password" />
      <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPass" ErrorMessage="La contraseña es obligatoria" CssClass="text-danger" />
    </div>

    <asp:Button ID="btnLogin" runat="server" Text="Entrar" CssClass="btn btn-primary w-100 mb-3" />

    <div class="text-center">
      <span class="text-muted">¿No tenés cuenta?</span>
      <a runat="server" href="~/Account/Register.aspx" class="text-decoration-none">Registrate</a>
    </div>
  </div>
</asp:Content>
