<%@ Page Title="Medicos - Panel de administracion" Language="C#" MasterPageFile="~/App.Master" AutoEventWireup="true" CodeBehind="Medicos.aspx.cs" Inherits="Presentacion.Admin.Medicos" %>
<asp:Content ID="HeadSeo" ContentPlaceHolderID="HeadExtra" runat="server">
    <title>Medicos - Panel de administracion</title>
    <meta name="description" content="Gestion de medicos"/>
</asp:Content>
<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
    <main class="container my-4" role="main" aria-labelledby="titulo-medicos">
        <h1 id="titulo-medicos" class="h3 mb-4">Medicos</h1>
        <div class="row g-4">
            <section class="col-lg-7" aria-labelledby="titulo-listado-medicos">
                <h2 id="titulo-listado-medicos" class="h5 mb-3">Listado de medicos</h2>
                <asp:GridView ID="gvMedicos" runat="server"
                    CssClass="table table-sm table-hover"
                    AutoGenerateColumns="false" DataKeyNames="IdMedico">
                    <Columns>
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre/es" />
                        <asp:BoundField DataField="Apellido" HeaderText="Apellido/s" />
                        <asp:BoundField DataField="FechaNacimiento" HeaderText="Fecha de Nacimiento" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField DataField="Genero" HeaderText="Género" />
                        <asp:BoundField DataField="DNI" HeaderText="DNI" />
                        <asp:BoundField DataField="Direccion" HeaderText="Dirección" />
                        <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
                        <asp:BoundField DataField="Email" HeaderText="Email" />
                        <asp:BoundField DataField="Matricula" HeaderText="Matrícula" />
                        <asp:BoundField DataField="Especialidad" HeaderText="NombreEspecialidad" />
                        <asp:BoundField DataField="TurnoDeTrabajo" HeaderText="TurnoDeTrabajo" />
                        <asp:CheckBoxField DataField="Activo" HeaderText="Activo" />
                    </Columns>
                    </asp:GridView>
                </section>
                <section class="col-lg-5" aria-labelledby="titulo-nuevo-medico">
                    <h2 id="titulo-nuevo-medico" class="h5 mb-3">Nuevo medico</h2>

                    <asp:ValidationSummary ID="ValidarMedico" runat="server"
                        CssClass="alert alert-danger"  EnableClientScript="true"/>
                    <div class="mb-3">
                       <label for="txtNombre" class="form-label">Nombre/es</label>
                        <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="rfvNombre" runat="server" ControlToValidate="txtNombre"
                            ErrorMessage="El nombre es obligatorio." CssClass="text-danger" Display="Dynamic" />
                    </div>
                    <div class="mb-3">
                       <label for="txtApellido" class="form-label">Apellido/s</label>
                        <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="rfvApellido" runat="server" ControlToValidate="txtApellido"
                            ErrorMessage="El apellido es obligatorio." CssClass="text-danger" Display="Dynamic" />
                        </div>
                    <div class="mb-3">
                          <label for="txtFechaNacimiento" class="form-label">Fecha de Nacimiento</label>
                            <asp:TextBox ID="txtFechaNacimiento" runat="server" CssClass="form-control" TextMode="Date" />
                            <asp:RequiredFieldValidator ID="rfvFechaNacimiento" runat="server" ControlToValidate="txtFechaNacimiento"
                             ErrorMessage="La fecha de nacimiento es obligatoria." CssClass="text-danger" Display="Dynamic" />
                        </div>
                    <div class="mb-3">
                            <label for="ddlGenero" class="form-label">Género</label>
                                <asp:DropDownList ID="ddlGenero" runat="server" CssClass="form-select">
                                    <asp:ListItem Text="Masculino" Value="Masculino" />
                                    <asp:ListItem Text="Femenino" Value="Femenino" />
                                    <asp:ListItem Text="Otro" Value="Otro" />
                                </asp:DropDownList>
                            </div>
                    <div class="mb-3">
                        <label for="txtDNI" class="form-label">DNI</label>
                        <asp:TextBox ID="txtDNI" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="rfvDNI" runat="server" ControlToValidate="txtDNI"
                            ErrorMessage="El DNI es obligatorio." CssClass="text-danger" Display="Dynamic" />
                        </div>
                    <div class="mb-3">
                        <label for="txtDireccion" class="form-label">Dirección</label>
                        <asp:TextBox ID="txtDireccion" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="rfvDireccion" runat="server" ControlToValidate="txtDireccion"
                            ErrorMessage="La dirección es obligatoria." CssClass="text-danger" Display="Dynamic" />
                        </div>
                    <div class="mb-3">
                        <label for="txtTelefono" class="form-label">Teléfono</label>
                        <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="rfvTelefono" runat="server" ControlToValidate="txtTelefono"
                            ErrorMessage="El teléfono es obligatorio." CssClass="text-danger" Display="Dynamic" />
                        </div>
                    <div class="mb-3">
                        <label for="txtEmail" class="form-label">Email</label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
                        <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                            ErrorMessage="El email es obligatorio." CssClass="text-danger" Display="Dynamic" />
                        </div>
                    <div class="mb-3">
                        <label for="txtMatricula" class="form-label">Matrícula</label>
                        <asp:TextBox ID="txtMatricula" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="rfvMatricula" runat="server" ControlToValidate="txtMatricula"
                            ErrorMessage="La matrícula es obligatoria." CssClass="text-danger" Display="Dynamic" />
                        </div>
                    <div class="mb-3">
                        <label class="form-label">Especialidades</label>
                        <asp:CheckBoxList ID="chkEspecialidades" data-rol="especialidades" runat="server" CssClass="form-check" RepeatDirection="Vertical" />
                        <asp:CustomValidator ID="cvEspecialidades" runat="server"
                            ErrorMessage="Debe seleccionar al menos una especialidad."
                            CssClass="text-danger" Display="Dynamic"
                            ClientValidationFunction="validarEspecialidades"></asp:CustomValidator>
                        </div>
                    <div class="mb-3">
                        <label for="ddlTurnoDeTrabajo" class="form-label">Turno de Trabajo</label>
                        <asp:DropDownList ID="ddlTurnoDeTrabajo" runat="server" CssClass="form-select" />
                        <asp:RequiredFieldValidator ID="rfvTurnoDeTrabajo" runat="server" ControlToValidate="ddlTurnoDeTrabajo"
                            InitialValue="" ErrorMessage="El turno de trabajo es obligatorio." CssClass="text-danger" Display="Dynamic" />
                        </div>
                    <asp:Button ID="btnGuarda" runat="server"
                        Text="Guardar" CssClass="btn btn-succes w-100" />
                    </section>
            </div>
    </main>
</asp:Content>
