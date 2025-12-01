<%@ Page Title="Médicos — Panel de administración" Language="C#"
    MasterPageFile="~/App.Master"
    AutoEventWireup="true"
    CodeBehind="Medicos.aspx.cs"
    Inherits="Presentacion.Admin.Medicos" %>

<asp:Content ID="HeadSeo" ContentPlaceHolderID="HeadExtra" runat="server">
  <title>Médicos — Panel de administración</title>
  <meta name="description" content="Administrá los profesionales de la clínica: alta, edición, turnos y especialidades." />
  <meta name="robots" content="noindex,follow" />
  <meta property="og:title" content="Médicos — Panel de administración" />
  <meta property="og:description" content="Gestión de médicos: altas, ediciones y especialidades." />
 
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
  <main class="container my-4" role="main" aria-labelledby="titulo-medicos" data-rol="page-medicos">

    <div class="d-flex justify-content-between align-items-center mb-4">
      <div>
        <h1 id="titulo-medicos" class="h3 mb-1">Médicos</h1>
        <p class="text-muted mb-0">Gestión de profesionales de la clínica.</p>
      </div>

      <!-- Botón ALTA -->
      <button type="button" class="btn btn-primary" data-rol="btn-alta">
        + Añadir médico
      </button>
    </div>

    <!-- LISTADO -->
    <asp:Panel ID="pnlListadoMedicos" runat="server" ClientIDMode="Static"
               CssClass="card shadow-sm mb-4" data-rol="panel-lista">
      <div class="card-header d-flex justify-content-between align-items-center">
        <h2 class="h6 mb-0">Listado de médicos</h2>
      </div>
      <div class="card-body p-0">
        <div class="table-responsive">
          <asp:GridView ID="gvMedicos" runat="server"
              CssClass="table table-sm table-hover mb-0 align-middle"
              AutoGenerateColumns="false"
              DataKeyNames="Id"
              OnRowCommand="gvMedicos_RowCommand"
              data-rol="grid-medicos">

            <Columns>
              <asp:TemplateField HeaderText="">
                <ItemTemplate>
                  <asp:LinkButton ID="btnEditar" runat="server"
                      Text="Editar"
                      CssClass="btn btn-outline-primary btn-sm"
                      CommandName="Editar"
                      CommandArgument='<%# Eval("Id") %>'
                      CausesValidation="false" UseSubmitBehavior="false"
                      data-rol="btn-editar" />
                </ItemTemplate>
              </asp:TemplateField>

              <asp:BoundField DataField="Nombre" HeaderText="Nombre/es" />
              <asp:BoundField DataField="Apellido" HeaderText="Apellido/s" />
              <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
              <asp:BoundField DataField="Email" HeaderText="Email" />
              <asp:BoundField DataField="Matricula" HeaderText="Matrícula" />
              <asp:BoundField DataField="EspecialidadTexto" HeaderText="Especialidades" />
              <asp:BoundField DataField="NombreTurnoTrabajo" HeaderText="Turno" />
              <asp:CheckBoxField DataField="Activo" HeaderText="Activo" />
            </Columns>
          </asp:GridView>
        </div>
      </div>
    </asp:Panel>

    <!-- FORMULARIO -->
    <asp:Panel ID="pnlNuevoMedico" runat="server" ClientIDMode="Static"
           CssClass="card shadow-sm mb-4" Style="display:none;" data-rol="panel-form">
  <asp:HiddenField ID="hfMedicoId" runat="server" ClientIDMode="Static" />

      <div class="card-header d-flex justify-content-between align-items-center">
        <h2 class="h6 mb-0" data-rol="form-title">Nuevo médico</h2>
        <button type="button" class="btn btn-sm btn-outline-secondary" data-rol="btn-cancelar">Cancelar</button>
      </div>

      <div class="card-body">
        <asp:ValidationSummary ID="ValidarMedico" runat="server"
            CssClass="alert alert-danger mb-3"
            EnableClientScript="true"
            ValidationGroup="MedicoForm"
            data-rol="val-summary" />

        <div class="row g-4">
          <div class="col-md-6">
            <div class="mb-3">
              <label for="txtNombre" class="form-label">Nombre/es</label>
              <asp:TextBox ID="txtNombre" runat="server" ClientIDMode="Static" CssClass="form-control" data-rol="inp-nombre" />
              <asp:RequiredFieldValidator ID="rfvNombre" runat="server"
                  ControlToValidate="txtNombre" ErrorMessage="El nombre es obligatorio."
                  CssClass="text-danger" ValidationGroup="MedicoForm" Display="Dynamic" />
            </div>

            <div class="mb-3">
              <label for="txtApellido" class="form-label">Apellido/s</label>
              <asp:TextBox ID="txtApellido" runat="server" ClientIDMode="Static" CssClass="form-control" data-rol="inp-apellido" />
              <asp:RequiredFieldValidator ID="rfvApellido" runat="server"
                  ControlToValidate="txtApellido" ErrorMessage="El apellido es obligatorio."
                  CssClass="text-danger" ValidationGroup="MedicoForm" Display="Dynamic" />
            </div>

            <div class="mb-3">
              <label for="txtTelefono" class="form-label">Teléfono</label>
              <asp:TextBox ID="txtTelefono" runat="server" ClientIDMode="Static" CssClass="form-control" data-rol="inp-telefono" />
              <asp:RequiredFieldValidator ID="rfvTelefono" runat="server"
                  ControlToValidate="txtTelefono" ErrorMessage="El teléfono es obligatorio."
                  CssClass="text-danger" ValidationGroup="MedicoForm" Display="Dynamic" />
            </div>

            <div class="mb-3">
              <label for="txtEmail" class="form-label">Email</label>
              <asp:TextBox ID="txtEmail" runat="server" ClientIDMode="Static" CssClass="form-control" TextMode="Email" data-rol="inp-email" />
              <asp:RequiredFieldValidator ID="rfvEmail" runat="server"
                  ControlToValidate="txtEmail" ErrorMessage="El email es obligatorio."
                  CssClass="text-danger" ValidationGroup="MedicoForm" Display="Dynamic" />
            </div>

            <div class="mb-3">
              <label for="txtPassword" class="form-label">Contraseña (usuario médico)</label>
              <asp:TextBox ID="txtPassword" runat="server" ClientIDMode="Static" CssClass="form-control" TextMode="Password" data-rol="inp-pass" />
              <asp:RequiredFieldValidator ID="rfvPassword" runat="server"
                  ControlToValidate="txtPassword" ErrorMessage="La contraseña es obligatoria."
                  CssClass="text-danger" ValidationGroup="MedicoForm" Display="Dynamic" />
            </div>

            <div class="mb-3">
              <label for="txtMatricula" class="form-label">Matrícula</label>
              <asp:TextBox ID="txtMatricula" runat="server" ClientIDMode="Static" CssClass="form-control" data-rol="inp-matricula" />
              <asp:RequiredFieldValidator ID="rfvMatricula" runat="server"
                  ControlToValidate="txtMatricula" ErrorMessage="La matrícula es obligatoria."
                  CssClass="text-danger" ValidationGroup="MedicoForm" Display="Dynamic" />
            </div>
          </div>

          <div class="col-md-6">
            <div class="mb-3">
              <label for="ddlTurnoTrabajo" class="form-label">Turno de trabajo</label>
              <asp:DropDownList ID="ddlTurnoTrabajo" runat="server" ClientIDMode="Static"
                  CssClass="form-select" AppendDataBoundItems="true" data-rol="ddl-turno">
                <asp:ListItem Text="-- Seleccionar --" Value="" />
              </asp:DropDownList>
              <asp:RequiredFieldValidator ID="rfvTurno" runat="server"
                  ControlToValidate="ddlTurnoTrabajo" InitialValue=""
                  ErrorMessage="Debe seleccionar un turno de trabajo."
                  CssClass="text-danger" ValidationGroup="MedicoForm" Display="Dynamic" />
            </div>

            <div class="mb-2">
              <label class="form-label">Especialidades</label>
              <div class="border rounded p-2" style="max-height:260px;overflow:auto" data-rol="especialidades">
                <asp:CheckBoxList ID="chkEspecialidades" runat="server"
                    ClientIDMode="Static" CssClass="form-check" RepeatDirection="Vertical" />
              </div>

              <asp:CustomValidator ID="cvEspecialidades" runat="server"
                ErrorMessage="Debe seleccionar 1 o 2 especialidades."
                CssClass="text-danger" ValidationGroup="MedicoForm" Display="Dynamic"
                ClientValidationFunction="validarEspecialidades"
                OnServerValidate="cvEspecialidades_ServerValidate" />
            </div>
          </div>
        </div>

        <div class="d-flex justify-content-end gap-2 mt-3">
          <button type="button" class="btn btn-outline-secondary" data-rol="btn-cancelar">Cancelar</button>
          <asp:Button ID="btnGuarda" runat="server" ClientIDMode="Static"
              Text="Guardar médico" CssClass="btn btn-success"
              OnClick="btnGuarda_Click" ValidationGroup="MedicoForm" data-rol="btn-guardar" />
        </div>
      </div>
    </asp:Panel>

    <div class="text-end mt-2">
      <a href="Default.aspx" class="btn btn-secondary">Volver al panel</a>
    </div>

  </main>
     <script src="<%= ResolveUrl("~/Script/Script.js") %>?v=<%= DateTime.Now.Ticks %>"></script>
</asp:Content>
