<%@ Page Title="Mi Agenda — Clínica Médica" Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="AgendaMedico.aspx.cs" Inherits="Presentacion.Medicos.AgendaMedico" %>

<asp:Content ID="HeadSeo" ContentPlaceHolderID="HeadExtra" runat="server">
    <meta name="description" content="Agenda de turnos del día. Filtre por fecha y seleccione pacientes para atención." />
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
    <main class="container my-4">
        <header class="mb-4 border-bottom pb-2">
            <h1 class="h2">Agenda de Turnos</h1>
        </header>

        <section class="bg-light p-4 rounded shadow-sm mb-4" aria-labelledby="filtros-busqueda">
            <h2 id="filtros-busqueda" class="h5 mb-3">Filtrar agenda</h2>
            <div class="d-flex gap-3 align-items-end flex-wrap">
                <div>
                    <asp:Label AssociatedControlID="txtFecha" runat="server" CssClass="form-label fw-bold">Seleccionar Fecha:</asp:Label>
                    <asp:TextBox ID="txtFecha" runat="server" CssClass="form-control" TextMode="Date" />
                </div>
                <asp:Button ID="btnBuscar" runat="server" Text="Buscar Turnos" CssClass="btn btn-primary px-4" OnClick="btnBuscar_Click" />
            </div>
        </section>

        <section aria-labelledby="listado-turnos">
            <h2 id="listado-turnos" class="visually-hidden">Listado de turnos encontrados</h2>
            
            <div class="table-responsive">
                <asp:GridView ID="gvTurnos" runat="server" 
                    CssClass="table table-striped table-hover align-middle" 
                    AutoGenerateColumns="false" 
                    GridLines="None"
                    EmptyDataText="No se encontraron turnos para la fecha seleccionada.">
                    <HeaderStyle CssClass="table-dark" />
                    <Columns>
                        <asp:BoundField DataField="Hora" HeaderText="Horario" HeaderStyle-CssClass="ps-3" ItemStyle-CssClass="ps-3 fw-bold" />
                        <asp:BoundField DataField="Paciente" HeaderText="Paciente" />
                        <asp:BoundField DataField="Motivo" HeaderText="Definicion/Resolucion" />
                        <asp:BoundField DataField="Estado" HeaderText="Estado Actual" />
                        <asp:TemplateField HeaderText="Acciones">
                            <ItemTemplate>
                                <a href="AtenderTurno.aspx?id=<%# Eval("IdTurno") %>" class="btn btn-sm btn-success" title="Atender a <%# Eval("Paciente") %>">
                                    Atender / Editar
                                </a>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </section>
    </main>
</asp:Content>