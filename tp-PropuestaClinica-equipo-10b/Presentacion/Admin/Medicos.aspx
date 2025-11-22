<%@ Page Title="Medicos - Panel de administracion" Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="Medicos.aspx.cs" Inherits="Presentacion.Admin.Medicos" %>

<asp:Content ID="HeadSeo" ContentPlaceHolderID="HeadExtra" runat="server">
  <title>Médicos - Panel de administración</title>
  <meta name="description" content="Gestión de médicos" />
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
  <main class="container my-4" role="main" aria-labelledby="titulo-medicos">

    <!-- ENCABEZADO -->
    <div class="d-flex justify-content-between align-items-center mb-4">
      <div>
        <h1 id="titulo-medicos" class="h3 mb-1">Médicos</h1>
        <p class="text-muted mb-0">Gestión de profesionales de la clínica.</p>
      </div>
      <button type="button" class="btn btn-primary"
        onclick="document.getElementById('pnlNuevoMedico').style.display='block';
                 document.getElementById('pnlListadoMedicos').style.display='none';
                 window.scrollTo({ top: document.getElementById('pnlNuevoMedico').offsetTop - 80, behavior: 'smooth' });">
        + Añadir médico
      </button>
    </div>

    <!-- LISTADO -->
    <asp:Panel ID="pnlListadoMedicos" runat="server" ClientIDMode="Static" CssClass="card shadow-sm mb-4">
      <div class="card-header d-flex justify-content-between align-items-center">
        <h2 id="titulo-listado-medicos" class="h6 mb-0">Listado de médicos</h2>
        <span class="text-muted small">Doble clic para editar</span>
      </div>
      <div class="card-body p-0">
        <div class="table-responsive">
          <asp:GridView ID="gvMedicos" runat="server"
            CssClass="table table-sm table-hover mb-0 align-middle"
            AutoGenerateColumns="false" DataKeyNames="Id">
            <HeaderStyle CssClass="table-light" />
            <Columns>
              <asp:BoundField DataField="Nombre" HeaderText="Nombre/es" />
              <asp:BoundField DataField="Apellido" HeaderText="Apellido/s" />
              <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
              <asp:BoundField DataField="Email" HeaderText="Email" />
              <asp:BoundField DataField="Matricula" HeaderText="Matrícula" />
              <asp:BoundField DataField="EspecialidadTexto" HeaderText="Especialidades" />
              <asp:BoundField DataField="NombreTurnoTrabajo" HeaderText="Turno" />
              <asp:CheckBoxField DataField="Activo" HeaderText="Activo" />
            </Columns>
            <EmptyDataTemplate>
              <div class="p-3 text-center text-muted">
                No hay médicos cargados todavía. Hacé clic en <strong>“Añadir médico”</strong> para crear el primero.
              </div>
            </EmptyDataTemplate>
          </asp:GridView>
        </div>
      </div>
    </asp:Panel>

    <!-- FORMULARIO ALTA -->
    <asp:Panel ID="pnlNuevoMedico" ClientIDMode="Static" runat="server" CssClass="card shadow-sm mb-4" Style="display:none;">
      <div class="card-header d-flex justify-content-between align-items-center">
        <h2 id="titulo-nuevo-medico" class="h6 mb-0">Nuevo médico</h2>
        <button type="button" class="btn btn-sm btn-outline-secondary" onclick="ocultarFormularioMedico()">Cancelar</button>
      </div>

      <div class="card-body">
        <asp:ValidationSummary ID="ValidarMedico" runat="server" CssClass="alert alert-danger mb-3" EnableClientScript="true" />

        <div class="row g-4">
          <!-- Columna izquierda -->
          <div class="col-md-6">
            <div class="mb-3">
              <label for="txtNombre" class="form-label">Nombre/es</label>
              <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" />
              <asp:RequiredFieldValidator ID="rfvNombre" runat="server" ControlToValidate="txtNombre"
                ErrorMessage="El nombre es obligatorio." CssClass="text-danger" Display="Dynamic" />
            </div>

            <div class="mb-3">
              <label for="txtApellido" class="form-label">Apellido/s</label>
              <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" />
              <asp:RequiredFieldValidator ID="rfvApellido" runat="server" ControlToValidate="txtApellido"
                ErrorMessage="El apellido es obligatorio." CssClass="text-danger" Display="Dynamic" />
            </div>

            <div class="mb-3">
              <label for="txtTelefono" class="form-label">Teléfono</label>
              <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" />
              <asp:RequiredFieldValidator ID="rfvTelefono" runat="server" ControlToValidate="txtTelefono"
                ErrorMessage="El teléfono es obligatorio." CssClass="text-danger" Display="Dynamic" />
            </div>

            <div class="mb-3">
              <label for="txtEmail" class="form-label">Email</label>
              <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
              <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                ErrorMessage="El email es obligatorio." CssClass="text-danger" Display="Dynamic" />
            </div>

            <div class="mb-3">
              <label for="txtPassword" class="form-label">Contraseña (usuario médico)</label>
              <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" />
              <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword"
                ErrorMessage="La contraseña es obligatoria." CssClass="text-danger" Display="Dynamic" />
            </div>

            <div class="mb-3">
              <label for="txtMatricula" class="form-label">Matrícula</label>
              <asp:TextBox ID="txtMatricula" runat="server" CssClass="form-control" />
              <asp:RequiredFieldValidator ID="rfvMatricula" runat="server" ControlToValidate="txtMatricula"
                ErrorMessage="La matrícula es obligatoria." CssClass="text-danger" Display="Dynamic" />
            </div>
          </div>

          <!-- Columna derecha -->
          <div class="col-md-6">
            <div class="mb-3">
              <label for="ddlTurnoTrabajo" class="form-label">Turno de trabajo</label>
              <asp:DropDownList ID="ddlTurnoTrabajo" DataValueField="TurnoTrabajoId" runat="server" CssClass="form-select" AppendDataBoundItems="true">
                <asp:ListItem Text="-- Seleccionar --" Value="" />
              </asp:DropDownList>
            </div>
              <asp:RequiredFieldValidator ID="rfvTurno" runat="server"
                ControlToValidate="ddlTurnoTrabajo"
                InitialValue=""
                ErrorMessage="Debe seleccionar un turno de trabajo."
                CssClass="text-danger" Display="Dynamic" />

            <div class="mb-2">
              <label class="form-label">Especialidades</label>
              <div class="border rounded p-2" style="max-height: 260px; overflow:auto">
                <asp:CheckBoxList ID="chkEspecialidades" data-rol="especialidades" runat="server"
                  CssClass="form-check" RepeatDirection="Vertical" />
              </div>
              <asp:CustomValidator ID="cvEspecialidades" runat="server"
                ErrorMessage="Debe seleccionar al menos una especialidad."
                CssClass="text-danger" Display="Dynamic"
                ClientValidationFunction="validarEspecialidades"></asp:CustomValidator>
            </div>
          </div>
        </div>

        <div class="d-flex justify-content-end gap-2 mt-3">
          <button type="button" class="btn btn-outline-secondary" onclick="ocultarFormularioMedico()">Cancelar</button>
          <asp:Button ID="btnGuarda" runat="server" Text="Guardar médico" CssClass="btn btn-success" OnClick="btnGuarda_Click" />
        </div>
      </div>
    </asp:Panel>

    <div class="text-end mt-2">
      <a href="Default.aspx" class="btn btn-secondary">Volver al panel</a>
    </div>
  </main>
</asp:Content>