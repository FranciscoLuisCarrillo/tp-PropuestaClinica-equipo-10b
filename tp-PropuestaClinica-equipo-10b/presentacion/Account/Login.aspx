<%@ Page Title="Acceso — Clínica Médica" Language="C#"
    AutoEventWireup="true" CodeBehind="Login.aspx.cs"
    Inherits="Presentacion.Account.Login" %>


<!DOCTYPE html>
<html lang="es" dir="ltr">
    <head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Acceso — Clínica Médica</title>
    <meta name="description" content="Iniciá sesión en tu cuenta para reservar y gestionar tus turnos médicos en la clínica médica." />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    </head>
    <body class="bg-light">
        <form id="form1" runat="server">

  <div class="container" style="max-width: 480px; margin-top:60px">
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

    <asp:Button ID="btnLogin" runat="server" Text="Entrar" CssClass="btn btn-primary w-100 mb-3" onclick="btnLogin_Click"/>

    <div class="text-center">
      <span class="text-muted">¿No tenés cuenta?</span>
      <a runat="server" href="~/Account/Register.aspx" class="text-decoration-none">Registrate</a>
    </div>
  </div>
        </form>
      <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
 </body>
 </html>
