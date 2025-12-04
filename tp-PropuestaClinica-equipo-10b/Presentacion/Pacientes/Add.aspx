<%@ Page Title="" Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="Add.aspx.cs" Inherits="Presentacion.Pacientes.Add" %>
<asp:Content ID="HeadSeo" ContentPlaceHolderID="HeadExtra" runat="server">
    <title>Completar Perfil — Clínica Médica</title>
    <meta name="description" content="Completa tus datos personales para poder reservar turnos." />
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <h2 class="mb-4">Completar Perfil</h2>
        
        <% if (Session["mensaje"] != null) { %>
            <div class="alert alert-warning">
                <%= Session["mensaje"] %>
            </div>
        <% Session["mensaje"] = null; } %>

        <div class="row g-3">
            <div class="col-md-6">
                <label class="form-label">Nombre</label>
                <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNombre" ErrorMessage="Requerido" CssClass="text-danger" Display="Dynamic" />
            </div>
            <div class="col-md-6">
                <label class="form-label">Apellido</label>
                <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtApellido" ErrorMessage="Requerido" CssClass="text-danger" Display="Dynamic" />
            </div>
            
            <div class="col-md-12">
                <label class="form-label">Email</label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" ReadOnly="true" />
            </div>

            <div class="col-md-6">
                <label class="form-label">DNI (*)</label>
                <asp:TextBox ID="txtDni" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtDni" ErrorMessage="El DNI es obligatorio para reservar turnos." CssClass="text-danger" Display="Dynamic" />
            </div>
            
            <div class="col-md-6">
                <label class="form-label">Fecha Nacimiento (*)</label>
                <asp:TextBox ID="txtFechaNacimiento" runat="server" CssClass="form-control" TextMode="Date" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtFechaNacimiento" ErrorMessage="La fecha es obligatoria." CssClass="text-danger" Display="Dynamic" />
            </div>

            <div class="col-md-6">
                <label class="form-label">Género</label>
                <asp:DropDownList ID="ddlGenero" runat="server" CssClass="form-select">
                    <asp:ListItem Text="Seleccionar" Value="" />
                    <asp:ListItem Text="Masculino" Value="Masculino" />
                    <asp:ListItem Text="Femenino" Value="Femenino" />
                    <asp:ListItem Text="Otro" Value="Otro" />
                </asp:DropDownList>
            </div>

            <div class="col-md-6">
                <label class="form-label">Teléfono</label>
                <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" />
            </div>
            
            <div class="col-md-6">
                <label class="form-label">Domicilio</label>
                <asp:TextBox ID="txtDireccion" runat="server" CssClass="form-control" />
            </div>

            <div class="col-md-6">
                <label class="form-label">Obra Social</label>
                <asp:TextBox ID="txtObraSocial" runat="server" CssClass="form-control" />
            </div>

            <div class="col-12 mt-4">
                <asp:Button ID="BtnGuardar" runat="server" Text="Guardar y Continuar" CssClass="btn btn-primary" OnClick="BtnGuardar_Click" />
                <a href="Default.aspx" class="btn btn-link">Cancelar</a>
            </div>
        </div>
    </div>
</asp:Content>