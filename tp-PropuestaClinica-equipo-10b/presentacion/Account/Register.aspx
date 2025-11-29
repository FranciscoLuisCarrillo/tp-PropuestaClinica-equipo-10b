<%@ Page Title="Registro - Clinica Medica" Language="C#" 
    AutoEventWireup="true" CodeBehind="Register.aspx.cs" 
    Inherits="Presentacion.Account.Register" %>

<!DOCTYPE html>
<html lang="es" dir="ltr">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Registro — Clínica Médica</title>
    <meta name="description" content="Registrate como paciente para poder reservar y gestionar tus turnos en la clínica médica." />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body class="bg-light">
    <form id="form1" runat="server">
        <div class="mx-auto bg-white shadow-sm rounded-3 p-4" style="max-width:720px; margin-top:40px;">
            <h1 class="h3 mb-3 text-center">Crear cuenta de paciente</h1>
            <p class="text-muted text-center mb-4">Solo necesitamos algunos datos para crear tu cuenta.</p>

            <asp:ValidationSummary ID="valSummary" runat="server" CssClass="alert alert-danger" />

            <div class="row g-3">
                <div class="col-md-6">
                    <label for="txtNombre" class="form-label">Nombre</label>
                    <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNombre"
                        ErrorMessage="El nombre es requerido." CssClass="text-danger" Display="Dynamic" />
                </div>

                <div class="col-md-6">
                    <label for="txtApellido" class="form-label">Apellido</label>
                    <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtApellido"
                        ErrorMessage="El apellido es requerido." CssClass="text-danger" Display="Dynamic" />
                </div>

                <div class="col-md-12">
                    <label for="txtEmail" class="form-label">Email</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmail"
                        ErrorMessage="El email es requerido." CssClass="text-danger" Display="Dynamic" />
                </div>

                <div class="col-md-6">
                    <label for="txtPass" class="form-label">Password</label>
                    <asp:TextBox ID="txtPass" runat="server" CssClass="form-control" TextMode="Password" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPass"
                        ErrorMessage="La contraseña es requerida." CssClass="text-danger" Display="Dynamic" />
                </div>

                <div class="col-md-6">
                    <label for="txtConfirmarPass" class="form-label">Confirmar Password</label>
                    <asp:TextBox ID="txtConfirmarPass" runat="server" CssClass="form-control" TextMode="Password" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtConfirmarPass"
                        ErrorMessage="La confirmación es requerida." CssClass="text-danger" Display="Dynamic" />
                    <asp:CompareValidator runat="server" ControlToValidate="txtConfirmarPass" ControlToCompare="txtPass"
                        ErrorMessage="Las contraseñas no coinciden." CssClass="text-danger" Display="Dynamic" />
                </div>
            </div>

            <asp:Button ID="btnCrearCuenta" runat="server"
                Text="Crear cuenta"
                CssClass="btn btn-success mt-4 w-100"
                OnClick="BtnCrearCuenta_Click"/>
             <div class="text-end mt-3">
                <a href="Default.aspx" class="btn btn-secondary">Atrás</a>
            </div>
        </div>
    </form>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
