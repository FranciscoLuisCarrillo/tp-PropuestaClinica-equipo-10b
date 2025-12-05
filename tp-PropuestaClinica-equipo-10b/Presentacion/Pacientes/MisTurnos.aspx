<%@ Page Title="Mis Turnos" Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="MisTurnos.aspx.cs" Inherits="Presentacion.Pacientes.MisTurnos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container my-5">
        <div class="d-flex justify-content-between align-items-center mb-4">
            <h2>Mis Turnos</h2>
            <a href="ReservarTurno.aspx" class="btn btn-primary">+ Nuevo Turno</a>
        </div>

        <asp:GridView ID="gvMisTurnos" runat="server"
            CssClass="table table-striped table-hover align-middle"
            AutoGenerateColumns="false"
            EmptyDataText="No tenés turnos registrados."
            DataKeyNames="IdTurno"
            OnRowCommand="gvMisTurnos_RowCommand">
            <Columns>
                <asp:BoundField DataField="Hora" HeaderText="Fecha y Hora" />
                <asp:BoundField DataField="Especialidad" HeaderText="Especialidad" />
                <asp:BoundField DataField="Medico" HeaderText="Médico" />
                <asp:BoundField DataField="Estado" HeaderText="Estado" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="btnCancelar" runat="server"
                            Text="Cancelar"
                            CssClass="btn btn-sm btn-danger"
                            CausesValidation="false"
                            CommandName="Cancelar"
                            CommandArgument='<%# Eval("IdTurno") %>'
                            Visible='<%# Eval("Estado").ToString() == "Pendiente" %>'
                            OnClientClick="return confirm('¿Seguro que querés cancelar este turno?');" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>

        
        <div class="mt-3">
             <a href="Default.aspx" class="btn btn-secondary">Volver al Menú</a>
        </div>
    </div>
</asp:Content>