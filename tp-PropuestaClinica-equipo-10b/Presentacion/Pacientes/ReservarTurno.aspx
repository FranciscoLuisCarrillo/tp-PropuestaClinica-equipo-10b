<%@ Page Title="Reservar Turno" Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="ReservarTurno.aspx.cs" Inherits="Presentacion.Pacientes.ReservarTurno" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container my-5" style="max-width: 800px;">
        <h2 class="mb-4">Reservar Nuevo Turno</h2>
        
        <asp:ValidationSummary ID="valTurno" runat="server" CssClass="alert alert-danger" />

        <div class="card shadow-sm">
            <div class="card-body p-4">
                <div class="row g-3">
      
                     <div class="col-md-6">
                        <label class="form-label">Especialidad</label>
                        <asp:DropDownList ID="ddlEspecialidades" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlEspecialidades_SelectedIndexChanged" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlEspecialidades" ErrorMessage="Seleccioná especialidad" CssClass="text-danger 
 small" Display="Dynamic" />
                    </div>

                    <div class="col-md-6">
                        <label class="form-label">Médico</label>
                        <asp:DropDownList ID="ddlMedico" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlMedico_SelectedIndexChanged" />
                         <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlMedico" ErrorMessage="Seleccioná médico" CssClass="text-danger small" Display="Dynamic" />
                         
                         <asp:Label ID="lblRangoHorario" runat="server" CssClass="d-block text-muted small mt-1 fw-bold"></asp:Label>
                    </div>

                    <div class="col-md-6">
                        <label class="form-label">Fecha</label>
    
                     <asp:TextBox ID="txtFecha"
                         runat="server"
                         CssClass="form-control"
                         TextMode="Date"
    
                      AutoPostBack="true"
                         OnTextChanged="txtFecha_TextChanged" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="txtFecha" ErrorMessage="Fecha requerida" CssClass="text-danger small" Display="Dynamic" />
                    </div>

 
                    <div class="col-md-6">
                        <label class="form-label">Hora</label>
                        <asp:DropDownList ID="ddlHora" runat="server" CssClass="form-select" />
                        <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlHora" 
 ErrorMessage="Hora requerida" CssClass="text-danger small" Display="Dynamic" />
                    </div>

                    <div class="col-12">
                        <label class="form-label">Motivo / Observaciones</label>
                        <asp:TextBox ID="txtObs" runat="server" 
 CssClass="form-control" TextMode="MultiLine" Rows="3" />
                    </div>

                    <div class="col-12 text-end mt-4">
                        <a href="Default.aspx" class="btn btn-link text-decoration-none">Volver</a>
                        <asp:Button ID="btnCrearTurno" 
 runat="server" Text="Confirmar Reserva" CssClass="btn btn-success px-4" OnClick="btnCrearTurno_Click" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>