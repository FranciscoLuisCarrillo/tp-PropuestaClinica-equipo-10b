<%@ Page Title="Recepción — Clínica Médica" Language="C#"
    MasterPageFile="~/App.Master"
    AutoEventWireup="true" CodeBehind="Default.aspx.cs"
    Inherits="Presentacion.Recepcion.Default" %>

<asp:Content ID="HeadSeo" ContentPlaceHolderID="HeadExtra" runat="server">
  <title>Recepción — Administración de turnos</title>
  <meta name="description" content="Gestión integral de turnos: alta, listado diario, reprogramación y cancelación." />
  <meta name="robots" content="noindex,follow" />
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
  <main class="container my-4" role="main" aria-labelledby="titulo-recepcion">

    <h1 id="titulo-recepcion" class="h3 mb-4">Recepción — Gestión de turnos</h1>

    <!-- ================== ALTA DE TURNO ================== -->
    <section class="card shadow-sm mb-4" aria-labelledby="alta-turno">
      <div class="card-header">
        <h2 id="alta-turno" class="h6 mb-0">Agendar nuevo turno</h2>
      </div>
      <div class="card-body">
        <asp:ValidationSummary ID="valAlta" runat="server" CssClass="alert alert-danger" ValidationGroup="Alta" />
        <div class="row g-3">
          <div class="col-md-4">
            <label class="form-label">Paciente</label>
            <asp:DropDownList ID="ddlPaciente" runat="server" CssClass="form-select" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlPaciente"
              InitialValue="" ErrorMessage="Seleccioná un paciente." CssClass="text-danger" ValidationGroup="Alta" />
          </div>

          <div class="col-md-4">
            <label class="form-label">Especialidad</label>
            <asp:DropDownList ID="ddlEspecialidad" runat="server" CssClass="form-select" AutoPostBack="true"
              OnSelectedIndexChanged="ddlEspecialidad_SelectedIndexChanged" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlEspecialidad"
              InitialValue="" ErrorMessage="Seleccioná una especialidad." CssClass="text-danger" ValidationGroup="Alta" />
          </div>

          <div class="col-md-4">
            <label class="form-label">Médico</label>
           <asp:DropDownList ID="ddlMedico" runat="server" CssClass="form-select"
             AutoPostBack="true" OnSelectedIndexChanged="ddlMedico_SelectedIndexChanged" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlMedico"
              InitialValue="" ErrorMessage="Seleccioná un médico." CssClass="text-danger" ValidationGroup="Alta" />
          </div>

          <div class="col-md-4">
            <label class="form-label">Fecha</label>
            <asp:TextBox ID="txtFecha" runat="server" CssClass="form-control" TextMode="Date" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtFecha"
              ErrorMessage="La fecha es obligatoria." CssClass="text-danger" ValidationGroup="Alta" />
          </div>

          <div class="col-md-4">
            <label class="form-label">Hora</label>
            <asp:DropDownList ID="ddlHora" runat="server" CssClass="form-select" />
            <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlHora"
              InitialValue="" ErrorMessage="La hora es obligatoria." CssClass="text-danger" ValidationGroup="Alta" />
          </div>

          <div class="col-md-12">
            <label class="form-label">Observaciones</label>
            <asp:TextBox ID="txtObs" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" />
          </div>
        </div>

        <div class="text-end mt-3">
          <asp:Button ID="btnCrearTurno" runat="server" Text="Confirmar turno" CssClass="btn btn-primary"
            OnClick="btnCrearTurno_Click" ValidationGroup="Alta" />
        </div>
      </div>
    </section>

    <!-- ================== TURNOS DEL DÍA ================== -->
    <section class="card shadow-sm" aria-labelledby="turnos-dia">
      <div class="card-header d-flex align-items-center justify-content-between">
        <h2 id="turnos-dia" class="h6 mb-0">Turnos del día</h2>
        <div class="d-flex align-items-center">
          <asp:TextBox ID="txtFechaListado" runat="server" CssClass="form-control form-control-sm w-auto me-2" TextMode="Date" />
          <asp:Button ID="btnBuscarTurnosDia" runat="server" Text="Buscar" CssClass="btn btn-outline-secondary btn-sm"
            OnClick="btnBuscarTurnosDia_Click" />
        </div>
      </div>
      <div class="card-body p-0">
        <asp:GridView ID="gvTurnosDia" runat="server"
            CssClass="table table-striped table-hover table-sm"
            AutoGenerateColumns="false"
            DataKeyNames="TurnoId"
            OnRowCommand="gvTurnosDia_RowCommand">
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
      </div>
    </section>

    <!-- ================== MODAL REPROGRAMAR ================== -->
    <div class="modal fade" id="mdlReprog" tabindex="-1" aria-hidden="true">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">Reprogramar turno</h5>
            <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Cerrar"></button>
          </div>
          <div class="modal-body">
            <asp:HiddenField ID="hfReprogId" runat="server" />
            <div class="mb-3">
              <label class="form-label">Nueva fecha</label>
              <asp:TextBox ID="txtReprogFecha" runat="server" CssClass="form-control" TextMode="Date" />
            </div>
            <div class="mb-3">
              <label class="form-label">Nueva hora</label>
              <asp:DropDownList ID="ddlReprogHora" runat="server" CssClass="form-select" />
            </div>
            <small class="text-muted">El médico se mantiene. Si querés cambiar de médico, cancelá y creá un turno nuevo.</small>
          </div>
          <div class="modal-footer">
            <button type="button" class="btn btn-outline-secondary" data-bs-dismiss="modal">Cerrar</button>
            <asp:Button ID="btnConfirmarReprog" runat="server" Text="Guardar cambios" CssClass="btn btn-primary"
              OnClick="btnConfirmarReprog_Click" />
          </div>
        </div>
      </div>
    </div>

  </main>

  
  
</asp:Content>
