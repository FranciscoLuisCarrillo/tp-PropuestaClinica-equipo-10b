<%@ Page Title="Recepcionistas - Panel de administración" Language="C#"
    MasterPageFile="~/App.Master"
    AutoEventWireup="true"
    CodeBehind="RecepcionistaAdmin.aspx.cs"
    Inherits="Presentacion.Admin.RecepcionistaAdmin" %>

<asp:Content ID="HeadSeo" ContentPlaceHolderID="HeadExtra" runat="server">
    <title>Recepcionistas - Panel de administración</title>
    <meta name="description" content="Gestión de recepcionistas de la clínica." />
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
  <main class="container my-4">
    <div class="d-flex justify-content-between align-items-center mb-4">
      <div>
        <h1 class="h3 mb-1">Recepcionistas</h1>
        <p class="text-muted mb-0">Alta y listado.</p>
      </div>
    </div>

    <div class="row g-4">
      <section class="col-lg-8">
        <div class="card shadow-sm">
          <div class="card-header bg-light">
              <h2 class="h6 mb-0">Listado de personal</h2>
          </div>
          <div class="card-body p-0">
            <asp:GridView ID="gvRecepcionistas" runat="server"
              CssClass="table table-striped table-hover mb-0 align-middle"
              AutoGenerateColumns="false" DataKeyNames="Id">
              <Columns>
                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                <asp:BoundField DataField="Apellido" HeaderText="Apellido" />
                <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
                <asp:BoundField DataField="Email" HeaderText="Email" />
                <asp:BoundField DataField="NombreTurnoTrabajo" HeaderText="Turno" />
                <asp:CheckBoxField DataField="Activo" HeaderText="Activo" ItemStyle-HorizontalAlign="Center" />
              </Columns>
              <EmptyDataTemplate>
                <div class="p-4 text-center text-muted">No hay recepcionistas cargadas.</div>
              </EmptyDataTemplate>
            </asp:GridView>
          </div>
        </div>
      </section>

      <section class="col-lg-4">
        <div class="card shadow-sm sticky-top" style="top: 20px; z-index: 1;">
          <div class="card-header bg-primary text-white">
              <h2 class="h6 mb-0">Nueva recepcionista</h2>
          </div>
          <div class="card-body">
            
            <asp:Label ID="lblMensaje" runat="server" CssClass="d-block mb-3 fw-bold"></asp:Label>
            
            <asp:ValidationSummary ID="valResumen" runat="server" CssClass="alert alert-danger" DisplayMode="BulletList" ShowMessageBox="false" />

            <div class="mb-3">
              <label for="txtNombre" class="form-label">Nombre</label>
              <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" />
              <asp:RequiredFieldValidator ID="rfvNombre" runat="server" ControlToValidate="txtNombre"
                ErrorMessage="El nombre es obligatorio." CssClass="text-danger small" Display="Dynamic" />
            </div>

            <div class="mb-3">
              <label for="txtApellido" class="form-label">Apellido</label>
              <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" />
              <asp:RequiredFieldValidator ID="rfvApellido" runat="server" ControlToValidate="txtApellido"
                ErrorMessage="El apellido es obligatorio." CssClass="text-danger small" Display="Dynamic" />
            </div>

            <div class="mb-3">
              <label for="txtTelefono" class="form-label">Teléfono</label>
              <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" />
              
              <asp:RequiredFieldValidator ID="rfvTelefono" runat="server" ControlToValidate="txtTelefono"
                ErrorMessage="El teléfono es obligatorio." CssClass="text-danger small" Display="Dynamic" />

              <asp:RegularExpressionValidator ID="revTelefono" runat="server" ControlToValidate="txtTelefono"
                ErrorMessage="Ingrese solo números (mínimo 10, sin guiones)." 
                CssClass="text-danger small" Display="Dynamic"
                ValidationExpression="^\d{10,}$" />
            </div>

            <div class="mb-3">
              <label for="txtEmail" class="form-label">Email</label>
              <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
              <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                ErrorMessage="El email es obligatorio." CssClass="text-danger small" Display="Dynamic" />
            </div>

            <div class="mb-3">
              <label for="txtPass" class="form-label">Contraseña (Login)</label>
              <asp:TextBox ID="txtPass" runat="server" CssClass="form-control" TextMode="Password" />
              <asp:RequiredFieldValidator ID="rfvPass" runat="server" ControlToValidate="txtPass"
                ErrorMessage="La contraseña es obligatoria." CssClass="text-danger small" Display="Dynamic" />
            </div>

            <div class="mb-3">
              <label for="ddlTurno" class="form-label">Turno de trabajo</label>
              <asp:DropDownList ID="ddlTurno" runat="server" CssClass="form-select" AppendDataBoundItems="true">
                <asp:ListItem Text="-- Seleccionar --" Value="" />
              </asp:DropDownList>
            </div>

            <div class="form-check mb-3">
              <asp:CheckBox ID="chkActivo" runat="server" CssClass="form-check-input" Checked="true" />
              <label class="form-check-label" for="chkActivo">Activo</label>
            </div>

            <div class="d-grid">
                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-success" OnClick="btnGuardar_Click" />
            </div>
          </div>
        </div>
      </section>
    </div>

    <div class="text-end mt-2">
  <a href="Default.aspx" class="btn btn-secondary">Volver al panel</a>
</div>
  </main>
</asp:Content>