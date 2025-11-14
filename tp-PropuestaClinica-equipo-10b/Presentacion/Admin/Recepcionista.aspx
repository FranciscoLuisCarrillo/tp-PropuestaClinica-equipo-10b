<%@ Page Title="Recepcionistas - Panel de administración" Language="C#"
    MasterPageFile="~/App.Master"
    AutoEventWireup="true"
    CodeBehind="Recepcionista.aspx.cs"
    Inherits="Presentacion.Admin.Recepcionista" %>

<asp:Content ID="HeadSeo" ContentPlaceHolderID="HeadExtra" runat="server">
    <title>Recepcionistas - Panel de administración</title>
    <meta name="description" content="Gestión de recepcionistas de la clínica." />
</asp:Content>

<asp:Content ID="Main" ContentPlaceHolderID="MainContent" runat="server">
    <main class="container my-4" role="main" aria-labelledby="titulo-recepcionista">
        <h1 id="titulo-recepcionista" class="h3 mb-4">Recepcionistas</h1>

        <div class="row g-4">
            <!-- LISTADO -->
            <section class="col-lg-7" aria-labelledby="titulo-listado-recepcionista">
                <h2 id="titulo-listado-recepcionista" class="h5 mb-3">Listado de recepcionistas</h2>

                <asp:GridView ID="gvRecepcionistas" runat="server"
                    CssClass="table table-sm table-hover"
                    AutoGenerateColumns="false"
                    DataKeyNames="IdRecepcionista">
                    <Columns>
                        <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                        <asp:BoundField DataField="Apellido" HeaderText="Apellido" />
                        <asp:BoundField DataField="FechaNacimiento" HeaderText="Fecha de Nacimiento"
                                        DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField DataField="Genero" HeaderText="Género" />
                        <asp:BoundField DataField="DNI" HeaderText="DNI" />
                        <asp:BoundField DataField="Direccion" HeaderText="Dirección" />
                        <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
                        <asp:BoundField DataField="Email" HeaderText="Email" />
                        <asp:BoundField DataField="TurnoDeTrabajo" HeaderText="Turno de trabajo" />
                        <asp:CheckBoxField DataField="Activo" HeaderText="Activo" />
                    </Columns>
                </asp:GridView>
            </section>

            <!-- ALTA -->
            <section class="col-lg-5" aria-labelledby="titulo-nueva-recepcionista">
                <h2 id="titulo-nueva-recepcionista" class="h5 mb-3">Nueva recepcionista</h2>

                <asp:ValidationSummary ID="valResumen" runat="server"
                    CssClass="alert alert-danger"
                    EnableClientScript="true" />

                <div class="mb-3">
                    <label for="txtNombre" class="form-label">Nombre</label>
                    <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator ID="rfvNombre" runat="server"
                        ControlToValidate="txtNombre"
                        ErrorMessage="El nombre es obligatorio."
                        CssClass="text-danger" Display="Dynamic" />
                </div>

                <div class="mb-3">
                    <label for="txtApellido" class="form-label">Apellido</label>
                    <asp:TextBox ID="txtApellido" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator ID="rfvApellido" runat="server"
                        ControlToValidate="txtApellido"
                        ErrorMessage="El apellido es obligatorio."
                        CssClass="text-danger" Display="Dynamic" />
                </div>

                <div class="mb-3">
                    <label for="txtFechaNacimiento" class="form-label">Fecha de nacimiento</label>
                    <asp:TextBox ID="txtFechaNacimiento" runat="server"
                        CssClass="form-control" TextMode="Date" />
                    <asp:RequiredFieldValidator ID="rfvFechaNacimiento" runat="server"
                        ControlToValidate="txtFechaNacimiento"
                        ErrorMessage="La fecha de nacimiento es obligatoria."
                        CssClass="text-danger" Display="Dynamic" />
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
                    <asp:RequiredFieldValidator ID="rfvDni" runat="server"
                        ControlToValidate="txtDNI"
                        ErrorMessage="El DNI es obligatorio."
                        CssClass="text-danger" Display="Dynamic" />
                </div>

                <div class="mb-3">
                    <label for="txtDireccion" class="form-label">Dirección</label>
                    <asp:TextBox ID="txtDireccion" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator ID="rfvDireccion" runat="server"
                        ControlToValidate="txtDireccion"
                        ErrorMessage="La dirección es obligatoria."
                        CssClass="text-danger" Display="Dynamic" />
                </div>

                <div class="mb-3">
                    <label for="txtTelefono" class="form-label">Teléfono</label>
                    <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator ID="rfvTelefono" runat="server"
                        ControlToValidate="txtTelefono"
                        ErrorMessage="El teléfono es obligatorio."
                        CssClass="text-danger" Display="Dynamic" />
                </div>

                <div class="mb-3">
                    <label for="txtEmail" class="form-label">Email</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server"
                        ControlToValidate="txtEmail"
                        ErrorMessage="El email es obligatorio."
                        CssClass="text-danger" Display="Dynamic" />
                </div>
                    <div class="mb-3">
                <label for="txtPass" class="form-label">Password</label>
                <asp:TextBox ID="txtPass" runat="server"
                             CssClass="form-control" TextMode="Password" />
                <asp:RequiredFieldValidator ID="rfvPass" runat="server"
                    ControlToValidate="txtPass"
                    ErrorMessage="La contraseña es obligatoria."
                    CssClass="text-danger" Display="Dynamic" />
            </div>

            <div class="mb-3">
                <label for="txtConfirmarPass" class="form-label">Confirmar password</label>
                <asp:TextBox ID="txtConfirmarPass" runat="server"
                             CssClass="form-control" TextMode="Password" />
                <asp:RequiredFieldValidator ID="rfvConfirmarPass" runat="server"
                    ControlToValidate="txtConfirmarPass"
                    ErrorMessage="La confirmación es obligatoria."
                    CssClass="text-danger" Display="Dynamic" />
                <asp:CompareValidator ID="cvPass" runat="server"
                    ControlToValidate="txtConfirmarPass"
                    ControlToCompare="txtPass"
                    ErrorMessage="Las contraseñas no coinciden."
                    CssClass="text-danger" Display="Dynamic" />
            </div>

                <div class="mb-3">
                    <label for="ddlTurnoDeTrabajo" class="form-label">Turno de trabajo</label>
                    <asp:DropDownList ID="ddlTurnoDeTrabajo" runat="server" CssClass="form-select" />
                    <asp:RequiredFieldValidator ID="rfvTurno" runat="server"
                        ControlToValidate="ddlTurnoDeTrabajo"
                        InitialValue=""
                        ErrorMessage="El turno de trabajo es obligatorio."
                        CssClass="text-danger" Display="Dynamic" />
                </div>

                <div class="form-check mb-3">
                    <asp:CheckBox ID="chkActivo" runat="server"
                        CssClass="form-check-input" Checked="true" />
                    <label class="form-check-label" for="chkActivo">Activo</label>
                </div>

                <asp:Button ID="btnGuardar" runat="server"
                    Text="Guardar"
                    CssClass="btn btn-success w-100"
                    OnClick="btnGuardar_Click" />
            </section>
        </div>

        <div class="text-end mt-3">
            <a href="Default.aspx" class="btn btn-secondary">Atrás</a>
        </div>
    </main>
</asp:Content>
