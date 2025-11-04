<%@ Page Title="Registro - Clinica Medica" Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="Presentacion.Account.Register" %>

<asp:Content ID="Head" ContentPlaceHolderID="HeadExtra" runat="server">
  <title>Registro — Clínica Médica</title>
  <meta name="description" content="Creá tu cuenta para gestionar turnos. Elegí si sos médico o paciente, y registrate con tu email y contraseña." />
  
</asp:Content>


<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
  <div class="mx-auto" style="max-width:720px;">
    <h1 class="h3 mb-3 text-center">Crear cuenta</h1>
   
    
<div class="mb-4">
  <label class="form-label d-block">Registrarme como</label>

  <div class="form-check form-check-inline">
    <asp:RadioButton ID="rbPaciente" runat="server" GroupName="rol"
                     AutoPostBack="true" OnCheckedChanged="cambioRolMarcado"
                     CssClass="form-check-input" Checked="true" />
    <asp:Label runat="server" AssociatedControlID="rbPaciente"
               CssClass="form-check-label">Paciente</asp:Label>
  </div>

  <div class="form-check form-check-inline">
    <asp:RadioButton ID="rbMedico" runat="server" GroupName="rol"
                     AutoPostBack="true" OnCheckedChanged="cambioRolMarcado"
                     CssClass="form-check-input" />
    <asp:Label runat="server" AssociatedControlID="rbMedico"
               CssClass="form-check-label">Médico</asp:Label>
  </div>
</div>
 
<div class="row g-3">
  <div class="col-md-6">
    <label for="txtNombre" class="form-label">Nombre</label>
    <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" />
    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNombre"
      ErrorMessage="Requerido" CssClass="text-danger" />
  </div>

  <div class="col-md-6">
    <label for="txtApellido" class="form-label">Apellido</label>
    <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" />
    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtApellido"
      ErrorMessage="Requerido" CssClass="text-danger" />
  </div>

  <div class="col-md-6">
    <label for="txtEmail" class="form-label">Email</label>
    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmail"
      ErrorMessage="Requerido" CssClass="text-danger" />
  </div>

  <div class="col-md-6">
    <label for="txtPass" class="form-label">Contraseña</label>
    <asp:TextBox ID="txtPass" runat="server" CssClass="form-control" TextMode="Password" />
    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtPass"
      ErrorMessage="Requerido" CssClass="text-danger" />
  </div>

    <div class="col-md-6" runat="server" id="grpMatricula">
    <label for="txtMatricula" class="form-label">Matrícula (solo médico)</label>
    <asp:TextBox ID="txtMatricula" runat="server" CssClass="form-control" />
    <asp:RequiredFieldValidator ID="reqMatricula" runat="server"
      ControlToValidate="txtMatricula" ErrorMessage="Requerido para médicos"
      CssClass="text-danger" Enabled="false" />
  </div>

  <div class="col-md-6" runat="server" id="grpObra">
    <label for="txtObra" class="form-label">Obra social (solo paciente)</label>
    <asp:TextBox ID="txtObra" runat="server" CssClass="form-control" />
    <asp:RequiredFieldValidator ID="reqObra" runat="server"
      ControlToValidate="txtObra" ErrorMessage="Requerido para pacientes"
      CssClass="text-danger" Enabled="true" />
  </div>
</div>

<asp:Button ID="btnRegistrar" runat="server" Text="Crear cuenta"
            CssClass="btn btn-success mt-4" OnClick="btnRegistrar_Click" />
</div>
</asp:Content>
