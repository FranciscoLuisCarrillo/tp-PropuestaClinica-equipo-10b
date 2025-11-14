<%@ Page Title="Recepción — Clínica Médica" Language="C#"
    MasterPageFile="~/App.Master"
    AutoEventWireup="true" CodeBehind="Default.aspx.cs"
    Inherits="Presentacion.Recepcion.Default" %>

<asp:Content ID="HeadSeo" ContentPlaceHolderID="HeadExtra" runat="server">
  <title>Recepción — Administración de turnos</title>
  <meta name="description" content="Gestión de turnos: creación, reprogramación y cancelación de turnos para pacientes de la clínica médica." />
  
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
  <main class="container my-5" role="main">
    <h1 class="h3 text-center mb-4">Gestión de turnos</h1>

    <section aria-labelledby="alta-turno">
      <h2 id="alta-turno" class="h5 mb-3">Alta de nuevo turno</h2>
      <asp:ValidationSummary ID="valTurno" runat="server" CssClass="alert alert-danger" />

      <div class="row g-3">
        <div class="col-md-4">
          <label class="form-label">Paciente</label>
          <asp:DropDownList ID="ddlPaciente" runat="server" CssClass="form-select" />
          <asp:RequiredFieldValidator runat="server"
              ControlToValidate="ddlPaciente"
              InitialValue=""
              ErrorMessage="Seleccioná un paciente."
              CssClass="text-danger"
              Display="Dynamic" />
        </div>

        <div class="col-md-4">
          <label class="form-label">Especialidad</label>
          <asp:DropDownList ID="ddlEspecialidad" runat="server"
                            CssClass="form-select"
                            AutoPostBack="true"
                             />
          <asp:RequiredFieldValidator runat="server"
              ControlToValidate="ddlEspecialidad"
              InitialValue=""
              ErrorMessage="Seleccioná una especialidad."
              CssClass="text-danger"
              Display="Dynamic" />
        </div>

        <div class="col-md-4">
          <label class="form-label">Médico</label>
          <asp:DropDownList ID="ddlMedico" runat="server" CssClass="form-select" />
          <asp:RequiredFieldValidator runat="server"
              ControlToValidate="ddlMedico"
              InitialValue=""
              ErrorMessage="Seleccioná un médico."
              CssClass="text-danger"
              Display="Dynamic" />
        </div>

        <div class="col-md-6">
          <label class="form-label">Fecha</label>
          <asp:TextBox ID="txtFecha" runat="server" CssClass="form-control" TextMode="Date" />
          <asp:RequiredFieldValidator runat="server"
              ControlToValidate="txtFecha"
              ErrorMessage="La fecha es obligatoria."
              CssClass="text-danger"
              Display="Dynamic" />
        </div>

        <div class="col-md-6">
          <label class="form-label">Hora</label>
          <asp:DropDownList ID="ddlHora" runat="server" CssClass="form-select" />
          <asp:RequiredFieldValidator runat="server"
              ControlToValidate="ddlHora"
              InitialValue=""
              ErrorMessage="La hora es obligatoria."
              CssClass="text-danger"
              Display="Dynamic" />
        </div>
        <div class="col-12">
          <label class="form-label">Observaciones</label>
          <asp:TextBox ID="txtObs" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3" />
        </div>
      </div>

      <div class="text-center mt-4">
        <asp:Button ID="btnCrearTurno" runat="server" Text="Confirmar turno" CssClass="btn btn-primary px-4" />
      </div>
    </section>

    <hr class="my-5"/>

    <section aria-labelledby="turnos-dia">
      <h2 id="turnos-dia" class="h5 mb-3">Turnos del día</h2>
      <div class="d-flex align-items-center mb-3">
        <asp:TextBox ID="txtFechaListado" runat="server" CssClass="form-control w-auto" TextMode="Date" />
        <asp:Button ID="btnBuscarTurnosDia" runat="server" Text="Buscar" CssClass="btn btn-outline-secondary ms-2" />
      </div>

      <asp:GridView ID="gvTurnosDia" runat="server"
            CssClass="table table-striped table-hover table-sm"
            AutoGenerateColumns="false"
            DataKeyNames="IdTurno"
       >
        <Columns>
          <asp:BoundField DataField="Hora" HeaderText="Hora" />
          <asp:BoundField DataField="Paciente" HeaderText="Paciente" />
          <asp:BoundField DataField="Medico" HeaderText="Médico" />
          <asp:BoundField DataField="Especialidad" HeaderText="Especialidad" />
          <asp:BoundField DataField="Estado" HeaderText="Estado" />
          <asp:ButtonField Text="Reprogramar" CommandName="Reprogramar" ButtonType="Button" CausesValidation="false" />
          <asp:ButtonField Text="Cancelar" CommandName="Cancelar" ButtonType="Button" CausesValidation="false" />
        </Columns>
      </asp:GridView>
    </section>
  </main>
</asp:Content>
