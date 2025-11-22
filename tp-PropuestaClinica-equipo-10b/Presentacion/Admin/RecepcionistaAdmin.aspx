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
  <main class="container my-4" role="main" aria-labelledby="titulo-recepcionista">
    <div class="d-flex justify-content-between align-items-center mb-4">
      <div>
        <h1 id="titulo-recepcionista" class="h3 mb-1">Recepcionistas</h1>
        <p class="text-muted mb-0">Alta y listado.</p>
      </div>
    </div>

    <div class="row g-4">
      <!-- LISTADO -->
      <section class="col-lg-7">
        <div class="card shadow-sm">
          <div class="card-header"><h2 class="h6 mb-0">Listado</h2></div>
          <div class="card-body p-0">
            <asp:GridView ID="gvRecepcionistas" runat="server"
              CssClass="table table-sm table-hover mb-0 align-middle"
              AutoGenerateColumns="false" DataKeyNames="Id">
              <HeaderStyle CssClass="table-light" />
              <Columns>
                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                <asp:BoundField DataField="Apellido" HeaderText="Apellido" />
                <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
                <asp:BoundField DataField="Email" HeaderText="Email" />
                <asp:BoundField DataField="NombreTurnoTrabajo" HeaderText="Turno" />
                <asp:CheckBoxField DataField="Activo" HeaderText="Activo" />
              </Columns>
              <EmptyDataTemplate>
                <div class="p-3 text-center text-muted">No hay recepcionistas cargadas.</div>
              </EmptyDataTemplate>
            </asp:GridView>
          </div>
        </div>
      </section>

      <!-- ALTA -->
      <section class="col-lg-5">
        <div class="card shadow-sm">
          <div class="card-header"><h2 class="h6 mb-0">Nueva recepcionista</h2></div>
          <div class="card-body">
            <asp:ValidationSummary ID="valResumen" runat="server" CssClass="alert alert-danger" EnableClientScript="true" />

            <div class="mb-3">
              <label for="txtNombre" class="form-label">Nombre</label>
              <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" />
              <asp:RequiredFieldValidator ID="rfvNombre" runat="server" ControlToValidate="txtNombre"
                ErrorMessage="El nombre es obligatorio." CssClass="text-danger" Display="Dynamic" />
            </div>

            <div class="mb-3">
              <label for="txtApellido" class="form-label">Apellido</label>
              <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" />
              <asp:RequiredFieldValidator ID="rfvApellido" runat="server" ControlToValidate="txtApellido"
                ErrorMessage="El apellido es obligatorio." CssClass="text-danger" Display="Dynamic" />
            </div>

            <div class="mb-3">
              <label for="txtTelefono" class="form-label">Teléfono</label>
              <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" />
            </div>

            <div class="mb-3">
              <label for="txtEmail" class="form-label">Email</label>
              <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
              <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                ErrorMessage="El email es obligatorio." CssClass="text-danger" Display="Dynamic" />
            </div>

            <div class="mb-3">
              <label for="txtPass" class="form-label">Contraseña (usuario recepcionista)</label>
              <asp:TextBox ID="txtPass" runat="server" CssClass="form-control" TextMode="Password" />
              <asp:RequiredFieldValidator ID="rfvPass" runat="server" ControlToValidate="txtPass"
                ErrorMessage="La contraseña es obligatoria." CssClass="text-danger" Display="Dynamic" />
            </div>

            <div class="mb-3">
              <label for="ddlTurno" class="form-label">Turno de trabajo</label>
              <asp:DropDownList ID="ddlTurno" runat="server" CssClass="form-select"
                                AppendDataBoundItems="true">
                <asp:ListItem Text="-- Seleccionar --" Value="" />
              </asp:DropDownList>
            </div>

            <div class="form-check mb-3">
              <asp:CheckBox ID="chkActivo" runat="server" CssClass="form-check-input" Checked="true" />
              <label class="form-check-label" for="chkActivo">Activo</label>
            </div>

            <asp:Button ID="btnGuardar" runat="server" Text="Guardar" CssClass="btn btn-success w-100" OnClick="btnGuardar_Click" />
          </div>
        </div>
      </section>
    </div>

    <div class="text-end mt-3">
      <a href="Default.aspx" class="btn btn-secondary">Atrás</a>
    </div>
  </main>
</asp:Content>
