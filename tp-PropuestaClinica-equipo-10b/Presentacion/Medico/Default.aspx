<%@ Page Title="Médico — Agenda Clínica Médica" Language="C#"
    MasterPageFile="~/App.Master"
    AutoEventWireup="true" CodeBehind="Default.aspx.cs"
    Inherits="Presentacion.Medico.Default"%>

<asp:Content ID="HeadSeo" ContentPlaceHolderID="HeadExtra" runat="server">
  <title>Agenda del Médico — Clínica Médica</title>
  <meta name="description" content="Visualizá y administrá tus turnos como médico en la clínica. Marcá asistencia y registrá diagnósticos de pacientes." />
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
  <main class="container mt-4 mb-5 medico-dashboard" role="main">
      <h1 class="h4 fw-semibold mb-3">Mi agenda</h1>
    
    <section aria-labelledby="filtro-turnos">
      <div class="d-flex justify-content-center mb-3">
        <asp:TextBox ID="txtFecha" runat="server" CssClass="form-control w-auto" TextMode="Date" />
        <asp:Button ID="btnVerFecha" runat="server" Text="Ver" CssClass="btn btn-primary ms-2" />
      </div>
    </section>

 
    <section aria-labelledby="tabla-turnos">
      <h2 id="tabla-turnos" class="visually-hidden">Turnos del médico</h2>
      <asp:GridView ID="gvTurnos" runat="server"
        CssClass="table table-striped table-hover"
        AutoGenerateColumns="false" DataKeyNames="IdTurno"
        >
        <Columns>
          <asp:BoundField DataField="Hora" HeaderText="Hora" />
          <asp:BoundField DataField="Paciente" HeaderText="Paciente" />
          <asp:BoundField DataField="ObraSocial" HeaderText="Obra social" />
          <asp:BoundField DataField="Estado" HeaderText="Estado" />
          <asp:ButtonField Text="Editar" CommandName="Editar" ButtonType="Button" />
        </Columns>
      </asp:GridView>
    </section>

  
    <section aria-labelledby="detalle-turno">
      <asp:Panel ID="pnlDetalle" runat="server" CssClass="card shadow-sm mt-4" Visible="false">
        <div class="card-body">
          <h2 id="detalle-turno" class="h5 mb-3">Detalle del turno</h2>

          <p><strong>Paciente:</strong> <asp:Label ID="lblPaciente" runat="server" /></p>
          <p><strong>Fecha y hora:</strong> <asp:Label ID="lblFechaHora" runat="server" /></p>
          <p><strong>Especialidad:</strong> <asp:Label ID="lblEspecialidad" runat="server" /></p>

          <div class="mb-3">
            <label class="form-label">Estado</label>
            <asp:DropDownList ID="ddlEstadoTurno" runat="server" CssClass="form-select">
              <asp:ListItem Text="Nuevo" Value="Nuevo" />
              <asp:ListItem Text="Reprogramado" Value="Reprogramado" />
              <asp:ListItem Text="Cancelado" Value="Cancelado" />
              <asp:ListItem Text="No Asistió" Value="No Asistio" />
              <asp:ListItem Text="Cerrado" Value="Cerrado" />
            </asp:DropDownList>
          </div>

          <div class="mb-3">
            <label class="form-label">Observaciones / Diagnóstico</label>
            <asp:TextBox ID="txtDiagnostico" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" />
          </div>

          <asp:Button ID="btnGuardarDiagnostico" runat="server"
                      Text="Guardar cambios" CssClass="btn btn-success" />
        </div>
      </asp:Panel>
    </section>
  </main>
</asp:Content>
