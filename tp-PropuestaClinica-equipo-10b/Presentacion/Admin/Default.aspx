<%@ Page Title="Administración — Clínica Médica" Language="C#"
    MasterPageFile="~/App.Master"
    AutoEventWireup="true" CodeBehind="Default.aspx.cs"
    Inherits="Presentacion.Admin.Default" %>

<asp:Content ID="HeadSeo" ContentPlaceHolderID="HeadExtra" runat="server">
  <title>Administración — Clínica Médica</title>
  <meta name="description" content="Panel de administración del sistema de turnos de Clínica Médica. Gestioná médicos, pacientes, especialidades y turnos de trabajo." />
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
  <main class="container my-4" role="main">
    <h1 class="h3 mb-4 text-center">Panel de administración</h1>

 
    <section aria-labelledby="metricas-titulo">
      <h2 id="metricas-titulo" class="visually-hidden">Métricas principales</h2>
      <div class="row g-3 mb-4">
        <div class="col-md-4">
          <div class="card text-center shadow-sm" aria-label="Pacientes registrados">
            <div class="card-body">
              <h3 class="text-muted fs-6">Pacientes</h3>
              <p class="display-6 fw-semibold mb-0"><asp:Label ID="lblPacientes" runat="server" Text="0" /></p>
            </div>
          </div>
        </div>
        <div class="col-md-4">
          <div class="card text-center shadow-sm" aria-label="Médicos activos">
            <div class="card-body">
              <h3 class="text-muted fs-6">Médicos</h3>
              <p class="display-6 fw-semibold mb-0"><asp:Label ID="lblMedicos" runat="server" Text="0" /></p>
            </div>
          </div>
        </div>
        <div class="col-md-4">
          <div class="card text-center shadow-sm" aria-label="Turnos de hoy">
            <div class="card-body">
              <h3 class="text-muted fs-6">Turnos de hoy</h3>
              <p class="display-6 fw-semibold mb-0"><asp:Label ID="lblTurnosHoy" runat="server" Text="0" /></p>
            </div>
          </div>
        </div>
      </div>
    </section>

   
    <section aria-labelledby="accesos-titulo">
      <h2 id="accesos-titulo" class="visually-hidden">Accesos rápidos</h2>
      <div class="row g-3 mb-4">
        <div class="col-md-3"><a runat="server" href="~/Admin/Medicos.aspx" class="btn btn-outline-primary w-100">Médicos</a></div>
        <div class="col-md-3"><a runat="server" href="~/Admin/Pacientes.aspx" class="btn btn-outline-primary w-100">Pacientes</a></div>
        <div class="col-md-3"><a runat="server" href="~/Admin/Especialidades.aspx" class="btn btn-outline-primary w-100">Especialidades</a></div>
        <div class="col-md-3"><a runat="server" href="~/Admin/Recepcionista.aspx" class="btn btn-outline-primary w-100">Recepcionista</a></div>
        <div class="col-md-3"><a runat="server" href="~/Admin/TurnosTrabajo.aspx" class="btn btn-outline-primary w-100">Turnos de trabajo</a></div>
      </div>
    </section>

    
    <section aria-labelledby="ultimos-turnos-titulo">
      <h2 id="ultimos-turnos-titulo" class="h5 mb-3">Últimos turnos registrados</h2>
      <asp:GridView ID="gvUltimosTurnos" runat="server"
        CssClass="table table-striped table-hover table-sm"
        AutoGenerateColumns="false">
        <Columns>
          <asp:BoundField DataField="NumeroTurno" HeaderText="N°" />
          <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy}" />
          <asp:BoundField DataField="Hora" HeaderText="Hora" />
          <asp:BoundField DataField="Paciente" HeaderText="Paciente" />
          <asp:BoundField DataField="Medico" HeaderText="Médico" />
          <asp:BoundField DataField="Especialidad" HeaderText="Especialidad" />
          <asp:BoundField DataField="Estado" HeaderText="Estado" />
        </Columns>
      </asp:GridView>
    </section>
  </main>
</asp:Content>
