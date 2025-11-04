<%@ Page Title="Panel del Paciente — Clínica Médica" Language="C#" 
    MasterPageFile="~/App.Master" AutoEventWireup="true" 
    CodeBehind="Dashboard.aspx.cs" Inherits="Presentacion.Paciente.Dashboard" %>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
<div class="container my-5">
  <h1 class="h3 mb-4 text-center">Mis Turnos</h1>

  
  <asp:GridView ID="gvTurnos" runat="server" CssClass="table table-striped table-hover" AutoGenerateColumns="false">
    <Columns>
      <asp:BoundField DataField="Fecha" HeaderText="Fecha" />
      <asp:BoundField DataField="Hora" HeaderText="Hora" />
      <asp:BoundField DataField="Especialidad" HeaderText="Especialidad" />
      <asp:BoundField DataField="Medico" HeaderText="Médico" />
      <asp:ButtonField Text="Cancelar" ButtonType="Button" CommandName="Cancelar" />
    </Columns>
  </asp:GridView>

 
  <hr class="my-5"/>

  
  <h2 class="h4 mb-3">Solicitar nuevo turno</h2>
  <div class="row g-3">
    <div class="col-md-4">
      <label class="form-label">Especialidad</label>
      <asp:DropDownList ID="ddlEspecialidad" runat="server" CssClass="form-select"  />
    </div>
    <div class="col-md-4">
      <label class="form-label">Médico</label>
      <asp:DropDownList ID="ddlMedico" runat="server" CssClass="form-select" />
    </div>
    <div class="col-md-4">
      <label class="form-label">Fecha</label>
      <asp:TextBox ID="txtFecha" runat="server" CssClass="form-control" TextMode="Date" />
    </div>
  </div>

  <div class="mt-4 text-center">
    <asp:Button ID="btnBuscarHorarios" runat="server" Text="Buscar horarios" CssClass="btn btn-primary" />
  </div>

  <asp:Panel ID="pnlHorarios" runat="server" CssClass="mt-4" Visible="false">
    <h3 class="h5">Horarios disponibles</h3>
    <asp:Repeater ID="rptHorarios" runat="server">
      <ItemTemplate>
        <button type="button" class="btn btn-outline-success btn-sm m-1"><%# Eval("Hora") %></button>
      </ItemTemplate>
    </asp:Repeater>
  </asp:Panel>
</div>
</asp:Content>
