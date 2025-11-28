<%@ Page Title="Turnos de trabajo - Panel de administración"
    Language="C#"
    MasterPageFile="~/App.Master"
    AutoEventWireup="true"
    CodeBehind="TurnosTrabajo.aspx.cs"
    Inherits="Presentacion.Admin.TurnosTrabajo" %>

<asp:Content ID="HeadSeo" ContentPlaceHolderID="HeadExtra" runat="server">
    <title>Turnos de trabajo — Clínica Médica</title>
    <meta name="description" content="Administración de turnos de trabajo para médicos: nombre, horario de entrada y salida." />
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
    <main class="container my-4" role="main" aria-labelledby="titulo-turnos">
        <h1 id="titulo-turnos" class="h3 mb-4">Turnos de trabajo</h1>

        <div class="row g-4">
            <!-- LISTADO -->
            <section class="col-lg-7" aria-labelledby="titulo-listado-turnos">
                <h2 id="titulo-listado-turnos" class="h5 mb-3">Listado de turnos de trabajo</h2>

                <asp:GridView ID="gvTurnosTrabajo" runat="server"
                    CssClass="table table-sm table-hover"
                    AutoGenerateColumns="false"
                    DataKeyNames="TurnoTrabajoId">
                    <Columns>
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                        <asp:BoundField DataField="HoraEntradaTexto" HeaderText="Hora de entrada" />
                        <asp:BoundField DataField="HoraSalidaTexto" HeaderText="Hora de salida" />
                    </Columns>
                </asp:GridView>
            </section>

            <!-- FORM ALTA -->
            <section class="col-lg-5" aria-labelledby="titulo-nuevo-turno">
                <h2 id="titulo-nuevo-turno" class="h5 mb-3">Nuevo turno de trabajo</h2>

                <asp:ValidationSummary ID="valTurno" runat="server"
                    CssClass="alert alert-danger"
                    EnableClientScript="true"
                    HeaderText=""
                    DisplayMode="BulletList" />

                <div class="mb-3">
                    <label for="txtNombreTurno" class="form-label">Nombre</label>
                    <asp:TextBox ID="txtNombreTurno" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator ID="rfvNombreTurno" runat="server"
                        ControlToValidate="txtNombreTurno"
                        ErrorMessage="El nombre del turno es obligatorio."
                        CssClass="text-danger" Display="Dynamic" />
                </div>

                <div class="mb-3">
                    <label for="txtHoraEntrada" class="form-label">Hora de entrada</label>
                    <asp:TextBox ID="txtHoraEntrada" runat="server"
                        CssClass="form-control"
                        TextMode="Time" />
                    <asp:RequiredFieldValidator ID="rfvHoraEntrada" runat="server"
                        ControlToValidate="txtHoraEntrada"
                        ErrorMessage="La hora de entrada es obligatoria."
                        CssClass="text-danger" Display="Dynamic" />
                </div>

                <div class="mb-3">
                    <label for="txtHoraSalida" class="form-label">Hora de salida</label>
                    <asp:TextBox ID="txtHoraSalida" runat="server"
                        CssClass="form-control"
                        TextMode="Time" />
                    <asp:RequiredFieldValidator ID="rfvHoraSalida" runat="server"
                        ControlToValidate="txtHoraSalida"
                        ErrorMessage="La hora de salida es obligatoria."
                        CssClass="text-danger" Display="Dynamic" />
                </div>

                <asp:Button ID="btnGuardarTurno" runat="server"
                    Text="Guardar turno"
                    CssClass="btn btn-success w-100"
                    OnClick="btnGuardarTurno_Click" />
            </section>
        </div>

        <div class="text-end mt-2">
          <a href="Default.aspx" class="btn btn-secondary">Volver al panel</a>
        </div>
    </main>
</asp:Content>
