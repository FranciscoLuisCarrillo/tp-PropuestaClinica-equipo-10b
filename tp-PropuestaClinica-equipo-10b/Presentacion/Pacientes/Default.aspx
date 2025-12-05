<%@ Page Title="Panel Paciente" Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Presentacion.Pacientes.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container my-5">
        <div class="text-center mb-5">
            <h2>Bienvenido, <asp:Label ID="lblNombreUsuario" runat="server" Text="Paciente"></asp:Label></h2>
            <p class="lead">¿Qué deseas hacer hoy?</p>
        </div>

        <div class="row justify-content-center g-4">
            <div class="col-md-5 col-lg-4">
                <div class="card h-100 shadow-sm hover-card">
                    <div class="card-body text-center p-5">
                        <h3 class="h4 mb-3">Nuevo Turno</h3>
                        <p class="text-muted mb-4">Buscá por especialidad o médico y reservá tu próxima cita.</p>
                        <a href="ReservarTurno.aspx" class="btn btn-primary w-100">Reservar ahora</a>
                    </div>
                </div>
            </div>

            


            <div class="col-md-5 col-lg-4">
                <div class="card h-100 shadow-sm hover-card">
                    <div class="card-body text-center p-5">
                        <h3 class="h4 mb-3">Mis Turnos</h3>
                        <p class="text-muted mb-4">Consultá tus turnos pendientes, historial y cancelaciones.</p>
                        <a href="MisTurnos.aspx" class="btn btn-primary w-100">Ver mis turnos</a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>