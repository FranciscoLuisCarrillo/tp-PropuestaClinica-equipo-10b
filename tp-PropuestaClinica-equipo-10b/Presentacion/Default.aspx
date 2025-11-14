<%@ Page Title="Inicio — Clínica Médica" Language="C#" 
    MasterPageFile="~/App.Master" 
    AutoEventWireup="true" CodeBehind="Default.aspx.cs" 
    Inherits="Presentacion.Default" %>

<asp:Content ID="HeadSeo" ContentPlaceHolderID="HeadExtra" runat="server">
  <title>Clínica Médica — Sistema de turnos online</title>
  <meta name="description" content="Bienvenido al sistema de turnos online de Clínica Médica. Iniciá sesión o registrate como paciente o médico." />
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
  <div class="container text-center py-5">
    <h1 class="display-5 mb-3 fw-semibold">Bienvenido a Clínica Médica</h1>
    <p class="lead text-muted mb-4">Gestioná tus turnos de forma rápida y segura.</p>

    <div class="d-flex justify-content-center gap-3">
      <a runat="server" href="~/Account/Login.aspx" class="btn btn-primary btn-lg px-4">Iniciar sesión</a>
      <a runat="server" href="~/Account/Register.aspx" class="btn btn-success btn-lg px-4">Registrarme</a>
    </div>
  </div>
</asp:Content>
