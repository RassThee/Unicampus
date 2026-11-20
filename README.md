# UniCampus - Plataforma de Actividades Académicas

## Descripción

UniCampus es una aplicación web desarrollada para gestionar actividades académicas de una institución educativa. Permite administrar actividades, inscripciones, reseñas y estadísticas, con acceso diferenciado para docentes y estudiantes.

Este proyecto fue desarrollado para la asignatura Desarrollo de Aplicaciones Web II de la Corporación Universitaria Americana.

## Tecnologías utilizadas

- C#
- ASP.NET Web Forms
- .NET Framework
- SQL Server
- ADO.NET
- Visual Studio
- Git y GitHub

## Estructura del proyecto

La solución está organizada en cuatro proyectos:

- **UniCampus.Web:** interfaz web y formularios ASP.NET.
- **UniCampus.BLL:** lógica de negocio y validaciones.
- **UniCampus.DAL:** acceso a datos y consultas SQL Server.
- **UniCampus.Entities:** entidades utilizadas por la aplicación.

## Funcionalidades

- Inicio de sesión y autenticación de usuarios.
- Gestión de actividades académicas.
- Inscripción de estudiantes a actividades.
- Gestión de reseñas y calificaciones.
- Consulta de estadísticas.
- Administración de estudiantes.
- Activación y desactivación de usuarios.
- Restablecimiento de contraseñas.

## Base de datos

La aplicación utiliza una base de datos SQL Server llamada `UniCampusDB`.

Para preparar la base de datos:

1. Abrir SQL Server Management Studio.
2. Conectarse a una instancia de SQL Server.
3. Abrir el archivo `03_BaseDatos.sql`, suministrado con los archivos de entrega.
4. Revisar la configuración del script y ejecutarlo en una instancia de prueba.
5. Configurar la conexión de la aplicación con la instancia SQL Server correspondiente.

## Cómo ejecutar el proyecto

1. Descargar o clonar este repositorio.
2. Abrir el archivo `Unicampus.slnx` en una versión compatible de Visual Studio.
3. Verificar que estén disponibles los componentes de ASP.NET Web Forms y .NET Framework necesarios.
4. Preparar la base de datos `UniCampusDB` en SQL Server.
5. Configurar la conexión en `UniCampus.DAL/ConexionBD.cs` según el servidor local.
6. Restaurar las dependencias necesarias y compilar la solución.
7. Ejecutar el proyecto `UniCampus.Web` desde Visual Studio.

## Seguridad

El proyecto utiliza autenticación de usuarios, validaciones y almacenamiento de contraseñas mediante hashes.

Las contraseñas reales y los archivos de configuración privados no deben publicarse en el repositorio.

## Información académica

**Institución:** Corporación Universitaria Americana

**Asignatura:** Desarrollo de Aplicaciones Web II

**Proyecto:** UniCampus - Plataforma de Registro de Actividades Académicas

**Docente:** Cindy Fuentes

**Estudiante:** Diego Zambrano

**Año:** 2026
