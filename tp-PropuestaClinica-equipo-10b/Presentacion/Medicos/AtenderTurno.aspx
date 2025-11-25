<%@ Page Title="Atención de Paciente — Clínica Médica" Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="AtenderTurno.aspx.cs" Inherits="Presentacion.Medicos.AtenderTurno" %>

<asp:Content ID="HeadSeo" ContentPlaceHolderID="HeadExtra" runat="server">
    <meta name="description" content="Ficha de atención médica. Registro de diagnóstico y evolución del paciente." />
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
    <main class="container my-5" style="max-width: 900px;">
        
        <nav aria-label="breadcrumb" class="mb-4">
            <a href="AgendaMedico.aspx" class="text-decoration-none text-secondary">
                &larr; Volver a la Agenda
            </a>
        </nav>

        <article class="card shadow border-0">
            <header class="card-header bg-primary text-white p-3">
                <h1 class="h4 mb-0">Ficha de Atención</h1>
            </header>

            <div class="card-body p-4">
                <section aria-labelledby="datos-paciente" class="mb-4 border-bottom pb-3">
                    <h2 id="datos-paciente" class="h5 text-primary mb-3">Información del Paciente</h2>
                    <div class="row g-3">
                        <div class="col-md-6">
                            <span class="d-block text-muted small">Nombre Completo</span>
                            <asp:Label ID="lblPaciente" runat="server" CssClass="fs-5 fw-bold" />
                        </div>
                        <div class="col-md-6">
                            <span class="d-block text-muted small">Horario del Turno</span>
                            <asp:Label ID="lblHorario" runat="server" CssClass="fs-5" />
                        </div>
                        <div class="col-12 mt-3">
                            <div class="p-3 bg-light rounded border-start border-4 border-info">
                                <span class="fw-bold d-block mb-1">Motivo de Consulta:</span>
                                <asp:Label ID="lblMotivo" runat="server" CssClass="fst-italic" />
                            </div>
                        </div>
                    </div>
                </section>

                <section aria-labelledby="evolucion-medica">
                    <h2 id="evolucion-medica" class="h5 text-primary mb-3">Evolución y Diagnóstico</h2>
                    
                    <div class="mb-4">
                        <asp:Label AssociatedControlID="txtDiagnostico" runat="server" CssClass="form-label fw-bold">Descripción clínica:</asp:Label>
                        <asp:TextBox ID="txtDiagnostico" runat="server" TextMode="MultiLine" Rows="6" CssClass="form-control" placeholder="Escriba aquí la evolución, diagnóstico y tratamiento..." />
                    </div>

                    <div class="row align-items-end g-3 bg-light p-3 rounded">
                        <div class="col-md-6">
                            <asp:Label AssociatedControlID="ddlEstado" runat="server" CssClass="form-label fw-bold">Estado del Turno:</asp:Label>
                            <asp:DropDownList ID="ddlEstado" runat="server" CssClass="form-select">
                                <asp:ListItem Text="Nuevo (Pendiente)" Value="Nuevo" />
                                <asp:ListItem Text="Atendido / Cerrado" Value="Cerrado" />
                                <asp:ListItem Text="Paciente Ausente" Value="No Asistio" />
                            </asp:DropDownList>
                        </div>
                        <div class="col-md-6 text-end">
                            <asp:Button ID="btnGuardar" runat="server" Text="Guardar Historia Clínica" CssClass="btn btn-success px-4 py-2" OnClick="btnGuardar_Click" />
                        </div>
                    </div>
                </section>
            </div>
        </article>
    </main>
</asp:Content>