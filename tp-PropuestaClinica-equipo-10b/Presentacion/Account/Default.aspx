<%@ Page Title="Inicio — Clínica Médica" Language="C#" 
    AutoEventWireup="true" CodeBehind="Default.aspx.cs" 
    Inherits="Presentacion.Default" %>

<!DOCTYPE html>
<html lang="es" dir="ltr">
    <head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>Inicio — Clínica Médica</title>
    <meta name="description" content="Bienvenido a la Clínica Médica. Reserva y gestiona tus turnos médicos online de manera fácil y rápida." />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" />
    </head>
    <body>
        <form id="form1" runat="server">
            <div class="container mt-5">
                <h1 class="display-5 mb-3 fw-semibold">Clinica medica</h1>
                <p class="lead text-muted mb-4">Bienvenido a la Clínica Médica. Reserva y gestiona tus turnos médicos online de manera fácil y rápida.</p>
                <div class="d-flex justify-content-center gap-3">
                    <a runat="server" href="~/Account/Login.aspx" class="btn btn-primary btn-lg px-4">Iniciar sesión</a>
                    <a runat="server" href="~/Account/Register.aspx" class="btn btn-success btn-lg px-4">Registrarse</a>
                  </div>
            </div>
       </form>
         <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
   </body>
 </html>
