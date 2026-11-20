
using System.Data;

namespace UniCampus.Web.Views
{
    public interface IEstadisticaView
    {
        // Datos del resumen general
        DataTable ResumenGeneral { set; }

        // Inscripciones y cupos por actividad
        DataTable InscripcionesPorActividad { set; }

        // Cantidad de actividades por estado
        DataTable ActividadesPorEstado { set; }

        // Calificaciones y reseñas por actividad
        DataTable CalificacionesPorActividad { set; }

        // Mostrar mensajes al usuario
        void MostrarMensaje(string mensaje);
    }
}
