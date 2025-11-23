<%@ Page Title="Esepcialidades - Panel de administracion" Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="Especialidades.aspx.cs" Inherits="Presentacion.Admin.Especialidades" %>
<asp:Content ID="HeadSeo" ContentPlaceHolderID="HeadExtra" runat="server">
    <title>Especialidades Panel de Administracion</title>
    <meta name="description" content="Gestion de especialidades"/>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
    <main class="container my-4" role="main" aria-labelledby="titulo-especialidades">
        <h1 id="titulo-especialidades" class="h3 mb-4">Especialidades medicas</h1>
        <div class="row g-4">
            <section class="col-lg-7" aria-labelledby="titulo-lista-especialidades">
                <h2 id="titulo-lista-especialidades" class="h5 mb-3">Listado de especialidades</h2>
                <asp:GridView ID="gvEspecialidades" runat="server"
                    CssClass="table table-sm table-hover"
                    AutoGenerateColumns="false" DataKeyNames="EspecialidadId" OnRowDataBound="gvEspecialidades_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre Especialidad" />
                       <asp:TemplateField HeaderText="Activa">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkActiva"
                                runat="server"
                                Checked='<%# Bind("Activa") %>'
                                AutoPostBack="true"
                                OnCheckedChanged="chkActiva_CheckedChanged" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </section>
            <section class="col-lg-5" aria-labelledby="titulo-nueva-especialidad">
                <h2 id="titulo-nueva-especialidad" class="h5 mb-3">Nueva especialidad</h2>
                <asp:ValidationSummary ID="ValidarEspecialidad" runat="server"
                    CssClass="alert alert-danger"  EnableClientScript="true"/>
                <div class="mb-3">
                   <label for="txtNombreEspecialidad" class="form-label">Nombre de la Especialidad</label>
                    <asp:TextBox ID="txtNombreEspecialidad" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator ID="rfvNombreEspecialidad" runat="server" ControlToValidate="txtNombreEspecialidad"
                        ErrorMessage="El nombre de la especialidad es obligatorio." CssClass="text-danger" Display="Dynamic" />
                </div>
                <asp:Button ID="btnAgregarEspecialidad" runat="server" Text="Agregar Especialidad" CssClass="btn btn-success w-100" OnClick="btnAgregarEspecialidad_Click" />
                <asp:Label ID="lblMsg" runat="server" EnableViewState="false"></asp:Label> 
            </section>
        </div>
         <div class="text-end mt-3">
             <a href="Default.aspx" class="btn btn-secondary">Atrás</a>
        </div>
       
    </main>
</asp:Content>
