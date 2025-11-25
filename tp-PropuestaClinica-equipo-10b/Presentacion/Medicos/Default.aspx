<%@ Page Title="Panel Médico — Clínica Médica" Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Presentacion.Medicos.Default" %>

<asp:Content ID="HeadSeo" ContentPlaceHolderID="HeadExtra" runat="server">
    <meta name="description" content="Panel de control para profesionales de salud. Acceda a su agenda diaria y gestione la atención de pacientes." />
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
    <main class="container my-5">
        <header class="text-center mb-5">
            <h1 class="display-5 fw-bold">Panel de Control Médico</h1>
            <p class="lead text-muted">Bienvenido al sistema de gestión clínica.</p>
        </header>

        <section aria-labelledby="accesos-rapidos" class="row justify-content-center">
            <h2 id="accesos-rapidos" class="visually-hidden">Accesos Rápidos</h2>
            
            <div class="col-md-6 col-lg-4">
                <article class="card shadow hover-card h-100 text-center border-0">
                    <div class="card-body p-5">
                        <div class="mb-3 text-primary">
                            <i class="bi bi-calendar-check fs-1"></i>
                        </div>
                        <h3 class="card-title h4">Mi Agenda</h3>
                        <p class="card-text text-muted mb-4">Visualizá tus turnos del día, consultá históricos y atendé a tus pacientes.</p>
                        <a href="AgendaMedico.aspx" class="btn btn-primary btn-lg w-100 stretched-link">Ver Agenda</a>
                    </div>
                </article>
            </div>
        </section>
    </main>
</asp:Content>