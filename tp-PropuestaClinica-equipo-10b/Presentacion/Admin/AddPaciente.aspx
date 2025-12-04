<%@ Page Title="Paciente — Admin" Language="C#"
    MasterPageFile="~/App.Master"
    AutoEventWireup="true" CodeBehind="AddPaciente.aspx.cs"
    Inherits="Presentacion.Admin.AddPaciente" %>

<asp:Content ID="HeadSeo" ContentPlaceHolderID="HeadExtra" runat="server">
  <meta name="robots" content="noindex, nofollow, noarchive, nosnippet" />
  <meta name="googlebot" content="noindex, nofollow, noarchive, nosnippet" />
  <link rel="canonical" href="<%: ResolveUrl("~/Admin/Pacientes.aspx") %>" />
  <meta name="viewport" content="width=device-width, initial-scale=1" />
  <meta name="format-detection" content="telephone=no" />
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
  <div class="container my-4" style="max-width: 900px;">
    <header class="mb-3 d-flex align-items-center justify-content-between">
      <h1 class="h4 mb-0"><asp:Literal ID="litTitulo" runat="server" /></h1>
      <a href='<%: ResolveUrl("~/Admin/Pacientes.aspx") %>' class="btn btn-outline-secondary">Volver al listado</a>
    </header>

    <asp:ValidationSummary ID="valResumen" runat="server" CssClass="alert alert-danger" />

    <div class="card shadow-sm">
      <div class="card-body">
        <asp:HiddenField ID="hfPacienteId" runat="server" />
        <asp:HiddenField ID="hfUsuarioId" runat="server" />

        <div class="row g-3">
          <!-- Datos personales -->
          <div class="col-md-6">
            <label class="form-label">Nombre</label>
            <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNombre" ErrorMessage="Nombre requerido" CssClass="text-danger small" />
          </div>
          <div class="col-md-6">
            <label class="form-label">Apellido</label>
            <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtApellido" ErrorMessage="Apellido requerido" CssClass="text-danger small" />
          </div>

          <div class="col-md-4">
            <label class="form-label">DNI</label>
            <asp:TextBox ID="txtDni" runat="server" CssClass="form-control" />
          </div>
          <div class="col-md-4">
            <label class="form-label">Fecha Nacimiento</label>
            <asp:TextBox ID="txtNacimiento" runat="server" TextMode="Date" CssClass="form-control" />
          </div>
          <div class="col-md-4">
            <label class="form-label">Teléfono</label>
            <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" />
          </div>

          <div class="col-md-6">
            <label class="form-label">Email</label>
            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" />
          </div>
          <div class="col-md-6">
            <label class="form-label">Obra Social</label>
            <asp:TextBox ID="txtObraSocial" runat="server" CssClass="form-control" />
          </div>

          <!-- Estado -->
          <div class="col-12">
            <div class="form-check">
              <asp:CheckBox ID="chkActivo" runat="server" CssClass="form-check-input" />
              <label class="form-check-label" for="chkActivo">Activo</label>
            </div>
          </div>

          <hr class="my-2" />

          <!-- Credenciales de usuario -->
          <div class="col-12">
            <h2 class="h6">Usuario del sistema</h2>
            <p class="text-muted small mb-2">
              Si el paciente no tiene usuario, se creará. Si ya tiene, podés resetear la contraseña.
            </p>
          </div>

          <div class="col-md-6">
            <label class="form-label">Contraseña (nueva)</label>
            <asp:TextBox ID="txtPass" runat="server" CssClass="form-control" TextMode="Password" />
          </div>
          <div class="col-md-6">
            <label class="form-label">Confirmar contraseña</label>
            <asp:TextBox ID="txtPass2" runat="server" CssClass="form-control" TextMode="Password" />
            <asp:CompareValidator runat="server" ControlToValidate="txtPass2" ControlToCompare="txtPass" Operator="Equal" Type="String" ErrorMessage="Las contraseñas no coinciden" CssClass="text-danger small" />
          </div>

          <div class="col-12 text-end">
            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-primary" OnClick="btnGuardar_Click" />
          </div>
        </div>
      </div>
    </div>
  </div>
</asp:Content>
