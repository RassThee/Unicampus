
using System;
using UniCampus.BLL;
using UniCampus.Web.Views;

namespace UniCampus.Web.Presenters
{
    public class EstadisticaPresenter
    {
        private IEstadisticaView vista;
        private EstadisticaService servicio;

        public EstadisticaPresenter(IEstadisticaView vista)
        {
            this.vista = vista;
            servicio = new EstadisticaService();
        }

        // ==========================================
        // CARGAR ESTADISTICAS SEGUN EL ROL
        // ==========================================

        public void CargarEstadisticas(string rolUsuario, int usuarioId)
        {
            try
            {
                if (rolUsuario == "Organizador")
                {
                    CargarEstadisticasDocente();
                }
                else if (rolUsuario == "Participante")
                {
                    if (usuarioId <= 0)
                    {
                        vista.MostrarMensaje(
                            "No se pudo identificar al estudiante.");
                        return;
                    }

                    CargarEstadisticasEstudiante(usuarioId);
                }
                else
                {
                    vista.MostrarMensaje(
                        "No tienes permisos para consultar las estadísticas.");
                }
            }
            catch (Exception)
            {
                vista.MostrarMensaje(
                    "No fue posible cargar las estadísticas. " +
                    "Verifica la conexión con la base de datos.");
            }
        }

        // ==========================================
        // ESTADISTICAS GENERALES DEL DOCENTE
        // ==========================================

        private void CargarEstadisticasDocente()
        {
            // RESUMEN GENERAL
            vista.ResumenGeneral =
                servicio.ObtenerResumenGeneral();

            // INSCRIPCIONES POR ACTIVIDAD
            vista.InscripcionesPorActividad =
                servicio.ObtenerInscripcionesPorActividad();

            // ACTIVIDADES POR ESTADO
            vista.ActividadesPorEstado =
                servicio.ObtenerActividadesPorEstado();

            // CALIFICACIONES POR ACTIVIDAD
            vista.CalificacionesPorActividad =
                servicio.ObtenerCalificacionesPorActividad();
        }

        // ==========================================
        // ESTADISTICAS PERSONALES DEL ESTUDIANTE
        // ==========================================

        private void CargarEstadisticasEstudiante(int usuarioId)
        {
            // RESUMEN PERSONAL
            vista.ResumenGeneral =
                servicio.ObtenerResumenEstudiante(usuarioId);

            // INSCRIPCIONES DEL ESTUDIANTE
            vista.InscripcionesPorActividad =
                servicio.ObtenerInscripcionesEstudiante(usuarioId);

            // ACTIVIDADES INSCRITAS POR ESTADO
            vista.ActividadesPorEstado =
                servicio.ObtenerActividadesPorEstadoEstudiante(usuarioId);

            // CALIFICACIONES DEL ESTUDIANTE
            vista.CalificacionesPorActividad =
                servicio.ObtenerCalificacionesEstudiante(usuarioId);
        }
    }
}
