<%@ Page Title="Gestión de Pacientes" Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="Pacientes.aspx.cs" Inherits="Presentacion.Admin.Pacientes" %>

<asp:Content ID="HeadSeo" ContentPlaceHolderID="HeadExtra" runat="server">
    <meta name="description" content="Panel de administración de pacientes." />
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
  <main class="container my-5">
    <header class="d-flex justify-content-between align-items-center mb-4 border-bottom pb-3">
      <div>
        <h1 class="h3 text-primary mb-0">Gestión de Pacientes</h1>
        <p class="text-muted small mb-0">Administración de usuarios y estados</p>
      </div>
      <a href="AddPaciente.aspx" class="btn btn-success shadow-sm">
      <i class="bi bi-person-plus-fill me-2"></i>Nuevo Paciente
    </a>
    </header>

    <section aria-labelledby="filtro-pacientes" class="card shadow-sm mb-4 border-0">
      <h2 id="filtro-pacientes" class="visually-hidden">Filtros de búsqueda</h2>
      <div class="card-body bg-light rounded">
        <div class="input-group" style="max-width:600px">
          <asp:Label AssociatedControlID="txtFiltro" runat="server" CssClass="visually-hidden">Buscar paciente</asp:Label>
          <asp:TextBox ID="txtFiltro" runat="server" CssClass="form-control"
                       placeholder="Buscar por nombre, apellido o DNI..."
                       AutoPostBack="true" OnTextChanged="txtFiltro_TextChanged"></asp:TextBox>
          <span class="input-group-text"><i class="bi bi-search"></i></span>
        </div>
      </div>
    </section>

  <section class="mb-4">
  <asp:Literal ID="litVacio" runat="server" />
  <div class="row row-cols-1 row-cols-sm-2 row-cols-lg-3 row-cols-xl-4 g-3" id="gridPacientes">
    <asp:Repeater ID="repPacientes" runat="server">
      <ItemTemplate>
        <div class="col">
          <article class="card h-100 shadow-sm p-3 d-flex">
            <header class="d-flex justify-content-between align-items-start mb-2">
              <h3 class="h6 mb-0">
                <%# Eval("Apellido") %>, <%# Eval("Nombre") %>
              </h3>
            </header>

            <ul class="list-unstyled small mb-3">
              <li><strong>DNI:</strong> <%# Eval("Dni") %></li>
              <li><strong>Email:</strong> <%# Eval("Email") %></li>
              <li><strong>Tel.:</strong> <%# Eval("Telefono") %></li>
            </ul>

            <div class="mt-auto d-flex gap-2">
              <button type="button"
                class="btn btn-sm btn-warning"
                onclick="abrirModalPassEmail('<%# Eval("Email") %>')"
                title="Cambiar contraseña"
                aria-label='Cambiar contraseña de <%# Eval("Nombre") %> <%# Eval("Apellido") %>'>
                <i class="bi bi-key-fill text-dark" aria-hidden="true"></i>
              </button>
            </div>
          </article>
        </div>
      </ItemTemplate>
    </asp:Repeater>
  </div>
</section>

  <asp:HiddenField ID="hfEmailDestino" runat="server" ClientIDMode="Static" />

    <div class="modal fade" id="modalPassword" tabindex="-1" aria-labelledby="modalPassLabel" aria-hidden="true">
      <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content shadow-lg border-0">
          <header class="modal-header bg-warning bg-gradient">
            <h3 class="modal-title fs-5 text-dark" id="modalPassLabel">
              <i class="bi bi-shield-lock-fill me-2" aria-hidden="true"></i>Cambiar Contraseña
            </h3>
            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar ventana"></button>
          </header>

          <div class="modal-body p-4">
            <div class="alert alert-light border mb-3" role="alert">
              <i class="bi bi-info-circle-fill me-2 text-primary"></i>
              Estás cambiando la clave para: <strong><span id="lblEmailModal"></span></strong>
            </div>

            <div class="mb-3">
              <asp:Label AssociatedControlID="txtNuevaPass" runat="server" CssClass="form-label fw-bold">Nueva Contraseña</asp:Label>
              <div class="input-group">
                <span class="input-group-text bg-light"><i class="bi bi-key" aria-hidden="true"></i></span>
                <asp:TextBox ID="txtNuevaPass" runat="server" CssClass="form-control" TextMode="Password"
                             placeholder="Ingrese la nueva contraseña..." ClientIDMode="Static" />
              </div>
              <small class="text-muted d-block mt-1">Mínimo 8 caracteres recomendados.</small>
            </div>
          </div>

          <footer class="modal-footer bg-light">
            <button type="button" class="btn btn-link text-decoration-none text-secondary" data-bs-dismiss="modal">Cancelar</button>
            <asp:Button ID="btnGuardarPass" runat="server" Text="Confirmar Cambio"
                        CssClass="btn btn-dark px-4" OnClick="btnGuardarPass_Click" />
          </footer>
        </div>
      </div>
    </div>
      <div class="text-end mt-2">
          <a href="Default.aspx" class="btn btn-secondary">Volver al panel</a>
        </div>
  </main>

  <script>
      function abrirModalPassEmail(email) {
          document.getElementById('hfEmailDestino').value = email;
          document.getElementById('lblEmailModal').innerText = email;
          var myModal = new bootstrap.Modal(document.getElementById('modalPassword'));
          myModal.show();
      }
  </script>
</asp:Content>