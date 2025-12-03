using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clinica.Datos
{
    // Clase estática para encapsular la cadena de conexión.
    public static class ConexionDB
    {
        // La cadena de conexión se genera directamente en el código, tal como lo solicitaste.
        // Utiliza la base de datos ClinicaDB que creaste.
        // TOMAS
        private const string CadenaConexion = "server=Tomas\\MSSQLSERVER01; database=ClinicaDB; integrated security=true;";
        // FRANCISCO
      //  private const string CadenaConexion = "server=.; database=ClinicaDB; integrated security=true;";
        
        // Método para obtener una nueva conexión a la base de datos.
        public static SqlConnection ObtenerConexion()
        {
            return new SqlConnection(CadenaConexion);
        }
    }
}




/*
Script SQL para la creación de las tablas necesarias para la gestión de turnos en una clínica médica.

-- 1. Crear la Base de Datos (si es necesario)
USE master;
GO
CREATE DATABASE ClinicaDB;
GO
USE ClinicaDB;
GO

-- 2. Tabla de Pacientes
CREATE TABLE Pacientes (
    PacienteId INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    Apellido NVARCHAR(100) NOT NULL,
    DNI NVARCHAR(20) NOT NULL UNIQUE,
    FechaNacimiento DATE NOT NULL,
    Telefono NVARCHAR(50),
    Email NVARCHAR(100) NOT NULL,
    Domicilio NVARCHAR(250)
);

-- 3. Tabla de Médicos (Se asume que los médicos también son usuarios del sistema para seguridad)
CREATE TABLE Medicos (
    MedicoId INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    Apellido NVARCHAR(100) NOT NULL,
    Matricula NVARCHAR(50) NOT NULL UNIQUE,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Telefono NVARCHAR(50)
);

-- 4. Tabla de Turnos de Trabajo (Ej: Mañana, Tarde, Noche)
CREATE TABLE TurnosTrabajo (
    TurnoTrabajoId INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(50) NOT NULL UNIQUE,
    HoraEntrada TIME NOT NULL,
    HoraSalida TIME NOT NULL
);

-- 5. Tabla de Especialidades
CREATE TABLE Especialidades (
    EspecialidadId INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL UNIQUE
);

-- 6. Tabla de Relación N:M entre Médicos y Especialidades
CREATE TABLE MedicoEspecialidades (
    MedicoId INT NOT NULL,
    EspecialidadId INT NOT NULL,
    
    PRIMARY KEY (MedicoId, EspecialidadId),
    
    -- Restricciones de Clave Foránea
    CONSTRAINT FK_MedicoEspecialidades_Medicos FOREIGN KEY (MedicoId)
        REFERENCES Medicos (MedicoId) ON DELETE CASCADE,
    CONSTRAINT FK_MedicoEspecialidades_Especialidades FOREIGN KEY (EspecialidadId)
        REFERENCES Especialidades (EspecialidadId) ON DELETE CASCADE
);

-- 7. Tabla de Turnos (La funcionalidad central)
CREATE TABLE Turnos (
    TurnoId INT PRIMARY KEY IDENTITY(1,1),
    
    -- Claves Foráneas
    PacienteId INT NOT NULL,
    MedicoId INT NOT NULL,
    EspecialidadId INT NOT NULL, -- Se puede deducir del médico, pero se mantiene para consultas rápidas
    
    -- Datos del Turno
    FechaHoraInicio DATETIME NOT NULL,
    FechaHoraFin DATETIME NOT NULL, -- Asumiendo 1 hora de duración (configurado en lógica de negocio)
    
    -- Datos de la Cita
    MotivoConsulta NVARCHAR(500) NOT NULL, -- Observaciones del paciente al solicitar
    DiagnosticoMedico NVARCHAR(1000),      -- Observaciones del médico (sólo se añade después)
    
    -- Estado y Manejo
    Estado INT NOT NULL, -- Mapeado al Enum EstadoTurno (0: Nuevo, 1: Reprogramado, etc.)
    FechaCreacion DATETIME NOT NULL DEFAULT GETDATE(),

    -- Restricciones de Clave Foránea
    CONSTRAINT FK_Turnos_Pacientes FOREIGN KEY (PacienteId)
        REFERENCES Pacientes (PacienteId),
    CONSTRAINT FK_Turnos_Medicos FOREIGN KEY (MedicoId)
        REFERENCES Medicos (MedicoId),
    CONSTRAINT FK_Turnos_Especialidades FOREIGN KEY (EspecialidadId)
        REFERENCES Especialidades (EspecialidadId),

    -- Restricciones de Negocio Críticas (No puede haber un médico y un paciente doblemente ocupados a la misma hora/día)
    CONSTRAINT UQ_Turno_Medico_Hora UNIQUE (MedicoId, FechaHoraInicio),
    CONSTRAINT UQ_Turno_Paciente_Hora UNIQUE (PacienteId, FechaHoraInicio)
);

-- 1. Agregar la columna para el Turno de Trabajo
ALTER TABLE Medicos
ADD TurnoTrabajoId INT;
GO

-- 2. Crear la relación (Foreign Key)
ALTER TABLE Medicos
ADD CONSTRAINT FK_Medicos_TurnosTrabajo
    FOREIGN KEY (TurnoTrabajoId)
    REFERENCES TurnosTrabajo (TurnoTrabajoId);
GO

 -------------------------------------------- NUEVO TABLA USUARIOS -------------------------------
USE ClinicaDB;
GO

CREATE TABLE Usuarios (
    UsuarioId INT PRIMARY KEY IDENTITY(1,1),
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Pass NVARCHAR(100) NOT NULL,
    
    -- 0=Admin, 1=Recepcionista, 2=Medico
    -- (Esto lo manejaremos con el Enum en C#)
    Perfil INT NOT NULL DEFAULT 1 
);
GO

-- Insertamos un usuario Administrador por defecto para poder entrar
INSERT INTO Usuarios (Email, Pass, Perfil) VALUES ('admin@clinica.com', 'admin', 0);
GO

------------------------------------------
--- Ultimos añadidos --- 
--------------------------------------------


ALTER TABLE Especialidades
ADD Activa BIT DEFAULT TRUE;

GO

ALTER TABLE Medicos
ADD Activo BIT DEFAULT 1;
GO

ALTER TABLE Pacientes
ADD ObraSocial NVARCHAR(100) NULL;




USE ClinicaDB;
GO

ALTER TABLE Pacientes
ADD ObraSocial NVARCHAR(100) NULL; -- Puede ser NULL si no tiene
GO


--------------------------------------------
datos generados para pruebas
--------------------------------------------

USE ClinicaDB;
GO

-- =============================================
-- 0. LIMPIEZA DE DATOS (Para evitar errores de duplicados)
-- =============================================
PRINT 'Iniciando limpieza de base de datos...';

-- Desactivamos temporalmente las restricciones para borrar sin problemas de orden
EXEC sp_msforeachtable "ALTER TABLE ? NOCHECK CONSTRAINT all";

-- Borramos los datos de las tablas
DELETE FROM Turnos;
DELETE FROM MedicoEspecialidades;
DELETE FROM Medicos;
DELETE FROM Pacientes;
DELETE FROM Usuarios;
DELETE FROM Especialidades;
DELETE FROM TurnosTrabajo;

-- Reseteamos los contadores de ID (Identity) a 0
DBCC CHECKIDENT ('Turnos', RESEED, 0);
DBCC CHECKIDENT ('Medicos', RESEED, 0);
DBCC CHECKIDENT ('Pacientes', RESEED, 0);
DBCC CHECKIDENT ('Usuarios', RESEED, 0);
DBCC CHECKIDENT ('Especialidades', RESEED, 0);
DBCC CHECKIDENT ('TurnosTrabajo', RESEED, 0);

-- Reactivamos las restricciones
EXEC sp_msforeachtable "ALTER TABLE ? WITH CHECK CHECK CONSTRAINT all";

PRINT 'Limpieza finalizada. Insertando datos de prueba...';
GO

-- =============================================
-- 1. TABLA DE TURNOS DE TRABAJO
-- =============================================
INSERT INTO TurnosTrabajo (Nombre, HoraEntrada, HoraSalida) VALUES 
('Mañana', '08:00:00', '14:00:00'),
('Tarde', '14:00:00', '20:00:00'),
('Noche', '20:00:00', '00:00:00'),
('Guardia 24hs', '00:00:00', '23:59:59');
GO

-- =============================================
-- 2. TABLA DE ESPECIALIDADES
-- =============================================
INSERT INTO Especialidades (Nombre) VALUES 
('Clínica Médica'),
('Pediatría'),
('Cardiología'),
('Dermatología'),
('Traumatología'),
('Ginecología'),
('Oftalmología'),
('Neurología'),
('Psiquiatría'),
('Nutrición');
GO

-- =============================================
-- 3. TABLA DE USUARIOS (Perfiles: 0=Admin, 1=Recepcionista, 2=Medico)
-- =============================================
INSERT INTO Usuarios (Email, Pass, Perfil) VALUES 
('admin@clinica.com', 'admin123', 0),           -- Administrador
('recepcion1@clinica.com', 'recepcion123', 1),  -- Recepcionista Mañana
('recepcion2@clinica.com', 'recepcion123', 1),  -- Recepcionista Tarde
('medico1@clinica.com', 'medico123', 2),        -- Usuario para Médico 1
('medico2@clinica.com', 'medico123', 2),        -- Usuario para Médico 2
('medico3@clinica.com', 'medico123', 2),        -- Usuario para Médico 3
('medico4@clinica.com', 'medico123', 2);        -- Usuario para Médico 4
GO

-- =============================================
-- 4. TABLA DE MEDICOS
-- =============================================
-- Como reseteamos los IDs, sabemos que TurnosTrabajoId 1='Mañana', 2='Tarde', etc.
INSERT INTO Medicos (Nombre, Apellido, Matricula, Email, Telefono, TurnoTrabajoId) VALUES 
('Gregory', 'House', 'MN-1111', 'medico1@clinica.com', '11-5555-1111', 1), -- Turno Mañana
('Meredith', 'Grey', 'MN-2222', 'medico2@clinica.com', '11-5555-2222', 2), -- Turno Tarde
('Stephen', 'Strange', 'MN-3333', 'medico3@clinica.com', '11-5555-3333', 3), -- Turno Noche
('Shaun', 'Murphy', 'MN-4444', 'medico4@clinica.com', '11-5555-4444', 1),   -- Turno Mañana
('Julius', 'Hibbert', 'MN-5555', 'hibbert@clinica.com', '11-5555-5555', 2), -- Turno Tarde
('Nick', 'Riviera', 'MN-6666', 'riviera@clinica.com', '11-5555-6666', 1);  -- Turno Mañana
GO

-- =============================================
-- 5. RELACIÓN MEDICO - ESPECIALIDADES
-- =============================================
-- Asignamos especialidades a los médicos (IDs reiniciados y predecibles)
-- House (ID 1) -> Clínica Médica (1) y Neurología (8)
INSERT INTO MedicoEspecialidades (MedicoId, EspecialidadId) VALUES (1, 1), (1, 8);

-- Grey (ID 2) -> Clínica Médica (1) y Traumatología (5)
INSERT INTO MedicoEspecialidades (MedicoId, EspecialidadId) VALUES (2, 1), (2, 5);

-- Strange (ID 3) -> Neurología (8) y Psiquiatría (9)
INSERT INTO MedicoEspecialidades (MedicoId, EspecialidadId) VALUES (3, 8), (3, 9);

-- Murphy (ID 4) -> Pediatría (2) y Cardiología (3)
INSERT INTO MedicoEspecialidades (MedicoId, EspecialidadId) VALUES (4, 2), (4, 3);

-- Hibbert (ID 5) -> Dermatologia (4) y Nutricion (10)
INSERT INTO MedicoEspecialidades (MedicoId, EspecialidadId) VALUES (5, 4), (5, 10);

-- Riviera (ID 6) -> Traumatologia (5)
INSERT INTO MedicoEspecialidades (MedicoId, EspecialidadId) VALUES (6, 5);
GO

-- =============================================
-- 6. TABLA DE PACIENTES
-- =============================================
INSERT INTO Pacientes (Nombre, Apellido, DNI, FechaNacimiento, Telefono, Email, Domicilio, ObraSocial) VALUES 
('Homero', 'Simpson', '10000001', '1980-05-12', '11-1111-1111', 'homero@mail.com', 'Av. Siempreviva 742', 'OSDE 210'),
('Marge', 'Bouvier', '10000002', '1982-10-01', '11-2222-2222', 'marge@mail.com', 'Av. Siempreviva 742', 'OSDE 210'),
('Bart', 'Simpson', '30000001', '2010-04-01', '11-3333-3333', 'bart@mail.com', 'Av. Siempreviva 742', NULL),
('Lisa', 'Simpson', '30000002', '2012-05-09', '11-4444-4444', 'lisa@mail.com', 'Av. Siempreviva 742', 'Swiss Medical'),
('Ned', 'Flanders', '10000003', '1975-02-15', '11-5555-5555', 'ned@mail.com', 'Av. Siempreviva 744', 'Galeno'),
('Barney', 'Gumble', '10000004', '1979-07-20', '11-6666-6666', 'barney@mail.com', 'Taverna de Moe', 'PAMI'),
('Montgomery', 'Burns', '00000001', '1920-09-15', '11-0000-0000', 'burns@nuclear.com', 'Mansion Burns', 'Particular');
GO

-- =============================================
-- 7. TABLA DE TURNOS (Generamos historial y futuros)
-- =============================================
-- Estados: 0=Nuevo, 1=Reprogramado, 2=Cancelado, 3=Cerrado/Atendido

-- Turnos PASADOS (Ya atendidos o cancelados)
INSERT INTO Turnos (PacienteId, MedicoId, EspecialidadId, FechaHoraInicio, FechaHoraFin, MotivoConsulta, DiagnosticoMedico, Estado) VALUES 
(1, 1, 1, DATEADD(day, -10, GETDATE()), DATEADD(day, -10, DATEADD(hour, 1, GETDATE())), 'Dolor de cabeza fuerte', 'Migraña por estrés. Se receta ibuprofeno.', 3), -- Atendido
(2, 2, 5, DATEADD(day, -5, GETDATE()), DATEADD(day, -5, DATEADD(hour, 1, GETDATE())), 'Dolor en la rodilla', 'Esguince leve.', 3), -- Atendido
(5, 3, 8, DATEADD(day, -20, GETDATE()), DATEADD(day, -20, DATEADD(hour, 1, GETDATE())), 'Veo gente muerta', 'Derivado a psiquiatría.', 3), -- Atendido
(6, 1, 1, DATEADD(day, -2, GETDATE()), DATEADD(day, -2, DATEADD(hour, 1, GETDATE())), 'Chequeo general', NULL, 2); -- Cancelado

-- Turnos FUTUROS
-- Nota: Usamos fechas fijas relativas a "ahora" para evitar violar restricciones UNIQUE si corres el script el mismo dia dos veces sin limpiar,
-- pero como ya limpiamos arriba, no habrá problema.
INSERT INTO Turnos (PacienteId, MedicoId, EspecialidadId, FechaHoraInicio, FechaHoraFin, MotivoConsulta, DiagnosticoMedico, Estado) VALUES 
(3, 4, 2, DATEADD(day, 1, CAST(GETDATE() AS DATE)), DATEADD(hour, 1, DATEADD(day, 1, CAST(GETDATE() AS DATE))), 'Control pediátrico anual', NULL, 0), 
(4, 4, 2, DATEADD(hour, 1, DATEADD(day, 1, CAST(GETDATE() AS DATE))), DATEADD(hour, 2, DATEADD(day, 1, CAST(GETDATE() AS DATE))), 'Dolor de garganta', NULL, 0), 
(1, 2, 1, DATEADD(day, 2, CAST(GETDATE() AS DATE)), DATEADD(hour, 1, DATEADD(day, 2, CAST(GETDATE() AS DATE))), 'Consulta de seguimiento', NULL, 0), 
(7, 6, 5, DATEADD(day, 3, CAST(GETDATE() AS DATE)), DATEADD(hour, 1, DATEADD(day, 3, CAST(GETDATE() AS DATE))), 'Rigidez articular', NULL, 0);
GO

PRINT 'Base de datos reiniciada y poblada con éxito.';


Ultimos agregados en la bd 21/11/2025
-- =============================================
-- 8. TABLA DE Recepcionistas
-- =============================================

CREATE TABLE Recepcionistas (
    RecepcionistaId INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Apellido NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    Telefono NVARCHAR(50),
    TurnoTrabajoId INT NULL,
    Activo BIT DEFAULT 1
);

ALTER TABLE Recepcionistas
ADD CONSTRAINT FK_Recepcionistas_TurnosTrabajo
    FOREIGN KEY (TurnoTrabajoId)
    REFERENCES TurnosTrabajo (TurnoTrabajoId);


// Agregados a las tablas 23/11/2025

ALTER TABLE Pacientes ALTER COLUMN DNI NVARCHAR(20) NULL;
ALTER TABLE Pacientes ALTER COLUMN Telefono NVARCHAR(50) NULL;
ALTER TABLE Pacientes ALTER COLUMN Domicilio NVARCHAR(250) NULL;
ALTER TABLE Pacientes ALTER COLUMN FechaNacimiento DATE NULL;


ALTER TABLE Pacientes ADD Activo BIT NOT NULL DEFAULT 1;

ALTER TABLE Usuarios ADD IdPaciente INT NULL;

ALTER TABLE Usuarios ADD IdMedico INT NULL;

ALTER TABLE Usuarios ADD IdRecepcionista INT NULL;

ALTER TABLE Usuarios ADD Activo BIT NOT NULL DEFAULT 1;


ALTER TABLE Usuarios ADD CONSTRAINT FK_Usuarios_Pacientes FOREIGN KEY (IdPaciente) REFERENCES Pacientes(PacienteId);
ALTER TABLE Usuarios ADD CONSTRAINT FK_Usuarios_Medicos FOREIGN KEY (IdMedico) REFERENCES Medicos(MedicoId);



// Agregado 24/11/2025
ALTER TABLE Pacientes ADD Genero NVARCHAR(20) NULL;


// AGREGADO PARA QUE FUNCIONE RECEPCIONISTA 26/11/2025

USE ClinicaDB;
GO

-- 1. Agregar columna Nombre (si no existe)
IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'Nombre' AND Object_ID = Object_ID(N'Usuarios'))
BEGIN
    ALTER TABLE Usuarios ADD Nombre NVARCHAR(50) NULL;
    PRINT 'Columna Nombre agregada.';
END

-- 2. Agregar columna Apellido (si no existe)
IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'Apellido' AND Object_ID = Object_ID(N'Usuarios'))
BEGIN
    ALTER TABLE Usuarios ADD Apellido NVARCHAR(50) NULL;
    PRINT 'Columna Apellido agregada.';
END

-- 3. Agregar columna Rol (si no existe)
IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'Rol' AND Object_ID = Object_ID(N'Usuarios'))
BEGIN
    ALTER TABLE Usuarios ADD Rol NVARCHAR(20) NULL;
    PRINT 'Columna Rol agregada.';
END
GO

//AGREGA A LA DDBB para la eliminacion de pacientes 27/11/2025


USE ClinicaDB;
GO

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'Activo' AND Object_ID = Object_ID(N'Pacientes'))
BEGIN
    ALTER TABLE Pacientes ADD Activo BIT DEFAULT 1;
    PRINT 'Columna Activo agregada a Pacientes.';
END
GO


    Agregado en Tabla turnos para cuando queres agendar un turno con la recepcionista no se necesario
Completar el campo observaciones 02/12/2025

ALTER TABLE Turnos
ALTER COLUMN MotivoConsulta NVARCHAR(500) NULL


*/