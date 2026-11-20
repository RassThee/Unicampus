
using System;
using System.Data;
using UniCampus.DAL;

namespace UniCampus.BLL
{
    public class EstadisticaService
    {
        private EstadisticaRepository repositorio;

        public EstadisticaService()
        {
            repositorio = new EstadisticaRepository();
        }

        // ==========================================
        // ESTADISTICAS GENERALES PARA DOCENTES
        // ==========================================

        // CONSULTAR ESTADISTICAS GENERALES
        public DataTable ObtenerResumenGeneral()
        {
            return repositorio.ObtenerResumenGeneral();
        }

        // CONSULTAR INSCRIPCIONES POR ACTIVIDAD
        public DataTable ObtenerInscripcionesPorActividad()
        {
            return repositorio.ObtenerInscripcionesPorActividad();
        }

        // CONSULTAR ACTIVIDADES POR ESTADO
        public DataTable ObtenerActividadesPorEstado()
        {
            return repositorio.ObtenerActividadesPorEstado();
        }

        // CONSULTAR CALIFICACIONES POR ACTIVIDAD
        public DataTable ObtenerCalificacionesPorActividad()
        {
            return repositorio.ObtenerCalificacionesPorActividad();
        }

        // ==========================================
        // ESTADISTICAS PERSONALES PARA ESTUDIANTES
        // ==========================================

        // CONSULTAR RESUMEN DEL ESTUDIANTE
        public DataTable ObtenerResumenEstudiante(int usuarioId)
        {
            return repositorio.ObtenerResumenEstudiante(usuarioId);
        }

        // CONSULTAR INSCRIPCIONES DEL ESTUDIANTE
        public DataTable ObtenerInscripcionesEstudiante(int usuarioId)
        {
            return repositorio.ObtenerInscripcionesEstudiante(usuarioId);
        }

        // CONSULTAR ACTIVIDADES POR ESTADO DEL ESTUDIANTE
        public DataTable ObtenerActividadesPorEstadoEstudiante(int usuarioId)
        {
            return repositorio.ObtenerActividadesPorEstadoEstudiante(usuarioId);
        }

        // CONSULTAR CALIFICACIONES DEL ESTUDIANTE
        public DataTable ObtenerCalificacionesEstudiante(int usuarioId)
        {
            return repositorio.ObtenerCalificacionesEstudiante(usuarioId);
        }
    }
}
    