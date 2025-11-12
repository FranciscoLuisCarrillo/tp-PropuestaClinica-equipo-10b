<%@ Page Title="Registro - Clinica Medica" Language="C#"  AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="Presentacion.Account.Register" %>

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
      <p class="text-muted text-center mb-4">Completa tus datos para poder solicitar el turno online</p>
   
      <asp:ValidationSummary ID="valSummary" runat="server" CssClass="alert alert-danger" />

 
<div class="row g-3">
    <!-- Nombre / Apellido -->
  <div class="col-md-6">
    <label for="txtNombre" class="form-label">Nombre</label>
    <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" />
    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNombre"
      ErrorMessage="El nombre es obligatorio" CssClass="text-danger" Display="Dynamic"/>
  </div>

  <div class="col-md-6">
    <label for="txtApellido" class="form-label">Apellido</label>
    <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" />
    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtApellido"
      ErrorMessage="El/Los Apellido/os es/son Requerido/os" CssClass="text-danger" Display="Dynamic" />
  </div>
    <!--Fecha de nacimiento / Género-->
     <div class="col-md-6">
     <label for="txtFechaNacimiento" class="form-label">Fecha de Nacimiento</label>
     <asp:TextBox ID="txtFechaNacimiento" runat="server" CssClass="form-control" TextMode="Date" />
     <asp:RequiredFieldValidator runat="server" ControlToValidate="txtFechaNacimiento"
         ErrorMessage="La Fecha de nacimiento es Requerida" CssClass="text-danger" Display="Dynamic" />
     </div>
 <div class="col-md-6">
     <label for="ddlGenero" class="form-label">Género</label>
     <asp:DropDownList ID="ddlGenero" runat="server" CssClass="form-select">
         <asp:ListItem Text="Masculino" Value="Masculino" />
         <asp:ListItem Text="Femenino" Value="Femenino" />
         <asp:ListItem Text="Otro" Value="Otro" />
     </asp:DropDownList>
     </div>
     <div class="col-md-6">
         <label for="txtDni" class="form-label">DNI</label>
            <asp:TextBox ID="txtDni" runat="server" CssClass="form-control" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtDni"
                ErrorMessage="El DNI es Requerido" CssClass="text-danger" Display="Dynamic" />
     </div>
     <div class="col-md-6">
         <label for="txtTelefono" class="form-label">Teléfono</label>
                <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtTelefono"
                    ErrorMessage="El Teléfono es Requerido" CssClass="text-danger" Display="Dynamic" />
     </div>
     <!-- Direccion -->
     <div class="col-md-12">
         <label for="txtDireccion" class="form-label">Direccion</label>
                <asp:TextBox ID="txtDireccion" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtDireccion"
                    ErrorMessage="La Direccion es Requerida" CssClass="text-danger" Display="Dynamic" />
     </div>
     <!-- Obra social -->
     <div class="col-md-12">
         <label for="txtObraSocial" class="form-label">Obra social</label>
         <asp:TextBox ID="txtObraSocial" runat="server" CssClass="form-control" />
         <asp:RequiredFieldValidator runat="server" ControlToValidate="txtObraSocial"
             ErrorMessage="La Obra Social es Requerida" CssClass="text-danger" Display="Dynamic" />
     </div>
     <!-- Email y confirmacion de contrasenia -->
  <div class="col-md-12">
    <label for="txtEmail" class="form-label">Email</label>
    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmail"
      ErrorMessage="Requerido" CssClass="text-danger" />
  </div>

     <div class="col-md-6">
         <label for="txtPass" class="form-label">Password</label>
         <asp:TextBox ID="txtPass" runat="server" CssClass="form-control" TextMode="Password"/>
         <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPass"
             ErrorMessage="La Contraseña es Requerida" CssClass="text-danger" Display="Dynamic" />
     </div>
        <div class="col-md-6">
            <label for="txtConfirmarPass" class="form-label">Confirmar Password</label>
            <asp:TextBox ID="txtConfirmarPass" runat="server" CssClass="form-control" TextMode="Password" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtConfirmarPass"
                ErrorMessage="La Confirmación de la Contraseña es Requerida" CssClass="text-danger" Display="Dynamic" />
            <asp:CompareValidator runat="server" ControlToValidate="txtConfirmarPass" ControlToCompare="txtPass"
                ErrorMessage="Las contraseñas no coinciden" CssClass="text-danger" Display="Dynamic" />
       </div>    
</div> 

  
<asp:Button ID="btnRegistrar" runat="server" Text="Crear cuenta"
            CssClass="btn btn-success mt-4" OnClick="btnRegistrar_Click"/>
</div>
            </form>
   <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
 </body>
</html>
