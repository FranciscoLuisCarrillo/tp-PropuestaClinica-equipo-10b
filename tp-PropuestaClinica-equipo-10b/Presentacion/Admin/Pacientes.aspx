<%@ Page Title="" Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="Pacientes.aspx.cs" Inherits="Presentacion.Admin.Pacientes" %>
<asp:Content ID="HeadSeo" ContentPlaceHolderID="HeadExtra" runat="server">
    <title>Pacientes - Panel de administracion</title>
    <meta name="description" content="Gestion Pacientes"/>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
    <main class="container my-4" role="main">
        <h1 class="h3 mb-4">Pacientes</h1>

        <asp:GridView ID="gvPacientes" runat="server"
            CssClass="table table-sm table-hover"
            AutoGenerateColumns="false"
            DataKeyNames="IdPaciente"
            onRowCommand="gvPacientes_RowCommand">
            <Columns>
                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                <asp:BoundField DataField="Apellido" HeaderText="Apellido" />
                <asp:BoundField DataField="Dni" HeaderText="DNI" />
                <asp:BoundField DataField="Email" HeaderText="Email" />
                <asp:CheckBoxField DataField="Activo" HeaderText="Activo" />
                <asp:ButtonField CommandName="ToggleActivo" Text="Cambiar estado" />
            </Columns>
        </asp:GridView>

        <div class="text-end mt-3">
            <a href="Default.aspx" class="btn btn-secondary">Atrás</a>
        </div>
    </main>
</asp:Content>
