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
        private const string CadenaConexion = "server=.; database=ClinicaDB; integrated security=true;";

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

 
 */